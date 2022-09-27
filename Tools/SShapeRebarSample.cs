using Autodesk.Revit;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Ribbon
{
    [Transaction(TransactionMode.Manual)]
    public class SShapeRebarSample : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Autodesk.Revit.DB.View view = doc.ActiveView;

            //平面视图或绘图视图执行本命令
            if (!(view is ViewPlan) && !(view is ViewDrafting))
                return Result.Succeeded;

            //新建事物并启动
            Transaction trans = new Transaction(doc, "S筋大样");
            trans.Start();

            #region 求名为“宽限5mm”的样式
            //新建收集器，过滤出所有线样式
            FilteredElementCollector gStyleCol = new FilteredElementCollector(doc);
            gStyleCol.OfClass(typeof(GraphicsStyle));

            //转换为线样式集合
            ICollection<Element> gStyles = gStyleCol.ToElements();

            //查找名为“宽线5号”的线样式
            GraphicsStyle gStyle = gStyles.FirstOrDefault(m => m.Name == "宽线5号") as GraphicsStyle;

            //如果没有样式则新建一个
            if (gStyle == null)
            {
                //取得“新样式”的类别Category
                Category gsCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

                //新建子类别
                Category subCat = doc.Settings.Categories.NewSubcategory(gsCategory, "宽线5号");

                //设置线宽为5号线（并非5mm的意思）
                subCat.SetLineWeight(5, GraphicsStyleType.Projection);

                //重新查找名为“宽线5号”的线样式

                gStyleCol = new FilteredElementCollector(doc);
                gStyleCol.OfClass(typeof(GraphicsStyle));
                gStyles = gStyleCol.ToElements();
                gStyle = gStyles.FirstOrDefault(m => m.Name == "宽线5号") as GraphicsStyle;
            }
            #endregion

            XYZ point1 = null;
            XYZ point2 = null;

            try
            {
                //选择第一个点
                point1 = uiDoc.Selection.PickPoint("选择起点");
                point2 = uiDoc.Selection.PickPoint("选择终点");
            }
            catch { }

            //当选择点为空时，结束命令
            if (point1 == null || point2 == null)
            {
                return Result.Succeeded;
            }

            #region 计算钢筋形状
            //弯钩端部延伸
            double extend = 100 / 304.8;
            double r = 50 / 304.8;
            double arc = Math.Acos(r / 2 / point1.DistanceTo(point2));

            //计算S筋的控制点
            XYZ offset1 = anticlockwise(point1, point2, r, Math.PI * 2 - arc);
            XYZ offset2 = anticlockwise(point2, point1, r, Math.PI - arc);
            XYZ offset3 = anticlockwise(point1, point2, r, Math.PI - arc);
            XYZ offset4 = anticlockwise(point2, point1, r, Math.PI * 2 - arc);
            XYZ offset5 = offset3 + (offset4 - offset3) / (offset4.DistanceTo(offset3)) * extend;
            XYZ offset6 = offset2 + (offset1 - offset2) / (offset1.DistanceTo(offset2)) * extend;

            //计算圆弧的X方向和Y方向
            XYZ xAxis = (offset1 - point1) / offset1.DistanceTo(point1);
            XYZ yAxis = new XYZ(-xAxis.Y, xAxis.X, xAxis.Z);
            XYZ xAxis1 = (offset4 - point2) / offset4.DistanceTo(point2);
            XYZ yAxis1 = new XYZ(-xAxis1.Y, xAxis1.X, xAxis1.Z);

            //计算S筋控制线
            Line line1 = Line.CreateBound(offset1, offset4);
            Line line2 = Line.CreateBound(offset3, offset5);
            Line line3 = Line.CreateBound(offset2, offset6);

            //计算S筋的半圆
            Arc arc1 = Arc.Create(point1, r, Math.PI, 2 * Math.PI, xAxis, yAxis);
            Arc arc2 = Arc.Create(point2, r, Math.PI, 2 * Math.PI, xAxis1, yAxis1);

            //绘制S筋详图线
            DetailCurve dc1 = doc.Create.NewDetailCurve(view, line1);
            DetailCurve dc2 = doc.Create.NewDetailCurve(view, line2);
            DetailCurve dc3 = doc.Create.NewDetailCurve(view, line3);
            DetailCurve darc1 = doc.Create.NewDetailCurve(view, arc1);
            DetailCurve darc2 = doc.Create.NewDetailCurve(view, arc2);
            #endregion

            //设置S筋样式
            dc1.LineStyle = gStyle;
            dc2.LineStyle = gStyle;
            dc3.LineStyle = gStyle;
            darc1.LineStyle = gStyle;
            darc2.LineStyle = gStyle;

            //提交事务
            trans.Commit();
            return Result.Succeeded;
        }

        //求点p绕点origin逆时针转angle弧度后的坐标
        public XYZ anticlockwise(XYZ origin, XYZ p, double d, double angle)
        {
            XYZ p1 = origin + d * (p - origin).Normalize();
            double dX = p1.X - origin.X;
            double dY = p1.Y - origin.Y;
            double newX = dX * Math.Cos(angle) - dY * Math.Sin(angle) + origin.X;
            double newY = dX * Math.Sin(angle) - dY * Math.Cos(angle) + origin.Y;
            XYZ end = new XYZ(newX, newY, origin.Z);
            return end;
        }
    }
}

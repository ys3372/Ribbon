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
    public class TextSample : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Autodesk.Revit.DB.View activeView = doc.ActiveView;

            //为了简化代码，限制在平面视图内运行
            if (!(activeView is ViewPlan))
            {
                MessageBox.Show("请在平面视图运行");
                return Result.Succeeded;
            }

            //获得默认文字类型
            ElementId tId = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);
            string str = "文字示例";

            //拾取点 此处忽略了取消拾取的处理
            XYZ p = uiDoc.Selection.PickPoint(ObjectSnapTypes.None);

            //新建文字属性选项
            TextNoteOptions opts = new TextNoteOptions(tId);

            //参数设定
            opts.HorizontalAlignment = HorizontalTextAlignment.Left; //水平对齐方式
            opts.Rotation = 45 * Math.PI / 180;//角度

            //新建事物并启动
            Transaction trans = new Transaction(doc, "文字示例");
            trans.Start();

            //新建文字注释方法1
            TextNote tn1 = TextNote.Create(doc, activeView.Id, p, str, opts);

            //新建文字注释方法2
            XYZ delta = new XYZ(10, 0, 0);
            TextNote tn2 = TextNote.Create(doc, activeView.Id, p + delta, str, tId);

            //新建文字注释方法3
            TextNote tn3 = TextNote.Create(doc, activeView.Id, p + delta * 2, 10 / 304.8, str, tId);

            //提交
            trans.Commit();

            //新建事务2并启动
            Transaction trans2 = new Transaction(doc, "yy");
            trans2.Start();

            //用详图线画出文字的范围框
            BoundingBoxRectangle(activeView, tn1.get_BoundingBox(activeView));
            BoundingBoxRectangle(activeView, tn2.get_BoundingBox(activeView));
            BoundingBoxRectangle(activeView, tn3.get_BoundingBox(activeView));

            //提交
            trans2.Commit();
            return Result.Succeeded;
        }

        public List<DetailLine> BoundingBoxRectangle(Autodesk.Revit.DB.View view, BoundingBoxXYZ box)
        {
            List<DetailLine> detailLines = new List<DetailLine>();

            //取得BoundingBox的4个角点
            XYZ p1 = new XYZ(box.Min.X, box.Min.Y, 0);
            XYZ p2 = new XYZ(box.Max.X, box.Min.Y, 0);
            XYZ p3 = new XYZ(box.Max.X, box.Max.Y, 0);
            XYZ p4 = new XYZ(box.Min.X, box.Max.Y, 0);

            //连直线
            Line line1 = Line.CreateBound(p1, p2);
            Line line2 = Line.CreateBound(p2, p3);
            Line line3 = Line.CreateBound(p3, p4);
            Line line4 = Line.CreateBound(p4, p1);

            //转换成详图线
            DetailCurve detailCurve1 = view.Document.Create.NewDetailCurve(view, line1);
            DetailCurve detailCurve2 = view.Document.Create.NewDetailCurve(view, line2);
            DetailCurve detailCurve3 = view.Document.Create.NewDetailCurve(view, line3);
            DetailCurve detailCurve4 = view.Document.Create.NewDetailCurve(view, line4);

            //加入集合
            detailLines.Add(detailCurve1 as DetailLine);
            detailLines.Add(detailCurve2 as DetailLine);
            detailLines.Add(detailCurve3 as DetailLine);
            detailLines.Add(detailCurve4 as DetailLine);

            return detailLines;

        }

    }
}

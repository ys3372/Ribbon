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
    public class CreateSectionFromWall : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Autodesk.Revit.DB.View activeView = uiDoc.ActiveView;

            //获取默认剖面类型
            ElementTypeGroup etGroup = ElementTypeGroup.ViewTypeSection;
            ElementId vId = doc.GetDefaultElementTypeId(etGroup);

            //选择构件，忽略了取消的处理
            Reference rf = uiDoc.Selection.PickObject(ObjectType.Element, "选择构件");

            //以墙体为例，但未做类型限制，假设用户选择的就是墙
            Wall wall = doc.GetElement(rf) as Wall;

            //获得墙定位线
            Curve curve = (wall.Location as LocationCurve).Curve;
            XYZ wallDirection = curve.ComputeDerivatives(0.5, true).BasisX.Normalize();

            //视图宽度深度设置
            double length = curve.Length + 500 / 304.8;
            double deep = 2000 / 304.8;

            //获得墙体的BoundingBox，求中心点及总高度（包络框外扩2000）
            BoundingBoxXYZ wallBox = wall.get_BoundingBox(activeView);
            XYZ center = (wallBox.Max + wallBox.Min) / 2;
            double height = wallBox.Max.Z - wallBox.Min.Z + 2000 / 304.8;

            //构造剖面的BoundingBox，设置其最大最小控制点
            BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
            sectionBox.Max = new XYZ(length / 2, height / 2, deep / 2);
            sectionBox.Min = new XYZ(-length / 2, -height / 2, -deep / 2);

            //构造一个坐标转换器，按前面的分析设置XYZ方向的值
            Transform transform = Transform.Identity;
            transform.Origin = center;
            transform.BasisX = -wallDirection;
            transform.BasisY = XYZ.BasisZ;

            //如果有两个方向已经确定，第三个方向一般用向量叉计算，避免出错
            transform.BasisZ = (-wallDirection).CrossProduct(XYZ.BasisZ);
            sectionBox.Transform = transform;


            //新建事物并启动
            Transaction trans = new Transaction(doc, "创建剖面视图实例");
            trans.Start();
            //创建剖面视图
            ViewSection viewSection = ViewSection.CreateSection(doc, vId, sectionBox);

            //结束
            trans.Commit();

            //将当前视图跳转到剖面
            uiDoc.ActiveView = viewSection;
            return Result.Succeeded;



        }
    }
}

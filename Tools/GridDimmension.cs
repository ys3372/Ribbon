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
    public class GridDimmension : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            //选择轴网，用SelectionFilter
            GridSelectionFilter gridFilter = new GridSelectionFilter();
            IList<Reference> rfList = uiDoc.Selection.PickObjects(ObjectType.Element, gridFilter, "选择轴网");
            //选择标注放置点，为避免捕捉干扰，设为无捕捉
            ObjectSnapTypes osType = ObjectSnapTypes.None;
            XYZ pickPoint = uiDoc.Selection.PickPoint(osType, "选择放置点");

            //标注引用集合
            ReferenceArray rfArray = new ReferenceArray();
            foreach (Reference refer in rfList)
            {
                Grid grid = doc.GetElement(refer) as Grid;
                rfArray.Append(refer);
            }

            //任意取一条轴网定位线，拾取点作为垂涎，以此作为尺寸标注线
            Grid tmpGrid = doc.GetElement(rfList.First()) as Grid;
            Line tempLine = tmpGrid.Curve as Line;
            //将直线无限延长，保证放置点能够投影到线上
            tempLine.MakeUnbound();

            //将点Z坐标变成0，因为轴网的定位线在标高0平面上
            pickPoint = new XYZ(pickPoint.X, pickPoint.Y, 0);


            //获得投影点
            XYZ targetPoint = tempLine.Project(pickPoint).XYZPoint;

            //两点确定标准方向向量
            XYZ direction = (targetPoint - pickPoint).Normalize();

            //创建标注线
            Line dimLine = Line.CreateUnbound(pickPoint, direction);

            //标注所在视图
            Autodesk.Revit.DB.View activeView = doc.ActiveView;

            //新建事物并启动
            Transaction trans = new Transaction(doc, "轴网标注");
            trans.Start();

            //创建标注
            Dimension dim = doc.Create.NewDimension(activeView, dimLine, rfArray);

            //提交事务
            trans.Commit();
            return Result.Succeeded;



        }

        public class GridSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                Categories categories = elem.Document.Settings.Categories;
                if (elem.Category.Id == categories.get_Item(BuiltInCategory.OST_Grids).Id)

                {
                    return true;
                }

                else
                    return false;
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return true;
            }
        }
    }
}

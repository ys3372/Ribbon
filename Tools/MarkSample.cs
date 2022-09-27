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
    public class MarkSample : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            ElementId vID = doc.ActiveView.Id;
            FoundationFilter filter = new FoundationFilter();

            //选择基础柱子，此处略去中断选择处理
            List<Reference> rs = uiDoc.Selection.PickObjects(ObjectType.Element, filter).ToList();
            //标记类型
            //包括TM_ADDBY_CATEGORY类别，TM_ADDBY_MULTICATEGORY类别
            //TM_ADDBY_MATERAL材质

            TagMode tMode = TagMode.TM_ADDBY_CATEGORY;
            //标记方向
            TagOrientation tOrn = TagOrientation.Horizontal;
            Transaction trans = new Transaction(doc, "桩标记");
            trans.Start();

            foreach (Reference re in rs)
            {
                //获得桩子的放置点（单桩的中心点）作为桩标记的标注点
                XYZ p = (doc.GetElement(re).Location as LocationPoint).Point;

                //放置标记，false表示没有引线
                IndependentTag tag = IndependentTag.Create(doc, vID, re, true, tMode, tOrn, p);

                //设置标记移动距离为500mm
                double distance = 500 / 304.8;

                //设置标记的阴线类型为自由端点，否则无法将标记端点放到桩的中心
                tag.LeaderEndCondition = LeaderEndCondition.Free;

                //设置引线末端（标注点位置）
                tag.LeaderEnd = p;

                //设置引线转折点位置，为右上方45度
                tag.LeaderElbow = p + new XYZ(1, 1, 0) * distance;

                //设置引线文字位置，为转折点水平右侧2个distance
                tag.TagHeadPosition = p + new XYZ(3, 1, 0) * distance;
            }

            trans.Commit();
            return Result.Succeeded;
        }

        public class FoundationFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                //通过Category的ID是否等于结构基础BuiltInCategory的ID判断
                Categories categories = elem.Document.Settings.Categories;
                if (elem is FamilyInstance && elem.Category.Id == categories.get_Item(BuiltInCategory.OST_StructuralFoundation).Id)
                {
                    return true;
                }
                return false;
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return true;
            }

        }
    }
}

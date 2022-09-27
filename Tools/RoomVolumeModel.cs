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
    public class RoomVolumeModel : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            //房间选择过滤器，然后用户选择
            RoomSelectionFilter roomFilter = new RoomSelectionFilter();
            List<Reference> refers = uiDoc.Selection.PickObjects(ObjectType.Element, roomFilter, "选择房间").ToList();

            //遍历房间处理
            foreach (Reference refer in refers)
            {
                Room room = doc.GetElement(refer) as Room;

                //获取房间边界线
                CurveLoop curveLoop = GetRoomCurveLoop(room);
                //规避无规则房间
                if (curveLoop.Count() == 0)
                    continue;
                //获取房间高度
                double height = room.UnboundedHeight;

                //新建事物并启动
                Transaction trans = new Transaction(doc, "生成体量");
                trans.Start();

                //生成体量
                DirectShape ds = GetDirectShape(doc, curveLoop, height);

                //提交事务
                trans.Commit();
            }
            return Result.Succeeded;

        }
        public class RoomSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                Categories categories = elem.Document.Settings.Categories;
                if (elem.Category.Id == categories.get_Item(BuiltInCategory.OST_Rooms).Id)
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

        public CurveLoop GetRoomCurveLoop(Room room)
        {
            //存储房间轮廓
            CurveLoop curveLoop = new CurveLoop();
            SpatialElementBoundaryOptions opts = new SpatialElementBoundaryOptions();

            //收集房间所有区域边界，先规避无规则房间
            if (room.GetBoundarySegments(opts) == null)
                return curveLoop;
            IList<IList<BoundarySegment>> blist = room.GetBoundarySegments(opts);


            //提取第一个房间边界
            IList<BoundarySegment> flist = blist.First();
            //存储房间边界
            foreach (BoundarySegment bs in flist)
            {
                curveLoop.Append(bs.GetCurve());
            }
            return curveLoop;
        }

        public DirectShape GetDirectShape(Document doc, CurveLoop cl, double h)
        {
            //创建体量的轮廓
            List<CurveLoop> cloops = new List<CurveLoop>();
            cloops.Add(cl);

            //体量拉伸方向
            XYZ dir = XYZ.BasisZ;


            //设置材质，为简化代码取默认第一个
            ElementId mtId = (new FilteredElementCollector(doc)).OfClass(typeof(Material)).FirstElementId();

            //设置图形样式，为简化代码取默认第一个
            ElementId gsId = (new FilteredElementCollector(doc)).OfClass(typeof(GraphicsStyle)).FirstElementId();

            //设置Solid选项，输入材质
            SolidOptions options = new SolidOptions(mtId, gsId);
            //生成带材质的Solid
            Solid solid = GeometryCreationUtilities.CreateExtrusionGeometry(cloops, dir, h, options);

            //之前的做法：根据轮廓线、方向和高度生产几何图形  
            //Solid solid = GeometryCreationUtilities.CreateExtrusionGeometry(cloops, dir, h);

            //获得体量类型
            BuiltInCategory biCate = BuiltInCategory.OST_Mass;

            //创建一个内建模型，类型为体量
            DirectShape ds = DirectShape.CreateElement(doc, new ElementId(biCate));

            //内建模型中添加几何模型Solid
            if (ds != null)
            {
                ds.AppendShape(new List<GeometryObject>() { solid });
            }

            return ds;
        }
    }
}

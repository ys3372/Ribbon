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
    public class test : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Autodesk.Revit.DB.View activeView = uiDoc.ActiveView;
            ICollection<ElementId> elemIds = uiDoc.Selection.GetElementIds();//先写好之后用
            List<Element> elems = new List<Element>();
            //Element elem;
            Group group;

            //构造一个BoundingBox作为范围框
            BoundingBoxXYZ boundingBox = new BoundingBoxXYZ();

            //新建事物并启动；
            Transaction trans = new Transaction(doc, "创建三维视图");
            trans.Start();

            if (elemIds.Count > 0)
            {
                group = doc.Create.NewGroup(elemIds);
                boundingBox = group.get_BoundingBox(uiDoc.ActiveView);
                group.UngroupMembers();
            }

            else if (elemIds.Count == 0)
            {
                IList<Reference> refers = new List<Reference>();
                try
                {
                    refers = uiDoc.Selection.PickObjects(ObjectType.Element);
                    //elem = doc.GetElement(refers) as Element;
                    foreach (Reference refer in refers)
                    {
                        elemIds.Add(refer.ElementId);
                    }
                    group = doc.Create.NewGroup(elemIds);
                    boundingBox = group.get_BoundingBox(uiDoc.ActiveView);
                    group.UngroupMembers();

                }
                catch
                {
                    return Result.Succeeded;
                }
            }
            //获取默认三维视图
            ElementTypeGroup typeGroup = ElementTypeGroup.ViewType3D;
            ElementId vId = doc.GetDefaultElementTypeId(typeGroup);
            //创建轴测图 

            //创建轴测图 
            View3D view3D = View3D.CreateIsometric(doc, vId);

            //设置剖面框范围
            view3D.SetSectionBox(boundingBox);
            trans.Commit();

            //将当前视图跳转到该平面
            uiDoc.ActiveView = view3D;
            return Result.Succeeded;
        }
    }
}


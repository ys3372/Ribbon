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


namespace Ribbon
{
    [Transaction(TransactionMode.Manual)]
    public class DoorWindowFilter : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            #region
            FilteredElementCollector levelCollector = new FilteredElementCollector(doc);
            List<ElementId> levelIds = levelCollector.OfClass(typeof(Level)).ToElementIds().ToList();

            ElementLevelFilter levelFilter = new ElementLevelFilter(levelIds[0]);
            #endregion
            FilteredElementCollector col = new FilteredElementCollector(doc);

            //过滤窗户
            BuiltInCategory wCategory = BuiltInCategory.OST_Windows;
            ElementCategoryFilter wFilter = new ElementCategoryFilter(wCategory);

            //过滤门
            BuiltInCategory dCategory = BuiltInCategory.OST_Doors;
            ElementCategoryFilter dFilter = new ElementCategoryFilter(dCategory);

            //找出窗加门
            LogicalOrFilter orFilter = new LogicalOrFilter(wFilter, dFilter);

            //过滤类型，他们的类型叫FamilyInstance
            ElementClassFilter fFilter = new ElementClassFilter(typeof(FamilyInstance));
            LogicalAndFilter andFilter = new LogicalAndFilter(orFilter, fFilter);

            //设置过滤器
            col.WherePasses(andFilter);

            //结果集合
            List<ElementId> elemIds = col.ToElementIds().ToList();

            //选中的高亮
            uiDoc.Selection.SetElementIds(elemIds);
            return Result.Succeeded;




            #region
            FilteredElementCollector wallCollector = new FilteredElementCollector(doc);
            wallCollector.OfClass(typeof(Wall)).WherePasses(levelFilter);

            List<ElementId> wallElemIds = wallElemIds = wallCollector.ToElementIds().ToList();
            #endregion

            uiDoc.Selection.SetElementIds(wallElemIds);
            return Result.Succeeded;


        }
    }
}
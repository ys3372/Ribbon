using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Electrical;


namespace Ribbon
{
    [Transaction(TransactionMode.Manual)]
    public class WallFilter : IExternalCommand
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

            #region
            FilteredElementCollector wallCollector = new FilteredElementCollector(doc);
            wallCollector.OfClass(typeof(Wall)).WherePasses(levelFilter);

            List<ElementId> wallElemIds = wallElemIds = wallCollector.ToElementIds().ToList();
            #endregion

            uiDoc.Selection.SetElementIds(wallElemIds);
            return Result.Succeeded;


            /*
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            #region 选择第一个标高作为过滤条件
            FilteredElementCollector levelCollector = new FilteredElementCollector(doc);
            
            ElementClassFilter levelClassFilter = new ElementClassFilter(typeof(Level));

            levelCollector.WherePasses(levelClassFilter);

            List<ElementId> levelIDs = levelCollector.OfClass(typeof(Level)).ToElementIds().ToList();

            ElementLevelFilter levelFilter = new ElementLevelFilter(levelIDs[0]);
            #endregion



            #region 通过类型及楼层过滤得到目标墙体

            FilteredElementCollector wallCollector = new FilteredElementCollector(doc);

            ElementClassFilter wallClassFilter = new ElementClassFilter(typeof(Wall));

            wallCollector.WherePasses(wallClassFilter).WherePasses(levelFilter);

            List<ElementId> wallElemIds = wallCollector.ToElementIds().ToList();
            #endregion



            uiDoc.Selection.SetElementIds(wallElemIds);
            return Result.Succeeded;
            */
        }
    }
}

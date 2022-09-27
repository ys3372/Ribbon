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
    public class CreateWall : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            ElementId wtId = doc.GetDefaultElementTypeId(ElementTypeGroup.WallType);
            Line wCurve = Line.CreateBound(new XYZ(), new XYZ(6000 / 304.8, 1000 / 304.8, 0));

            double height = 3000 / 304.8;

            double offset = 100 / 304.8;

            bool flip = false;

            ElementId levelId = doc.ActiveView.GenLevel.Id;

            Transaction trans = new Transaction(doc, "创建墙体"); trans.Start();

            Wall wall = Wall.Create(doc, wCurve, wtId, levelId, height, offset, flip, true);
            trans.Commit();

            return Result.Succeeded;

        }
    }
}

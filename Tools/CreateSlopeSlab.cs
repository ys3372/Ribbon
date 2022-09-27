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
    public class CreateSlopeSlab : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            double len = 8000 / 304.8;

            Line line1 = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(0, len, 0));
            Line line2 = Line.CreateBound(new XYZ(0, len, 0), new XYZ(len, len, 0));
            Line line3 = Line.CreateBound(new XYZ(len, len, 0), new XYZ(len, 0, 0));
            Line line4 = Line.CreateBound(new XYZ(len, 0, 0), new XYZ());

            CurveArray curveArray = new CurveArray();
            curveArray.Append(line1);
            curveArray.Append(line2);
            curveArray.Append(line3);
            curveArray.Append(line4);

            Level level = doc.ActiveView.GenLevel;
            Line sLine = Line.CreateBound(new XYZ(0, len, 0), new XYZ(0, 0, 0));
            double angle = 15 * Math.PI / 180;
            double slope = Math.Tan(angle);

            Transaction trans = new Transaction(doc, "创建楼板");
            trans.Start();

            Floor slopeSlab = doc.Create.NewSlab(curveArray, level, sLine, slope, true);

            trans.Commit();
            return Result.Succeeded;


        }
    }
}

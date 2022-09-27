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
    public class PickBox : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            try
            {
                PickedBox pickedBox = uiDoc.Selection.PickBox(PickBoxStyle.Enclosing);
                XYZ pick1 = pickedBox.Min;
                XYZ pick2 = pickedBox.Max;

                MessageBox.Show("起点" + pick1 + "终点：" + pick2);
                return Result.Succeeded;
            }

            catch
            {
                return Result.Cancelled;
            }
        }
    }
}

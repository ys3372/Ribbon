#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace Ribbon
{
    [Transaction(TransactionMode.Manual)]
    public class TextDetection : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            //TaskDialog.Show("test","hello world!");

            FilteredElementCollector TextNoteCollector = new FilteredElementCollector(doc);
            TextNoteCollector.OfCategory(BuiltInCategory.OST_TextNotes);
            TextNoteCollector.WhereElementIsNotElementType();

            TaskDialog.Show("test", TextNoteCollector.GetElementCount().ToString() + " text notes found.");

            Transaction curTrans = new Transaction(doc, "中联数字");
            curTrans.Start();

            foreach (TextNote curNote in TextNoteCollector)
            {
                curNote.Text = curNote.Text.ToUpper();

            }

            curTrans.Commit();
            curTrans.Dispose();

            return Result.Succeeded;
        }
    }
}

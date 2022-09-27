using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Reflection;

namespace Ribbon
{
    [Transaction(TransactionMode.Manual)]
    public class ActiveViewManager: IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string str, ElementSet elements)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                var View = doc.ActiveView;
                var vName = View.Name;
                var vId = View.Id;
                var vTemplateId = View.ViewTemplateId;
                var vTemplate = vTemplateId.ToString() == "-1" ? "None" : doc.GetElement(vTemplateId).Name;

                MainWindow window = new MainWindow(uiDoc);
                window.label_Name.Content = $"Name:{vName}";
                window.label_Id.Content = $"Id: {vId}";
                window.label_Template.Content = $"View template: {vTemplate}";
                window.ShowDialog();

                return Result.Succeeded; 
            }
            catch(Exception ex)
            {
               str = ex.Message;
                return Result.Failed;
            }
        }
    }
}

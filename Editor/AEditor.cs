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
using System.Security.Cryptography;

namespace Ribbon
{
    [Transaction(TransactionMode.Manual)]
    public class AEditor : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string str, ElementSet elements)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {

                Editor window = new Editor(uiDoc);
                window.ShowDialog();

                return Result.Succeeded;
            }
            catch (Exception ex)
            { 
                str = ex.Message;
                return Result.Failed;
            }
        }
    }
}
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
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
    public class CW_Ceshi : IExternalCommand
    {
        UIDocument uiDoc = null;
        Document doc = null;
        Application application = null;

        public Result Execute(ExternalCommandData cD, ref string message, ElementSet elements)
        {
            UIApplication uiApp = cD.Application;
            Application application = uiApp.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            using (Transaction t = new Transaction(doc, "tran"))
            {
                t.Start();
                BreakMEPCurvew();
                t.Commit();
            }
            //创建窗体对象
            //Ribbon.Form.BreakMEPCurveForm breakMEPCurveForm = new Ribbon.Form.BreakMEPCurveForm(this);
            ////throw new NotImplementedException();
            //breakMEPCurveForm.Show();
            return Result.Succeeded;
        }


        public void BreakMEPCurvew()
        {
            MEPCurveSelectionFilter filter = new MEPCurveSelectionFilter();
            Reference refer = uiDoc.Selection.PickObject(ObjectType.Element, filter, "");
            MEPCurve mEPCurve = doc.GetElement(refer) as MEPCurve;

            XYZ breakXYZ = uiDoc.Selection.PickPoint();
            ICollection<ElementId> ids = ElementTransformUtils.CopyElement(doc, mEPCurve.Id, new XYZ(0, 0, 0));
            ElementId newId = ids.FirstOrDefault();
            MEPCurve mEPCurveCopy = doc.GetElement(newId) as MEPCurve;


            Curve curve = (mEPCurve.Location as LocationCurve).Curve;
            XYZ startXYZ = curve.GetEndPoint(0);
            XYZ endXYZ = curve.GetEndPoint(1);

            //映射点
            breakXYZ = curve.Project(breakXYZ).XYZPoint;

            Line line1 = Line.CreateBound(startXYZ, breakXYZ);
            Line line2 = Line.CreateBound(breakXYZ, endXYZ);

            (mEPCurve.Location as LocationCurve).Curve = line1;
            (mEPCurveCopy.Location as LocationCurve).Curve = line2;

        }
        public class MEPCurveSelectionFilter : ISelectionFilter
        {

            public bool AllowElement(Element elem)
            {

                if (elem is MEPCurve && !(elem is InsulationLiningBase))
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
    }

}

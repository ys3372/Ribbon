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
    public class FloorAreaCalculation : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            FloorSelectionFilter floorFilter = new FloorSelectionFilter();

            IList<Reference> refers = uiDoc.Selection.PickObjects(ObjectType.Element, floorFilter, "选择楼板");
            double area = 0;
            try
            {

                foreach (Reference refer in refers)
                {
                    Floor floor = doc.GetElement(refer) as Floor;
                    area += floor.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();
                }

                double cvtAreas = UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS);

                MessageBox.Show("所选楼板共计：" + Math.Round(cvtAreas, 2) + "m\x00B2");

                return Result.Succeeded;

            }

            catch
            {
                return Result.Cancelled;
            }

        }
    }

    public class FloorSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            Categories categories = elem.Document.Settings.Categories;
            if (elem.Category.Id == categories.get_Item(BuiltInCategory.OST_Floors).Id)

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

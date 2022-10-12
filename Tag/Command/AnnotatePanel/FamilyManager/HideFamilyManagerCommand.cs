using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;

namespace Ribbon.Tag
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class HideFamilyManagerCommand:IExternalCommand
    {
        #region public methods
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {

            var dpid = new DockablePaneId(PaneIdentifiers.GetManagePaneIdentifier());
            var dp = cD.Application.GetDockablePane(dpid);
            dp.Hide();
            //TaskDialog.Show("info", "test hide...");

            return Result.Succeeded;
        }

        public static string GetPath()
        {
            return typeof(HideFamilyManagerCommand).Namespace + "." + nameof(HideFamilyManagerCommand);
        }
        #endregion
    }
}

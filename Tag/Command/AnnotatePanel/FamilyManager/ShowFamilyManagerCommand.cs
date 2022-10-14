using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;


namespace Ribbon.Tag
{
    /// <summary>
    /// Show family manager dockable pane
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ShowFamilyManagerCommand : IExternalCommand
    {
        #region public methods
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            //TaskDialog.Show("info", "test show...");
            var dpid = new DockablePaneId(PaneIdentifiers.GetManagePaneIdentifier());
            var dp = cD.Application.GetDockablePane(dpid);
            dp.Show();

            return Result.Succeeded;
        }

        public static string GetPath()
        {
            return typeof(ShowFamilyManagerCommand).Namespace + "." + nameof(ShowFamilyManagerCommand);
        }
        #endregion
    }
}

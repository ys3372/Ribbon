using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.IO;
using TSZ.RevitBaseDll.EntityData;
using TSZ.RevitBaseDll.Extends;


public class CreateColumn : ExternalCommand
{
    
    public override Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements)
    {

        //if (base.Execute(commandData, ref message, elements) == Result.Cancelled)
            //return Result.Cancelled;
        try
        {
            if (this.RvtWrapper.ActiveViewExt.ViewType == ViewType.Elevation && !this.RvtWrapper.Doc.GoToNewPlanView())
                return Result.Cancelled;
            this.RvtWrapper.UiApp.PostCommandExt(PostableCommand.StructuralColumn);
            //RvtWrapper是ExternalRvtWrapper类型的新建；ExternalRvtWrapper里有这句this.UiApp = commandData.Application;
            //public static void PostCommandExt(this UIApplication uiApp, PostableCommand command) => uiApp.PostCommand(RevitCommandId.LookupPostableCommandId(command));
            //PostCommand是一个读取命令代号的功能，PostCommandExt是代号,34974
            //commandData.Application.PostCommandExt(34974);



            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            //int num = (int)MessageShow.Show(ex, "", true);
        }
        return Result.Succeeded;
    }
}

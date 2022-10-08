
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Ribbon.Tag.Command.Helper.Type;
using Ribbon.Tag.Command.Selection;
using Ribbon.Tag.Utility;
using System;
using System.Text;
using System.Windows.Forms;

namespace Ribbon.Tag
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class TagWallLayersCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet elements)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            if (doc.IsFamilyDocument)
            {
                Command.Helper.Message.Display("Can't use command in family content", Command.Helper.Type.WindowType.Warning);
                return Result.Cancelled;
            }

            //Get access to current view
            var activeView = uiDoc.ActiveView;

            //Check if TextNote can be created in currently active project view
            bool canCreateTextNoteInView = false;
            switch (activeView.ViewType)
            {
                case ViewType.FloorPlan:
                    canCreateTextNoteInView = true;
                    break;
                case ViewType.CeilingPlan:
                    canCreateTextNoteInView = true;
                    break;
                case ViewType.Detail:
                    canCreateTextNoteInView = true;
                    break ;
                case ViewType.Elevation:
                    canCreateTextNoteInView = true;
                    break;
                case ViewType.Section:
                    canCreateTextNoteInView = true;
                    break;

            }
            var userInfo = new TagWallLayersCommandData();

            if (!canCreateTextNoteInView)
            {
                Command.Helper.Message.Display("Text note element cant be created in the current view.", Command.Helper.Type.WindowType.Error);
                return Result.Cancelled;
            }
            using (var window = new Command.TagWallLayersForm(uiDoc))
            {
                window.ShowDialog();
                if (window.DialogResult == DialogResult.Cancel)
                    return Result.Cancelled;

                userInfo = window.GetInformation();
            }

            //select a basic wall
            try
            {
                var selectionReference = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, new SelectionFilterByCategory("墙"), "Select a basic wall");
                var selectionElement = doc.GetElement(selectionReference);

                var wall = selectionElement as Wall;

                if (wall.IsStackedWall)
                {
                    Command.Helper.Message.Display("Wall you selected is category of the Stacked Wall.\n It's not supported by this command.", WindowType.Warning);
                    return Result.Cancelled;
                }

                var layers = wall.WallType.GetCompoundStructure().GetLayers();
                var msg = new StringBuilder();
                foreach (var layer in layers)
                {
                    var material = doc.GetElement(layer.MaterialId) as Material;

                    //msg.AppendLine(layer.Function.ToString() + "" + material.Name + "" + layer.Width.ToString());
                    msg.AppendLine();

                    if(userInfo.Function)
                        msg.Append(layer.Function.ToString());

                    if (userInfo.Name)
                        if (material.Name != null)
                            msg.Append("" + material.Name);
                        else
                            msg.Append("    <by category>");

                    if (userInfo.Thickness)
                        msg.Append("" + LengthUnitConverter.ConvertToMetric(layer.Width, userInfo.UnitType, userInfo.Decimals).ToString());
                }

                var textNoteOptions = new TextNoteOptions
                {
                    VerticalAlignment = VerticalTextAlignment.Top,
                    HorizontalAlignment = HorizontalTextAlignment.Center,
                    TypeId = userInfo.TextTypeId
                };

                using (Transaction trans = new Transaction(doc))
                { 
                    trans.Start("Tag Wall Layers");

                    var pt = new XYZ();
                    if(activeView.ViewType == ViewType.Elevation || activeView.ViewType == ViewType.Section)
                    {
                        var plane = Plane.CreateByNormalAndOrigin(activeView.ViewDirection, activeView.Origin);
                        var sketchPlane = SketchPlane.Create(doc, plane);
                        activeView.SketchPlane = sketchPlane;

                        pt = uiDoc.Selection.PickPoint("Pick One Point");
                    }
                    else
                    {
                        pt = uiDoc.Selection.PickPoint("Pick One Point");
                    }
                    //var pt = uiDoc.Selection.PickPoint("Pick text note location point");
                    var textnote = TextNote.Create(doc, activeView.Id, pt, msg.ToString(), textNoteOptions);
                
                
                    trans.Commit();
                }
            }

            catch (Exception ex)
            {
                ms = ex.Message;
                return Result.Failed;
            }


            return Result.Succeeded;
        }
    }
}


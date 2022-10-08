using Autodesk.Revit.UI;
using Ribbon.Tag.Command.Helper.Type;
using System;

namespace Ribbon.Tag.Command.Helper
{
    public static class Message
    {
        #region
        public static void Display(string message, WindowType windowType)
        {
            string title = "";
            var icon = TaskDialogIcon.TaskDialogIconNone;//就是没Icon

            switch (windowType)
            {
                case WindowType.Information:
                    title = "~ Information ~";
                    icon = TaskDialogIcon.TaskDialogIconInformation;
                    break;
                case WindowType.Warning:
                    title = "~ Warning ~";
                    icon = TaskDialogIcon.TaskDialogIconWarning;
                    break;
                case WindowType.Error:
                    title = "~ Error ~";
                    icon= TaskDialogIcon.TaskDialogIconError;
                    break;
                default:
                    break;
            }

            var window = new TaskDialog(title)
            {
                MainContent = message,
                MainIcon = icon,
                CommonButtons = TaskDialogCommonButtons.Ok
            };

        }
        #endregion
    }
}

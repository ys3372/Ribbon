using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Ribbon.Tag.Command.Selection
{
    public class SelectionFilterByCategory : ISelectionFilter
    {
        #region private members
        private string mCategory = "";
        #endregion

        #region constructor
        public SelectionFilterByCategory(string category)
        {
            mCategory = category;
        }
        #endregion

        #region public methods

        public bool AllowElement(Element element)
        {
            if (element.Category.Name == mCategory)
            {
                return true;
            }
            return false;
        }



        public bool AllowReference(Reference refer, XYZ xyz)
        {
            return false;
        }
        #endregion
    }
}

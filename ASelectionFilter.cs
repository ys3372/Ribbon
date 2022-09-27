
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using TSZ.RevitBaseDll.Extends;

namespace Ribbon
{
    public class SelectionFilter2 : ISelectionFilter
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


    /// <summary>建筑柱</summary>
    public class ArchColumnSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem) => elem.IsAcColumnExt();

        public bool AllowReference(Reference reference, XYZ pt) => true;

        //public static bool IsAcColumnExt(this Element instance) => instance.Category != null && instance.Category.IsEqualExt(BuiltInCategory.OST_Columns);
        //public static bool IsEqualExt(this Category cat, BuiltInCategory bic) => cat != null && cat.GetBuiltInCategoryExt() == bic;//类型cat不为null，且内置类型和输入（上一句）的一致
        //public static BuiltInCategory GetBuiltInCategoryExt(this Category cat) => cat.Id.GetBuiltInCategoryExt();

        
    }

    /// <summary>建筑墙 ljy</summary>
    public class ArchWallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem) => elem.IsWallExt() && !elem.ToWallExt().IsStructExt();

        //public static bool IsWallExt(this Autodesk.Revit.DB.Element elem) => elem is Wall; //返回一个是不是墙的bool判断
        //public static Wall ToWallExt(this Autodesk.Revit.DB.Element elem) => elem as Wall;//总之是要转化为墙
        //wall.GetParameterInteger(BuiltInParameter.WALL_STRUCTURAL_SIGNIFICANT) == 1 //返回一个结构墙的bool判断，去掉结构墙筛选建筑墙

    public bool AllowReference(Reference reference, XYZ position) => true;
    }

    /// <summary>选择柱的垂直面</summary>
    public class AllColumnVerticalFaceSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem) => elem.IsAcColumnExt() || elem.IsStColumnExt();

        public bool AllowReference(Reference reference, XYZ pt)
        {
            Face objectFromReference = ExternalDataWrapper.Current.Doc.GetElement(reference).GetGeometryObjectFromReference(reference) as Face;
            return (GeometryObject)objectFromReference != (GeometryObject)null && !objectFromReference.GetPlaneExt().Normal.IsEqualExt(XYZ.BasisZ) && !objectFromReference.GetPlaneExt().Normal.IsEqualExt(-XYZ.BasisZ);
        }
    }


    /// <summary>MEP选择器</summary>
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


    public class ColumnSelectionFilter : ISelectionFilter
    {

        public bool AllowElement(Element elem)
        {
            Categories categories = elem.Document.Settings.Categories;
            if (elem is FamilyInstance && (elem.Category.Id == categories.get_Item(BuiltInCategory.OST_StructuralColumns).Id || elem.Category.Id == categories.get_Item(BuiltInCategory.OST_Columns).Id))
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


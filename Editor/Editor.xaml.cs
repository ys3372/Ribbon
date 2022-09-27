using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TSZ.RevitBaseDll.Extends;

namespace Ribbon
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {
        public UIDocument uiDoc { get; }
        public Autodesk.Revit.DB.Document doc { get; }

        CW_ColumnRevise columnRevise = null;
        public Editor(CW_ColumnRevise columnRevise)
        {
            this.columnRevise = columnRevise;
            InitializeComponent();
        }

     
        public Editor(UIDocument UiDoc)
        {
            uiDoc = UiDoc;
            doc = UiDoc.Document;
            InitializeComponent();
            Title = "中国联合";
            //ColumnSelectionFilter filter = new ColumnSelectionFilter();
            //Reference refer = uiDoc.Selection.PickObject(ObjectType.Element, filter, "选择柱体");
            //Editor window = new Editor(uiDoc);
            //Element elem = doc.GetElement(refer) as Element;
        }
        private void SetParameter (object sender, RoutedEventArgs e)
        {
            Transaction trans = new Transaction(doc, "设置参数");
            trans.Start();
            {
                string para = Para.Text;
                ColumnSelectionFilter filter = new ColumnSelectionFilter();
                Reference refer = uiDoc.Selection.PickObject(ObjectType.Element, filter, "选择柱体");
                Editor window = new Editor(uiDoc);
                FamilyInstance fi = doc.GetElement(refer) as FamilyInstance;
                fi.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).Set(Convert.ToDouble(para)/304.8);

            }
            trans.Commit();
        }


        //#region 这两个用来研究的，不调用
        //public static void SetTopLevelExt(this Autodesk.Revit.DB.FamilyInstance instance, Autodesk.Revit.DB.ElementId levelId)
        //{
        //    instance.SetParameterExt(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM, levelId);
        //}
        //public static bool SetParameterExt(this Autodesk.Revit.DB.Element elem, BuiltInParameter index, Autodesk.Revit.DB.ElementId value)
        //{
        //    Parameter parameterExt = elem.GetParameterExt(index);
        //    if (parameterExt == null)
        //    {
        //        return false;
        //    }

        //    if (parameterExt.IsReadOnly)
        //    {
        //        return false;
        //    }

        //    StorageType storageType = parameterExt.StorageType;
        //    if (StorageType.ElementId != storageType)
        //    {
        //        return false;
        //    }

        //    return elem.get_Parameter(index).Set(value);
        //}
        //#endregion




    }
}

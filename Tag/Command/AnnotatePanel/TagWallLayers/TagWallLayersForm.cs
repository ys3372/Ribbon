using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;


namespace Ribbon.Tag.Command
{
    public partial class TagWallLayersForm : System.Windows.Forms.Form
    {
        #region private members
        private UIDocument uiDoc = null;

        private ElementId textTypeId = null;

        private LengthUnitType unitType = LengthUnitType.milimeter;

        private int decimals = 1;

        #endregion


        #region constructor
        public TagWallLayersForm(UIDocument uIDocument)
        {
            InitializeComponent();
            uiDoc = uIDocument;
        }

        #endregion

        #region events
        private void btnOK_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void TagWallLayersForm_Load(object sender, System.EventArgs e)
        {
            PopulateTextNoteTypeList();
            PopulateUnitTypeList();
            PopulateDecimalPlacesList();
        }

        private void cmbTextNoteElementType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            textTypeId = ((KeyValuePair<string, ElementId>)cmbTextNoteElementType.SelectedItem).Value;
        }

        private void cmbUnitType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            unitType = (LengthUnitType)cmbUnitType.SelectedValue;
        }

        private void cmbDecimalPlaces_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            decimals = (int)cmbDecimalPlaces.SelectedValue;
        }

        #endregion

        #region public methods
        public TagWallLayersCommandData GetInformation()
        {
            var information = new TagWallLayersCommandData()
            {
                Function = chkFunction.Checked,
                Name = chkName.Checked,
                Thickness = chkThickness.Checked,
                TextTypeId = textTypeId,
                UnitType = unitType,
                Decimals = decimals
            };
            return information;
        }
        #endregion

        #region private methods
        private void PopulateTextNoteTypeList()
        {
            var doc = uiDoc.Document;

            var allTextElementTypes = new FilteredElementCollector(doc).OfClass(typeof(TextElementType));

            var list = new List<KeyValuePair<string, ElementId>>();

            foreach(var item in allTextElementTypes)
                list.Add(new KeyValuePair<string, ElementId>(item.Name, item.Id));

            cmbTextNoteElementType.DataSource = null;
            cmbTextNoteElementType.DataSource = new BindingSource(list, null);
            cmbTextNoteElementType.DisplayMember = "Key";
            cmbTextNoteElementType.ValueMember = "Value";
        }

        private void PopulateUnitTypeList()
        {
            cmbUnitType.DataSource = Enum.GetValues(typeof(LengthUnitType));
        }

        private void PopulateDecimalPlacesList()
        {
            var values = new List<int>() { 0, 1, 2, 3 };

            var source = new BindingSource
            {
                DataSource = values,
            };

            cmbDecimalPlaces.DataSource = source.DataSource;
            cmbDecimalPlaces.SelectedItem = values[2];
        }


        #endregion


    }
}

namespace Ribbon.Tag.Command
{
    partial class TagWallLayersForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagWallLayersForm));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.TagProperties = new System.Windows.Forms.GroupBox();
            this.chkThickness = new System.Windows.Forms.CheckBox();
            this.chkName = new System.Windows.Forms.CheckBox();
            this.chkFunction = new System.Windows.Forms.CheckBox();
            this.cmbTextNoteElementType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbUnitType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDecimalPlaces = new System.Windows.Forms.ComboBox();
            this.TagProperties.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(108, 270);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(56, 22);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(170, 270);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 22);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // TagProperties
            // 
            this.TagProperties.Controls.Add(this.chkThickness);
            this.TagProperties.Controls.Add(this.chkName);
            this.TagProperties.Controls.Add(this.chkFunction);
            this.TagProperties.Location = new System.Drawing.Point(12, 14);
            this.TagProperties.Name = "TagProperties";
            this.TagProperties.Size = new System.Drawing.Size(214, 94);
            this.TagProperties.TabIndex = 2;
            this.TagProperties.TabStop = false;
            this.TagProperties.Text = "Properties";
            // 
            // chkThickness
            // 
            this.chkThickness.AutoSize = true;
            this.chkThickness.Checked = true;
            this.chkThickness.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkThickness.Location = new System.Drawing.Point(8, 63);
            this.chkThickness.Name = "chkThickness";
            this.chkThickness.Size = new System.Drawing.Size(78, 16);
            this.chkThickness.TabIndex = 2;
            this.chkThickness.Text = "Thickness";
            this.chkThickness.UseVisualStyleBackColor = true;
            // 
            // chkName
            // 
            this.chkName.AutoSize = true;
            this.chkName.Checked = true;
            this.chkName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkName.Location = new System.Drawing.Point(8, 41);
            this.chkName.Name = "chkName";
            this.chkName.Size = new System.Drawing.Size(48, 16);
            this.chkName.TabIndex = 1;
            this.chkName.Text = "Name";
            this.chkName.UseVisualStyleBackColor = true;
            // 
            // chkFunction
            // 
            this.chkFunction.AutoSize = true;
            this.chkFunction.Checked = true;
            this.chkFunction.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFunction.Location = new System.Drawing.Point(8, 19);
            this.chkFunction.Name = "chkFunction";
            this.chkFunction.Size = new System.Drawing.Size(72, 16);
            this.chkFunction.TabIndex = 0;
            this.chkFunction.Text = "Function";
            this.chkFunction.UseVisualStyleBackColor = true;
            // 
            // cmbTextNoteElementType
            // 
            this.cmbTextNoteElementType.FormattingEnabled = true;
            this.cmbTextNoteElementType.Location = new System.Drawing.Point(20, 121);
            this.cmbTextNoteElementType.Name = "cmbTextNoteElementType";
            this.cmbTextNoteElementType.Size = new System.Drawing.Size(189, 20);
            this.cmbTextNoteElementType.TabIndex = 3;
            this.cmbTextNoteElementType.SelectedIndexChanged += new System.EventHandler(this.cmbTextNoteElementType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Text Type";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbDecimalPlaces);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbUnitType);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 148);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 116);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Units";
            // 
            // cmbUnitType
            // 
            this.cmbUnitType.FormattingEnabled = true;
            this.cmbUnitType.Location = new System.Drawing.Point(8, 32);
            this.cmbUnitType.Name = "cmbUnitType";
            this.cmbUnitType.Size = new System.Drawing.Size(189, 20);
            this.cmbUnitType.TabIndex = 3;
            this.cmbUnitType.SelectedIndexChanged += new System.EventHandler(this.cmbUnitType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "UnitType";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Decimal Places";
            // 
            // cmbDecimalPlaces
            // 
            this.cmbDecimalPlaces.FormattingEnabled = true;
            this.cmbDecimalPlaces.Location = new System.Drawing.Point(8, 74);
            this.cmbDecimalPlaces.Name = "cmbDecimalPlaces";
            this.cmbDecimalPlaces.Size = new System.Drawing.Size(189, 20);
            this.cmbDecimalPlaces.TabIndex = 3;
            this.cmbDecimalPlaces.SelectedIndexChanged += new System.EventHandler(this.cmbDecimalPlaces_SelectedIndexChanged);
            // 
            // TagWallLayersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 305);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TagProperties);
            this.Controls.Add(this.cmbTextNoteElementType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(255, 344);
            this.MinimizeBox = false;
            this.Name = "TagWallLayersForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.TagWallLayersForm_Load);
            this.TagProperties.ResumeLayout(false);
            this.TagProperties.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox TagProperties;
        private System.Windows.Forms.CheckBox chkThickness;
        private System.Windows.Forms.CheckBox chkName;
        private System.Windows.Forms.CheckBox chkFunction;
        private System.Windows.Forms.ComboBox cmbTextNoteElementType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbDecimalPlaces;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbUnitType;
        private System.Windows.Forms.Label label2;
    }
}
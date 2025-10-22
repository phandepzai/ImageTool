// MainForm.Designer.cs
using System.Windows.Forms;

namespace CircularImageGenerator
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.BtnSelectImage = new System.Windows.Forms.Button();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.BtnSelectOutput = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numStartAngle = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numEndAngle = new System.Windows.Forms.NumericUpDown();
            this.BtnGenerate = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numStepAngle = new System.Windows.Forms.NumericUpDown();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.labelPreview = new System.Windows.Forms.Label();
            this.timerButtonEffect = new System.Windows.Forms.Timer(this.components);
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabRotator = new System.Windows.Forms.TabPage();
            this.labelPreviewImg = new System.Windows.Forms.Label();
            this.tabConverter = new System.Windows.Forms.TabPage();
            this.labelPixel = new System.Windows.Forms.Label();
            this.labelPreviewICO = new System.Windows.Forms.Label();
            this.picPreviewICO = new System.Windows.Forms.PictureBox();
            this.lblStatusICO = new System.Windows.Forms.Label();
            this.btnConvertICO = new System.Windows.Forms.Button();
            this.txtOutputICO = new System.Windows.Forms.TextBox();
            this.btnSelectOutputICO = new System.Windows.Forms.Button();
            this.txtInputImageICO = new System.Windows.Forms.TextBox();
            this.btnSelectImageICO = new System.Windows.Forms.Button();
            this.cmbIconSize = new System.Windows.Forms.ComboBox();
            this.lblIconSize = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numStartAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStepAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.tabControlMain.SuspendLayout();
            this.tabRotator.SuspendLayout();
            this.tabConverter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreviewICO)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnSelectImage
            // 
            resources.ApplyResources(this.BtnSelectImage, "BtnSelectImage");
            this.BtnSelectImage.Name = "BtnSelectImage";
            this.BtnSelectImage.UseVisualStyleBackColor = true;
            this.BtnSelectImage.Click += new System.EventHandler(this.BtnSelectImage_Click);
            // 
            // txtImagePath
            // 
            resources.ApplyResources(this.txtImagePath, "txtImagePath");
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.ReadOnly = true;
            this.txtImagePath.Click += new System.EventHandler(this.BtnSelectImage_Click);
            // 
            // BtnSelectOutput
            // 
            resources.ApplyResources(this.BtnSelectOutput, "BtnSelectOutput");
            this.BtnSelectOutput.Name = "BtnSelectOutput";
            this.BtnSelectOutput.UseVisualStyleBackColor = true;
            this.BtnSelectOutput.Click += new System.EventHandler(this.BtnSelectOutput_Click);
            // 
            // txtOutputPath
            // 
            resources.ApplyResources(this.txtOutputPath, "txtOutputPath");
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.ReadOnly = true;
            this.txtOutputPath.Click += new System.EventHandler(this.BtnSelectOutput_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // numStartAngle
            // 
            resources.ApplyResources(this.numStartAngle, "numStartAngle");
            this.numStartAngle.Name = "numStartAngle";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // numEndAngle
            // 
            resources.ApplyResources(this.numEndAngle, "numEndAngle");
            this.numEndAngle.Name = "numEndAngle";
            // 
            // BtnGenerate
            // 
            this.BtnGenerate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            resources.ApplyResources(this.BtnGenerate, "BtnGenerate");
            this.BtnGenerate.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnGenerate.Name = "BtnGenerate";
            this.BtnGenerate.UseVisualStyleBackColor = false;
            this.BtnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // labelStatus
            // 
            resources.ApplyResources(this.labelStatus, "labelStatus");
            this.labelStatus.ForeColor = System.Drawing.Color.ForestGreen;
            this.labelStatus.Name = "labelStatus";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // numStepAngle
            // 
            resources.ApplyResources(this.numStepAngle, "numStepAngle");
            this.numStepAngle.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStepAngle.Name = "numStepAngle";
            this.numStepAngle.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // picPreview
            // 
            this.picPreview.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.picPreview, "picPreview");
            this.picPreview.Name = "picPreview";
            this.picPreview.TabStop = false;
            // 
            // labelPreview
            // 
            resources.ApplyResources(this.labelPreview, "labelPreview");
            this.labelPreview.Name = "labelPreview";
            // 
            // timerButtonEffect
            // 
            this.timerButtonEffect.Interval = 200;
            this.timerButtonEffect.Tick += new System.EventHandler(this.TimerButtonEffect_Tick);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabRotator);
            this.tabControlMain.Controls.Add(this.tabConverter);
            resources.ApplyResources(this.tabControlMain, "tabControlMain");
            this.tabControlMain.Multiline = true;
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControlMain.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TabControlMain_DrawItem);
            // 
            // tabRotator
            // 
            this.tabRotator.BackColor = System.Drawing.SystemColors.Control;
            this.tabRotator.Controls.Add(this.labelPreviewImg);
            this.tabRotator.Controls.Add(this.labelPreview);
            this.tabRotator.Controls.Add(this.picPreview);
            this.tabRotator.Controls.Add(this.labelStatus);
            this.tabRotator.Controls.Add(this.BtnGenerate);
            this.tabRotator.Controls.Add(this.numEndAngle);
            this.tabRotator.Controls.Add(this.label2);
            this.tabRotator.Controls.Add(this.numStartAngle);
            this.tabRotator.Controls.Add(this.label1);
            this.tabRotator.Controls.Add(this.txtOutputPath);
            this.tabRotator.Controls.Add(this.BtnSelectOutput);
            this.tabRotator.Controls.Add(this.txtImagePath);
            this.tabRotator.Controls.Add(this.BtnSelectImage);
            this.tabRotator.Controls.Add(this.numStepAngle);
            this.tabRotator.Controls.Add(this.label3);
            resources.ApplyResources(this.tabRotator, "tabRotator");
            this.tabRotator.Name = "tabRotator";
            // 
            // labelPreviewImg
            // 
            resources.ApplyResources(this.labelPreviewImg, "labelPreviewImg");
            this.labelPreviewImg.Name = "labelPreviewImg";
            // 
            // tabConverter
            // 
            this.tabConverter.BackColor = System.Drawing.SystemColors.Control;
            this.tabConverter.Controls.Add(this.labelPixel);
            this.tabConverter.Controls.Add(this.labelPreviewICO);
            this.tabConverter.Controls.Add(this.picPreviewICO);
            this.tabConverter.Controls.Add(this.lblStatusICO);
            this.tabConverter.Controls.Add(this.btnConvertICO);
            this.tabConverter.Controls.Add(this.txtOutputICO);
            this.tabConverter.Controls.Add(this.btnSelectOutputICO);
            this.tabConverter.Controls.Add(this.txtInputImageICO);
            this.tabConverter.Controls.Add(this.btnSelectImageICO);
            this.tabConverter.Controls.Add(this.cmbIconSize);
            this.tabConverter.Controls.Add(this.lblIconSize);
            resources.ApplyResources(this.tabConverter, "tabConverter");
            this.tabConverter.Name = "tabConverter";
            // 
            // labelPixel
            // 
            resources.ApplyResources(this.labelPixel, "labelPixel");
            this.labelPixel.Name = "labelPixel";
            // 
            // labelPreviewICO
            // 
            resources.ApplyResources(this.labelPreviewICO, "labelPreviewICO");
            this.labelPreviewICO.Name = "labelPreviewICO";
            // 
            // picPreviewICO
            // 
            this.picPreviewICO.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.picPreviewICO, "picPreviewICO");
            this.picPreviewICO.Name = "picPreviewICO";
            this.picPreviewICO.TabStop = false;
            // 
            // lblStatusICO
            // 
            resources.ApplyResources(this.lblStatusICO, "lblStatusICO");
            this.lblStatusICO.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblStatusICO.Name = "lblStatusICO";
            // 
            // btnConvertICO
            // 
            this.btnConvertICO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            resources.ApplyResources(this.btnConvertICO, "btnConvertICO");
            this.btnConvertICO.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnConvertICO.Name = "btnConvertICO";
            this.btnConvertICO.UseVisualStyleBackColor = false;
            this.btnConvertICO.Click += new System.EventHandler(this.BtnConvertICO_Click);
            // 
            // txtOutputICO
            // 
            resources.ApplyResources(this.txtOutputICO, "txtOutputICO");
            this.txtOutputICO.Name = "txtOutputICO";
            this.txtOutputICO.ReadOnly = true;
            this.txtOutputICO.Click += new System.EventHandler(this.BtnSelectOutputICO_Click);
            // 
            // btnSelectOutputICO
            // 
            resources.ApplyResources(this.btnSelectOutputICO, "btnSelectOutputICO");
            this.btnSelectOutputICO.Name = "btnSelectOutputICO";
            this.btnSelectOutputICO.UseVisualStyleBackColor = true;
            this.btnSelectOutputICO.Click += new System.EventHandler(this.BtnSelectOutputICO_Click);
            // 
            // txtInputImageICO
            // 
            resources.ApplyResources(this.txtInputImageICO, "txtInputImageICO");
            this.txtInputImageICO.Name = "txtInputImageICO";
            this.txtInputImageICO.ReadOnly = true;
            this.txtInputImageICO.Click += new System.EventHandler(this.BtnSelectImageICO_Click);
            // 
            // btnSelectImageICO
            // 
            resources.ApplyResources(this.btnSelectImageICO, "btnSelectImageICO");
            this.btnSelectImageICO.Name = "btnSelectImageICO";
            this.btnSelectImageICO.UseVisualStyleBackColor = true;
            this.btnSelectImageICO.Click += new System.EventHandler(this.BtnSelectImageICO_Click);
            // 
            // cmbIconSize
            // 
            this.cmbIconSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbIconSize, "cmbIconSize");
            this.cmbIconSize.ForeColor = System.Drawing.Color.MediumBlue;
            this.cmbIconSize.FormattingEnabled = true;
            this.cmbIconSize.Name = "cmbIconSize";
            // 
            // lblIconSize
            // 
            resources.ApplyResources(this.lblIconSize, "lblIconSize");
            this.lblIconSize.Name = "lblIconSize";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.numStartAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStepAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.tabControlMain.ResumeLayout(false);
            this.tabRotator.ResumeLayout(false);
            this.tabRotator.PerformLayout();
            this.tabConverter.ResumeLayout(false);
            this.tabConverter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreviewICO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnSelectImage;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.Button BtnSelectOutput;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numStartAngle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numEndAngle;
        private System.Windows.Forms.Button BtnGenerate;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numStepAngle;
        private System.Windows.Forms.PictureBox picPreview; // THÊM MỚI
        private System.Windows.Forms.Label labelPreview; // THÊM MỚI
        private System.Windows.Forms.Timer timerButtonEffect; // THÊM MỚI
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabRotator;
        private System.Windows.Forms.TabPage tabConverter;
        private System.Windows.Forms.Button btnSelectImageICO;
        private System.Windows.Forms.TextBox txtInputImageICO;
        private System.Windows.Forms.Button btnSelectOutputICO;
        private System.Windows.Forms.TextBox txtOutputICO;
        private System.Windows.Forms.Button btnConvertICO;
        private System.Windows.Forms.Label lblStatusICO;
        private System.Windows.Forms.PictureBox picPreviewICO;
        private System.Windows.Forms.Label labelPreviewICO;
        private System.Windows.Forms.ComboBox cmbIconSize;
        private System.Windows.Forms.Label lblIconSize;
        private System.Windows.Forms.Label labelPreviewImg;
        private System.Windows.Forms.Label labelPixel;
    }
}
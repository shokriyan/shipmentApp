namespace shipment
{
    partial class frmRPTPack
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRPTPack));
            this.packinglistDataSet = new shipment.packinglistDataSet();
            this.detailBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.detailTableAdapter = new shipment.packinglistDataSetTableAdapters.detailTableAdapter();
            this.reportViewerReport = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.packinglistDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // packinglistDataSet
            // 
            this.packinglistDataSet.DataSetName = "packinglistDataSet";
            this.packinglistDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // detailBindingSource
            // 
            this.detailBindingSource.DataMember = "detail";
            this.detailBindingSource.DataSource = this.packinglistDataSet;
            // 
            // detailTableAdapter
            // 
            this.detailTableAdapter.ClearBeforeFill = true;
            // 
            // reportViewerReport
            // 
            this.reportViewerReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewerReport.Location = new System.Drawing.Point(0, 0);
            this.reportViewerReport.Name = "reportViewerReport";
            this.reportViewerReport.Size = new System.Drawing.Size(985, 497);
            this.reportViewerReport.TabIndex = 0;
            // 
            // frmRPTPack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 497);
            this.Controls.Add(this.reportViewerReport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRPTPack";
            this.ShowInTaskbar = false;
            this.Text = "Report Center";
            this.Load += new System.EventHandler(this.frmRPTPack_Load);
            ((System.ComponentModel.ISupportInitialize)(this.packinglistDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource detailBindingSource;
        private packinglistDataSet packinglistDataSet;
        private packinglistDataSetTableAdapters.detailTableAdapter detailTableAdapter;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewerReport;
    }
}
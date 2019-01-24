namespace shipment
{
    partial class frmWaybill
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWaybill));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxShipID = new System.Windows.Forms.TextBox();
            this.textBoxLine = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLPO = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxWaybill = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Shipment ID";
            // 
            // textBoxShipID
            // 
            this.textBoxShipID.Location = new System.Drawing.Point(144, 27);
            this.textBoxShipID.Name = "textBoxShipID";
            this.textBoxShipID.Size = new System.Drawing.Size(208, 22);
            this.textBoxShipID.TabIndex = 3;
            this.textBoxShipID.TabStop = false;
            // 
            // textBoxLine
            // 
            this.textBoxLine.Location = new System.Drawing.Point(144, 72);
            this.textBoxLine.Name = "textBoxLine";
            this.textBoxLine.Size = new System.Drawing.Size(208, 22);
            this.textBoxLine.TabIndex = 4;
            this.textBoxLine.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "Line #";
            // 
            // textBoxLPO
            // 
            this.textBoxLPO.Location = new System.Drawing.Point(144, 119);
            this.textBoxLPO.Name = "textBoxLPO";
            this.textBoxLPO.Size = new System.Drawing.Size(208, 22);
            this.textBoxLPO.TabIndex = 5;
            this.textBoxLPO.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "LPO #";
            // 
            // dateTimePickerDate
            // 
            this.dateTimePickerDate.Location = new System.Drawing.Point(144, 168);
            this.dateTimePickerDate.Name = "dateTimePickerDate";
            this.dateTimePickerDate.Size = new System.Drawing.Size(208, 22);
            this.dateTimePickerDate.TabIndex = 6;
            this.dateTimePickerDate.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 14);
            this.label4.TabIndex = 7;
            this.label4.Text = "Date";
            // 
            // textBoxWaybill
            // 
            this.textBoxWaybill.Location = new System.Drawing.Point(144, 213);
            this.textBoxWaybill.Name = "textBoxWaybill";
            this.textBoxWaybill.Size = new System.Drawing.Size(208, 22);
            this.textBoxWaybill.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 216);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 14);
            this.label5.TabIndex = 8;
            this.label5.Text = "Waybill NO";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(144, 251);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(208, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save - Close";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // frmWaybill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 286);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxWaybill);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateTimePickerDate);
            this.Controls.Add(this.textBoxLPO);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxLine);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxShipID);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWaybill";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Waybill";
            this.Load += new System.EventHandler(this.frmWaybill_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxShipID;
        private System.Windows.Forms.TextBox textBoxLine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLPO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePickerDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxWaybill;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonSave;
    }
}
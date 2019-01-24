namespace shipment
{
    partial class frmClearance
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxRes = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonChanges = new System.Windows.Forms.Button();
            this.textBoxMaterial = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxChargable = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxInvoice = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonSlip = new System.Windows.Forms.Button();
            this.buttonOther = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
            this.textBoxPO = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxProject = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxShipID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxLine = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxGround = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.dataGridViewDetail = new System.Windows.Forms.DataGridView();
            this.line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.decs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxAmount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxDesc = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxRes);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.buttonDelete);
            this.groupBox1.Controls.Add(this.buttonChanges);
            this.groupBox1.Controls.Add(this.textBoxMaterial);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.textBoxChargable);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxInvoice);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.buttonSlip);
            this.groupBox1.Controls.Add(this.buttonOther);
            this.groupBox1.Controls.Add(this.buttonSave);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dateTimePickerDate);
            this.groupBox1.Controls.Add(this.textBoxPO);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxProject);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxShipID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(960, 166);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Clearance";
            // 
            // textBoxRes
            // 
            this.textBoxRes.Location = new System.Drawing.Point(492, 134);
            this.textBoxRes.Name = "textBoxRes";
            this.textBoxRes.Size = new System.Drawing.Size(203, 21);
            this.textBoxRes.TabIndex = 19;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(344, 138);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 15);
            this.label11.TabIndex = 20;
            this.label11.Text = "Responsible";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(844, 76);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(110, 23);
            this.buttonDelete.TabIndex = 18;
            this.buttonDelete.TabStop = false;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonChanges
            // 
            this.buttonChanges.Location = new System.Drawing.Point(717, 76);
            this.buttonChanges.Name = "buttonChanges";
            this.buttonChanges.Size = new System.Drawing.Size(110, 23);
            this.buttonChanges.TabIndex = 17;
            this.buttonChanges.TabStop = false;
            this.buttonChanges.Text = "Save Changes";
            this.buttonChanges.UseVisualStyleBackColor = true;
            this.buttonChanges.Click += new System.EventHandler(this.buttonChanges_Click);
            // 
            // textBoxMaterial
            // 
            this.textBoxMaterial.Location = new System.Drawing.Point(122, 134);
            this.textBoxMaterial.Name = "textBoxMaterial";
            this.textBoxMaterial.Size = new System.Drawing.Size(164, 21);
            this.textBoxMaterial.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 137);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 15);
            this.label9.TabIndex = 16;
            this.label9.Text = "Material";
            // 
            // textBoxChargable
            // 
            this.textBoxChargable.Location = new System.Drawing.Point(492, 96);
            this.textBoxChargable.Name = "textBoxChargable";
            this.textBoxChargable.Size = new System.Drawing.Size(203, 21);
            this.textBoxChargable.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(344, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 15);
            this.label7.TabIndex = 14;
            this.label7.Text = "Chargable Amount AFN";
            // 
            // textBoxInvoice
            // 
            this.textBoxInvoice.Location = new System.Drawing.Point(122, 97);
            this.textBoxInvoice.Name = "textBoxInvoice";
            this.textBoxInvoice.Size = new System.Drawing.Size(164, 21);
            this.textBoxInvoice.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 15);
            this.label8.TabIndex = 11;
            this.label8.Text = "Invoice Amount";
            // 
            // buttonSlip
            // 
            this.buttonSlip.Location = new System.Drawing.Point(844, 129);
            this.buttonSlip.Name = "buttonSlip";
            this.buttonSlip.Size = new System.Drawing.Size(110, 23);
            this.buttonSlip.TabIndex = 14;
            this.buttonSlip.TabStop = false;
            this.buttonSlip.Text = "Print Slip";
            this.buttonSlip.UseVisualStyleBackColor = true;
            this.buttonSlip.Click += new System.EventHandler(this.buttonSlip_Click);
            // 
            // buttonOther
            // 
            this.buttonOther.Location = new System.Drawing.Point(717, 129);
            this.buttonOther.Name = "buttonOther";
            this.buttonOther.Size = new System.Drawing.Size(110, 23);
            this.buttonOther.TabIndex = 13;
            this.buttonOther.TabStop = false;
            this.buttonOther.Text = "Print Other";
            this.buttonOther.UseVisualStyleBackColor = true;
            this.buttonOther.Click += new System.EventHandler(this.buttonOther_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(717, 22);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(237, 23);
            this.buttonSave.TabIndex = 12;
            this.buttonSave.TabStop = false;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(344, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Date";
            // 
            // dateTimePickerDate
            // 
            this.dateTimePickerDate.Location = new System.Drawing.Point(492, 58);
            this.dateTimePickerDate.Name = "dateTimePickerDate";
            this.dateTimePickerDate.Size = new System.Drawing.Size(203, 21);
            this.dateTimePickerDate.TabIndex = 5;
            // 
            // textBoxPO
            // 
            this.textBoxPO.Location = new System.Drawing.Point(492, 22);
            this.textBoxPO.Name = "textBoxPO";
            this.textBoxPO.Size = new System.Drawing.Size(203, 21);
            this.textBoxPO.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(344, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "PO #";
            // 
            // textBoxProject
            // 
            this.textBoxProject.Location = new System.Drawing.Point(122, 59);
            this.textBoxProject.Name = "textBoxProject";
            this.textBoxProject.Size = new System.Drawing.Size(164, 21);
            this.textBoxProject.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Project";
            // 
            // textBoxShipID
            // 
            this.textBoxShipID.Location = new System.Drawing.Point(122, 23);
            this.textBoxShipID.Name = "textBoxShipID";
            this.textBoxShipID.Size = new System.Drawing.Size(164, 21);
            this.textBoxShipID.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Shipment ID";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxLine);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.textBoxGround);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.buttonAdd);
            this.groupBox2.Controls.Add(this.dataGridViewDetail);
            this.groupBox2.Controls.Add(this.textBoxAmount);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBoxDesc);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 184);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(960, 315);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Detail";
            // 
            // textBoxLine
            // 
            this.textBoxLine.Location = new System.Drawing.Point(122, 34);
            this.textBoxLine.Name = "textBoxLine";
            this.textBoxLine.Size = new System.Drawing.Size(207, 21);
            this.textBoxLine.TabIndex = 16;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 37);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 15);
            this.label12.TabIndex = 17;
            this.label12.Text = "Line No";
            // 
            // textBoxGround
            // 
            this.textBoxGround.Location = new System.Drawing.Point(122, 269);
            this.textBoxGround.Name = "textBoxGround";
            this.textBoxGround.Size = new System.Drawing.Size(207, 21);
            this.textBoxGround.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 272);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(78, 15);
            this.label10.TabIndex = 15;
            this.label10.Text = "Ground Total";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(179, 194);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(150, 23);
            this.buttonAdd.TabIndex = 9;
            this.buttonAdd.Text = "Add to Table";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // dataGridViewDetail
            // 
            this.dataGridViewDetail.AllowUserToAddRows = false;
            this.dataGridViewDetail.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridViewDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.line,
            this.decs,
            this.amount});
            this.dataGridViewDetail.Location = new System.Drawing.Point(351, 20);
            this.dataGridViewDetail.Name = "dataGridViewDetail";
            this.dataGridViewDetail.Size = new System.Drawing.Size(603, 289);
            this.dataGridViewDetail.TabIndex = 10;
            this.dataGridViewDetail.TabStop = false;
            this.dataGridViewDetail.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDetail_CellValueChanged);
            // 
            // line
            // 
            this.line.DataPropertyName = "line";
            this.line.HeaderText = "Line";
            this.line.Name = "line";
            this.line.Width = 70;
            // 
            // decs
            // 
            this.decs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.decs.DataPropertyName = "description";
            this.decs.HeaderText = "Description";
            this.decs.Name = "decs";
            // 
            // amount
            // 
            this.amount.DataPropertyName = "amountAFN";
            this.amount.HeaderText = "Amount";
            this.amount.Name = "amount";
            this.amount.Width = 200;
            // 
            // textBoxAmount
            // 
            this.textBoxAmount.Location = new System.Drawing.Point(122, 126);
            this.textBoxAmount.Name = "textBoxAmount";
            this.textBoxAmount.Size = new System.Drawing.Size(207, 21);
            this.textBoxAmount.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 129);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 15);
            this.label6.TabIndex = 13;
            this.label6.Text = "Amount";
            // 
            // textBoxDesc
            // 
            this.textBoxDesc.Location = new System.Drawing.Point(122, 75);
            this.textBoxDesc.Name = "textBoxDesc";
            this.textBoxDesc.Size = new System.Drawing.Size(207, 21);
            this.textBoxDesc.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "Discription";
            // 
            // frmClearance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 511);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmClearance";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clearance Form";
            this.Load += new System.EventHandler(this.frmClearance_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxInvoice;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonSlip;
        private System.Windows.Forms.Button buttonOther;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePickerDate;
        private System.Windows.Forms.TextBox textBoxPO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxProject;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxShipID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxAmount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxDesc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxChargable;
        private System.Windows.Forms.TextBox textBoxMaterial;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView dataGridViewDetail;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonChanges;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.TextBox textBoxGround;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxRes;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxLine;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridViewTextBoxColumn line;
        private System.Windows.Forms.DataGridViewTextBoxColumn decs;
        private System.Windows.Forms.DataGridViewTextBoxColumn amount;
    }
}
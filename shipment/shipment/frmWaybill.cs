using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration; 

namespace shipment
{
    public partial class frmWaybill : Form
    {
        string connection = ConfigurationManager.ConnectionStrings["shipment"].ConnectionString;
        SqlConnection conShipment; 
        public frmWaybill()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            if (keyData == Keys.Enter)
                saveWaybill();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public string shipID { set; get;}
        public string line { set; get; }
        public string lpo { set; get; }


        private void frmWaybill_Load(object sender, EventArgs e)
        {
            conShipment = new SqlConnection(connection); 
            textBoxShipID.Text = shipID;
            textBoxLine.Text = line;
            textBoxLPO.Text = lpo; 
        }

        public void saveWaybill()
        {
            string shipID = textBoxShipID.Text;
            string line = textBoxLine.Text;
            string lpo = textBoxLPO.Text;
            DateTime date = DateTime.Parse(dateTimePickerDate.Value.ToString());
            string waybill = textBoxWaybill.Text;
            bool receipt = false;

            if (waybill != string.Empty)
            {
                SqlCommand cmdSave = new SqlCommand();
                cmdSave.Connection = conShipment;
                cmdSave.CommandText = "update detail set waybill = @waybill, receiptDate = @date, receipt=@receipt where shipmentID = @shipID and line=@line";
                cmdSave.Parameters.AddWithValue("@date", date);
                cmdSave.Parameters.AddWithValue("@waybill", waybill);
                cmdSave.Parameters.AddWithValue("@receipt", receipt);
                cmdSave.Parameters.AddWithValue("@shipID", shipID);
                cmdSave.Parameters.AddWithValue("@line", line);

                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdSave.ExecuteNonQuery();
                conShipment.Close();


                this.Close();
            }
            else
            {
                MessageBox.Show("You didn't Enter Waybill, Check Again");
                return;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveWaybill();   
        }
    }
}

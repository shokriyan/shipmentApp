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
    public partial class frmMRequeset : Form
    {
        string connection = ConfigurationManager.ConnectionStrings["shipment"].ConnectionString;
        SqlConnection conShipment;

        public string shipID { set; get; }
        public frmMRequeset()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
                calculate(); 
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void frmMRequeset_Load(object sender, EventArgs e)
        {
            conShipment = new SqlConnection(connection);
            txtBoxMRID.Text = shipID;
            txtBoxMRAmount.Enabled = false;
            txtBoxMRID.Enabled = false;
            txtBoxMRTaxTotal.Enabled = false;
            txtBoxMRTotal.Enabled = false;
            txtPer2.Text = "3";
            txtPer3.Text = "2";
            txtPer4.Text = "2";
            txtBoxMRother.Text = "12000";



            //get shipment price from SQL
            SqlCommand cmdAmount = new SqlCommand(@"select sum(detail.amount)*shipment.rate as sumAmount
from detail
inner join shipment on shipment.shipmentID = detail.shipmentID
where shipment.shipmentID = @shipID
group by shipment.rate", conShipment);
            cmdAmount.Parameters.AddWithValue("@shipID", shipID);
            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            string amount = cmdAmount.ExecuteScalar().ToString();
            conShipment.Close();
            txtBoxMRAmount.Text = amount;

        }

        public void calculate ()
        {
            double amtPer = 0.02;
            double shipRate = double.Parse(txtBoxMRrate.Text.ToString());
            double invAmount = double.Parse(txtBoxMRAmount.Text.ToString());
            double exRate = double.Parse(txtMrExRate.Text.ToString());
            double txPer1 = double.Parse(txtPer1.Text.ToString());
            double txPer2 = double.Parse(txtPer2.Text.ToString());
            double txPer3 = double.Parse(txtPer3.Text.ToString());
            double txPer4 = double.Parse(txtPer4.Text.ToString());
            double other = double.Parse(txtBoxMRother.Text.ToString());
            double afgAmount = (invAmount + (invAmount * amtPer) + shipRate) * exRate;
            label6.Text = Math.Round(afgAmount).ToString();
            double tax1 = afgAmount * txPer1 / 100;
            label7.Text = Math.Round(tax1).ToString();
            double tax2 = (afgAmount + tax1) * txPer2 / 100;
            label8.Text = Math.Round((afgAmount + tax1)).ToString();
            label9.Text = Math.Round(tax2).ToString();
            label11.Text = Math.Round(tax1).ToString();
            double tax3 = Math.Round(tax1 * txPer3 / 100);
            label12.Text = tax3.ToString();
            label10.Text = Math.Round(afgAmount + tax1).ToString();
            double tax4 = Math.Round((afgAmount + tax1) * txPer4 / 100);
            label13.Text = tax4.ToString();
            double amtTotal = Math.Round(tax1 + tax2 + tax3 + tax4 + 300);
            txtBoxMRTotal.Text = amtTotal.ToString();
            txtBoxMRTaxTotal.Text = Math.Round(amtTotal + other).ToString(); 
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            calculate();
        }
    }
}

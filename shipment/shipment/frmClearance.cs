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
    public partial class frmClearance : Form
    {
        string connection = ConfigurationManager.ConnectionStrings["shipment"].ConnectionString;
        SqlConnection conShipment; 
        public frmClearance()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public string shipID { set; get; }

        public void loadInfo()
        {
            string invoice = "";
            string curr = "";

            SqlCommand cmdInvoice = new SqlCommand("Select Sum (amount) from detail where shipmentID = @shipID", conShipment);
            cmdInvoice.Parameters.AddWithValue("shipID", shipID);
            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            invoice = cmdInvoice.ExecuteScalar().ToString();

            SqlCommand cmdCur = new SqlCommand("select currency from shipment where shipmentID = @shipID", conShipment);
            cmdCur.Parameters.AddWithValue("@shipID", shipID);
            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            curr = cmdCur.ExecuteScalar().ToString();

            conShipment.Close();

            string invAmount = invoice + " - " + curr;
            textBoxInvoice.Text = invAmount.ToString();
            textBoxShipID.Text = shipID.ToString();
            textBoxLine.Text = "1";
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int line = int.Parse(textBoxLine.Text.ToString());
            string desc = textBoxDesc.Text;
            float amount = float.Parse(textBoxAmount.Text.ToString());

            dataGridViewDetail.Rows.Add(line, desc, amount);
            textBoxDesc.Clear();
            textBoxAmount.Clear();

            float totalAmount = 0;

            foreach (DataGridViewRow dr in dataGridViewDetail.Rows)
            {
                float cellAmount = float.Parse(dr.Cells[2].Value.ToString());
                totalAmount += cellAmount;
            }

            textBoxGround.Text = totalAmount.ToString();

            lineNo();
        }

        bool flag = false;

        public void checkMode()
        {
            SqlCommand cmdCheck = new SqlCommand("Select shipmentID from clearance where shipmentID = @shipID",conShipment);
            cmdCheck.Parameters.AddWithValue("@shipID", shipID);
            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            //string temp = cmdCheck.ExecuteScalar().ToString();
            if (cmdCheck.ExecuteScalar() == null)
                flag = false;
            else
                flag = true; 
        }

        private void frmClearance_Load(object sender, EventArgs e)
        {
            conShipment = new SqlConnection(connection);
            checkMode();
            if (flag == false)
            {
                loadInfo();
                buttonChanges.Enabled = false;
                buttonDelete.Enabled = false;

                textBoxGround.Text = "0";
            }
            else
            {
                editMode();
                buttonChanges.Enabled = true;
                buttonDelete.Enabled = true;

                float totalAmount = 0;

                foreach (DataGridViewRow dr in dataGridViewDetail.Rows)
                {
                    float cellAmount = float.Parse(dr.Cells[2].Value.ToString());
                    totalAmount += cellAmount;
                }

                textBoxGround.Text = totalAmount.ToString();
            }
        }

        public void lineEditMode()
        {
            int line = 0;
            if (dataGridViewDetail.Rows.Count == 0)
                line = 0;
            else
                line = dataGridViewDetail.Rows.Count;

            int newLine = line + 1;
            textBoxLine.Text = newLine.ToString(); 
        }

        public void editMode()
        {


            SqlDataAdapter daList = new SqlDataAdapter();
            SqlCommand cmdClearance = new SqlCommand("select * from clearance where shipmentID = @shipID", conShipment);
            cmdClearance.Parameters.AddWithValue("@shipID",shipID); 
            daList.SelectCommand = cmdClearance; 
            DataTable dtList = new DataTable(); 
            daList.Fill(dtList);
            textBoxShipID.Text = shipID; 
            textBoxProject.Text = dtList.Rows[0][2].ToString();
            textBoxMaterial.Text = dtList.Rows[0][3].ToString();
            textBoxRes.Text = dtList.Rows[0][4].ToString();
            textBoxInvoice.Text = dtList.Rows[0][5].ToString();
            textBoxPO.Text = dtList.Rows[0][6].ToString();
            DateTime date = DateTime.Parse(dtList.Rows[0][7].ToString());
            dateTimePickerDate.Value = date; 
            textBoxChargable.Text = dtList.Rows[0][8].ToString();

            SqlDataAdapter daDetail = new SqlDataAdapter();
            SqlCommand cmdDetail = new SqlCommand("select line, description, amountAFN from clDetail where shipmentID = @shipID", conShipment);
            cmdDetail.Parameters.AddWithValue("@shipID", shipID);
            daDetail.SelectCommand = cmdDetail;
            DataTable dtDetail = new DataTable();
            daDetail.Fill(dtDetail);
            dataGridViewDetail.AutoGenerateColumns = false;
            dataGridViewDetail.DataSource = dtDetail;

            lineEditMode();

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string shipID = textBoxShipID.Text;
            string prj = textBoxProject.Text;
            string material = textBoxMaterial.Text;
            string invoice = textBoxInvoice.Text;
            string po = textBoxPO.Text;
            DateTime clDate = DateTime.Parse(dateTimePickerDate.Value.ToShortDateString());
            float chargable = float.Parse(textBoxChargable.Text);
            string resp = textBoxRes.Text; 

            SqlCommand cmdSave = new SqlCommand();
            cmdSave.Connection = conShipment;
            cmdSave.CommandText = "insert clearance values (@shipID, @project, @material, @resp, @invoice, @po, @date, @chargable)";
            cmdSave.Parameters.AddWithValue("@shipID", shipID);
            cmdSave.Parameters.AddWithValue("@project", prj);
            cmdSave.Parameters.AddWithValue("@material", material);
            cmdSave.Parameters.AddWithValue("@resp", resp); 
            cmdSave.Parameters.AddWithValue("@invoice", invoice);
            cmdSave.Parameters.AddWithValue("@po", po);
            cmdSave.Parameters.AddWithValue("@date", clDate);
            cmdSave.Parameters.AddWithValue("@chargable", chargable);

            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            cmdSave.ExecuteNonQuery();
            conShipment.Close();

            foreach (DataGridViewRow dr in dataGridViewDetail.Rows)
            {
                int line = int.Parse(dr.Cells[0].Value.ToString());
                string desc = dr.Cells[1].Value.ToString();
                float amount = float.Parse(dr.Cells[2].Value.ToString());

                SqlCommand cmdDetail = new SqlCommand("insert clDetail values (@line, @shipID, @desc, @amount)", conShipment);
                cmdDetail.Parameters.AddWithValue("@line", line);
                cmdDetail.Parameters.AddWithValue("@shipID", shipID);
                cmdDetail.Parameters.AddWithValue("@desc", desc);
                cmdDetail.Parameters.AddWithValue("@amount", amount);

                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdDetail.ExecuteNonQuery();
                conShipment.Close();
            }

            MessageBox.Show("Clearance charges saved successfully");
        }

        public void lineNo()
        {
            int lineNo = 0;
            if (dataGridViewDetail.Rows.Count == 0)
                lineNo = 1;
            else
                lineNo = int.Parse(dataGridViewDetail.Rows.Count.ToString());

            int newLine = lineNo + 1;
            textBoxLine.Text = newLine.ToString();
        }

        private void buttonChanges_Click(object sender, EventArgs e)
        {
            string prj = textBoxProject.Text;
            string invAmount = textBoxInvoice.Text;
            string material = textBoxMaterial.Text;
            string po = textBoxPO.Text;
            DateTime date = DateTime.Parse(dateTimePickerDate.Value.ToShortDateString());
            float charge = float.Parse(textBoxChargable.Text.ToString());
            string resp = textBoxRes.Text;

            SqlCommand cmdChanges = new SqlCommand();
            cmdChanges.Connection = conShipment;
            cmdChanges.CommandText = "update clearance set projects = @proj, Mateial = @material, responsible = @resp, invoiceAmount = @inv , poNO = @po , date = @date, chargable = @charge where shipmentID = @shipID";
            cmdChanges.Parameters.AddWithValue("@proj", prj); 
            cmdChanges.Parameters.AddWithValue("@material",material);
            cmdChanges.Parameters.AddWithValue("@resp", resp);
            cmdChanges.Parameters.AddWithValue("@inv",invAmount); 
            cmdChanges.Parameters.AddWithValue("@po",po); 
            cmdChanges.Parameters.AddWithValue("@date",date); 
            cmdChanges.Parameters.AddWithValue ("@charge",charge); 
            cmdChanges.Parameters.AddWithValue("@shipID",shipID); 
            if (conShipment.State != ConnectionState.Open)
                conShipment.Open(); 
            cmdChanges.ExecuteNonQuery(); 
            conShipment.Close(); 

            foreach (DataGridViewRow dr in dataGridViewDetail.Rows)
            {
                int line = int.Parse(dr.Cells[0].Value.ToString());
                string desc = dr.Cells[1].Value.ToString(); 
                float amount =float.Parse( dr.Cells[2].Value.ToString());

                SqlCommand cmdUpdate = new SqlCommand(); 
                cmdUpdate.Connection = conShipment; 
                cmdUpdate.CommandText = "update clDetail set Description = @desc , AmountAFN = @amount where shipmentID = @shipID and line = @line"; 
                cmdUpdate.Parameters.AddWithValue("@desc",desc); 
                cmdUpdate.Parameters.AddWithValue("@amount",amount);
                cmdUpdate.Parameters.AddWithValue("@shipID", shipID);
                cmdUpdate.Parameters.AddWithValue("@line", line); 

                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdUpdate.ExecuteNonQuery();
                conShipment.Close();

            }

            MessageBox.Show("Your changes have been successfully updated"); 
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (DialogResult.No != MessageBox.Show("do you want to Delete", "warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                SqlCommand cmdDel = new SqlCommand("delete from clDetail where shipmentID = @shipID", conShipment);
                cmdDel.Parameters.AddWithValue("@shipID", shipID);
                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdDel.ExecuteNonQuery();
                cmdDel.CommandText = "delete from clearance where shipmentID = @shipID1";
                cmdDel.Connection = conShipment;
                cmdDel.Parameters.AddWithValue("@shipID1", shipID);
                cmdDel.ExecuteNonQuery();
                conShipment.Close();

                textBoxProject.Clear();
                textBoxMaterial.Clear();
                textBoxInvoice.Clear();
                textBoxPO.Clear();
                textBoxChargable.Clear();
                dataGridViewDetail.Rows.Clear();
                MessageBox.Show("Your Operation done successfully"); 
            }
            else
                return; 
        }

        private void dataGridViewDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            float totalAmount = 0;

            foreach (DataGridViewRow dr in dataGridViewDetail.Rows)
            {
                float cellAmount = float.Parse(dr.Cells[2].Value.ToString());
                totalAmount += cellAmount;
            }

            textBoxGround.Text = totalAmount.ToString(); 
        }

        private void buttonOther_Click(object sender, EventArgs e)
        {
            string shipmentID = textBoxShipID.Text;

            SqlCommand cmdCheck = new SqlCommand("select shipmentID from clDetail where shipmentID = @shipID", conShipment);
            cmdCheck.Parameters.AddWithValue("@shipID", shipmentID);

            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            if (cmdCheck.ExecuteScalar() != null)
            {
                frmRPTPack fReport = new frmRPTPack("rptOther.rdlc");
                fReport.shipID = shipmentID;
                fReport.Show();
            }
            else
                return;


        }

        private void buttonSlip_Click(object sender, EventArgs e)
        {
            string shipmentID = textBoxShipID.Text;
                        SqlCommand cmdCheck = new SqlCommand("select shipmentID from clDetail where shipmentID = @shipID", conShipment);
            cmdCheck.Parameters.AddWithValue("@shipID", shipmentID);

            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            if (cmdCheck.ExecuteScalar() != null)
            {
                frmRPTPack fReport = new frmRPTPack("rptSlip.rdlc");
                fReport.shipID = shipmentID;
                fReport.Show();
            }
            else
                return; 
        }

    }
}

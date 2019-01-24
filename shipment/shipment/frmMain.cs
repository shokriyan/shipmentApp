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
    public partial class btnShipment : Form
    {
        string connection = ConfigurationManager.ConnectionStrings["shipment"].ConnectionString;
        SqlConnection conShipment;
        SqlDataAdapter daList;
        DataTable dtlist;

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult.No == MessageBox.Show("Do you want to Close", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                e.Cancel = true;
            else
                e.Cancel = false;
            base.OnClosing(e);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                searchReport();
                searchWaybill();
                clSearch();
                usdTotal();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public btnShipment()
        {
            string masterConnection = ConfigurationManager.ConnectionStrings["masterCon"].ConnectionString;
            using (SqlConnection masterCon = new SqlConnection(masterConnection))
            {
                masterCon.Open();
                SqlCommand cmdCount = new SqlCommand("select count(*) from master.dbo.sysdatabases where name='shipment'", masterCon);
                int countDB = (int)cmdCount.ExecuteScalar();
                if (countDB < 1)
                {
                    cmdCount.CommandText = string.Format("sp_attach_db @dbname = 'shipment', @filename1='{0}\\shipment.mdf',@filename2='{0}\\shipment_log.ldf'", Environment.CurrentDirectory);
                    cmdCount.ExecuteNonQuery();
                }
                masterCon.Close();
            }
            InitializeComponent();
        }

        public void direction()
        {
            SqlCommand cmdDir = new SqlCommand("Select fileDir from info", conShipment);
            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            if (cmdDir.ExecuteScalar() == null)
            {
                buttonSaveDir.Enabled = true;
                buttonChangeDir.Enabled = false;
            }
            else
                textBoxDirection.Text = cmdDir.ExecuteScalar().ToString();
            conShipment.Close();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            conShipment = new SqlConnection(connection);
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            prjLoad();
            cmbPrj();
            loadShipReport();
            dataGridViewSreport.ContextMenuStrip = contextMenuStripSreport;
            dataGridViewWaybill.ContextMenuStrip = contextMenuStripWaybill;
            dataGridViewCresult.ContextMenuStrip = contextMenuStripClearance;
            loadWaybillRe();
            buttonSaveDir.Enabled = false;
            direction();
            loadClReport();
            usdTotal();
            comboBoxReceipt.SelectedItem = "All";
            comboBoxFinish.SelectedItem = "All";
            tabControl1.Selected += new TabControlEventHandler(TabControl1_Selectring);
        }
        // Loading Project List

        public void prjLoad()
        {
            SqlDataAdapter daList = new SqlDataAdapter();
            SqlCommand cmdList = new SqlCommand("Select * from project", conShipment);
            daList.SelectCommand = cmdList;
            DataTable dtList = new DataTable();
            daList.Fill(dtList);

            dataGridViewProject.AutoGenerateColumns = false;
            dataGridViewProject.DataSource = dtList;
        }

        // loading combobox prject
        public void cmbPrj()
        {
            SqlDataAdapter daList = new SqlDataAdapter();
            SqlCommand cmdlist = new SqlCommand("select prjCode , (CONVERT (nvarChar(10),prjCode) + ' - ' + prjName) as prjName from project", conShipment);
            daList.SelectCommand = cmdlist;
            DataTable dtList = new DataTable();
            daList.Fill(dtList);

            comboBoxPrj.DataSource = dtList;
            comboBoxPrj.DisplayMember = "prjName";
            comboBoxPrj.ValueMember = "prjCode";
        }

        private void buttonSProject_Click(object sender, EventArgs e)
        {
            if (textBoxPrjCode.Text != string.Empty)
            {

                int prjCode = int.Parse(textBoxPrjCode.Text.ToString());
                string prjName = textBoxName.Text;
                string location = textBoxLocation.Text;


                SqlCommand cmdSave = new SqlCommand("insert project values (@prjCode, @prjName, @prjLocation)", conShipment);
                cmdSave.Parameters.AddWithValue("@prjCode", prjCode);
                cmdSave.Parameters.AddWithValue("@prjName", prjName);
                cmdSave.Parameters.AddWithValue("@prjLocation", location);
                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdSave.ExecuteNonQuery();
                conShipment.Close();
                textBoxPrjCode.Clear();
                textBoxName.Clear();
                textBoxLocation.Clear();
                prjLoad();
            }
            else
            {
                MessageBox.Show("Erroe (0001), \nProject Code may not be empty.");
                return;
            }

        }

        private void buttonChProject_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridViewProject.Rows)
            {
                int prjCode = int.Parse(dr.Cells[0].Value.ToString());
                string prjName = dr.Cells[1].Value.ToString();
                string prjLocation = dr.Cells[2].Value.ToString();

                SqlCommand cmdSave = new SqlCommand("update project set prjName = @prjName , prjLocation=@Location where prjCode = @prjCode", conShipment);
                cmdSave.Parameters.AddWithValue("@prjName", prjName);
                cmdSave.Parameters.AddWithValue("@location", prjLocation);
                cmdSave.Parameters.AddWithValue("@prjCode", prjCode);

                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdSave.ExecuteNonQuery();
                conShipment.Close();
            }

            MessageBox.Show("Changes on Project data have been successfully done");
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int line = int.Parse(textBoxLine.Text.ToString());
            string lpo = textBoxLPO.Text;
            string req = textBoxReq.Text;
            int prjCode = int.Parse(comboBoxPrj.SelectedValue.ToString());
            string material = textBoxMaterial.Text;
            string remark = textBoxRemark.Text;
            float amount = float.Parse(textBoxAmount.Text.ToString());

            if (dtlist != null)
                dataGridViewDetail.Enabled = true; 

            dataGridViewDetail.Rows.Add(line, lpo, req, prjCode, material, remark, amount);
            textBoxLPO.Clear();
            textBoxReq.Clear();
            textBoxMaterial.Clear();
            textBoxRemark.Clear();
            textBoxAmount.Clear();

            float totalAmount = 0;

            foreach (DataGridViewRow dr in dataGridViewDetail.Rows)
            {
                float cellAmount = float.Parse(dr.Cells[6].Value.ToString());
                totalAmount += cellAmount;
            }

            textBoxTotal.Text = totalAmount.ToString();

            int lineNo = 0;
            if (dataGridViewDetail.Rows.Count == 0)
                lineNo = 1;
            else
                lineNo = int.Parse(dataGridViewDetail.Rows.Count.ToString());

            int newLine = lineNo + 1;
            textBoxLine.Text = newLine.ToString();
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            cmbPrj();
            searchReport();
            searchWaybill();
            clSearch();
            int line = 0;
            if (dataGridViewDetail.Rows.Count == 0)
                line = 1;
            else
                line = int.Parse(dataGridViewDetail.Rows.Count.ToString());
            textBoxLine.Text = line.ToString();

            direction();
        }

        private void buttonSShip_Click(object sender, EventArgs e)
        {
            if (textBoxShipID.Text != string.Empty)
            {
                string shipID = textBoxShipID.Text;
                SqlCommand cmdCheck = new SqlCommand("select shipmentID from shipment where shipmentID = @shipment", conShipment);
                cmdCheck.Parameters.AddWithValue("@shipment", shipID);
                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                if (cmdCheck.ExecuteScalar() != null)
                {
                    MessageBox.Show("Shipment is already existed");
                    return;
                }
                else
                {
                    string from = textBoxFrom.Text;
                    string date1 = dateTimePickerShipDate.Text;
                    DateTime date = DateTime.Parse(dateTimePickerShipDate.Text);
                    int duration = int.Parse(textBoxDuration.Text.ToString());
                    var dateoff = DateTimeOffset.Parse(date1);
                    var newDatetime = dateoff.AddDays(duration);
                    var newDate1 = newDatetime.ToString("O");
                    DateTime newDate = DateTime.Parse(newDate1);
                    string custom = textBoxCustom.Text;
                    string respon = textBoxRespon.Text;
                    string weight = textBoxWeight.Text;
                    string status = comboBoxStatus.Text;
                    string currency = textBoxCurrency.Text;
                    float rate = float.Parse(textBoxRate.Text.ToString());
                    string mode = comboBoxMode.Text;
                    string docs = textBoxDocs.Text;

                    SqlCommand cmdSship = new SqlCommand();
                    cmdSship.CommandText = "insert shipment values (@shipID, @from, @date, @arrive , @custom , @respon, @weight , @status, @currency , @rate , @mode , @doc, 0)";
                    cmdSship.Parameters.AddWithValue("@shipID", shipID);
                    cmdSship.Parameters.AddWithValue("@from", from);
                    cmdSship.Parameters.AddWithValue("@date", date);
                    cmdSship.Parameters.AddWithValue("@arrive", newDate);
                    cmdSship.Parameters.AddWithValue("@custom", custom);
                    cmdSship.Parameters.AddWithValue("@respon", respon);
                    cmdSship.Parameters.AddWithValue("@weight", weight);
                    cmdSship.Parameters.AddWithValue("@status", status);
                    cmdSship.Parameters.AddWithValue("@currency", currency);
                    cmdSship.Parameters.AddWithValue("@rate", rate);
                    cmdSship.Parameters.AddWithValue("@mode", mode);
                    cmdSship.Parameters.AddWithValue("@doc", docs);
                    cmdSship.Connection = conShipment;
                    if (conShipment.State != ConnectionState.Open)
                        conShipment.Open();
                    cmdSship.ExecuteNonQuery();
                    conShipment.Close();

                    if (dataGridViewDetail.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow dr in dataGridViewDetail.Rows)
                        {
                            int line = int.Parse(dr.Cells[0].Value.ToString());
                            string lpo = dr.Cells[1].Value.ToString();
                            string req = dr.Cells[2].Value.ToString();
                            int prjCode = int.Parse(dr.Cells[3].Value.ToString());
                            string material = dr.Cells[4].Value.ToString();
                            string remark = dr.Cells[5].Value.ToString();
                            float amount = float.Parse(dr.Cells[6].Value.ToString());

                            SqlCommand cmdSDetail = new SqlCommand();
                            cmdSDetail.Connection = conShipment;
                            cmdSDetail.CommandText = "insert detail values (@shipID , @line, @lpo, @req , @prjCode , @material , @remark , @amount,0,0,0)";
                            cmdSDetail.Parameters.AddWithValue("@shipID", shipID);
                            cmdSDetail.Parameters.AddWithValue("@line", line);
                            cmdSDetail.Parameters.AddWithValue("@lpo", lpo);
                            cmdSDetail.Parameters.AddWithValue("@req", req);
                            cmdSDetail.Parameters.AddWithValue("@prjCode", prjCode);
                            cmdSDetail.Parameters.AddWithValue("@material", material);
                            cmdSDetail.Parameters.AddWithValue("@remark", remark);
                            cmdSDetail.Parameters.AddWithValue("@amount", amount);

                            if (conShipment.State != ConnectionState.Open)
                                conShipment.Open();
                            cmdSDetail.ExecuteNonQuery();
                            conShipment.Close();
                        }

                    }


                    else
                        return;

                    if (DialogResult.Yes == MessageBox.Show("Do you want to Print", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                    {
                        frmRPTPack fPacking = new frmRPTPack("rptPaking.rdlc");
                        fPacking.shipID = shipID;
                        fPacking.ShowDialog();
                    }
                }
            }
            else
            {
                MessageBox.Show("Error (0002) \nYou must enter shipment ID");
            }

        }

        private void dataGridViewDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonNShip_Click(object sender, EventArgs e)
        {
            if (daList != null)
                daList.Dispose();
            else
                dataGridViewDetail.Rows.Clear(); 
            if (dtlist != null)
                dtlist.Clear();
            textBoxShipID.Clear();
            textBoxFrom.Clear();
            textBoxDuration.Clear();
            textBoxCustom.Clear();
            textBoxRespon.Clear();
            textBoxWeight.Clear();
            textBoxCurrency.Clear();
            textBoxRate.Clear();
            textBoxDocs.Clear();
            textBoxTotal.Text = "0";
            textBoxLine.Text = "1";
            dateTimePickerShipDate.Enabled = true;
            textBoxDuration.Enabled = true;
            buttonSShip.Enabled = true;
        }

        //Loading Shipment report
        public void loadShipReport()
        {
            SqlDataAdapter daList = new SqlDataAdapter();
            SqlCommand cmdList = new SqlCommand();
            cmdList.Connection = conShipment;
            cmdList.CommandText = @"select shipment.shipmentID, shipment.shipFrom, shipment.shipDate , shipment.arrivalDate , shipment.custom , shipment.mode ,SUM(detail.amount) as totalAmount, (sum(detail.amount)*shipment.rate) as usdAmount, shipment.currency, shipment.status, shipment.finish,shipment.documentation
from shipment
inner join detail on shipment.shipmentID = detail.shipmentID
group by shipment.shipmentID, shipment.shipFrom, shipment.shipDate , shipment.arrivalDate , shipment.custom , shipment.mode , shipment.status, shipment.finish,shipment.documentation, shipment.rate, shipment.currency";
            daList.SelectCommand = cmdList;
            DataTable dtList = new DataTable();
            daList.Fill(dtList);

            dataGridViewSreport.AutoGenerateColumns = false;
            dataGridViewSreport.DataSource = dtList;
        }
        private void buttonReset_Click(object sender, EventArgs e)
        {
            loadShipReport();
            textBoxSrcShipId.Clear();
            comboBoxSrcMode.Text = "";
            comboBoxSrcStatus.Text = "";
            comboBoxFinish.SelectedItem = "All";
            usdTotal();
        }

        public void searchReport()
        {
            string mode = comboBoxSrcMode.Text;
            string status = comboBoxSrcStatus.Text;
            string shipID = textBoxSrcShipId.Text;
            string tFinish = comboBoxFinish.Text;
            bool finish = false;

            if (tFinish == "Yes")
                finish = true;
            else
                finish = false;

            string query = @"select shipment.shipmentID, shipment.shipFrom, shipment.shipDate , shipment.arrivalDate , shipment.custom , shipment.mode ,SUM(detail.amount) as totalAmount, (sum(detail.amount)*shipment.rate) as usdAmount, shipment.currency, shipment.status, shipment.finish,shipment.documentation
from shipment
inner join detail on shipment.shipmentID = detail.shipmentID";
            string criteria = "";
            string groupBy = " group by shipment.shipmentID, shipment.shipFrom, shipment.shipDate , shipment.arrivalDate , shipment.custom , shipment.mode , shipment.status, shipment.finish,shipment.documentation, shipment.rate, shipment.currency";
            bool flag = false;
            if (tFinish == "All")
            {
                if (mode == string.Empty & status == string.Empty & shipID == string.Empty)
                    loadShipReport();

                else
                {
                    if (mode != string.Empty)
                    {
                        criteria = string.Format(" where shipment.mode = '{0}'", mode);
                        flag = true;
                    }
                    if (status != string.Empty)
                        if (flag)
                            if (status != "Critical")
                                criteria += string.Format(" and shipment.status = '{0}'", status);
                            else
                                criteria += " and shipment.status <> 'Done'";

                        else
                        {
                            if (status != "Critical")
                            {
                                criteria += string.Format(" where shipment.status = '{0}'", status);
                                flag = true;
                            }
                            else
                            {
                                criteria += " where shipment.status <> 'Done'";
                                flag = true;
                            }
                        }
                    if (shipID != string.Empty)
                        if (flag)
                            criteria += string.Format(" and shipment.shipmentID Like '%{0}%'", shipID);
                        else
                        {
                            criteria += string.Format(" where shipment.shipmentID Like '%{0}%'", shipID);
                            flag = true;
                        }
                    if (tFinish == "All")
                        if (flag)
                            criteria += "";
                        else
                            flag = true;
                    if (tFinish != "All")
                    {
                        if (finish == true)
                            if (flag)
                                criteria += string.Format(" and shipment.finish = '{0}'", finish);
                            else
                            {
                                criteria += string.Format(" where shipment.finish = '{0}'", finish);
                                flag = true;
                            }
                        if (finish == false)
                            if (flag)
                                criteria += string.Format(" and shipment.finish = '{0}'", finish);
                            else
                            {
                                criteria += string.Format(" where shipment.finish = '{0}'", finish);
                                flag = true;
                            }
                    }

                    SqlDataAdapter daList = new SqlDataAdapter();
                    SqlCommand cmdList = new SqlCommand();
                    cmdList.Connection = conShipment;
                    cmdList.CommandText = query + criteria + groupBy;
                    daList.SelectCommand = cmdList;
                    DataTable dtList = new DataTable();
                    daList.Fill(dtList);
                    dataGridViewSreport.AutoGenerateColumns = false;
                    dataGridViewSreport.DataSource = dtList;
                }
            }
            else
            {
                if (mode == string.Empty & status == string.Empty & shipID == string.Empty & tFinish == "All")
                    loadShipReport();

                else
                {
                    if (mode != string.Empty)
                    {
                        criteria = string.Format(" where shipment.mode = '{0}'", mode);
                        flag = true;
                    }
                    if (status != string.Empty)
                        if (flag)
                            if (status != "Critical")
                                criteria += string.Format(" and shipment.status = '{0}'", status);
                            else
                                criteria += " and shipment.status <> 'Done'";

                        else
                        {
                            if (status != "Critical")
                            {
                                criteria += string.Format(" where shipment.status = '{0}'", status);
                                flag = true;
                            }
                            else
                            {
                                criteria += " where shipment.status <> 'Done'";
                                flag = true;
                            }
                        }
                    if (shipID != string.Empty)
                        if (flag)
                            criteria += string.Format(" and shipment.shipmentID Like '%{0}%'", shipID);
                        else
                        {
                            criteria += string.Format(" where shipment.shipmentID Like '%{0}%'", shipID);
                            flag = true;
                        }
                    if (tFinish == "All")
                        if (flag)
                            criteria += "";
                        else
                            flag = true;
                    if (tFinish != "All")
                    {
                        if (finish == true)
                            if (flag)
                                criteria += string.Format(" and shipment.finish = '{0}'", finish);
                            else
                            {
                                criteria += string.Format(" where shipment.finish = '{0}'", finish);
                                flag = true;
                            }
                        if (finish == false)
                            if (flag)
                                criteria += string.Format(" and shipment.finish = '{0}'", finish);
                            else
                            {
                                criteria += string.Format(" where shipment.finish = '{0}'", finish);
                                flag = true;
                            }
                    }

                    SqlDataAdapter daList = new SqlDataAdapter();
                    SqlCommand cmdList = new SqlCommand();
                    cmdList.Connection = conShipment;
                    cmdList.CommandText = query + criteria + groupBy;
                    daList.SelectCommand = cmdList;
                    DataTable dtList = new DataTable();
                    daList.Fill(dtList);
                    dataGridViewSreport.AutoGenerateColumns = false;
                    dataGridViewSreport.DataSource = dtList;
                }
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            searchReport();
            usdTotal();
        }

        private void underShipmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewSreport.SelectedCells.Count > 0)
            {
                int selectedCell = dataGridViewSreport.SelectedCells[0].RowIndex;
                string shipID = dataGridViewSreport[0, selectedCell].Value.ToString();

                SqlCommand cmdUpdate = new SqlCommand("update shipment set status = 'Under Shipment' where shipmentID = @shipID", conShipment);
                cmdUpdate.Parameters.AddWithValue("@shipID", shipID);
                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdUpdate.ExecuteNonQuery();
                conShipment.Close();

                searchReport();
                MessageBox.Show(String.Format("The status of - {0} - have been successfully changes.", shipID));
            }
        }

        private void underClearanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewSreport.SelectedCells.Count > 0)
            {
                int selectedCell = dataGridViewSreport.SelectedCells[0].RowIndex;
                string shipID = dataGridViewSreport[0, selectedCell].Value.ToString();

                SqlCommand cmdUpdate = new SqlCommand("update shipment set status = 'Under Clearance' where shipmentID = @shipID", conShipment);
                cmdUpdate.Parameters.AddWithValue("@shipID", shipID);
                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdUpdate.ExecuteNonQuery();
                conShipment.Close();

                searchReport();
                MessageBox.Show(String.Format("The status of - {0} - have been successfully changes.", shipID));
            }
        }

        private void doneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewSreport.SelectedCells.Count > 0)
            {
                int selectedCell = dataGridViewSreport.SelectedCells[0].RowIndex;
                string shipID = dataGridViewSreport[0, selectedCell].Value.ToString();

                SqlCommand cmdUpdate = new SqlCommand("update shipment set status = 'Done' where shipmentID = @shipID", conShipment);
                cmdUpdate.Parameters.AddWithValue("@shipID", shipID);
                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdUpdate.ExecuteNonQuery();
                conShipment.Close();

                searchReport();
                MessageBox.Show(String.Format("The status of - {0} - have been successfully changes.", shipID));
            }
        }

        private void holdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewSreport.Rows.Count > 0)
            {
                int selectedCell = dataGridViewSreport.SelectedCells[0].RowIndex;
                string shipID = dataGridViewSreport[0, selectedCell].Value.ToString();

                SqlCommand cmdUpdate = new SqlCommand("update shipment set status = 'Hold' where shipmentID = @shipID", conShipment);
                cmdUpdate.Parameters.AddWithValue("@shipID", shipID);
                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdUpdate.ExecuteNonQuery();
                conShipment.Close();

                searchReport();
                MessageBox.Show(String.Format("The status of - {0} - have been successfully changes.", shipID));
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewSreport.Rows.Count > 0)
            {
                int selectedCell = dataGridViewSreport.SelectedCells[0].RowIndex;
                string shipID = dataGridViewSreport[0, selectedCell].Value.ToString();

                if (DialogResult.No == MessageBox.Show(string.Format("Do you sure to delete shipment ID = '{0}' from Detabase", shipID), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    return;
                else
                {

                    SqlCommand cmdDelete = new SqlCommand();
                    cmdDelete.Connection = conShipment;
                    cmdDelete.CommandText = @"delete from detail where shipmentID = @shipID delete from shipment where shipmentID = @shipID";
                    cmdDelete.Parameters.AddWithValue("@shipID", shipID);
                    if (conShipment.State != ConnectionState.Open)
                        conShipment.Open();
                    cmdDelete.ExecuteNonQuery();
                    conShipment.Close();
                    loadShipReport();
                }
            }
        }
        public void loadWaybillRe()
        {
            SqlDataAdapter daList = new SqlDataAdapter();
            SqlCommand cmdList = new SqlCommand();
            cmdList.CommandText = @"select detail.shipmentID, detail.line , detail.lpo , detail.requisition, detail.material, detail.waybill, detail.receipt
from detail
inner join shipment on shipment.shipmentID = detail.shipmentID
where shipment.status = 'Done'";
            cmdList.Connection = conShipment;
            daList.SelectCommand = cmdList;
            DataTable dtList = new DataTable();
            daList.Fill(dtList);

            dataGridViewWaybill.AutoGenerateColumns = false;
            dataGridViewWaybill.DataSource = dtList;
        }

        public void searchWaybill()
        {
            string shipID = textBoxWShip.Text;
            string lpo = textBoxWLPO.Text;
            string req = textBoxWReq.Text;
            string waybill = textBoxWaybill.Text;
            string receipt = comboBoxReceipt.Text;
            string material = textBoxWMaterial.Text;
            bool bReceipt = false;

            if (comboBoxReceipt.Text == "Yes")
                bReceipt = true;
            else
                bReceipt = false;


            string query = @"select detail.shipmentID, detail.line , detail.lpo , detail.requisition, detail.material, detail.waybill, detail.receipt
from detail
inner join shipment on shipment.shipmentID = detail.shipmentID
where shipment.status = 'Done'";
            string criteria = "";

            bool flag = false;

            if (receipt == "All")
            {
                if (shipID == "" & lpo == "" & req == "" & waybill == "" & material == "")
                    loadWaybillRe();
                else
                {

                    if (shipID != string.Empty)
                    {
                        criteria += string.Format(" and detail.shipmentID Like '%{0}%'", shipID);
                        flag = true;
                    }
                    else
                    {
                        criteria += "";
                        flag = true;
                    }
                    if (lpo != string.Empty)
                        if (flag)
                            criteria += string.Format(" and detail.lpo Like '%{0}%'", lpo);
                        else
                        {
                            criteria += "";
                            flag = true;
                        }
                    if (req != string.Empty)
                        if (flag)
                            criteria += string.Format(" and detail.requisition Like '%{0}%'", req);
                        else
                        {
                            criteria += "";
                            flag = true;
                        }
                    if (waybill != string.Empty)
                        if (flag)
                            criteria += string.Format(" and detail.waybill Like '%{0}%'", waybill);
                        else
                        {
                            criteria += "";
                            flag = true;
                        }
                    if (receipt == "All")
                        if (flag)
                            criteria += "";
                        else
                        {
                            criteria += "";
                            flag = true;
                        }
                    if (receipt != "All")
                    {
                        if (bReceipt == true)
                            if (flag)
                                criteria += string.Format(" and detail.receipt = '{0}'", bReceipt);
                            else
                            {
                                criteria += "";
                                flag = true;
                            }
                        if (bReceipt == false)
                            if (flag)
                                criteria += string.Format(" and detail.receipt = '{0}'", bReceipt);
                            else
                            {
                                criteria += "";
                                flag = true;
                            }
                    }

                    if (material != string.Empty)
                        if (flag)
                            criteria += string.Format(" and detail.material Like '%{0}%'", material);
                        else
                        {
                            criteria += "";
                            flag = true;
                        }
                    SqlDataAdapter dalist = new SqlDataAdapter();
                    SqlCommand cmdList = new SqlCommand();
                    cmdList.CommandText = query + criteria;
                    cmdList.Connection = conShipment;
                    dalist.SelectCommand = cmdList;

                    DataTable dtList = new DataTable();
                    dalist.Fill(dtList);

                    dataGridViewWaybill.AutoGenerateColumns = false;
                    dataGridViewWaybill.DataSource = dtList;

                }
            }
            else
            {
                if (shipID == "" & lpo == "" & req == "" & waybill == "" & material == "" & receipt == "All")
                    loadWaybillRe();
                else
                {

                    if (shipID != string.Empty)
                    {
                        criteria += string.Format(" and detail.shipmentID Like '%{0}%'", shipID);
                        flag = true;
                    }
                    else
                    {
                        criteria += "";
                        flag = true;
                    }
                    if (lpo != string.Empty)
                        if (flag)
                            criteria += string.Format(" and detail.lpo Like '%{0}%'", lpo);
                        else
                        {
                            criteria += "";
                            flag = true;
                        }
                    if (req != string.Empty)
                        if (flag)
                            criteria += string.Format(" and detail.requisition Like '%{0}%'", req);
                        else
                        {
                            criteria += "";
                            flag = true;
                        }
                    if (waybill != string.Empty)
                        if (flag)
                            criteria += string.Format(" and detail.waybill Like '%{0}%'", waybill);
                        else
                        {
                            criteria += "";
                            flag = true;
                        }
                    if (receipt == "All")
                        if (flag)
                            criteria += "";
                        else
                        {
                            criteria += "";
                            flag = true;
                        }
                    if (receipt != "All")
                    {
                        if (bReceipt == true)
                            if (flag)
                                criteria += string.Format(" and detail.receipt = '{0}'", bReceipt);
                            else
                            {
                                criteria += "";
                                flag = true;
                            }
                        if (bReceipt == false)
                            if (flag)
                                criteria += string.Format(" and detail.receipt = '{0}'", bReceipt);
                            else
                            {
                                criteria += "";
                                flag = true;
                            }
                    }

                    if (material != string.Empty)
                        if (flag)
                            criteria += string.Format(" and detail.material Like '%{0}%'", material);
                        else
                        {
                            criteria += "";
                            flag = true;
                        }
                    SqlDataAdapter dalist = new SqlDataAdapter();
                    SqlCommand cmdList = new SqlCommand();
                    cmdList.CommandText = query + criteria;
                    cmdList.Connection = conShipment;
                    dalist.SelectCommand = cmdList;

                    DataTable dtList = new DataTable();
                    dalist.Fill(dtList);

                    dataGridViewWaybill.AutoGenerateColumns = false;
                    dataGridViewWaybill.DataSource = dtList;

                }
            }
        }

        private void buttonSrcWaybill_Click(object sender, EventArgs e)
        {
            searchWaybill();
        }

        private void buttonResetWaybill_Click(object sender, EventArgs e)
        {
            loadWaybillRe();
            textBoxWShip.Clear();
            textBoxWLPO.Clear();
            textBoxWReq.Clear();
            textBoxWaybill.Clear();
            textBoxWMaterial.Clear();
            comboBoxReceipt.SelectedItem = "All";
        }

        private void addWaybillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewWaybill.SelectedCells.Count > 0)
            {
                int selectedCell = dataGridViewWaybill.SelectedCells[0].RowIndex;
                string waShipID = dataGridViewWaybill[0, selectedCell].Value.ToString();
                string waLine = dataGridViewWaybill[1, selectedCell].Value.ToString();
                string waLPO = dataGridViewWaybill[2, selectedCell].Value.ToString();

                frmWaybill fWaybill = new frmWaybill();
                fWaybill.shipID = waShipID;
                fWaybill.line = waLine;
                fWaybill.lpo = waLPO;
                fWaybill.ShowDialog();
                searchWaybill();
            }
        }

        private void buttonReceipt_Click(object sender, EventArgs e)
        {
            bool receipt;
            foreach (DataGridViewRow dr in dataGridViewWaybill.Rows)
            {
                string shipID = dr.Cells[0].Value.ToString();
                int line = int.Parse(dr.Cells[1].Value.ToString());
                bool temp = bool.Parse(dr.Cells[6].Value.ToString());
                if (temp == true)
                    receipt = true;
                else
                    receipt = false;

                SqlCommand cmdUpdate = new SqlCommand("update detail set receipt = @receipt where shipmentID = @shipID and line = @line", conShipment);
                cmdUpdate.Parameters.AddWithValue("@receipt", receipt);
                cmdUpdate.Parameters.AddWithValue("@shipID", shipID);
                cmdUpdate.Parameters.AddWithValue("@line", line);
                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdUpdate.ExecuteNonQuery();
                conShipment.Close();
            }
            searchWaybill();
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewSreport.SelectedCells.Count > 0)
            {
                int selectedCell = dataGridViewSreport.SelectedCells[0].RowIndex;
                string shipID = dataGridViewSreport[0, selectedCell].Value.ToString();
                string mode = dataGridViewSreport[5, selectedCell].Value.ToString();
                string firstLine = textBoxDirection.Text;
                string secondLine = mode + @"\" + shipID;

                string docsPath = firstLine + secondLine;
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string windir = Environment.GetEnvironmentVariable("WINDIR");
                System.Diagnostics.Process prc = new System.Diagnostics.Process();
                prc.StartInfo.FileName = windir + @"\explorer.exe";
                prc.StartInfo.Arguments = docsPath;
                prc.Start();

            }
        }

        //private void buttonPShip_Click(object sender, EventArgs e)
        //{
        //    if (textBoxShipID.Text != string.Empty)
        //    {
        //        string shipID = textBoxShipID.Text;

        //        frmRPTPack fPacking = new frmRPTPack("rptPaking.rdlc");
        //        fPacking.shipID = shipID;
        //        fPacking.ShowDialog();
        //    }
        //    else
        //        return;
        //}

        private void printPackingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewSreport.SelectedCells.Count > 0)
            {
                int selectedCell = dataGridViewSreport.SelectedCells[0].RowIndex;
                string shipID = dataGridViewSreport[0, selectedCell].Value.ToString();

                frmRPTPack fPacking = new frmRPTPack("rptPaking.rdlc");
                fPacking.shipID = shipID;
                fPacking.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string status = comboBoxSrcStatus.Text;

            frmRPTPack fReport = new frmRPTPack("rptCritical.rdlc");
            fReport.status = status;
            fReport.ShowDialog();
        }

        private void buttonSaveDir_Click(object sender, EventArgs e)
        {
            string direction = textBoxDirection.Text;
            SqlCommand cmdSave = new SqlCommand("insert info values (@direction)", conShipment);
            cmdSave.Parameters.AddWithValue("@direction", direction);
            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            cmdSave.ExecuteNonQuery();
            conShipment.Close();

            buttonSaveDir.Enabled = false;
            buttonChangeDir.Enabled = true;
        }

        private void buttonChangeDir_Click(object sender, EventArgs e)
        {
            string direction = textBoxDirection.Text;
            SqlCommand cmdUpdate = new SqlCommand("update info set fileDir = @fileDir", conShipment);
            cmdUpdate.Parameters.AddWithValue("@fileDir", direction);
            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            cmdUpdate.ExecuteNonQuery();
            conShipment.Close();

            buttonSaveDir.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            string fileDate = DateTime.Now.ToShortDateString().Replace('/', '_');
            saveDialogBackup.DefaultExt = "Bak";
            saveDialogBackup.FileName = "BackupFile" + fileDate;
            saveDialogBackup.Filter = "SQl Backup files (*.Bak) | *.Bak|ALL Files (*.*) | *.*";
            saveDialogBackup.FilterIndex = 1;
            saveDialogBackup.OverwritePrompt = true;
            saveDialogBackup.Title = "Backup Shipment Database";

            if (saveDialogBackup.ShowDialog() == DialogResult.OK)
            {
                fileName = saveDialogBackup.FileName;
            }
            else
                return;

            try
            {
                string command = @"backup database shipment to Disk = '" + fileName + "'";
                this.Cursor = Cursors.WaitCursor;

                SqlCommand cmdBackup = null;
                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdBackup = new SqlCommand();
                cmdBackup.Connection = conShipment;
                cmdBackup.CommandText = command;
                cmdBackup.ExecuteNonQuery();
                conShipment.Close();
                this.Cursor = Cursors.Default;
                MessageBox.Show("Backup operation successfully done. \n" + DateTime.Now.ToLongDateString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurd:" + ex.Message);
                this.Cursor = Cursors.Default;
            }
        }

        private void buttonRestore_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            openFileDialogRestore.Filter = "SQl Backup files (*.Bak) | *.Bak|ALL Files (*.*) | *.*";
            openFileDialogRestore.FilterIndex = 1;
            openFileDialogRestore.Title = "Restore Shipment Database";

            if (openFileDialogRestore.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialogRestore.FileName;
                try
                {
                    string msg = "Do you want to restore the database to " + openFileDialogRestore.FileName + "\n you will lose all changes during the date";
                    if (DialogResult.No == MessageBox.Show(msg, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3))
                        return;
                    else
                    {
                        string command = "alter database shipment set single_user with rollback immediate " + "use master" + " Restore database shipment from disk ='" + fileName + "'";
                        this.Cursor = Cursors.WaitCursor;

                        SqlCommand cmdRestore = new SqlCommand();
                        if (conShipment.State != ConnectionState.Open)
                            conShipment.Open();
                        cmdRestore.Connection = conShipment;
                        cmdRestore.CommandText = command;

                        cmdRestore.ExecuteNonQuery();
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Restore operation completely done" + DateTime.Now.ToLongDateString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Occured: " + ex.Message);
                    this.Cursor = Cursors.Default;
                }
            }
            else
                return;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show("Are you sure?", "Waring", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                return;
            else
            {
                SqlCommand cmdReset = new SqlCommand();
                cmdReset.CommandText = @"delete from waybill
delete from detail
delete from shipment
delete from project
delete from info

dbcc checkident (waybill, reseed)
dbcc checkident (detail, reseed)
dbcc checkident (info, reseed)";
                cmdReset.Connection = conShipment;
                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdReset.ExecuteNonQuery();
                conShipment.Close();
            }
        }

        private void openFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridViewWaybill.SelectedCells.Count > 0)
            {
                SqlCommand cmdMode = new SqlCommand("select mode from shipment where shipmentID = @shipmentID", conShipment);
                int selectedCell = dataGridViewWaybill.SelectedCells[0].RowIndex;
                string shipID = dataGridViewWaybill[0, selectedCell].Value.ToString();
                cmdMode.Parameters.AddWithValue("@shipmentID", shipID);
                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                string mode = cmdMode.ExecuteScalar().ToString();
                conShipment.Close();
                string firstLine = textBoxDirection.Text;
                string secondLine = mode + @"\" + shipID;

                string docsPath = firstLine + secondLine;
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string windir = Environment.GetEnvironmentVariable("WINDIR");
                System.Diagnostics.Process prc = new System.Diagnostics.Process();
                prc.StartInfo.FileName = windir + @"\explorer.exe";
                prc.StartInfo.Arguments = docsPath;
                prc.Start();

            }
        }

        private void printPackingListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridViewWaybill.SelectedCells.Count > 0)
            {
                int selectedCell = dataGridViewWaybill.SelectedCells[0].RowIndex;
                string shipID = dataGridViewWaybill[0, selectedCell].Value.ToString();

                frmRPTPack fPacking = new frmRPTPack("rptPaking.rdlc");
                fPacking.shipID = shipID;
                fPacking.ShowDialog();
            }
        }

        private void buttonSFinish_Click(object sender, EventArgs e)
        {
            bool sent;
            foreach (DataGridViewRow dr in dataGridViewSreport.Rows)
            {
                string shipID = dr.Cells[0].Value.ToString();
                bool temp = bool.Parse(dr.Cells[11].Value.ToString());
                if (temp == true)
                    sent = true;
                else
                    sent = false;

                SqlCommand cmdUpdate = new SqlCommand("update shipment set finish = @finish where shipmentID = @shipID", conShipment);
                cmdUpdate.Parameters.AddWithValue("@finish", sent);
                cmdUpdate.Parameters.AddWithValue("@shipID", shipID);
                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdUpdate.ExecuteNonQuery();
                conShipment.Close();
            }
            searchReport();
        }

        private void clearanceFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewSreport.SelectedCells.Count > 0)
            {
                int selectedCell = dataGridViewSreport.SelectedCells[0].RowIndex;
                string shipmID = dataGridViewSreport[0, selectedCell].Value.ToString();

                frmClearance fClearance = new frmClearance();
                fClearance.shipID = shipmID;
                fClearance.Show();
            }
        }
        public void clSearch()
        {
            string shipID = textBoxCshipID.Text;
            string start = dateTimePickerCstart.Value.ToShortDateString();
            string end = dateTimePickerCend.Value.ToShortDateString();
            string query = @"select clearance.shipmentID, clearance.projects, clearance.Mateial, clearance.date , clearance.responsible,clearance.poNO, clearance.chargable + SUM(clDetail.amountAFN) as totalAmount
from clearance
inner join clDetail on clearance.shipmentID = clDetail.shipmentID";
            string groupBy = @" group by clearance.shipmentID, clearance.projects, clearance.Mateial, clearance.date,clearance.responsible, clearance.poNO, clearance.chargable";
            string where = "";

            if (shipID == string.Empty)
                where = string.Format(" where date >= '{0}' and date <= '{1}'", start, end);
            else
                where = string.Format(" where clearance.shipmentID Like '%{0}%' and date >= '{1}' and date <= '{2}'", shipID, start, end);

            SqlDataAdapter daList = new SqlDataAdapter();
            SqlCommand cmdList = new SqlCommand();
            cmdList.Connection = conShipment;
            cmdList.CommandText = query + where + groupBy;
            daList.SelectCommand = cmdList;
            DataTable dtList = new DataTable();
            daList.Fill(dtList);

            dataGridViewCresult.AutoGenerateColumns = false;
            dataGridViewCresult.DataSource = dtList;

            clTotal();

        }

        public void clTotal()
        {
            if (dataGridViewCresult.Rows.Count > 0)
            {
                try
                {
                    decimal totalAmount = 0;

                    foreach (DataGridViewRow dr in dataGridViewCresult.Rows)
                    {
                        decimal cellAmount = decimal.Parse(dr.Cells[6].Value.ToString());
                        totalAmount += cellAmount;
                    }

                                      
                    textBoxGrandTotal.Text = totalAmount.ToString();
                    textBoxGrandTotal.TextAlign = HorizontalAlignment.Center;
                    
                }
                catch
                {
                    textBoxGrandTotal.Text = "0";
                    textBoxGrandTotal.TextAlign = HorizontalAlignment.Center;
                }
            }
            else
            {
                textBoxGrandTotal.Text = "0";
                textBoxGrandTotal.TextAlign = HorizontalAlignment.Center;
            }
        }

        public void usdTotal()
        {
            if (dataGridViewSreport.Rows.Count > 0)
            {
                try
                {
                    decimal usdAmount = 0;

                    foreach (DataGridViewRow dr in dataGridViewSreport.Rows)
                    {
                        decimal cellAmount = decimal.Parse(dr.Cells[8].Value.ToString());
                        usdAmount += cellAmount;
                    }

                    decimal usdAmount1 = Math.Round(usdAmount, 2); 
                    textBoxTusdAmount.Text = usdAmount1.ToString();
                    textBoxTusdAmount.TextAlign = HorizontalAlignment.Center; 

                }
                catch
                {
                    textBoxTusdAmount.Text = "0";
                    textBoxTusdAmount.TextAlign = HorizontalAlignment.Center;
                }
            }
            else
            {
                textBoxTusdAmount.Text = "0";
                textBoxTusdAmount.TextAlign = HorizontalAlignment.Center;
            }
        }
        public void loadClReport()
        {
            SqlDataAdapter daList = new SqlDataAdapter();
            SqlCommand cmdList = new SqlCommand();
            cmdList.Connection = conShipment;
            cmdList.CommandText = @"select clearance.shipmentID, clearance.projects, clearance.Mateial, clearance.date , clearance.responsible, clearance.poNO, clearance.chargable + SUM(clDetail.amountAFN) as totalAmount
from clearance
inner join clDetail on clearance.shipmentID = clDetail.shipmentID
group by clearance.shipmentID, clearance.projects, clearance.Mateial, clearance.date,clearance.responsible, clearance.poNO,clearance.chargable";
            daList.SelectCommand = cmdList;

            DataTable dtList = new DataTable();
            daList.Fill(dtList);

            dataGridViewCresult.AutoGenerateColumns = false;
            dataGridViewCresult.DataSource = dtList;

            clTotal();
        }

        private void buttonCsearch_Click(object sender, EventArgs e)
        {
            clSearch();
        }

        private void buttonCreset_Click(object sender, EventArgs e)
        {
            textBoxCshipID.Clear();
            dateTimePickerCstart.Value = DateTime.Parse("10/1/2013");
            dateTimePickerCend.Value = DateTime.Now;
            loadClReport();
            clTotal();
        }

        private void clearanceFormToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridViewCresult.SelectedCells.Count > 0)
            {
                int selectedCell = dataGridViewCresult.SelectedCells[0].RowIndex;
                string shipment = dataGridViewCresult[0, selectedCell].Value.ToString();

                frmClearance fClearance = new frmClearance();
                fClearance.shipID = shipment;
                fClearance.Show();
            }
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(" do you want to delete clearance ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                if (dataGridViewCresult.SelectedCells.Count > 0)
                {
                    int selectedCell = dataGridViewCresult.SelectedCells[0].RowIndex;
                    string shipment = dataGridViewCresult[0, selectedCell].Value.ToString();

                    SqlCommand cmdDelete = new SqlCommand("delete from clDetail where shipmentID = @shipID delete from clearance where shipmentID = @shipID1", conShipment);
                    cmdDelete.Parameters.AddWithValue("shipID", shipment);
                    cmdDelete.Parameters.AddWithValue("@shipID1", shipment);
                    if (conShipment.State != ConnectionState.Open)
                        conShipment.Open();
                    cmdDelete.ExecuteNonQuery();
                    conShipment.Close();

                    loadClReport();
                    MessageBox.Show(string.Format("The clearance date for Shipment ID - {0} - have been successfully deleted", shipment));

                }
                else
                    return;
            }
        }

        private void daFind()
        {

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string shipID = textBoxShipID.Text;
            SqlDataAdapter daFind = new SqlDataAdapter(); 
            SqlCommand cmdFind = new SqlCommand("select * from shipment where shipmentID = @shipment", conShipment);
            cmdFind.Parameters.AddWithValue("@shipment", shipID);
            daFind.SelectCommand = cmdFind;

            DataTable dtFind = new DataTable();
            daFind.Fill(dtFind);

            string from = dtFind.Rows[0][1].ToString();
            DateTime shipDate = DateTime.Parse(dtFind.Rows[0][2].ToString());
            //DateTime arrive = DateTime.Parse(dtFind.Rows[0][3].ToString());

            //int duration = arrival - shipDate;

            string custom = dtFind.Rows[0][4].ToString();
            string respo = dtFind.Rows[0][5].ToString();
            string weigth = dtFind.Rows[0][6].ToString();
            string status = dtFind.Rows[0][7].ToString();
            string currency = dtFind.Rows[0][8].ToString();
            float rate = float.Parse(dtFind.Rows[0][9].ToString());
            string mode = dtFind.Rows[0][10].ToString();
            string doc = dtFind.Rows[0][11].ToString();

            textBoxShipID.Text = shipID; 
            textBoxFrom.Text = from; 
            dateTimePickerShipDate.Value = shipDate;
            textBoxCustom.Text = custom; 
            textBoxRespon.Text = respo; 
            textBoxWeight.Text = weigth; 
            comboBoxStatus.Text = status; 
            textBoxCurrency.Text = currency; 
            textBoxRate.Text = rate.ToString(); 
            comboBoxMode.Text = mode; 
            textBoxDocs.Text = doc;

            daList = new SqlDataAdapter();
            SqlCommand cmdlist = new SqlCommand("select line,lpo,requisition,prjCode,material,remark,amount from detail where shipmentID = @shipid", conShipment);
            cmdlist.Parameters.AddWithValue("@shipid", shipID);
            daList.SelectCommand = cmdlist;

            dtlist = new DataTable();
            daList.Fill(dtlist);

            dataGridViewDetail.AutoGenerateColumns = false;
            dataGridViewDetail.DataSource = dtlist;

            dateTimePickerShipDate.Enabled = false;
            textBoxDuration.Enabled = false;
            buttonSShip.Enabled = false; 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string shipID = textBoxShipID.Text;
            string from = textBoxFrom.Text;
            string custom = textBoxCustom.Text;
            string respo = textBoxRespon.Text;
            string weight = textBoxWeight.Text;
            string status = comboBoxStatus.Text;
            string currency = textBoxCurrency.Text;
            string rate = textBoxRate.Text;
            string mode = comboBoxMode.Text;
            string docs = textBoxDocs.Text;

            SqlCommand cmdupdate = new SqlCommand();
            cmdupdate.CommandText = "update shipment set shipFrom = @from , custom = @custom , responsible=@resp, weight=@weight , status = @status , currency = @currency , rate = @rate , mode=@mode, documentation=@docs where shipmentID = @shipID";
            cmdupdate.Connection = conShipment;

            cmdupdate.Parameters.AddWithValue("@from", from);
            cmdupdate.Parameters.AddWithValue("@custom", custom);
            cmdupdate.Parameters.AddWithValue("@resp", respo);
            cmdupdate.Parameters.AddWithValue("@weight", weight);
            cmdupdate.Parameters.AddWithValue("@status", status);
            cmdupdate.Parameters.AddWithValue("@currency", currency);
            cmdupdate.Parameters.AddWithValue("@rate", rate);
            cmdupdate.Parameters.AddWithValue("@mode", mode);
            cmdupdate.Parameters.AddWithValue("docs", docs);
            cmdupdate.Parameters.AddWithValue("@shipID", shipID);

            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            cmdupdate.ExecuteNonQuery();

            conShipment.Close();


            foreach (DataGridViewRow dr in dataGridViewDetail.Rows)
            {
                string ship = textBoxShipID.Text; 
                string line = dr.Cells[0].Value.ToString();
                string lpo = dr.Cells[1].Value.ToString();
                string req = dr.Cells[2].Value.ToString();
                int prjCode = int.Parse(dr.Cells[3].Value.ToString()); 
                string material = dr.Cells[4].Value.ToString();
                string remark = dr.Cells[5].Value.ToString();
                float amount = float.Parse(dr.Cells[6].Value.ToString());

                SqlCommand cmdlistUpdate = new SqlCommand();
                cmdlistUpdate.CommandText = "update detail set lpo=@lpo, requisition=@req , prjCode=@prjCode , material = @material , remark = @remark , amount=@amount where shipmentID = @ship and line = @line";
                cmdlistUpdate.Connection = conShipment;
                cmdlistUpdate.Parameters.AddWithValue("@lpo", lpo);
                cmdlistUpdate.Parameters.AddWithValue("@req", req);
                cmdlistUpdate.Parameters.AddWithValue("@prjCode", prjCode);
                cmdlistUpdate.Parameters.AddWithValue("@material", material); 
                cmdlistUpdate.Parameters.AddWithValue("@remark",remark);
                cmdlistUpdate.Parameters.AddWithValue("@amount", amount);
                cmdlistUpdate.Parameters.AddWithValue("@ship", ship);
                cmdlistUpdate.Parameters.AddWithValue("@line", line);

                if (conShipment.State != ConnectionState.Open)
                    conShipment.Open();
                cmdlistUpdate.ExecuteNonQuery();
                conShipment.Close();
            }

            MessageBox.Show("The changes successfuly saved");
        }

        void TabControl1_Selectring(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabPageWaybill)
            {
                searchWaybill();
            }
        }

        private void moneyRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewSreport.SelectedCells.Count > 0)
            {
                int selectedCell = dataGridViewSreport.SelectedCells[0].RowIndex;
                string shipmID = dataGridViewSreport[0, selectedCell].Value.ToString();

                frmMRequeset fMrequest = new frmMRequeset();
                fMrequest.shipID = shipmID;
                fMrequest.Show();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

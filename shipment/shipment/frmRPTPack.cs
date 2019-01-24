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
using Microsoft.Reporting.WinForms;

namespace shipment
{
    public partial class frmRPTPack : Form
    {
        string connection = ConfigurationManager.ConnectionStrings["shipment"].ConnectionString;
        SqlConnection conShipment;
        string reportName;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            if (keyData == Keys.F12)
                reportViewerReport.PrintDialog();

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public string shipID { set; get; }
        public string status { set; get; }

        public frmRPTPack(string _reportName)
        {
            reportName = _reportName;
            InitializeComponent();
        }

        private void frmRPTPack_Load(object sender, EventArgs e)
        {
            conShipment = new SqlConnection(connection);
            //// TODO: This line of code loads data into the 'packinglistDataSet.detail' table. You can move, or remove it, as needed.
            //this.detailTableAdapter.Fill(this.packinglistDataSet.detail);
            //this.reportViewerPacking.RefreshReport();
            switch (reportName)
            {
                case "rptCritical.rdlc":
                    rptCritical();
                    break;
                case "rptPaking.rdlc":
                    rptPackingList();
                    break;
                case "rptOther.rdlc":
                    rptOther();
                    break;
                case "rptSlip.rdlc":
                    rptSlip();
                    break;
            }


            this.reportViewerReport.RefreshReport();
        }

        public void rptPackingList()
        {
            SqlDataAdapter daHeader = new SqlDataAdapter();
            SqlCommand cmdHeader = new SqlCommand();
            cmdHeader.CommandText = "select * from shipment where shipmentID = @shipID";
            cmdHeader.Parameters.AddWithValue("@shipID", shipID);
            cmdHeader.Connection = conShipment;

            daHeader.SelectCommand = cmdHeader;
            DataTable dtHeader = new DataTable();
            daHeader.Fill(dtHeader);

            string shipmentID = dtHeader.Rows[0][0].ToString();
            string shipFrom = dtHeader.Rows[0][1].ToString();
            string shipDate = dtHeader.Rows[0][2].ToString();
            string arrival = dtHeader.Rows[0][3].ToString();
            string custom = dtHeader.Rows[0][4].ToString();
            string respon = dtHeader.Rows[0][5].ToString();
            string weight = dtHeader.Rows[0][6].ToString();
            string status = dtHeader.Rows[0][7].ToString();
            string currency = dtHeader.Rows[0][8].ToString();
            string rate = dtHeader.Rows[0][9].ToString();
            string mode = dtHeader.Rows[0][10].ToString();
            string docs = dtHeader.Rows[0][11].ToString();

            SqlDataAdapter daBody = new SqlDataAdapter();
            SqlCommand cmdBody = new SqlCommand();
            cmdBody.CommandText = @"SELECT        detail.id, detail.shipmentID, detail.line, detail.lpo, detail.requisition, detail.prjCode, 
                         detail.material, detail.remark, detail.amount
FROM            detail INNER JOIN
                         project ON detail.prjCode = project.prjCode
where detail.shipmentID = @shipID";
            cmdBody.Parameters.AddWithValue("@shipID", shipID);
            cmdBody.Connection = conShipment;
            daBody.SelectCommand = cmdBody;
            DataTable dtBody = new DataTable();
            daBody.Fill(dtBody);

            SqlCommand cmdFooter = new SqlCommand("select sum(amount) from detail where shipmentID = @shipID", conShipment);
            cmdFooter.Parameters.AddWithValue("@shipID", shipID);
            SqlCommand cmdUSD = new SqlCommand();
            cmdUSD.CommandText = @"select (sum(detail.Amount * shipment.rate)) as usdAmount
from detail
inner join shipment on shipment.shipmentID = detail.shipmentID
where detail.shipmentID = @shipID
group By shipment.rate";
            cmdUSD.Parameters.AddWithValue("@shipID", shipID);
            cmdUSD.Connection = conShipment;



            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            string groundTotal = cmdFooter.ExecuteScalar().ToString();
            string usdAmount = cmdUSD.ExecuteScalar().ToString();
            conShipment.Close();

            ReportDataSource rdsPacking = new ReportDataSource();
            rdsPacking.Name = "DataSet1";
            rdsPacking.Value = dtBody;

            reportViewerReport.LocalReport.ReportEmbeddedResource = string.Format("shipment.{0}", reportName);
            reportViewerReport.LocalReport.DataSources.Clear();
            reportViewerReport.LocalReport.DataSources.Add(rdsPacking);

            reportViewerReport.LocalReport.SetParameters(new ReportParameter("shipID", shipmentID));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("shipFrom", shipFrom));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("shipDate", shipDate));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("arrival", arrival));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("mode", mode));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("currency", currency));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("rate", rate));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("custom", custom));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("responsible", respon));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("weight", weight));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("status", status));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("docs", docs));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("groundTotal", groundTotal));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("usdAmount", usdAmount));


        }

        public void rptCritical()
        {

            string query = "select shipmentID, shipFrom, shipDate, arrivalDate, responsible,mode, custom, status  from shipment";
            string criteria = "";

            if (status == "Critical")
                criteria = " where status <> 'Done'";
            else
                criteria = string.Format(" where status = '{0}'", status);

            SqlDataAdapter daBody = new SqlDataAdapter();
            SqlCommand cmdBody = new SqlCommand();
            cmdBody.CommandText = query + criteria;
            cmdBody.Connection = conShipment;
            daBody.SelectCommand = cmdBody;
            DataTable dtBody = new DataTable();
            daBody.Fill(dtBody);


            ReportDataSource rdsCritical = new ReportDataSource();
            rdsCritical.Name = "DataSet1";
            rdsCritical.Value = dtBody;

            reportViewerReport.LocalReport.ReportEmbeddedResource = string.Format("shipment.{0}", reportName);
            reportViewerReport.LocalReport.DataSources.Clear();
            reportViewerReport.LocalReport.DataSources.Add(rdsCritical);
        }

        public void rptOther()
        {
            SqlDataAdapter daHeader = new SqlDataAdapter();
            SqlCommand cmdHeader = new SqlCommand("select shipmentID, date from clearance where shipmentID = @shipID", conShipment);
            cmdHeader.Parameters.AddWithValue("@shipID", shipID);
            daHeader.SelectCommand = cmdHeader;
            DataTable dtHeader = new DataTable();
            daHeader.Fill(dtHeader);

            string shipmentID = dtHeader.Rows[0][0].ToString();
            string date = dtHeader.Rows[0][1].ToString();

            SqlDataAdapter daBody = new SqlDataAdapter();
            SqlCommand cmdBody = new SqlCommand("select description, amountAFN from clDetail where shipmentID = @shipID", conShipment);
            cmdBody.Parameters.AddWithValue("@shipID", shipID);
            daBody.SelectCommand = cmdBody;

            DataTable dtBody = new DataTable();
            daBody.Fill(dtBody);

            SqlCommand cmdFooter = new SqlCommand("select sum (amountAFN) from clDetail where shipmentID = @shipID", conShipment);
            cmdFooter.Parameters.AddWithValue("@shipID", shipID);
            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            string groundTotal = cmdFooter.ExecuteScalar().ToString();
            conShipment.Close();

            ReportDataSource rdsOther = new ReportDataSource();
            rdsOther.Name = "DataSet";
            rdsOther.Value = dtBody;

            reportViewerReport.LocalReport.ReportEmbeddedResource = string.Format("shipment.{0}", reportName);
            reportViewerReport.LocalReport.DataSources.Clear();
            reportViewerReport.LocalReport.DataSources.Add(rdsOther);

            reportViewerReport.LocalReport.SetParameters(new ReportParameter("shipID", shipmentID));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("date", date));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("gTotal", groundTotal));

        }

        public void rptSlip()
        {
            SqlDataAdapter daSlip = new SqlDataAdapter();
            SqlCommand cmdSlip = new SqlCommand("select * from clearance where shipmentID = @shipID", conShipment);
            cmdSlip.Parameters.AddWithValue("@shipID", shipID);
            daSlip.SelectCommand = cmdSlip;
            DataTable dtSlip = new DataTable();
            daSlip.Fill(dtSlip);

            string resp = dtSlip.Rows[0][4].ToString();

            SqlCommand cmdOther = new SqlCommand("select sum(AmountAFN) from clDetail where shipmentID = @shipID", conShipment);
            cmdOther.Parameters.AddWithValue("@shipID", shipID);

            SqlCommand cmdTotal = new SqlCommand();
            cmdTotal.CommandText = @"select clearance.chargable + SUM(cldetail.amountAFN) as Total 
from clearance
inner join clDetail on clearance.shipmentID = clDetail.shipmentID
where clearance.shipmentID = @shipID
group by clearance.chargable";
            cmdTotal.Connection = conShipment;
            cmdTotal.Parameters.AddWithValue("@shipID", shipID);

            if (conShipment.State != ConnectionState.Open)
                conShipment.Open();
            string other = cmdOther.ExecuteScalar().ToString();
            string total = cmdTotal.ExecuteScalar().ToString();

            conShipment.Close();

            //string project = dtSlip.Rows[0][2].ToString();
            //string material = dtSlip.Rows[0][3].ToString();
            //string invoice = dtSlip.Rows[0][4].ToString();
            //string po = dtSlip.Rows[0][5].ToString();
            //string date = dtSlip.Rows[0][6].ToString();
            //string charge = dtSlip.Rows[0][7].ToString();

            ReportDataSource rdsSlip = new ReportDataSource();
            rdsSlip.Name = "DataSet";
            rdsSlip.Value = dtSlip;

            reportViewerReport.LocalReport.ReportEmbeddedResource = string.Format("shipment.{0}", reportName);
            reportViewerReport.LocalReport.DataSources.Clear();
            reportViewerReport.LocalReport.DataSources.Add(rdsSlip);

            reportViewerReport.LocalReport.SetParameters(new ReportParameter("other", other));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("total", total));
            reportViewerReport.LocalReport.SetParameters(new ReportParameter("respo", resp));
        }
    }
}

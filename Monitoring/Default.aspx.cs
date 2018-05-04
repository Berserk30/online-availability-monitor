using System;
using System.Configuration;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using MySql.Data.MySqlClient;

namespace Monitoring
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(ddlURL.Items.Count == 1 && String.IsNullOrEmpty(ddlURL.Items[0].Text)))
            {
                return;
            }

            String connString = ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();

                String queryStr = "SELECT Url FROM config";
                using (var cmd = new MySqlCommand(queryStr, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var url = reader.GetString(0);
                            ddlURL.Items.Add(url);
                        }
                    }
                }
            }
        }

        protected void ddlURL_SelectedIndexChanged(object sender, EventArgs e)
        {
            Series s = LatencyChart.Series["LatencySeries"];

            String connString = ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
            String url = ddlURL.SelectedValue;
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();

                String latencyQuery = $"SELECT TimestampUtc, LatencyInMsec FROM manusys.data WHERE Url = '{url}' && TimestampUtc > subtime((SELECT MAX(TimestampUtc) AS LatestTimestampUtc FROM manusys.data), '03:00:00.000');";
                DrawChart(LatencyChart.Series["LatencySeries"], conn, latencyQuery);

                String statusQuery = $"SELECT TimestampUtc, HttpStatusCode FROM manusys.data WHERE Url = '{url}' && TimestampUtc > subtime((SELECT MAX(TimestampUtc) AS LatestTimestampUtc FROM manusys.data), '03:00:00.000');";
                DrawChart(StatusChart.Series["StatusSeries"], conn, statusQuery);
            }
        }

        private void DrawChart(Series s, MySqlConnection conn, string queryStr)
        {
            using (var cmd = new MySqlCommand(queryStr, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataPoint p = new DataPoint();
                        p.XValue = DateTime.Parse(reader.GetString(0)).Ticks / 10000000;
                        p.YValues = new double[] { Int32.Parse(reader.GetString(1)) };
                        s.Points.Add(p);
                    }
                }
            }
        }
    }
}

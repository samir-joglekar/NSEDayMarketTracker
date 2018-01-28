using System;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace NSEDayMarketTracker
{
    /// <summary>
    /// Contains logic for the market tracker form.
    /// </summary>
    public partial class MarketTracker : Form
    {
        const string equitiesStockWatchURL = "http://www.nseindia.com/live_market/dynaContent/live_watch/stock_watch/niftyStockWatch.json";

        /// <summary>
        /// The constructor.
        /// </summary>
        public MarketTracker()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Defines what to do when the form is loaded.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The current event object</param>
        private void MarketTracker_Load(object sender, EventArgs e)
        {
            marketSelectComboBox.SelectedIndex = 0;
            JObject niftyEquitiesStockWatchDataJObject = DownloadEquitiesStockWatchData();
            SetMarketOpenCloseValues(niftyEquitiesStockWatchDataJObject);
            SetDateTimeWeek(niftyEquitiesStockWatchDataJObject);
        }

        /// <summary>
        /// Downloads equities stock watch JSON data from the web.
        /// </summary>
        /// <returns>JObject to readily read from.</returns>
        private JObject DownloadEquitiesStockWatchData()
        {
            string niftyStockWatchJSONString = string.Empty;

            using (var webClient = new WebClient())
            {
                // Set headers to download the data
                webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");

                // Download the data
                niftyStockWatchJSONString = webClient.DownloadString(equitiesStockWatchURL);

                // Serialise it into a JObject
                JObject jObject = JObject.Parse(niftyStockWatchJSONString);
                return jObject;
            }
        }

        /// <summary>
        /// Sets market open and close values.
        /// </summary>
        /// <param name="niftyEquitiesStockWatchJObject">The JObject to read from.</param>
        private void SetMarketOpenCloseValues(JObject niftyEquitiesStockWatchJObject)
        {
            openValueLabel.Text = niftyEquitiesStockWatchJObject["latestData"][0]["open"].ToString();
            currentValueLabel.Text = niftyEquitiesStockWatchJObject["latestData"][0]["ltp"].ToString();

            // Calculate percentage difference
            decimal difference = Convert.ToDecimal(currentValueLabel.Text) - Convert.ToDecimal(openValueLabel.Text);
            decimal percentage = Math.Round(difference / Convert.ToDecimal(openValueLabel.Text) * 100, 2);
            string percentageDifference = "" + percentage;
            currentValuePercentageLabel.Text = percentageDifference;

            // Set colours according to result
            if (Convert.ToDecimal(currentValueLabel.Text) >= Convert.ToDecimal(openValueLabel.Text))
            {
                currentValueLabel.BackColor = Color.Green;
                currentValuePercentageLabel.BackColor = Color.Green;
            }
            else
            {
                currentValueLabel.BackColor = Color.Red;
                currentValuePercentageLabel.BackColor = Color.Red;
            }
        }

        /// <summary>
        /// Sets the date, time and the week for the data.
        /// </summary>
        /// <param name="niftyEquitiesStockWatchJObject">The JObject to read from.</param>
        private void SetDateTimeWeek(JObject niftyEquitiesStockWatchJObject)
        {
            dateLabel.Text = niftyEquitiesStockWatchJObject["time"].ToString();

            // Set the week number
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            int weekNumber = cultureInfo.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            weekLabel.Text = "Week " + weekNumber;
        }

        /// <summary>
        /// Refreshes the data and resets all the values to the UI.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The current event object</param>
        private void RefreshMarketButton_Click(object sender, EventArgs e)
        {
            JObject niftyEquitiesStockWatchDataJObject = DownloadEquitiesStockWatchData();
            SetMarketOpenCloseValues(niftyEquitiesStockWatchDataJObject);
            SetDateTimeWeek(niftyEquitiesStockWatchDataJObject);
        }
    }
}
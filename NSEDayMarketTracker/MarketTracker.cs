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
        private void MarketTracker_Load(object sender, System.EventArgs e)
        {
            marketSelectComboBox.SelectedIndex = 0;
            openValueLabel.Text = GetMarketOpenValue();
        }

        /// <summary>
        /// Gets market open value.
        /// </summary>
        /// <returns>Market open value.</returns>
        private string GetMarketOpenValue()
        {
            var equitiesStockWatchURL = "http://www.nseindia.com/live_market/dynaContent/live_watch/stock_watch/niftyStockWatch.json";
            var niftyStockWatchJSONString = string.Empty;
            string marketOpenValue = string.Empty;

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
                niftyStockWatchJSONString = webClient.DownloadString(equitiesStockWatchURL);

                JObject jObject = JObject.Parse(niftyStockWatchJSONString);
                marketOpenValue = jObject["latestData"][0]["open"].ToString();

                return marketOpenValue;
            }
        }
    }
}
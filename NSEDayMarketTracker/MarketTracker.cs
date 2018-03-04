using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace NSEDayMarketTracker
{
    /// <summary>
    /// Contains logic for the market tracker form.
    /// </summary>
    public partial class MarketTracker : Form
    {
        const string EquitiesStockWatchURL = "https://www.nseindia.com/live_market/dynaContent/live_watch/stock_watch/niftyStockWatch.json";
        const string NavigationMenuURL = "https://www.nseindia.com/common/xml/navigation.xml";
        const string NSEIndiaWebsiteURL = "https://www.nseindia.com";

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
            int openMarketBaseNumber = SetMarketOpenCloseValues(niftyEquitiesStockWatchDataJObject);
            SetDateTimeWeek(niftyEquitiesStockWatchDataJObject);
            string liveMarketURL = GetLiveMarketURL();
            HtmlNodeCollection workSetRows = DownloadNIFTYMarketData(liveMarketURL, openMarketBaseNumber);
            RenderStrikePriceDayTable(workSetRows);
            //string[] ara = new string[7] { "Hello", "Hello", "Hello", "Hello", "Hello", "Hello", "Hello"};
            //strikePriceTableDataGridView.Rows.Add(ara);
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
                niftyStockWatchJSONString = webClient.DownloadString(EquitiesStockWatchURL);

                // Serialise it into a JObject
                JObject jObject = JObject.Parse(niftyStockWatchJSONString);
                return jObject;
            }
        }

        /// <summary>
        /// Sets market open and close values.
        /// </summary>
        /// <param name="niftyEquitiesStockWatchJObject">The JObject to read from.</param>
        /// <returns>The open market base number.</returns>
        private int SetMarketOpenCloseValues(JObject niftyEquitiesStockWatchJObject)
        {
            int openMarketBaseNumber = 0;
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

            // Calculate the base open market value
            string precedingNumber = openValueLabel.Text.Split('.')[0];
            precedingNumber = precedingNumber.Replace(",", "");

            if(precedingNumber.Length == 5)
            {
                openMarketBaseNumber = Int32.Parse(precedingNumber) - (Int32.Parse(precedingNumber) % 100);
            }
            return openMarketBaseNumber;
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

        /// <summary>
        /// Downloads the navigation XML file and returns the live market URL.
        /// </summary>
        /// <returns>The live market URL</returns>
        private string GetLiveMarketURL()
        {
            string navigationXML = string.Empty;
            string liveMarketURL = string.Empty;

            using(var webClient = new WebClient())
            {
                // Set headers to download the data
                webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");

                // Download the data
                navigationXML = webClient.DownloadString(NavigationMenuURL);
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
                {
                    IgnoreWhitespace = true
                };
                using(XmlReader xmlReader = XmlReader.Create(new StringReader(navigationXML), xmlReaderSettings))
                {
                    xmlReader.MoveToContent();
                    xmlReader.ReadToDescendant("item");
                    xmlReader.ReadToNextSibling("item");
                    xmlReader.ReadToDescendant("submenu");
                    xmlReader.ReadToNextSibling("submenu");
                    xmlReader.ReadToDescendant("submenuitem");
                    liveMarketURL = xmlReader.GetAttribute("link");
                    return NSEIndiaWebsiteURL + liveMarketURL;
                }
            }
        }

        /// <summary>
        /// Grabs the required trs from the market table after calculating the range from the base number.
        /// </summary>
        /// <param name="marketURL">The market URL</param>
        /// <param name="openMarketBaseNumber">The open market base number</param>
        /// <returns>HtmlNodeCollection</returns>
        private HtmlNodeCollection DownloadNIFTYMarketData(string marketURL, int openMarketBaseNumber)
        {
            // Define the range
            decimal baseNumber = Math.Round(Convert.ToDecimal(openMarketBaseNumber), 2);
            decimal baseNumberPlus50 = baseNumber + 50;
            decimal baseNumberPlus100 = baseNumber + 100;
            decimal baseNumberPlus150 = baseNumber + 150;
            decimal baseNumberPlus200 = baseNumber + 200;
            decimal baseNumberMinus50 = baseNumber - 50;
            decimal baseNumberMinus100 = baseNumber - 100;

            // Grab all rows
            var htmlWeb = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument htmlDocument = htmlWeb.Load(marketURL);
            HtmlNodeCollection tableRows = htmlDocument.DocumentNode.SelectNodes("//table[@id=\"octable\"]//tr");
            tableRows.RemoveAt(tableRows.Count - 1);
            tableRows.RemoveAt(0);
            tableRows.RemoveAt(0);

            // Get only those rows which contain values for the defined tange
            HtmlNodeCollection workSetRows = new HtmlNodeCollection(null);
            foreach (var currentTableRow in tableRows)
            {
                if(currentTableRow.InnerHtml.Contains(baseNumber.ToString()) || currentTableRow.InnerHtml.Contains(baseNumberPlus50.ToString())
                    || currentTableRow.InnerHtml.Contains(baseNumberPlus100.ToString()) || currentTableRow.InnerHtml.Contains(baseNumberMinus50.ToString())
                    || currentTableRow.InnerHtml.Contains(baseNumberMinus100.ToString()) || currentTableRow.InnerHtml.Contains(baseNumberPlus150.ToString())
                    || currentTableRow.InnerHtml.Contains(baseNumberPlus200.ToString()))
                {
                    workSetRows.Add(currentTableRow);
                }
            }
            return workSetRows;
        }

        /// <summary>
        /// Renders data into the Strike Price Day Table.
        /// </summary>
        /// <param name="workSetRows">A collection of HTML nodes</param>
        private void RenderStrikePriceDayTable(HtmlNodeCollection workSetRows)
        {
            List<List<string>> strikePriceDayTableValues = new List<List<string>>();

            // Fetch the list of tds in the row
            foreach(HtmlNode currentWorkSetRow in workSetRows)
            {
                List<string> strikePriceDayTableRowValues = new List<string>();
                foreach(HtmlNode currentNodeInWorkSetRow in currentWorkSetRow.ChildNodes)
                {
                    if(currentNodeInWorkSetRow.Name == "td" && currentNodeInWorkSetRow.InnerText != "")
                    {
                        strikePriceDayTableRowValues.Add(currentNodeInWorkSetRow.InnerText.Trim());
                    }
                }
                strikePriceDayTableValues.Add(strikePriceDayTableRowValues);
            }

            // Remove unwanted data
            foreach(List<string> currentValuesSet in strikePriceDayTableValues)
            {
                currentValuesSet.RemoveAt(0);
                currentValuesSet.RemoveAt(1);
                currentValuesSet.RemoveAt(1);
                currentValuesSet.RemoveAt(2);
                currentValuesSet.RemoveAt(2);
                currentValuesSet.RemoveAt(2);
                currentValuesSet.RemoveAt(2);
                currentValuesSet.RemoveAt(2);
                currentValuesSet.RemoveAt(3);
                currentValuesSet.RemoveAt(4);
                currentValuesSet.RemoveAt(4);
                currentValuesSet.RemoveAt(4);
                currentValuesSet.RemoveAt(4);
                currentValuesSet.RemoveAt(4);
                currentValuesSet.RemoveAt(4);
                currentValuesSet.RemoveAt(5);
                currentValuesSet.Insert(0, "Dummy");
                currentValuesSet.Add("Dummy");

                // And render
                strikePriceTableDataGridView.Rows.Add(currentValuesSet[0], currentValuesSet[1], currentValuesSet[2], currentValuesSet[3], currentValuesSet[4],
                    currentValuesSet[5], currentValuesSet[6]);
            }
        }
    }
}
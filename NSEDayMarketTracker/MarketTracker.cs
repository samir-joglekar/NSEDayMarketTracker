using System;
using System.Collections.Generic;
using System.Drawing;
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
        private const string NSEIndiaWebsiteURL = "https://www1.nseindia.com";
        private const string NIFTYStockWatchURL = NSEIndiaWebsiteURL + "/live_market/dynaContent/live_watch/stock_watch/niftyStockWatch.json";
        private const string BankNIFTYStockWatchURL = NSEIndiaWebsiteURL + "/live_market/dynaContent/live_watch/stock_watch/bankNiftyStockWatch.json";
        private const string NavigationMenuURL = NSEIndiaWebsiteURL + "/common/xml/navigation.xml";
        private const string VIXDetailsJSONURL = NSEIndiaWebsiteURL + "/live_market/dynaContent/live_watch/VixDetails.json";

        private decimal baseNumber = 0;
        private decimal baseNumberPlus50 = 0;
        private decimal baseNumberPlus100 = 0;
        private decimal baseNumberPlus150 = 0;
        private decimal baseNumberPlus200 = 0;
        private decimal baseNumberMinus50 = 0;
        private decimal baseNumberMinus100 = 0;
 
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
            refreshMarketButton.Visible = false;
        }
    
        /// <summary>
        /// Downloads JSON data from the URL.
        /// </summary>
        /// <param name="webResourceURL">The web resource URL of the JSON file.</param>
        /// <returns>JObject to readily read from.</returns>
        private JObject DownloadJSONDataFromURL(string webResourceURL)
        {
            string stockWatchJSONString = string.Empty;

            using(var webClient = new WebClient())
            {
                // Set headers to download the data
                webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");

                // Download the data
                stockWatchJSONString = webClient.DownloadString(webResourceURL);

                // Serialise it into a JObject
                JObject jObject = JObject.Parse(stockWatchJSONString);

                return jObject;
            }
        }

        /// <summary>
        /// Sets market open and close values.
        /// </summary>
        /// <param name="equitiesStockWatchJObject">The JObject to read from.</param>
        /// <returns>The open market base number.</returns>
        private int SetMarketOpenCloseValues(JObject equitiesStockWatchJObject)
        {
            int openMarketBaseNumber = 0;
            openValueLabel.Text = equitiesStockWatchJObject["latestData"][0]["open"].ToString();
            currentValueLabel.Text = equitiesStockWatchJObject["latestData"][0]["ltp"].ToString();

            // Calculate percentage difference
            decimal difference = Convert.ToDecimal(currentValueLabel.Text) - Convert.ToDecimal(openValueLabel.Text);
            decimal percentage = Math.Round(difference / Convert.ToDecimal(openValueLabel.Text) * 100, 2);
            string percentageDifference = "" + percentage;
            currentValuePercentageLabel.Text = percentageDifference;

            // Set colours according to result
            if(Convert.ToDecimal(currentValueLabel.Text) >= Convert.ToDecimal(openValueLabel.Text))
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
        /// <param name="equitiesStockWatchJObject">The JObject to read from.</param>
        private void SetDateTimeWeek(JObject equitiesStockWatchJObject)
        {
            dateLabel.Text = equitiesStockWatchJObject["time"].ToString();
            int weekNumber = 1 | DateTime.Now.Day / 7;
            weekNumber = (DateTime.Now.Day % 7 == 0) ? weekNumber - 1 : weekNumber;
            weekLabel.Text = "Week " + weekNumber;
        }

        /// <summary>
        /// Refreshes the data and resets all the values to the UI.
        /// </summary>
        /// <param name="sender">The sender object</param>*
        /// <param name="e">The current event object</param>
        private void RefreshMarketButton_Click(object sender, EventArgs e)
        {
            refreshMarketButton.Text = "Refreshing...";
            JObject equitiesStockWatchDataJObject = null;

            if(marketSelectComboBox.SelectedItem.ToString() == "NIFTY")
            {
                equitiesStockWatchDataJObject = DownloadJSONDataFromURL(NIFTYStockWatchURL);
            }
            else if(marketSelectComboBox.SelectedItem.ToString() == "Bank NIFTY")
            {
                equitiesStockWatchDataJObject = DownloadJSONDataFromURL(BankNIFTYStockWatchURL);
            }
            int openMarketBaseNumber = SetMarketOpenCloseValues(equitiesStockWatchDataJObject);

            SetDateTimeWeek(equitiesStockWatchDataJObject);

            string liveMarketURL = GetLiveMarketURL();

            if(marketSelectComboBox.SelectedItem.ToString() == "NIFTY")
            {
                HtmlNodeCollection workSetRows = DownloadMarketData(liveMarketURL, openMarketBaseNumber);
                RenderStrikePriceDayTable(workSetRows);
            }
            else if(marketSelectComboBox.SelectedItem.ToString() == "Bank NIFTY")
            {
                string bankNIFTYMarketURL = GetBankNIFTYMarketURL(liveMarketURL);
                HtmlNodeCollection workSetRows = DownloadMarketData(bankNIFTYMarketURL, openMarketBaseNumber);
                RenderStrikePriceDayTable(workSetRows);
            }
            
            refreshMarketButton.Text = "Refresh";
        }

        /// <summary>
        /// Downloads the navigation XML file and returns the live market URL.
        /// </summary>
        /// <returns>The live market URL</returns>
        private string GetLiveMarketURL()
        {
            string marketvalue = marketSelectComboBox.SelectedItem.ToString();
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
        private HtmlNodeCollection DownloadMarketData(string marketURL, int openMarketBaseNumber)
        {
            // Define the range
            baseNumber = Math.Round(Convert.ToDecimal(openMarketBaseNumber), 2);
            baseNumberPlus50 = baseNumber + 50;
            baseNumberPlus100 = baseNumber + 100;
            baseNumberPlus150 = baseNumber + 150;
            baseNumberPlus200 = baseNumber + 200;
            baseNumberMinus50 = baseNumber - 50;
            baseNumberMinus100 = baseNumber - 100;

            // Grab all rows
            var htmlWeb = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument htmlDocument = htmlWeb.LoadFromBrowser(marketURL);

            HtmlNodeCollection tableRows = htmlDocument.DocumentNode.SelectNodes("//table[@id=\"octable\"]//tr");
            tableRows.RemoveAt(tableRows.Count - 1);
            tableRows.RemoveAt(0);
            tableRows.RemoveAt(0);

            // Get only those rows which contain values for the defined tange
            HtmlNodeCollection workSetRows = new HtmlNodeCollection(null);
            foreach(var currentTableRow in tableRows)
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
        /// Returns the Bank NIFTY page URL from the main market URL JavaScript snippet.
        /// </summary>
        /// <param name="marketURL">The market URL for Bank NIFTY.</param>
        /// <returns>The Bank NIFTY URL.</returns>
        private string GetBankNIFTYMarketURL(string marketURL)
        {
            // Load the web page and get the JavaScript
            var htmlWeb = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument htmlDocument = htmlWeb.LoadFromBrowser(marketURL);
            HtmlNodeCollection scriptTags = htmlDocument.DocumentNode.SelectNodes("//script[@type=\"text/javascript\"]");
            string bankNIFTYMarketURL = scriptTags[17].InnerHtml;

            // Process the JavaScript and get the Bank NIFTY URL
            bankNIFTYMarketURL = bankNIFTYMarketURL.Replace("\r\n", "");
            bankNIFTYMarketURL = bankNIFTYMarketURL.Replace("\t", "");
            bankNIFTYMarketURL = bankNIFTYMarketURL.Remove(0, bankNIFTYMarketURL.IndexOf("BANKNIFTY"));
            bankNIFTYMarketURL = bankNIFTYMarketURL.Remove(0, 28);
            bankNIFTYMarketURL = bankNIFTYMarketURL.Remove(bankNIFTYMarketURL.IndexOf(";"), bankNIFTYMarketURL.Length - bankNIFTYMarketURL.IndexOf(";"));
            bankNIFTYMarketURL = bankNIFTYMarketURL.Replace("'", "");

            return NSEIndiaWebsiteURL + "/" + bankNIFTYMarketURL;
        }

        /// <summary>
        /// Renders data into the Strike Price Day Table.
        /// </summary>
        /// <param name="workSetRows">A collection of HTML nodes</param>
        private void RenderStrikePriceDayTable(HtmlNodeCollection workSetRows)
        {
            strikePriceTableDataGridView.Rows.Clear();
            List<List<string>> strikePriceDayTableValues = new List<List<string>>();
            List<int> indicesOfRowsToHighlight = new List<int>();

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
                currentValuesSet.Add("Put Writers");

                // And render
                strikePriceTableDataGridView.Rows.Add(currentValuesSet[0], currentValuesSet[1], currentValuesSet[2], currentValuesSet[3], currentValuesSet[4],
                    currentValuesSet[5], currentValuesSet[6]);
            }

            // Find rows to highlight
            foreach(DataGridViewRow currentRowCells in strikePriceTableDataGridView.Rows)
            {
                for(int currentRowCellsIndex = 0; currentRowCellsIndex < currentRowCells.Cells.Count; currentRowCellsIndex++)
                {
                    if(currentRowCells.Cells[1].Value.ToString().Contains("-"))
                    {
                        currentRowCells.Cells[0].Style.BackColor = Color.LightGreen;
                        currentRowCells.Cells[0].Value = "CEW Exiting";
                    }
                    else
                    {
                        currentRowCells.Cells[0].Style.BackColor = Color.PaleVioletRed;
                        currentRowCells.Cells[0].Value = "Call Writers";
                    }
                    currentRowCells.Cells[6].Style.BackColor = Color.LightGreen;
                }

                foreach(DataGridViewCell currentRowCell in currentRowCells.Cells)
                {
                    if(currentRowCell.Value.Equals(baseNumber.ToString() + ".00") || currentRowCell.Value.Equals(baseNumberPlus50.ToString() + ".00")
                        || currentRowCell.Value.Equals(baseNumberPlus100.ToString() + ".00"))
                    {
                        indicesOfRowsToHighlight.Add(currentRowCell.RowIndex);
                    }
                }
            }

            // Highlight range cells with blue
            foreach(int rowIndex in indicesOfRowsToHighlight)
            {
                DataGridViewRow currentRowCells = strikePriceTableDataGridView.Rows[rowIndex];

                foreach(DataGridViewCell currentRowCell in currentRowCells.Cells)
                {
                    if(!(currentRowCell.Value.ToString().Equals("CEW Exiting") || currentRowCell.Value.ToString().Equals("Call Writers")
                        || currentRowCell.Value.ToString().Equals("Put Writers")))
                    {
                        currentRowCell.Style.BackColor = Color.LightBlue;
                    }
                }
            }

            UpdateDayTable(indicesOfRowsToHighlight);
        }

        /// <summary>
        /// Updates day table with rows.
        /// </summary>
        /// <param name="rowsIndex">The list of rows indices with which to work upon.</param>
        private void UpdateDayTable(List<int> rowsIndex)
        {
            List<string> ceValues = new List<string>();
            List<string> peValues = new List<string>();

            foreach(int currentRowIndex in rowsIndex)
            {
                DataGridViewRow currentHighlightedRowCells = strikePriceTableDataGridView.Rows[currentRowIndex];
                ceValues.Add(currentHighlightedRowCells.Cells[1].Value.ToString());
                peValues.Add(currentHighlightedRowCells.Cells[5].Value.ToString());
            }

            int ceTotal = 0;
            int peTotal = 0;

            foreach(string currentCEValue in ceValues)
            {
                ceTotal += Int32.Parse(currentCEValue.Replace(",", ""));
            }

            foreach(string currentPEValue in peValues)
            {
                peTotal += Int32.Parse(currentPEValue.Replace(",", ""));
            }

            int percentage = (peTotal - ceTotal) / ceTotal * 100;
            dayTableDataGridView.Rows.Add(DateTime.Now.ToLocalTime().ToLongTimeString(), currentValueLabel.Text, ceTotal, percentage, peTotal);
        }

        /// <summary>
        /// Refreshes the data after every 5 seconds.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The current event object</param>
        private void RefreshTimer_Tick(object sender, EventArgs eventArgs)
        {
            if(marketSelectComboBox.SelectedItem != null)
            {
                refreshMarketButton.Visible = true;
                refreshMarketButton.PerformClick();
            }
        }

        /// <summary>
        /// Sets values for VIX.
        /// </summary>
        /// <param name="vixJObject"></param>
        private void SetVIXValues(JObject vixJObject)
        {
            vixValueLabel.Text = vixJObject["currentVixSnapShot"][0]["CURRENT_PRICE"].ToString();
            vixValuePercentageLabel.Text = vixJObject["currentVixSnapShot"][0]["PERC_CHANGE"].ToString();
            string previousVIXClose = vixJObject["currentVixSnapShot"][0]["PREV_CLOSE"].ToString();

            // Calculate percentage difference
            decimal difference = Convert.ToDecimal(vixValueLabel.Text) - Convert.ToDecimal(previousVIXClose);
            decimal percentage = Math.Round(difference / Convert.ToDecimal(vixValueLabel.Text) * 100, 2);
            string percentageDifference = "" + percentage;

            // Set colours according to result
            if(Convert.ToDecimal(vixValueLabel.Text) >= Convert.ToDecimal(previousVIXClose))
            {
                vixValueLabel.BackColor = Color.Green;
                vixValuePercentageLabel.BackColor = Color.Green;
            }
            else
            {
                vixValueLabel.BackColor = Color.Red;
                vixValuePercentageLabel.BackColor = Color.Red;
            }
        }
    }
}
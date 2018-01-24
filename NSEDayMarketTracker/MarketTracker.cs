using System.Windows.Forms;

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
        }
    }
}
namespace NSEDayMarketTracker
{
    partial class MarketTracker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.marketSummaryGroupBox = new System.Windows.Forms.GroupBox();
            this.weekLabel = new System.Windows.Forms.Label();
            this.dateLabel = new System.Windows.Forms.Label();
            this.vixValuePercentageLabel = new System.Windows.Forms.Label();
            this.vixValueLabel = new System.Windows.Forms.Label();
            this.vixLabel = new System.Windows.Forms.Label();
            this.currentValuePercentageLabel = new System.Windows.Forms.Label();
            this.currentValueLabel = new System.Windows.Forms.Label();
            this.currentLabel = new System.Windows.Forms.Label();
            this.openValueLabel = new System.Windows.Forms.Label();
            this.openLabel = new System.Windows.Forms.Label();
            this.marketSelectComboBox = new System.Windows.Forms.ComboBox();
            this.refreshMarketButton = new System.Windows.Forms.Button();
            this.dayTableGroupBox = new System.Windows.Forms.GroupBox();
            this.dayTableDataGridView = new System.Windows.Forms.DataGridView();
            this.PETotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Percentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CETotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.marketSummaryGroupBox.SuspendLayout();
            this.dayTableGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dayTableDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // marketSummaryGroupBox
            // 
            this.marketSummaryGroupBox.BackColor = System.Drawing.SystemColors.Window;
            this.marketSummaryGroupBox.Controls.Add(this.weekLabel);
            this.marketSummaryGroupBox.Controls.Add(this.dateLabel);
            this.marketSummaryGroupBox.Controls.Add(this.vixValuePercentageLabel);
            this.marketSummaryGroupBox.Controls.Add(this.vixValueLabel);
            this.marketSummaryGroupBox.Controls.Add(this.vixLabel);
            this.marketSummaryGroupBox.Controls.Add(this.currentValuePercentageLabel);
            this.marketSummaryGroupBox.Controls.Add(this.currentValueLabel);
            this.marketSummaryGroupBox.Controls.Add(this.currentLabel);
            this.marketSummaryGroupBox.Controls.Add(this.openValueLabel);
            this.marketSummaryGroupBox.Controls.Add(this.openLabel);
            this.marketSummaryGroupBox.Controls.Add(this.marketSelectComboBox);
            this.marketSummaryGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.marketSummaryGroupBox.Location = new System.Drawing.Point(10, 10);
            this.marketSummaryGroupBox.Name = "marketSummaryGroupBox";
            this.marketSummaryGroupBox.Size = new System.Drawing.Size(988, 60);
            this.marketSummaryGroupBox.TabIndex = 0;
            this.marketSummaryGroupBox.TabStop = false;
            this.marketSummaryGroupBox.Text = "Market";
            // 
            // weekLabel
            // 
            this.weekLabel.AutoSize = true;
            this.weekLabel.Location = new System.Drawing.Point(829, 25);
            this.weekLabel.Name = "weekLabel";
            this.weekLabel.Size = new System.Drawing.Size(64, 17);
            this.weekLabel.TabIndex = 1;
            this.weekLabel.Text = "Week 04";
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(704, 25);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(119, 17);
            this.dateLabel.TabIndex = 9;
            this.dateLabel.Text = "January 24, 2017";
            // 
            // vixValuePercentageLabel
            // 
            this.vixValuePercentageLabel.AutoSize = true;
            this.vixValuePercentageLabel.BackColor = System.Drawing.Color.Green;
            this.vixValuePercentageLabel.ForeColor = System.Drawing.Color.White;
            this.vixValuePercentageLabel.Location = new System.Drawing.Point(616, 25);
            this.vixValuePercentageLabel.Name = "vixValuePercentageLabel";
            this.vixValuePercentageLabel.Size = new System.Drawing.Size(48, 17);
            this.vixValuePercentageLabel.TabIndex = 8;
            this.vixValuePercentageLabel.Text = "0.51%";
            // 
            // vixValueLabel
            // 
            this.vixValueLabel.AutoSize = true;
            this.vixValueLabel.BackColor = System.Drawing.Color.Green;
            this.vixValueLabel.ForeColor = System.Drawing.Color.White;
            this.vixValueLabel.Location = new System.Drawing.Point(541, 25);
            this.vixValueLabel.Name = "vixValueLabel";
            this.vixValueLabel.Size = new System.Drawing.Size(68, 17);
            this.vixValueLabel.TabIndex = 7;
            this.vixValueLabel.Text = "12345.67";
            // 
            // vixLabel
            // 
            this.vixLabel.AutoSize = true;
            this.vixLabel.Location = new System.Drawing.Point(501, 25);
            this.vixLabel.Name = "vixLabel";
            this.vixLabel.Size = new System.Drawing.Size(33, 17);
            this.vixLabel.TabIndex = 6;
            this.vixLabel.Text = "VIX:";
            // 
            // currentValuePercentageLabel
            // 
            this.currentValuePercentageLabel.AutoSize = true;
            this.currentValuePercentageLabel.BackColor = System.Drawing.Color.Green;
            this.currentValuePercentageLabel.ForeColor = System.Drawing.Color.White;
            this.currentValuePercentageLabel.Location = new System.Drawing.Point(424, 25);
            this.currentValuePercentageLabel.Name = "currentValuePercentageLabel";
            this.currentValuePercentageLabel.Size = new System.Drawing.Size(48, 17);
            this.currentValuePercentageLabel.TabIndex = 5;
            this.currentValuePercentageLabel.Text = "0.51%";
            // 
            // currentValueLabel
            // 
            this.currentValueLabel.AutoSize = true;
            this.currentValueLabel.BackColor = System.Drawing.Color.Green;
            this.currentValueLabel.ForeColor = System.Drawing.Color.White;
            this.currentValueLabel.Location = new System.Drawing.Point(350, 25);
            this.currentValueLabel.Name = "currentValueLabel";
            this.currentValueLabel.Size = new System.Drawing.Size(68, 17);
            this.currentValueLabel.TabIndex = 4;
            this.currentValueLabel.Text = "12345.67";
            // 
            // currentLabel
            // 
            this.currentLabel.AutoSize = true;
            this.currentLabel.Location = new System.Drawing.Point(285, 25);
            this.currentLabel.Name = "currentLabel";
            this.currentLabel.Size = new System.Drawing.Size(59, 17);
            this.currentLabel.TabIndex = 3;
            this.currentLabel.Text = "Current:";
            // 
            // openValueLabel
            // 
            this.openValueLabel.AutoSize = true;
            this.openValueLabel.ForeColor = System.Drawing.Color.Blue;
            this.openValueLabel.Location = new System.Drawing.Point(197, 25);
            this.openValueLabel.Name = "openValueLabel";
            this.openValueLabel.Size = new System.Drawing.Size(68, 17);
            this.openValueLabel.TabIndex = 2;
            this.openValueLabel.Text = "12345.67";
            // 
            // openLabel
            // 
            this.openLabel.AutoSize = true;
            this.openLabel.Location = new System.Drawing.Point(144, 25);
            this.openLabel.Name = "openLabel";
            this.openLabel.Size = new System.Drawing.Size(47, 17);
            this.openLabel.TabIndex = 1;
            this.openLabel.Text = "Open:";
            // 
            // marketSelectComboBox
            // 
            this.marketSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.marketSelectComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.marketSelectComboBox.FormattingEnabled = true;
            this.marketSelectComboBox.Items.AddRange(new object[] {
            "NIFTY",
            "Bank NIFTY"});
            this.marketSelectComboBox.Location = new System.Drawing.Point(6, 22);
            this.marketSelectComboBox.Name = "marketSelectComboBox";
            this.marketSelectComboBox.Size = new System.Drawing.Size(121, 24);
            this.marketSelectComboBox.TabIndex = 0;
            // 
            // refreshMarketButton
            // 
            this.refreshMarketButton.BackColor = System.Drawing.Color.White;
            this.refreshMarketButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshMarketButton.Location = new System.Drawing.Point(1024, 24);
            this.refreshMarketButton.Name = "refreshMarketButton";
            this.refreshMarketButton.Size = new System.Drawing.Size(116, 38);
            this.refreshMarketButton.TabIndex = 1;
            this.refreshMarketButton.Text = "Refresh Data";
            this.refreshMarketButton.UseVisualStyleBackColor = false;
            // 
            // dayTableGroupBox
            // 
            this.dayTableGroupBox.Controls.Add(this.dayTableDataGridView);
            this.dayTableGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dayTableGroupBox.Location = new System.Drawing.Point(10, 76);
            this.dayTableGroupBox.Name = "dayTableGroupBox";
            this.dayTableGroupBox.Size = new System.Drawing.Size(562, 258);
            this.dayTableGroupBox.TabIndex = 2;
            this.dayTableGroupBox.TabStop = false;
            this.dayTableGroupBox.Text = "Day Table";
            // 
            // dayTableDataGridView
            // 
            this.dayTableDataGridView.AllowUserToAddRows = false;
            this.dayTableDataGridView.AllowUserToDeleteRows = false;
            this.dayTableDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.dayTableDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dayTableDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time,
            this.Price,
            this.CETotal,
            this.Percentage,
            this.PETotal});
            this.dayTableDataGridView.Location = new System.Drawing.Point(7, 22);
            this.dayTableDataGridView.Name = "dayTableDataGridView";
            this.dayTableDataGridView.ReadOnly = true;
            this.dayTableDataGridView.Size = new System.Drawing.Size(546, 224);
            this.dayTableDataGridView.TabIndex = 0;
            // 
            // PETotal
            // 
            this.PETotal.Frozen = true;
            this.PETotal.HeaderText = "PE Total";
            this.PETotal.Name = "PETotal";
            this.PETotal.ReadOnly = true;
            // 
            // Percentage
            // 
            this.Percentage.Frozen = true;
            this.Percentage.HeaderText = "Percentage";
            this.Percentage.Name = "Percentage";
            this.Percentage.ReadOnly = true;
            // 
            // CETotal
            // 
            this.CETotal.Frozen = true;
            this.CETotal.HeaderText = "CE Total";
            this.CETotal.Name = "CETotal";
            this.CETotal.ReadOnly = true;
            // 
            // Price
            // 
            this.Price.Frozen = true;
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            // 
            // Time
            // 
            this.Time.Frozen = true;
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            // 
            // MarketTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1244, 523);
            this.Controls.Add(this.dayTableGroupBox);
            this.Controls.Add(this.refreshMarketButton);
            this.Controls.Add(this.marketSummaryGroupBox);
            this.MaximizeBox = false;
            this.Name = "MarketTracker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NSE Day Market Tracker";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MarketTracker_Load);
            this.marketSummaryGroupBox.ResumeLayout(false);
            this.marketSummaryGroupBox.PerformLayout();
            this.dayTableGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dayTableDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox marketSummaryGroupBox;
        private System.Windows.Forms.ComboBox marketSelectComboBox;
        private System.Windows.Forms.Label openLabel;
        private System.Windows.Forms.Label openValueLabel;
        private System.Windows.Forms.Label currentLabel;
        private System.Windows.Forms.Label currentValueLabel;
        private System.Windows.Forms.Label currentValuePercentageLabel;
        private System.Windows.Forms.Label vixLabel;
        private System.Windows.Forms.Label vixValueLabel;
        private System.Windows.Forms.Label vixValuePercentageLabel;
        private System.Windows.Forms.Label weekLabel;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Button refreshMarketButton;
        private System.Windows.Forms.GroupBox dayTableGroupBox;
        private System.Windows.Forms.DataGridView dayTableDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn CETotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Percentage;
        private System.Windows.Forms.DataGridViewTextBoxColumn PETotal;
    }
}
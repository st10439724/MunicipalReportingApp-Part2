using System;
using System.Drawing;
using System.Windows.Forms;
using MunicipalReportingApp.Common;

namespace MunicipalReportingApp.Forms
{
    partial class EventsForm
    {
        private System.ComponentModel.IContainer components = null;

        // ---- Header ----
        private Panel pnlHeader = null!;
        private PictureBox pbLogo = null!;
        private Label lblTitle = null!;
        private Label lblHeaderSub = null!;
        private Button btnBack = null!;

        // ---- Announcements strip ----
        private Panel pnlAnnouncements = null!;
        private Label chipAnnouncements = null!;
        private FlowLayoutPanel flpAnnouncements = null!;

        // ---- Search bar ----
        private TableLayoutPanel tblSearch = null!;
        private Label chipKeyword = null!;
        private Label chipCategory = null!;
        private Label chipFrom = null!;
        private Label chipTo = null!;
        private TextBox txtKeyword = null!;
        private ComboBox cmbCategory = null!;
        private DateTimePicker dtpFrom = null!;
        private DateTimePicker dtpTo = null!;
        private FlowLayoutPanel pnlSearchButtons = null!;
        private Button btnSearch = null!;
        private Button btnClear = null!;
        private Button btnPrevious = null!;

        // ---- Body: events (left) and side panels (right) ----
        private TableLayoutPanel tblBody = null!;
        private Panel pnlLeft = null!;
        private Label lblResults = null!;
        private FlowLayoutPanel flpEvents = null!;

        private TableLayoutPanel tblSide = null!;

        private Panel pnlDetails = null!;
        private Label chipDetails = null!;
        private Label lblDetailTitle = null!;
        private Label lblDetailMeta = null!;
        private Label lblDetailBody = null!;
        private Label lblRelated = null!;

        private Panel pnlRecommended = null!;
        private Label chipRecommended = null!;
        private Button btnResetPrefs = null!;
        private FlowLayoutPanel flpRecommended = null!;

        private Panel pnlRecent = null!;
        private TableLayoutPanel tblRecent = null!;
        private Label chipRecentViewed = null!;
        private Label chipRecentSearches = null!;
        private ListBox lstRecentViewed = null!;
        private ListBox lstRecentSearches = null!;

        private ToolTip toolTip = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.pnlHeader = new Panel();
            this.pbLogo = new PictureBox();
            this.lblTitle = new Label();
            this.lblHeaderSub = new Label();
            this.btnBack = new Button();

            this.pnlAnnouncements = new Panel();
            this.chipAnnouncements = new Label();
            this.flpAnnouncements = new FlowLayoutPanel();

            this.tblSearch = new TableLayoutPanel();
            this.chipKeyword = new Label();
            this.chipCategory = new Label();
            this.chipFrom = new Label();
            this.chipTo = new Label();
            this.txtKeyword = new TextBox();
            this.cmbCategory = new ComboBox();
            this.dtpFrom = new DateTimePicker();
            this.dtpTo = new DateTimePicker();
            this.pnlSearchButtons = new FlowLayoutPanel();
            this.btnSearch = new Button();
            this.btnClear = new Button();
            this.btnPrevious = new Button();

            this.tblBody = new TableLayoutPanel();
            this.pnlLeft = new Panel();
            this.lblResults = new Label();
            this.flpEvents = new FlowLayoutPanel();

            this.tblSide = new TableLayoutPanel();

            this.pnlDetails = new Panel();
            this.chipDetails = new Label();
            this.lblDetailTitle = new Label();
            this.lblDetailMeta = new Label();
            this.lblDetailBody = new Label();
            this.lblRelated = new Label();

            this.pnlRecommended = new Panel();
            this.chipRecommended = new Label();
            this.btnResetPrefs = new Button();
            this.flpRecommended = new FlowLayoutPanel();

            this.pnlRecent = new Panel();
            this.tblRecent = new TableLayoutPanel();
            this.chipRecentViewed = new Label();
            this.chipRecentSearches = new Label();
            this.lstRecentViewed = new ListBox();
            this.lstRecentSearches = new ListBox();

            this.toolTip = new ToolTip(this.components);

            this.SuspendLayout();

            // Set first: the header button and announcement strip are placed
            // relative to the form width.
            this.ClientSize = new Size(1100, 700);

            // =============================================================
            //  HEADER (deep blue - distinct from the teal menu and purple report form)
            // =============================================================
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 70;
            this.pnlHeader.BackColor = AppTheme.TagBlueText;
            this.pnlHeader.Controls.Add(this.pbLogo);
            this.pnlHeader.Controls.Add(this.btnBack);
            this.pnlHeader.Controls.Add(this.lblHeaderSub);
            this.pnlHeader.Controls.Add(this.lblTitle);

            // White tile so the logo's white background looks intentional on the blue header.
            this.pbLogo.BackColor = Color.White;
            this.pbLogo.SizeMode = PictureBoxSizeMode.Zoom;
            this.pbLogo.Image = BrandImages.Logo;
            this.pbLogo.Size = new Size(108, 54);
            this.pbLogo.Location = new Point(16, 8);

            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = Color.Transparent;
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            this.lblTitle.Location = new Point(140, 10);
            this.lblTitle.Text = "Local Events and Announcements";

            this.lblHeaderSub.AutoSize = true;
            this.lblHeaderSub.BackColor = Color.Transparent;
            this.lblHeaderSub.ForeColor = AppTheme.TagBlueTint;
            this.lblHeaderSub.Font = AppTheme.SubtitleFont;
            this.lblHeaderSub.Location = new Point(142, 42);
            this.lblHeaderSub.Text = "Find out what is happening in your community.";

            this.btnBack.Text = "← Back to Main Menu";
            this.btnBack.Font = AppTheme.FieldFont;
            this.btnBack.BackColor = Color.White;
            this.btnBack.ForeColor = AppTheme.TagBlueText;
            this.btnBack.FlatStyle = FlatStyle.Flat;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.Size = new Size(180, 38);
            this.btnBack.Location = new Point(this.ClientSize.Width - 204, 16);
            this.btnBack.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnBack.Cursor = Cursors.Hand;

            // =============================================================
            //  ANNOUNCEMENTS STRIP
            // =============================================================
            this.pnlAnnouncements.Dock = DockStyle.Top;
            this.pnlAnnouncements.Height = 118;
            this.pnlAnnouncements.BackColor = AppTheme.BackgroundColor;
            this.pnlAnnouncements.Controls.Add(this.flpAnnouncements);
            this.pnlAnnouncements.Controls.Add(this.chipAnnouncements);

            UiHelpers.StyleAsTag(this.chipAnnouncements, AppTheme.TagAmberTint, AppTheme.TagAmberText);
            this.chipAnnouncements.Text = "Announcements (urgent first)";
            this.chipAnnouncements.Location = new Point(24, 8);

            this.flpAnnouncements.Location = new Point(24, 38);
            this.flpAnnouncements.Size = new Size(this.ClientSize.Width - 48, 74);
            this.flpAnnouncements.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.flpAnnouncements.FlowDirection = FlowDirection.LeftToRight;
            this.flpAnnouncements.WrapContents = false;
            this.flpAnnouncements.AutoScroll = true;
            this.flpAnnouncements.BackColor = AppTheme.BackgroundColor;

            // =============================================================
            //  SEARCH BAR
            // =============================================================
            this.tblSearch.Dock = DockStyle.Top;
            this.tblSearch.Height = 92;
            this.tblSearch.BackColor = AppTheme.PanelColor;
            this.tblSearch.Padding = new Padding(24, 8, 24, 8);
            this.tblSearch.ColumnCount = 5;
            this.tblSearch.RowCount = 2;
            this.tblSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));      // keyword
            this.tblSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));      // category
            this.tblSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));    // from
            this.tblSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));    // to
            this.tblSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 370F));    // buttons
            this.tblSearch.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            this.tblSearch.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));

            UiHelpers.StyleAsTag(this.chipKeyword, AppTheme.TagBlueTint, AppTheme.TagBlueText);
            this.chipKeyword.Text = "Search events";

            UiHelpers.StyleAsTag(this.chipCategory, AppTheme.TagPurpleTint, AppTheme.TagPurpleText);
            this.chipCategory.Text = "Category";

            UiHelpers.StyleAsTag(this.chipFrom, AppTheme.TagTealTint, AppTheme.TagTealText);
            this.chipFrom.Text = "From date";

            UiHelpers.StyleAsTag(this.chipTo, AppTheme.TagTealTint, AppTheme.TagTealText);
            this.chipTo.Text = "To date";

            this.txtKeyword.Font = AppTheme.FieldFont;
            this.txtKeyword.PlaceholderText = "e.g. music, clinic, market...";
            this.txtKeyword.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtKeyword.Margin = new Padding(0, 6, 12, 0);

            this.cmbCategory.Font = AppTheme.FieldFont;
            this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCategory.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.cmbCategory.Margin = new Padding(0, 6, 12, 0);

            this.dtpFrom.Font = AppTheme.FieldFont;
            this.dtpFrom.Format = DateTimePickerFormat.Short;
            this.dtpFrom.ShowCheckBox = true;
            this.dtpFrom.Checked = false;
            this.dtpFrom.MinDate = DateTime.Today;
            this.dtpFrom.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.dtpFrom.Margin = new Padding(0, 6, 12, 0);

            this.dtpTo.Font = AppTheme.FieldFont;
            this.dtpTo.Format = DateTimePickerFormat.Short;
            this.dtpTo.ShowCheckBox = true;
            this.dtpTo.Checked = false;
            this.dtpTo.MinDate = DateTime.Today;
            this.dtpTo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.dtpTo.Margin = new Padding(0, 6, 12, 0);

            this.pnlSearchButtons.Dock = DockStyle.Fill;
            this.pnlSearchButtons.FlowDirection = FlowDirection.LeftToRight;
            this.pnlSearchButtons.WrapContents = false;
            this.pnlSearchButtons.Controls.Add(this.btnSearch);
            this.pnlSearchButtons.Controls.Add(this.btnClear);
            this.pnlSearchButtons.Controls.Add(this.btnPrevious);

            this.btnSearch.Text = "\U0001F50D  Search";
            this.btnSearch.Font = AppTheme.ButtonFont;
            this.btnSearch.BackColor = AppTheme.AccentColor;
            this.btnSearch.ForeColor = Color.White;
            this.btnSearch.FlatStyle = FlatStyle.Flat;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.Size = new Size(120, 36);
            this.btnSearch.Margin = new Padding(0, 4, 8, 0);
            this.btnSearch.Cursor = Cursors.Hand;

            this.btnClear.Text = "Clear";
            this.btnClear.Font = AppTheme.FieldFont;
            this.btnClear.BackColor = AppTheme.PanelColor;
            this.btnClear.ForeColor = AppTheme.TextColor;
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.FlatAppearance.BorderColor = AppTheme.BorderColor;
            this.btnClear.Size = new Size(84, 36);
            this.btnClear.Margin = new Padding(0, 4, 8, 0);
            this.btnClear.Cursor = Cursors.Hand;

            this.btnPrevious.Text = "↶ Previous search";
            this.btnPrevious.Font = AppTheme.FieldFont;
            this.btnPrevious.BackColor = AppTheme.PanelColor;
            this.btnPrevious.ForeColor = AppTheme.TextColor;
            this.btnPrevious.FlatStyle = FlatStyle.Flat;
            this.btnPrevious.FlatAppearance.BorderColor = AppTheme.BorderColor;
            this.btnPrevious.Size = new Size(140, 36);
            this.btnPrevious.Margin = new Padding(0, 4, 0, 0);
            this.btnPrevious.Cursor = Cursors.Hand;
            this.btnPrevious.Enabled = false;

            this.tblSearch.Controls.Add(this.chipKeyword, 0, 0);
            this.tblSearch.Controls.Add(this.chipCategory, 1, 0);
            this.tblSearch.Controls.Add(this.chipFrom, 2, 0);
            this.tblSearch.Controls.Add(this.chipTo, 3, 0);
            this.tblSearch.Controls.Add(this.txtKeyword, 0, 1);
            this.tblSearch.Controls.Add(this.cmbCategory, 1, 1);
            this.tblSearch.Controls.Add(this.dtpFrom, 2, 1);
            this.tblSearch.Controls.Add(this.dtpTo, 3, 1);
            this.tblSearch.Controls.Add(this.pnlSearchButtons, 4, 1);

            // =============================================================
            //  BODY: event list (left) + details / recommendations / recent (right)
            // =============================================================
            this.tblBody.Dock = DockStyle.Fill;
            this.tblBody.BackColor = AppTheme.BackgroundColor;
            this.tblBody.Padding = new Padding(24, 12, 24, 12);
            this.tblBody.ColumnCount = 2;
            this.tblBody.RowCount = 1;
            this.tblBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            this.tblBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            this.tblBody.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tblBody.Controls.Add(this.pnlLeft, 0, 0);
            this.tblBody.Controls.Add(this.tblSide, 1, 0);

            // ---- left: results label + scrolling list of event cards ----
            this.pnlLeft.Dock = DockStyle.Fill;
            this.pnlLeft.Margin = new Padding(0, 0, 8, 0);
            this.pnlLeft.Controls.Add(this.flpEvents);     // Fill first
            this.pnlLeft.Controls.Add(this.lblResults);    // then Top

            this.lblResults.Dock = DockStyle.Top;
            this.lblResults.Height = 28;
            this.lblResults.Font = AppTheme.EngagementFont;
            this.lblResults.ForeColor = AppTheme.MutedTextColor;
            this.lblResults.TextAlign = ContentAlignment.MiddleLeft;

            this.flpEvents.Dock = DockStyle.Fill;
            this.flpEvents.FlowDirection = FlowDirection.TopDown;
            this.flpEvents.WrapContents = false;
            this.flpEvents.AutoScroll = true;
            this.flpEvents.BackColor = AppTheme.BackgroundColor;

            // ---- right: three stacked white panels ----
            this.tblSide.Dock = DockStyle.Fill;
            this.tblSide.Margin = new Padding(8, 0, 0, 0);
            this.tblSide.ColumnCount = 1;
            this.tblSide.RowCount = 3;
            this.tblSide.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tblSide.RowStyles.Add(new RowStyle(SizeType.Percent, 36F));
            this.tblSide.RowStyles.Add(new RowStyle(SizeType.Percent, 38F));
            this.tblSide.RowStyles.Add(new RowStyle(SizeType.Percent, 26F));
            this.tblSide.Controls.Add(this.pnlDetails, 0, 0);
            this.tblSide.Controls.Add(this.pnlRecommended, 0, 1);
            this.tblSide.Controls.Add(this.pnlRecent, 0, 2);

            // ---- details panel ----
            this.pnlDetails.Dock = DockStyle.Fill;
            this.pnlDetails.BackColor = AppTheme.PanelColor;
            this.pnlDetails.Padding = new Padding(12);
            this.pnlDetails.Margin = new Padding(0, 0, 0, 8);
            // Fill first, then Bottom, then the Top ones from lowest to highest.
            this.pnlDetails.Controls.Add(this.lblDetailBody);
            this.pnlDetails.Controls.Add(this.lblRelated);
            this.pnlDetails.Controls.Add(this.lblDetailMeta);
            this.pnlDetails.Controls.Add(this.lblDetailTitle);
            this.pnlDetails.Controls.Add(MakeSectionHeader(this.chipDetails, null));

            UiHelpers.StyleAsTag(this.chipDetails, AppTheme.TagBlueTint, AppTheme.TagBlueText);
            this.chipDetails.Text = "Event details";

            this.lblDetailTitle.Dock = DockStyle.Top;
            this.lblDetailTitle.Height = 28;
            this.lblDetailTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblDetailTitle.ForeColor = AppTheme.TextColor;
            this.lblDetailTitle.AutoEllipsis = true;
            this.lblDetailTitle.Text = "Select an event";

            this.lblDetailMeta.Dock = DockStyle.Top;
            this.lblDetailMeta.Height = 42;
            this.lblDetailMeta.Font = AppTheme.LabelFont;
            this.lblDetailMeta.ForeColor = AppTheme.TagBlueText;

            this.lblDetailBody.Dock = DockStyle.Fill;
            this.lblDetailBody.Font = AppTheme.LabelFont;
            this.lblDetailBody.ForeColor = AppTheme.TextColor;
            this.lblDetailBody.Text = "Click any event in the list to see its full details here.";

            this.lblRelated.Dock = DockStyle.Bottom;
            this.lblRelated.Height = 40;
            this.lblRelated.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            this.lblRelated.ForeColor = AppTheme.AccentDarkColor;

            // ---- recommended panel ----
            this.pnlRecommended.Dock = DockStyle.Fill;
            this.pnlRecommended.BackColor = AppTheme.PanelColor;
            this.pnlRecommended.Padding = new Padding(12);
            this.pnlRecommended.Margin = new Padding(0, 0, 0, 8);
            this.pnlRecommended.Controls.Add(this.flpRecommended);
            this.pnlRecommended.Controls.Add(MakeSectionHeader(this.chipRecommended, this.btnResetPrefs));

            UiHelpers.StyleAsTag(this.chipRecommended, AppTheme.AccentTintColor, AppTheme.AccentDarkColor);
            this.chipRecommended.Text = "Recommended for you";

            this.btnResetPrefs.Text = "Reset";
            this.btnResetPrefs.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            this.btnResetPrefs.BackColor = AppTheme.PanelColor;
            this.btnResetPrefs.ForeColor = AppTheme.MutedTextColor;
            this.btnResetPrefs.FlatStyle = FlatStyle.Flat;
            this.btnResetPrefs.FlatAppearance.BorderColor = AppTheme.BorderColor;
            this.btnResetPrefs.Size = new Size(60, 24);
            this.btnResetPrefs.Cursor = Cursors.Hand;

            this.flpRecommended.Dock = DockStyle.Fill;
            this.flpRecommended.FlowDirection = FlowDirection.TopDown;
            this.flpRecommended.WrapContents = false;
            this.flpRecommended.AutoScroll = true;
            this.flpRecommended.BackColor = AppTheme.PanelColor;

            // ---- recent panel: recently viewed (stack) | recent searches (queue) ----
            this.pnlRecent.Dock = DockStyle.Fill;
            this.pnlRecent.BackColor = AppTheme.PanelColor;
            this.pnlRecent.Padding = new Padding(12);
            this.pnlRecent.Margin = new Padding(0);
            this.pnlRecent.Controls.Add(this.tblRecent);

            this.tblRecent.Dock = DockStyle.Fill;
            this.tblRecent.ColumnCount = 2;
            this.tblRecent.RowCount = 2;
            this.tblRecent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tblRecent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.tblRecent.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.tblRecent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            UiHelpers.StyleAsTag(this.chipRecentViewed, AppTheme.TagPurpleTint, AppTheme.TagPurpleText);
            this.chipRecentViewed.Text = "Recently viewed";

            UiHelpers.StyleAsTag(this.chipRecentSearches, AppTheme.TagTealTint, AppTheme.TagTealText);
            this.chipRecentSearches.Text = "Recent searches";

            this.lstRecentViewed.Dock = DockStyle.Fill;
            this.lstRecentViewed.Font = AppTheme.LabelFont;
            this.lstRecentViewed.BorderStyle = BorderStyle.None;
            this.lstRecentViewed.Margin = new Padding(0, 0, 6, 0);
            this.lstRecentViewed.HorizontalScrollbar = true;

            this.lstRecentSearches.Dock = DockStyle.Fill;
            this.lstRecentSearches.Font = AppTheme.LabelFont;
            this.lstRecentSearches.BorderStyle = BorderStyle.None;
            this.lstRecentSearches.Margin = new Padding(6, 0, 0, 0);
            this.lstRecentSearches.HorizontalScrollbar = true;

            this.tblRecent.Controls.Add(this.chipRecentViewed, 0, 0);
            this.tblRecent.Controls.Add(this.chipRecentSearches, 1, 0);
            this.tblRecent.Controls.Add(this.lstRecentViewed, 0, 1);
            this.tblRecent.Controls.Add(this.lstRecentSearches, 1, 1);

            // =============================================================
            //  FORM
            //  Dock order: Fill control first, then the Top-docked controls
            //  from lowest to highest (the header is added last = at the very top).
            // =============================================================
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(1100, 700);
            this.MinimumSize = new Size(960, 640);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Local Events and Announcements - Sol Plaatjie Municipal Community Portal";
            this.BackColor = AppTheme.BackgroundColor;
            this.Controls.Add(this.tblBody);
            this.Controls.Add(this.tblSearch);
            this.Controls.Add(this.pnlAnnouncements);
            this.Controls.Add(this.pnlHeader);

            this.ResumeLayout(false);

            UiHelpers.MakeRounded(this.pbLogo, 8);
            UiHelpers.MakeRounded(this.btnBack, 8);
            UiHelpers.MakeRounded(this.btnSearch, 8);
            UiHelpers.MakeRounded(this.btnClear, 8);
            UiHelpers.MakeRounded(this.btnPrevious, 8);
            UiHelpers.MakeRounded(this.btnResetPrefs, 6);
        }

        /// <summary>
        /// A slim strip across the top of a white panel holding a chip label on
        /// the left and, optionally, a small button pinned to the right.
        /// </summary>
        private static Panel MakeSectionHeader(Label chip, Button? rightButton)
        {
            var header = new Panel { Dock = DockStyle.Top, Height = 32 };

            if (rightButton != null)
            {
                rightButton.Dock = DockStyle.Right;
                header.Controls.Add(rightButton);
            }

            chip.Location = new Point(0, 2);
            header.Controls.Add(chip);
            return header;
        }
    }
}

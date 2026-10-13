using System.Drawing;
using System.Windows.Forms;
using MunicipalReportingApp.Common;

namespace MunicipalReportingApp.Forms
{
    partial class ReportIssueForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader = null!;
        private Label lblTitle = null!;
        private TableLayoutPanel tblMain = null!;

        private Label chipLocation = null!;
        private TextBox txtLocation = null!;

        private Label chipStreetName = null!;
        private TextBox txtStreetName = null!;

        private Label chipCategory = null!;
        private ComboBox cmbCategory = null!;

        private Label chipDescription = null!;
        private RichTextBox rtbDescription = null!;

        private Label chipAttachments = null!;
        private FlowLayoutPanel pnlAttachControls = null!;
        private Button btnAttach = null!;
        private ListBox lstAttachments = null!;

        private Panel pnlEngagement = null!;
        private ProgressBar progressEngagement = null!;
        private Label lblEngagement = null!;

        private FlowLayoutPanel pnlActions = null!;
        private Button btnSubmit = null!;
        private Button btnBack = null!;

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
            this.pnlHeader = new Panel();
            this.lblTitle = new Label();
            this.tblMain = new TableLayoutPanel();

            this.chipLocation = new Label();
            this.txtLocation = new TextBox();

            this.chipStreetName = new Label();
            this.txtStreetName = new TextBox();

            this.chipCategory = new Label();
            this.cmbCategory = new ComboBox();

            this.chipDescription = new Label();
            this.rtbDescription = new RichTextBox();

            this.chipAttachments = new Label();
            this.pnlAttachControls = new FlowLayoutPanel();
            this.btnAttach = new Button();
            this.lstAttachments = new ListBox();

            this.pnlEngagement = new Panel();
            this.progressEngagement = new ProgressBar();
            this.lblEngagement = new Label();

            this.pnlActions = new FlowLayoutPanel();
            this.btnSubmit = new Button();
            this.btnBack = new Button();

            this.SuspendLayout();

            
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 64;
            this.pnlHeader.BackColor = AppTheme.TagPurpleText;
            this.pnlHeader.Controls.Add(this.lblTitle);

            
            this.lblTitle.AutoSize = true;
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitle.Location = new Point(24, 18);
            this.lblTitle.Text = "Report an Issue";

            
            this.tblMain.Dock = DockStyle.Fill;
            this.tblMain.BackColor = AppTheme.BackgroundColor;
            this.tblMain.ColumnCount = 1;
            this.tblMain.RowCount = 6;
            this.tblMain.Padding = new Padding(24, 16, 24, 10);
            this.tblMain.AutoScroll = true;
            this.tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));   // Location
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));   // Category
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));    // Description
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));    // Attachments
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));   // Engagement
            this.tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));   // Actions

            UiHelpers.StyleAsTag(this.chipLocation, AppTheme.TagBlueTint, AppTheme.TagBlueText);
            this.chipLocation.Text = "Location";

            UiHelpers.StyleAsTag(this.chipStreetName, AppTheme.TagBlueTint, AppTheme.TagBlueText);
            this.chipStreetName.Text = "Street Name"; //because our muni has extensions that we can add 

            UiHelpers.StyleAsTag(this.chipCategory, AppTheme.TagPurpleTint, AppTheme.TagPurpleText);
            this.chipCategory.Text = "Category";

            UiHelpers.StyleAsTag(this.chipDescription, AppTheme.TagTealTint, AppTheme.TagTealText);
            this.chipDescription.Text = "Description";

            UiHelpers.StyleAsTag(this.chipAttachments, AppTheme.TagAmberTint, AppTheme.TagAmberText);
            this.chipAttachments.Text = "Attachments (optional)";

            
            this.txtLocation.Font = AppTheme.FieldFont;
            this.txtLocation.PlaceholderText = "e.g. Section 5, or Area";

            this.txtStreetName.Font = AppTheme.FieldFont;
            this.txtStreetName.PlaceholderText = "e.g. Main Street";

            this.cmbCategory.Font = AppTheme.FieldFont;
            this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;

            
            this.rtbDescription.Font = AppTheme.FieldFont;
            this.rtbDescription.BorderStyle = BorderStyle.FixedSingle;

            
            this.pnlAttachControls.Dock = DockStyle.Fill;
            this.pnlAttachControls.FlowDirection = FlowDirection.TopDown;
            this.pnlAttachControls.WrapContents = false;
            this.pnlAttachControls.Controls.Add(this.btnAttach);
            this.pnlAttachControls.Controls.Add(this.lstAttachments);

            
            this.btnAttach.Text = "\U0001F4CE  Attach Image / Document...";
            this.btnAttach.Font = AppTheme.FieldFont;
            this.btnAttach.BackColor = AppTheme.AmberTintColor;
            this.btnAttach.ForeColor = AppTheme.AmberTextColor;
            this.btnAttach.FlatStyle = FlatStyle.Flat;
            this.btnAttach.FlatAppearance.BorderSize = 0;
            this.btnAttach.Size = new Size(300, 34);
            this.btnAttach.Cursor = Cursors.Hand;
            this.btnAttach.Margin = new Padding(0, 0, 0, 6);

            this.lstAttachments.Font = AppTheme.LabelFont;
            this.lstAttachments.Size = new Size(400, 66);
            this.lstAttachments.BorderStyle = BorderStyle.FixedSingle;
            this.lstAttachments.HorizontalScrollbar = true;

            
            this.pnlEngagement.Dock = DockStyle.Fill;
            this.pnlEngagement.Margin = new Padding(0, 8, 0, 0);
            this.pnlEngagement.Controls.Add(this.lblEngagement);
            this.pnlEngagement.Controls.Add(this.progressEngagement);

            
            this.progressEngagement.Dock = DockStyle.Top;
            this.progressEngagement.Height = 18;
            this.progressEngagement.Minimum = 0;
            this.progressEngagement.Maximum = 100;
            this.progressEngagement.Value = 0;
            this.progressEngagement.Style = ProgressBarStyle.Continuous;

            
            this.lblEngagement.Dock = DockStyle.Top;
            this.lblEngagement.Height = 28;
            this.lblEngagement.Font = AppTheme.EngagementFont;
            this.lblEngagement.ForeColor = AppTheme.AccentDarkColor;
            this.lblEngagement.TextAlign = ContentAlignment.MiddleLeft;
            this.lblEngagement.Text = "Let's get started - tell us where the issue is.";

            this.pnlActions.Dock = DockStyle.Fill;
            this.pnlActions.FlowDirection = FlowDirection.RightToLeft;
            this.pnlActions.Margin = new Padding(0, 6, 0, 0);
            this.pnlActions.Controls.Add(this.btnSubmit);
            this.pnlActions.Controls.Add(this.btnBack);

            
            this.btnSubmit.Text = "Submit Report";
            this.btnSubmit.Font = AppTheme.ButtonFont;
            this.btnSubmit.BackColor = AppTheme.AccentColor;
            this.btnSubmit.ForeColor = Color.White;
            this.btnSubmit.FlatStyle = FlatStyle.Flat;
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.Size = new Size(160, 40);
            this.btnSubmit.Cursor = Cursors.Hand;
            this.btnSubmit.Margin = new Padding(8, 4, 0, 4);

            
            this.btnBack.Text = "\u2190 Back to Main Menu";
            this.btnBack.Font = AppTheme.FieldFont;
            this.btnBack.BackColor = AppTheme.PanelColor;
            this.btnBack.ForeColor = AppTheme.TextColor;
            this.btnBack.FlatStyle = FlatStyle.Flat;
            this.btnBack.FlatAppearance.BorderColor = AppTheme.BorderColor;
            this.btnBack.Size = new Size(170, 40);
            this.btnBack.Cursor = Cursors.Hand;
            this.btnBack.Margin = new Padding(0, 4, 0, 4);

            this.tblMain.Controls.Add(BuildFieldGroup(this.chipLocation, this.txtLocation), 0, 0);
            this.tblMain.Controls.Add(BuildFieldGroup(this.chipStreetName, this.txtStreetName), 0, 1);
            this.tblMain.Controls.Add(BuildFieldGroup(this.chipCategory, this.cmbCategory), 0, 1);
            this.tblMain.Controls.Add(BuildFieldGroup(this.chipDescription, this.rtbDescription), 0, 2);
            this.tblMain.Controls.Add(BuildFieldGroup(this.chipAttachments, this.pnlAttachControls), 0, 3);
            this.tblMain.Controls.Add(this.pnlEngagement, 0, 4);
            this.tblMain.Controls.Add(this.pnlActions, 0, 5);

          
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(620, 700);
            this.MinimumSize = new Size(560, 580);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Report an Issue - Sol Plaatjie Municipal Community Portal";
            this.BackColor = AppTheme.BackgroundColor;
            this.Controls.Add(this.tblMain);
            this.Controls.Add(this.pnlHeader);

            this.ResumeLayout(false);

            UiHelpers.MakeRounded(this.btnAttach, 8);
            UiHelpers.MakeRounded(this.btnSubmit, 8);
            UiHelpers.MakeRounded(this.btnBack, 8);
        }

       
        private static Panel BuildFieldGroup(Label chip, Control field)
        {
            var chipWrap = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(0)
            };
            chipWrap.Controls.Add(chip);

            field.Dock = DockStyle.Fill;
            field.Margin = new Padding(0, 4, 0, 0);

            var group = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 4) };
            group.Controls.Add(field);     
            group.Controls.Add(chipWrap);   

            return group;
        }
    }
}

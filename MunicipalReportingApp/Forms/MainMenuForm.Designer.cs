using System.Drawing;
using System.Windows.Forms;
using MunicipalReportingApp.Common;

namespace MunicipalReportingApp.Forms
{
    partial class MainMenuForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlLogo = null!;
        private PictureBox pbLogo = null!;
        private Panel pnlHeader = null!;
        private Label lblTitle = null!;
        private Label lblSubtitle = null!;
        private Panel pnlMenu = null!;
        private Button btnReportIssues = null!;
        private Button btnEvents = null!;
        private Button btnServiceStatus = null!;
        private Panel pnlFooter = null!;
        private Label lblFooterBadge = null!;
        private ToolTip toolTipDisabled = null!;

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
            this.pnlLogo = new Panel();
            this.pbLogo = new PictureBox();
            this.pnlHeader = new Panel();
            this.lblTitle = new Label();
            this.lblSubtitle = new Label();
            this.pnlMenu = new Panel();
            this.btnReportIssues = new Button();
            this.btnEvents = new Button();
            this.btnServiceStatus = new Button();
            this.pnlFooter = new Panel();
            this.lblFooterBadge = new Label();
            this.toolTipDisabled = new ToolTip(this.components);

            this.SuspendLayout();

         
            // ---- pnlLogo (white band holding the municipality logo) ----
            this.pnlLogo.Dock = DockStyle.Top;
            this.pnlLogo.Height = 110;
            this.pnlLogo.BackColor = Color.White;
            this.pnlLogo.Padding = new Padding(16, 8, 16, 8);
            this.pnlLogo.Controls.Add(this.pbLogo);

            this.pbLogo.Dock = DockStyle.Fill;
            this.pbLogo.BackColor = Color.White;
            this.pbLogo.SizeMode = PictureBoxSizeMode.Zoom;   // keeps the proportions
            this.pbLogo.Image = BrandImages.Logo;

            this.pnlHeader.BackColor = AppTheme.PrimaryColor;
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 110;
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);

            
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = Color.Transparent;
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Font = AppTheme.TitleFont;
            this.lblTitle.Location = new Point(30, 18);
            this.lblTitle.Text = "Sol Plaatje Municipal Community Portal";

            
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.BackColor = Color.Transparent;
            this.lblSubtitle.ForeColor = AppTheme.PrimaryLightColor;
            this.lblSubtitle.Font = AppTheme.SubtitleFont;
            this.lblSubtitle.Location = new Point(32, 60);
            this.lblSubtitle.Text = "Report issues. Stay informed. Track your requests.";

            
            this.pnlMenu.Dock = DockStyle.Fill;
            this.pnlMenu.BackColor = AppTheme.BackgroundColor;
            this.pnlMenu.Padding = new Padding(40, 40, 40, 20);
            this.pnlMenu.Controls.Add(this.btnServiceStatus);
            this.pnlMenu.Controls.Add(this.btnEvents);
            this.pnlMenu.Controls.Add(this.btnReportIssues);

            
            this.btnReportIssues.Text = "\U0001F4E2  Report Issues";
            this.btnReportIssues.Font = AppTheme.ButtonFont;
            this.btnReportIssues.BackColor = AppTheme.AccentColor;
            this.btnReportIssues.ForeColor = Color.White;
            this.btnReportIssues.FlatStyle = FlatStyle.Flat;
            this.btnReportIssues.FlatAppearance.BorderSize = 0;
            this.btnReportIssues.TextAlign = ContentAlignment.MiddleLeft;
            this.btnReportIssues.Padding = new Padding(16, 0, 0, 0);
            this.btnReportIssues.Size = new Size(380, 64);
            this.btnReportIssues.Location = new Point(40, 40);
            this.btnReportIssues.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.btnReportIssues.Cursor = Cursors.Hand;
            this.btnReportIssues.Click += new System.EventHandler(this.BtnReportIssues_Click);

            
            this.btnEvents.Text = "\U0001F4C5  Local Events and Announcements";
            this.btnEvents.Font = AppTheme.ButtonFont;
            this.btnEvents.BackColor = AppTheme.TagBlueText;
            this.btnEvents.ForeColor = Color.White;
            this.btnEvents.FlatStyle = FlatStyle.Flat;
            this.btnEvents.FlatAppearance.BorderSize = 0;
            this.btnEvents.TextAlign = ContentAlignment.MiddleLeft;
            this.btnEvents.Padding = new Padding(16, 0, 0, 0);
            this.btnEvents.Size = new Size(380, 64);
            this.btnEvents.Location = new Point(40, 120);
            this.btnEvents.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.btnEvents.Cursor = Cursors.Hand;
            this.btnEvents.Click += new System.EventHandler(this.BtnEvents_Click);

            
            this.btnServiceStatus.Text = "\U0001F4CB  Service Request Status  (coming soon)";
            this.btnServiceStatus.Font = AppTheme.ButtonFont;
            this.btnServiceStatus.BackColor = AppTheme.DisabledFillColor;
            this.btnServiceStatus.ForeColor = AppTheme.DisabledTextColor;
            this.btnServiceStatus.FlatStyle = FlatStyle.Flat;
            this.btnServiceStatus.FlatAppearance.BorderSize = 0;
            this.btnServiceStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.btnServiceStatus.Padding = new Padding(16, 0, 0, 0);
            this.btnServiceStatus.Size = new Size(380, 64);
            this.btnServiceStatus.Location = new Point(40, 200);
            this.btnServiceStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.btnServiceStatus.Enabled = false;

            
            this.pnlFooter.Dock = DockStyle.Bottom;
            this.pnlFooter.Height = 56;
            this.pnlFooter.BackColor = AppTheme.PanelColor;
            this.pnlFooter.Controls.Add(this.lblFooterBadge);
            this.pnlFooter.Resize += (_, _) => CenterFooterBadge();

            
            this.lblFooterBadge.AutoSize = true;
            this.lblFooterBadge.Font = AppTheme.BadgeFont;
            this.lblFooterBadge.BackColor = AppTheme.AmberTintColor;
            this.lblFooterBadge.ForeColor = AppTheme.AmberTextColor;
            this.lblFooterBadge.TextAlign = ContentAlignment.MiddleCenter;
            this.lblFooterBadge.Padding = new Padding(14, 6, 14, 6);
            this.lblFooterBadge.Text = "Be the first to report an issue!";
            this.lblFooterBadge.SizeChanged += (_, _) => CenterFooterBadge();

            // ---- MainMenuForm ----
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(460, 650);
            this.MinimumSize = new Size(420, 610);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sol Plaatje Municipal Community Portal";
            this.BackColor = AppTheme.BackgroundColor;
            this.Controls.Add(this.pnlMenu);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlLogo);
            this.Controls.Add(this.pnlFooter);

            this.ResumeLayout(false);

            UiHelpers.MakeRounded(this.btnReportIssues, 10);
            UiHelpers.MakeRounded(this.btnEvents, 10);
            UiHelpers.MakeRounded(this.btnServiceStatus, 10);
            UiHelpers.MakeRounded(this.lblFooterBadge, 12);
        }

        
        private void CenterFooterBadge()
        {
            this.lblFooterBadge.Location = new Point(
                (this.pnlFooter.Width - this.lblFooterBadge.Width) / 2,
                (this.pnlFooter.Height - this.lblFooterBadge.Height) / 2);
        }
    }
}

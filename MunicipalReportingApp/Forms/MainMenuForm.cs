using System;
using System.Windows.Forms;
using MunicipalReportingApp.Services;

namespace MunicipalReportingApp.Forms
{
    /// <summary>
    /// The application's landing screen. Presents the three top-level
    /// citizen services. "Report Issues" (Part 1) and "Local Events and
    /// Announcements" (Part 2) are functional; "Service Request Status"
    /// is visibly present but disabled, ready for Task 3.
    /// </summary>
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
            ConfigureUpcomingFeatures();
            RefreshFooter();
        }

        /// <summary>
        /// "Service Request Status" is not built yet (Task 3), so it stays
        /// visible but disabled, with a hint explaining why.
        /// </summary>
        private void ConfigureUpcomingFeatures()
        {
            btnServiceStatus.Enabled = false;

            toolTipDisabled.SetToolTip(btnServiceStatus, "This feature is coming in a future release.");
        }

        /// <summary>
        /// Engagement strategy (part 2): the main menu shows a running
        /// community counter, reinforcing that other citizens are
        /// actively participating and encouraging the user to do the same.
        /// </summary>
        public void RefreshFooter()
        {
            int count = IssueRepository.Instance.Count;

            lblFooterBadge.Text = count == 0
                ? "Be the first to report an issue!"
                : $"{count} issue(s) reported by citizens like you";
        }

        private void BtnReportIssues_Click(object? sender, EventArgs e)
        {
            using var reportForm = new ReportIssueForm();
            this.Hide();
            reportForm.ShowDialog(this);
            this.Show();
            RefreshFooter();
        }

        private void BtnEvents_Click(object? sender, EventArgs e)
        {
            using var eventsForm = new EventsForm();
            this.Hide();
            eventsForm.ShowDialog(this);
            this.Show();
        }
    }
}

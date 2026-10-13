using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using MunicipalReportingApp.Common;
using MunicipalReportingApp.Services;

namespace MunicipalReportingApp.Forms
{
    
    public partial class ReportIssueForm : Form
    {
        // Full paths of files the user has attached via the OpenFileDialog. But it was not reacting to anything at first!!
        private readonly List<string> _attachedFilePaths = new();

        public ReportIssueForm()
        {
            InitializeComponent();
            PopulateCategoryList();
            WireEventHandlers();

            
            this.Load += (_, _) => UpdateEngagement();
        }

        private void PopulateCategoryList()
        {
            cmbCategory.Items.AddRange(new object[]
            {
                "Water & Utilities",
                "Sanitation & Waste",
                "Roads & Potholes",
                "Street Lighting",
                "Parks & Recreation",
                "Public Safety",
                "Noise Complaint",
                "Other Issue"
            });
        }

        private void WireEventHandlers()
        {
            txtLocation.TextChanged += (_, _) => UpdateEngagement();
            txtLocation.TextChanged += (_, _) => UpdateEngagement();
            txtStreetName.TextChanged += (_, _) => UpdateEngagement();
            cmbCategory.SelectedIndexChanged += (_, _) => UpdateEngagement();
            rtbDescription.TextChanged += (_, _) => UpdateEngagement();

            btnAttach.Click += BtnAttach_Click;
            btnSubmit.Click += BtnSubmit_Click;
            btnBack.Click += (_, _) => this.Close();
        }

        
        /// Requirement 4: lets the user attach images or documents via
       
        private void BtnAttach_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Attach images or documents related to the issue",
                Filter = "Supported files (*.jpg;*.jpeg;*.png;*.bmp;*.pdf;*.docx;*.txt)|" +
                         "*.jpg;*.jpeg;*.png;*.bmp;*.pdf;*.docx;*.txt|All files (*.*)|*.*",
                Multiselect = true,
                CheckFileExists = true
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            foreach (var filePath in dialog.FileNames)
            {
                if (!_attachedFilePaths.Contains(filePath))
                {
                    _attachedFilePaths.Add(filePath);
                    lstAttachments.Items.Add(Path.GetFileName(filePath));
                }
            }

            UpdateEngagement();
        }

       
        //I dont get how this thing works! the spacing is just not it man
       
        private void BtnSubmit_Click(object? sender, EventArgs e)
        {
            var location = txtLocation.Text.Trim();
            var streetName = txtStreetName.Text.Trim();
            var category = cmbCategory.SelectedItem as string ?? string.Empty;
            var description = rtbDescription.Text.Trim();

            var missingFields = new List<string>();
            if (string.IsNullOrWhiteSpace(location)) missingFields.Add("Location");
            if (string.IsNullOrWhiteSpace(category)) missingFields.Add("Category");
            if (string.IsNullOrWhiteSpace(description)) missingFields.Add("Description");

            if (missingFields.Count > 0)
            {
                MessageBox.Show(
                    this,
                    "Please complete the following before submitting:" + Environment.NewLine + Environment.NewLine +
                    "\u2022 " + string.Join(Environment.NewLine + "\u2022 ", missingFields),
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var issue = IssueRepository.Instance.Add(
                location,
                streetName,
                category,
                description,
                new List<string>(_attachedFilePaths));

            MessageBox.Show(
                this,
                "Thank you! Your report has been submitted successfully." + Environment.NewLine + Environment.NewLine +
                $"Reference ID: #{issue.Id}" + Environment.NewLine +
                $"Category: {issue.Category}" + Environment.NewLine +
                $"Location: {issue.Location}" + Environment.NewLine + Environment.NewLine +
                "Our community-appointed municipal team will review it shortly.",
                "Report Submitted",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            ResetForm();
        }

        private void ResetForm()
        {
            txtLocation.Clear();
            txtStreetName.Clear();
            cmbCategory.SelectedIndex = -1;
            rtbDescription.Clear();
            lstAttachments.Items.Clear();
            _attachedFilePaths.Clear();
            UpdateEngagement();
            txtLocation.Focus();
        }

        private void UpdateEngagement()
        {
            const int totalSteps = 4;
            int completedSteps = 0;

            if (!string.IsNullOrWhiteSpace(txtLocation.Text)) completedSteps++;
            if (cmbCategory.SelectedIndex != -1) completedSteps++;
            if (!string.IsNullOrWhiteSpace(rtbDescription.Text)) completedSteps++;
            if (_attachedFilePaths.Count > 0) completedSteps++;

            int percentComplete = (int)((double)completedSteps / totalSteps * 100);
            progressEngagement.Value = Math.Clamp(percentComplete, 0, 100);

            var barColor = percentComplete >= 100 ? AppTheme.SuccessColor : AppTheme.InProgressColor;
            UiHelpers.SetProgressBarColor(progressEngagement, barColor);
            lblEngagement.ForeColor = percentComplete >= 100 ? AppTheme.TagTealText : AppTheme.AccentDarkColor;

            lblEngagement.Text = percentComplete switch
            {
                0 => "Let's get started - tell us where the issue is.",
                <= 25 => "Continue! - select a category next.",
                <= 50 => "Nice progress! Add a description to help us understand the issue.",
                <= 75 => "Almost there! Attach a photo if you can, or submit now.",
                _ => "Great! Your report is ready to submit."
            };
        }
    }
}

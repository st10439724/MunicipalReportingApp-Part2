using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MunicipalReportingApp.Common;
using MunicipalReportingApp.Models;
using MunicipalReportingApp.Services;

namespace MunicipalReportingApp.Forms
{
    /// <summary>
    /// Local Events and Announcements page.
    ///
    ///  - Announcements strip across the top (urgent first, from a PriorityQueue).
    ///  - Search by keyword, category and date range.
    ///  - Upcoming events grouped by date, newest-first-to-come (SortedDictionary).
    ///  - Details panel for the event the user clicked.
    ///  - "Recommended for you": suggestions learned from the user's searches and clicks.
    ///  - Recently viewed (Stack) and recent searches (Queue); "Previous search" (Stack).
    /// </summary>
    public partial class EventsForm : Form
    {
        private static readonly Font BadgeDayFont = new("Segoe UI", 13F, FontStyle.Bold);
        private static readonly Font ReasonFont = new("Segoe UI", 8.5F, FontStyle.Italic);

        // Each category gets one of these colour pairs (the same category always gets the same pair).
        private static readonly (Color Tint, Color Text)[] CategoryPalette =
        {
            (AppTheme.TagBlueTint, AppTheme.TagBlueText),
            (AppTheme.TagPurpleTint, AppTheme.TagPurpleText),
            (AppTheme.TagTealTint, AppTheme.TagTealText),
            (AppTheme.TagAmberTint, AppTheme.TagAmberText),
            (AppTheme.AccentTintColor, AppTheme.AccentDarkColor)
        };

        private readonly EventRepository _events = EventRepository.Instance;
        private readonly RecommendationEngine _recommender = RecommendationEngine.Instance;
        private readonly UserActivityTracker _activity = new();

        private int _selectedEventId = -1;

        public EventsForm()
        {
            InitializeComponent();
            ConfigureControls();
            WireEventHandlers();

            this.WindowState = FormWindowState.Maximized;
            this.Load += EventsForm_Load;
        }

        // ------------------------------------------------------------------
        //  Setup
        // ------------------------------------------------------------------

        private void ConfigureControls()
        {
            // The category list comes from the repository's HashSet of unique categories.
            cmbCategory.Items.Add(EventRepository.AllCategories);
            foreach (var category in _events.GetCategories())
            {
                cmbCategory.Items.Add(category);
            }
            cmbCategory.SelectedIndex = 0;

            this.AcceptButton = btnSearch;   // Enter key runs the search

            toolTip.SetToolTip(btnPrevious, "Go back to the search you made before this one.");
            toolTip.SetToolTip(btnResetPrefs, "Forget the interests the app has learned about you.");
            toolTip.SetToolTip(lstRecentViewed, "Double-click an event to open it again.");
            toolTip.SetToolTip(lstRecentSearches, "Double-click a search to run it again.");
        }

        private void WireEventHandlers()
        {
            btnSearch.Click += (_, _) => RunSearchFromControls();
            btnClear.Click += BtnClear_Click;
            btnPrevious.Click += BtnPrevious_Click;
            btnBack.Click += (_, _) => this.Close();
            btnResetPrefs.Click += BtnResetPrefs_Click;

            lstRecentViewed.DoubleClick += LstRecentViewed_DoubleClick;
            lstRecentSearches.DoubleClick += LstRecentSearches_DoubleClick;

            flpEvents.SizeChanged += (_, _) => ResizeCards(flpEvents);
            flpRecommended.SizeChanged += (_, _) => ResizeCards(flpRecommended);
        }

        private void EventsForm_Load(object? sender, EventArgs e)
        {
            RenderAnnouncements();
            ExecuteSearch(new SearchCriteria(), track: true);
        }

        // ------------------------------------------------------------------
        //  Searching
        // ------------------------------------------------------------------

        private SearchCriteria ReadCriteria()
        {
            var category = cmbCategory.SelectedItem as string ?? string.Empty;
            if (category == EventRepository.AllCategories)
            {
                category = string.Empty;
            }

            return new SearchCriteria(
                txtKeyword.Text.Trim(),
                category,
                dtpFrom.Checked ? dtpFrom.Value.Date : null,
                dtpTo.Checked ? dtpTo.Value.Date : null);
        }

        private void ApplyCriteriaToControls(SearchCriteria criteria)
        {
            txtKeyword.Text = criteria.Keyword;

            int index = string.IsNullOrWhiteSpace(criteria.Category)
                ? 0
                : cmbCategory.Items.IndexOf(criteria.Category);
            cmbCategory.SelectedIndex = index < 0 ? 0 : index;

            SetPicker(dtpFrom, criteria.From);
            SetPicker(dtpTo, criteria.To);
        }

        private static void SetPicker(DateTimePicker picker, DateTime? value)
        {
            if (value.HasValue)
            {
                picker.Value = value.Value < picker.MinDate ? picker.MinDate : value.Value;
                picker.Checked = true;
            }
            else
            {
                picker.Checked = false;
            }
        }

        private void RunSearchFromControls()
        {
            var criteria = ReadCriteria();

            if (criteria.From.HasValue && criteria.To.HasValue && criteria.From > criteria.To)
            {
                MessageBox.Show(
                    this,
                    "The \"To\" date is before the \"From\" date. Please correct the dates and try again.",
                    "Check your dates",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            ExecuteSearch(criteria, track: true);
        }

        /// <summary>
        /// Runs a search and refreshes everything that depends on it.
        /// <paramref name="track"/> is false when stepping back, so going back
        /// does not count as a brand-new search.
        /// </summary>
        private void ExecuteSearch(SearchCriteria criteria, bool track)
        {
            var results = _events.Search(criteria.Keyword, criteria.Category, criteria.From, criteria.To);
            RenderEvents(results, criteria);

            if (track)
            {
                _activity.PushSearch(criteria);
                _recommender.RecordSearch(criteria, results);
            }

            btnPrevious.Enabled = _activity.CanGoBack;
            RefreshRecent();
            RefreshRecommendations();
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            txtKeyword.Clear();
            cmbCategory.SelectedIndex = 0;
            dtpFrom.Checked = false;
            dtpTo.Checked = false;
            ExecuteSearch(new SearchCriteria(), track: true);
        }

        private void BtnPrevious_Click(object? sender, EventArgs e)
        {
            var previous = _activity.GoBack();
            if (previous == null) return;

            ApplyCriteriaToControls(previous);
            ExecuteSearch(previous, track: false);
        }

        // ------------------------------------------------------------------
        //  Event list
        // ------------------------------------------------------------------

        private void RenderEvents(List<EventItem> results, SearchCriteria criteria)
        {
            ClearAndDispose(flpEvents);
            flpEvents.SuspendLayout();

            string count = results.Count == 1 ? "1 upcoming event" : $"{results.Count} upcoming events";
            lblResults.Text = criteria.IsEmpty ? count : $"{count} matching your search";

            if (results.Count == 0)
            {
                flpEvents.Controls.Add(BuildNote(
                    "No upcoming events match your search. Try a different word, category or date range, or press Clear.",
                    CardWidth(flpEvents), 60));
            }
            else
            {
                DateTime? lastDay = null;
                foreach (var item in results)
                {
                    // Results arrive in date order, so a new heading is needed
                    // whenever the day changes.
                    if (lastDay != item.Date.Date)
                    {
                        flpEvents.Controls.Add(BuildDateHeader(item.Date.Date));
                        lastDay = item.Date.Date;
                    }
                    flpEvents.Controls.Add(BuildEventCard(item));
                }
            }

            flpEvents.ResumeLayout();
            HighlightSelectedCard();
        }

        private Label BuildDateHeader(DateTime day)
        {
            return new Label
            {
                AutoSize = false,
                Width = CardWidth(flpEvents),
                Height = 26,
                Margin = new Padding(0, 6, 0, 2),
                Font = AppTheme.EngagementFont,
                ForeColor = AppTheme.TagBlueText,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = day.ToString("dddd, d MMMM yyyy")
            };
        }

        private Panel BuildEventCard(EventItem item)
        {
            int width = CardWidth(flpEvents);

            var card = new Panel
            {
                Size = new Size(width, 88),
                BackColor = AppTheme.PanelColor,
                Margin = new Padding(0, 0, 0, 8),
                Tag = item
            };

            var badge = new Label
            {
                Dock = DockStyle.Left,
                Width = 70,
                BackColor = AppTheme.TagBlueTint,
                ForeColor = AppTheme.TagBlueText,
                Font = BadgeDayFont,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = item.Date.Day + Environment.NewLine + item.Date.ToString("MMM").ToUpper()
            };

            var title = new Label
            {
                AutoSize = false,
                AutoEllipsis = true,
                Text = item.Title,
                Font = AppTheme.ButtonFont,
                ForeColor = AppTheme.TextColor,
                Location = new Point(84, 8),
                Size = new Size(width - 96, 24),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            var chip = new Label { Text = item.Category, Location = new Point(84, 38) };
            var colors = CategoryColors(item.Category);
            UiHelpers.StyleAsTag(chip, colors.Tint, colors.Text);

            var info = new Label
            {
                AutoSize = false,
                AutoEllipsis = true,
                Text = $"{item.Date:HH:mm}  •  {item.Venue}",
                Font = AppTheme.LabelFont,
                ForeColor = AppTheme.MutedTextColor,
                Location = new Point(84, 62),
                Size = new Size(width - 96, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            card.Controls.Add(badge);
            card.Controls.Add(title);
            card.Controls.Add(chip);
            card.Controls.Add(info);

            UiHelpers.MakeRounded(card, 8);
            MakeClickable(card, () => SelectEvent(item));
            return card;
        }

        private static (Color Tint, Color Text) CategoryColors(string category)
        {
            int sum = 0;
            foreach (char ch in category)
            {
                sum += ch;
            }
            return CategoryPalette[sum % CategoryPalette.Length];
        }

        private void HighlightSelectedCard()
        {
            foreach (Control control in flpEvents.Controls)
            {
                if (control.Tag is EventItem item)
                {
                    control.BackColor = item.Id == _selectedEventId
                        ? AppTheme.TagTealTint
                        : AppTheme.PanelColor;
                }
            }
        }

        // ------------------------------------------------------------------
        //  Selecting an event
        // ------------------------------------------------------------------

        private void SelectEvent(EventItem item)
        {
            _selectedEventId = item.Id;

            _activity.AddViewed(item);        // Stack: recently viewed
            _recommender.RecordView(item);    // learn from this click

            ShowDetails(item);
            HighlightSelectedCard();
            RefreshRecent();
            RefreshRecommendations();
        }

        private void ShowDetails(EventItem item)
        {
            lblDetailTitle.Text = item.Title;
            lblDetailMeta.Text =
                $"{item.Date:dddd, d MMMM yyyy} at {item.Date:HH:mm}" + Environment.NewLine +
                $"{item.Venue}  •  {item.Category}";
            lblDetailBody.Text = item.Description;

            var related = _recommender.GetRelated(item, 3);
            lblRelated.Text = related.Count == 0
                ? string.Empty
                : "Related events: " + string.Join("  |  ", related.Select(r => r.Event.Title));
        }

        // ------------------------------------------------------------------
        //  Recommendations
        // ------------------------------------------------------------------

        private void RefreshRecommendations()
        {
            ClearAndDispose(flpRecommended);
            flpRecommended.SuspendLayout();

            int width = CardWidth(flpRecommended);

            if (!_recommender.HasHistory)
            {
                flpRecommended.Controls.Add(BuildNote(
                    "Search for events or open one and your personal suggestions will appear here. Until then, these are coming up soon:",
                    width, 50));
            }

            // Skip events already opened this visit so the list keeps offering new ideas.
            foreach (var recommendation in _recommender.GetRecommendations(4, _activity.ViewedIds))
            {
                flpRecommended.Controls.Add(BuildRecommendationCard(recommendation, width));
            }

            flpRecommended.ResumeLayout();
        }

        private Panel BuildRecommendationCard(Recommendation recommendation, int width)
        {
            var item = recommendation.Event;

            var card = new Panel
            {
                Size = new Size(width, 62),
                BackColor = AppTheme.BackgroundColor,
                Margin = new Padding(0, 0, 0, 6),
                Tag = item
            };

            var title = new Label
            {
                AutoSize = false,
                AutoEllipsis = true,
                Text = item.Title,
                Font = AppTheme.EngagementFont,
                ForeColor = AppTheme.TextColor,
                Location = new Point(10, 5),
                Size = new Size(width - 20, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            var meta = new Label
            {
                AutoSize = false,
                AutoEllipsis = true,
                Text = $"{item.Date:ddd d MMM, HH:mm}  •  {item.Category}",
                Font = AppTheme.LabelFont,
                ForeColor = AppTheme.MutedTextColor,
                Location = new Point(10, 25),
                Size = new Size(width - 20, 18),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            var reason = new Label
            {
                AutoSize = false,
                AutoEllipsis = true,
                Text = recommendation.Reason,
                Font = ReasonFont,
                ForeColor = AppTheme.AccentDarkColor,
                Location = new Point(10, 43),
                Size = new Size(width - 20, 16),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            card.Controls.Add(title);
            card.Controls.Add(meta);
            card.Controls.Add(reason);

            UiHelpers.MakeRounded(card, 8);
            MakeClickable(card, () => SelectEvent(item));
            return card;
        }

        private void BtnResetPrefs_Click(object? sender, EventArgs e)
        {
            var answer = MessageBox.Show(
                this,
                "Forget the interests the app has learned from your searches and the events you opened?",
                "Reset recommendations",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (answer != DialogResult.Yes) return;

            _recommender.ResetProfile();
            RefreshRecommendations();
        }

        // ------------------------------------------------------------------
        //  Recently viewed (Stack) and recent searches (Queue)
        // ------------------------------------------------------------------

        private void RefreshRecent()
        {
            lstRecentViewed.BeginUpdate();
            lstRecentViewed.Items.Clear();
            foreach (var item in _activity.RecentlyViewed)
            {
                lstRecentViewed.Items.Add(item);
            }
            lstRecentViewed.EndUpdate();

            lstRecentSearches.BeginUpdate();
            lstRecentSearches.Items.Clear();
            foreach (var search in _activity.RecentSearches)
            {
                lstRecentSearches.Items.Add(search);
            }
            lstRecentSearches.EndUpdate();
        }

        private void LstRecentViewed_DoubleClick(object? sender, EventArgs e)
        {
            if (lstRecentViewed.SelectedItem is EventItem item)
            {
                SelectEvent(item);
            }
        }

        private void LstRecentSearches_DoubleClick(object? sender, EventArgs e)
        {
            if (lstRecentSearches.SelectedItem is SearchCriteria criteria)
            {
                ApplyCriteriaToControls(criteria);
                ExecuteSearch(criteria, track: true);
            }
        }

        // ------------------------------------------------------------------
        //  Announcements
        // ------------------------------------------------------------------

        private void RenderAnnouncements()
        {
            ClearAndDispose(flpAnnouncements);

            // The repository returns them already ordered by the PriorityQueue.
            foreach (var announcement in _events.GetAnnouncementsByPriority())
            {
                flpAnnouncements.Controls.Add(BuildAnnouncementCard(announcement));
            }
        }

        private Panel BuildAnnouncementCard(Announcement announcement)
        {
            var (tint, text, label) = announcement.Priority switch
            {
                AnnouncementPriority.Urgent => (AppTheme.AccentTintColor, AppTheme.AccentDarkColor, "URGENT"),
                AnnouncementPriority.Important => (AppTheme.AmberTintColor, AppTheme.AmberTextColor, "IMPORTANT"),
                _ => (AppTheme.TagBlueTint, AppTheme.TagBlueText, "NOTICE")
            };

            var card = new Panel
            {
                Size = new Size(300, 52),
                BackColor = tint,
                Margin = new Padding(0, 0, 10, 0),
                Tag = announcement
            };

            var tag = new Label { Text = label, Location = new Point(8, 5) };
            UiHelpers.StyleAsTag(tag, text, Color.White);

            var title = new Label
            {
                AutoSize = false,
                AutoEllipsis = true,
                Text = announcement.Title,
                Font = AppTheme.EngagementFont,
                ForeColor = text,
                Location = new Point(8, 29),
                Size = new Size(284, 18)
            };

            card.Controls.Add(tag);
            card.Controls.Add(title);

            toolTip.SetToolTip(card, "Click to read the full announcement.");
            UiHelpers.MakeRounded(card, 8);
            MakeClickable(card, () => ShowAnnouncement(announcement));
            return card;
        }

        private void ShowAnnouncement(Announcement announcement)
        {
            MessageBox.Show(
                this,
                announcement.Message + Environment.NewLine + Environment.NewLine +
                $"Posted: {announcement.DatePosted:d MMMM yyyy}",
                announcement.Title,
                MessageBoxButtons.OK,
                announcement.Priority == AnnouncementPriority.Urgent
                    ? MessageBoxIcon.Warning
                    : MessageBoxIcon.Information);
        }

        // ------------------------------------------------------------------
        //  Small helpers
        // ------------------------------------------------------------------

        private static int CardWidth(FlowLayoutPanel panel)
        {
            return Math.Max(200, panel.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4);
        }

        private static void ResizeCards(FlowLayoutPanel panel)
        {
            int width = CardWidth(panel);
            foreach (Control control in panel.Controls)
            {
                control.Width = width;
            }
        }

        private static Label BuildNote(string text, int width, int height)
        {
            return new Label
            {
                AutoSize = false,
                Width = width,
                Height = height,
                Margin = new Padding(0, 0, 0, 8),
                Font = AppTheme.LabelFont,
                ForeColor = AppTheme.MutedTextColor,
                Text = text
            };
        }

        /// <summary>Makes a control and everything inside it react to a click.</summary>
        private static void MakeClickable(Control control, Action onClick)
        {
            control.Cursor = Cursors.Hand;
            control.Click += (_, _) => onClick();

            foreach (Control child in control.Controls)
            {
                MakeClickable(child, onClick);
            }
        }

        /// <summary>
        /// Removes every control from a container and disposes it. Disposal is
        /// deferred because this can be triggered by a click on one of the very
        /// controls being removed.
        /// </summary>
        private void ClearAndDispose(Control container)
        {
            var old = container.Controls.Cast<Control>().ToList();
            container.Controls.Clear();

            void DisposeAll()
            {
                foreach (var control in old)
                {
                    control.Dispose();
                }
            }

            if (IsHandleCreated)
            {
                BeginInvoke(new Action(DisposeAll));
            }
            else
            {
                DisposeAll();
            }
        }
    }
}

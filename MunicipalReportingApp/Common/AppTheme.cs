using System.Drawing;

namespace MunicipalReportingApp.Common
{
    
 
    public static class AppTheme
    {
        // ---- Core brand / chrome ----
        public static readonly Color PrimaryColor = ColorTranslator.FromHtml("#085041");   // Deep teal (headers)
        public static readonly Color PrimaryLightColor = ColorTranslator.FromHtml("#9FE1CB"); // Light teal (header subtitle)
        public static readonly Color AccentColor = ColorTranslator.FromHtml("#D85A30");    // Coral (primary action / submit)
        public static readonly Color AccentDarkColor = ColorTranslator.FromHtml("#993C1D"); // Coral text-on-tint
        public static readonly Color AccentTintColor = ColorTranslator.FromHtml("#FAECE7"); // Coral tint background

        // ---- Highlight / attachments ----
        public static readonly Color AmberTintColor = ColorTranslator.FromHtml("#FAEEDA");
        public static readonly Color AmberTextColor = ColorTranslator.FromHtml("#633806");
        public static readonly Color AmberColor = ColorTranslator.FromHtml("#EF9F27");

        // ---- Field "tag" colours (Location / Category / Description / Attachments) ----
        public static readonly Color TagBlueTint = ColorTranslator.FromHtml("#E6F1FB");
        public static readonly Color TagBlueText = ColorTranslator.FromHtml("#0C447C");

        public static readonly Color TagPurpleTint = ColorTranslator.FromHtml("#EEEDFE");
        public static readonly Color TagPurpleText = ColorTranslator.FromHtml("#3C3489");

        public static readonly Color TagTealTint = ColorTranslator.FromHtml("#E1F5EE");
        public static readonly Color TagTealText = ColorTranslator.FromHtml("#085041");

        public static readonly Color TagAmberTint = ColorTranslator.FromHtml("#FAEEDA");
        public static readonly Color TagAmberText = ColorTranslator.FromHtml("#633806");

        
        public static readonly Color SuccessColor = ColorTranslator.FromHtml("#639922");   // Green (progress complete)
        public static readonly Color InProgressColor = ColorTranslator.FromHtml("#EF9F27"); // Amber (progress in-flight)

        
        public static readonly Color BackgroundColor = ColorTranslator.FromHtml("#F6F5F1");
        public static readonly Color PanelColor = Color.White;
        public static readonly Color TextColor = ColorTranslator.FromHtml("#222222");
        public static readonly Color MutedTextColor = ColorTranslator.FromHtml("#5A6472");
        public static readonly Color DisabledFillColor = ColorTranslator.FromHtml("#F1EFE8");
        public static readonly Color DisabledTextColor = ColorTranslator.FromHtml("#888780");
        public static readonly Color BorderColor = ColorTranslator.FromHtml("#D8DCE1");

        
        public static readonly Font TitleFont = new Font("Segoe UI", 18F, FontStyle.Bold);
        public static readonly Font SubtitleFont = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static readonly Font ButtonFont = new Font("Segoe UI", 11F, FontStyle.Bold);
        public static readonly Font LabelFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font FieldFont = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static readonly Font TagFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        public static readonly Font EngagementFont = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        public static readonly Font BadgeFont = new Font("Segoe UI", 9F, FontStyle.Bold);
    }
}

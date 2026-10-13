using System;
using System.Drawing;
using System.Reflection;

namespace MunicipalReportingApp.Common
{
    /// <summary>
    /// Loads the municipality logo. The picture is compiled INTO the program
    /// (an embedded resource), so it can never go missing when the app is
    /// copied or run from another folder.
    /// </summary>
    public static class BrandImages
    {
        private const string LogoResourceName = "MunicipalReportingApp.Images.SolPlaatjeLogo.png";

        private static Image? _logo;

        /// <summary>
        /// The Sol Plaatje Municipality logo, or null if it cannot be loaded
        /// (the screens then simply show without it instead of crashing).
        /// </summary>
        public static Image? Logo
        {
            get
            {
                if (_logo != null) return _logo;

                try
                {
                    using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(LogoResourceName);
                    if (stream == null) return null;

                    // Copy into a Bitmap so the stream can be closed safely.
                    using var loaded = Image.FromStream(stream);
                    _logo = new Bitmap(loaded);
                }
                catch
                {
                    _logo = null;
                }
                return _logo;
            }
        }
    }
}

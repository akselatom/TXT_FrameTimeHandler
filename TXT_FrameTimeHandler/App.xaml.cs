
namespace TXT_FrameTimeHandler
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Windows;

    /// <summary>
    /// Interaction Logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        /// <summary>
        /// List of available cultures in program
        /// </summary>
        private static List<CultureInfo> mLanguages = new List<CultureInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            mLanguages.Clear();
            mLanguages.Add(new CultureInfo("en-US"));
            mLanguages.Add(new CultureInfo("ru-RU")); // Neutral culture for that project
        }

        /// <summary>
        /// Event that notifies all windows about language change.
        /// </summary>
        public static event EventHandler LanguageChanged;

        /// <summary>
        /// Gets the languages list. <see cref="mLanguages"/>
        /// </summary>
        public static List<CultureInfo> Languages => mLanguages;

        /// <summary>
        /// Gets or sets current culture. Will replace the resource dictionary of the previous culture with a new one.
        /// Trigger an event allowing all windows to perform additional actions when changing the culture.
        /// </summary>
        public static CultureInfo Language
        {
            get => Thread.CurrentThread.CurrentUICulture;

            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == Thread.CurrentThread.CurrentUICulture) return;

                // Change application language.
                Thread.CurrentThread.CurrentUICulture = value;

                // Create new ResourceDictionary for new culture.
                var dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "en-US":
                        dict.Source = new Uri($"Resources/lang.{value.Name}.xaml", UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("Resources/lang.xaml", UriKind.Relative);
                        break;
                }

                // Switch old ResourceDictionary with new one.
                var oldDict = (from d in Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("Resources/lang.")
                                              select d).First();

                if (oldDict != null)
                {
                    var ind = Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Current.Resources.MergedDictionaries.Remove(oldDict);
                    Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Current.Resources.MergedDictionaries.Add(dict);
                }

                // Trigger event to alert all windows.
                LanguageChanged(Application.Current, new EventArgs());
            }
        }

    }
}

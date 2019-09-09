namespace TXT_FrameTimeHandler
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction Logic for для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            App.LanguageChanged += this.LanguageChanged;
            App.Language = Properties.Settings.Default.DefaultLanguage;
            this.FillMenuItem();
        }

        /// <summary>
        /// Fill <see cref="menuLanguage"/> with a list of available languages.
        /// </summary>
        private void FillMenuItem()
        {
            var curLang = App.Language;
            this.menuLanguage.Items.Clear();
            foreach (var lang in App.Languages)
            {
                var menuLang = new MenuItem { Header = lang.DisplayName, Tag = lang, IsChecked = lang.Equals(curLang) };
                menuLang.Click += this.ChangeLanguageClick;
                this.menuLanguage.Items.Add(menuLang);
            }
        }

        /// <summary>
        /// Handler for <see cref="App.LanguageChanged"/>. changes checkbox of the current language in menuItem.
        /// </summary>
        private void LanguageChanged(object sender, EventArgs e)
        {
            var curLang = App.Language;

            foreach (MenuItem i in this.menuLanguage.Items)
            {
                i.IsChecked = i.Tag is CultureInfo ci && ci.Equals(curLang);
            }
        }

        /// <summary>
        /// Culture change click handler
        /// </summary>
        private void ChangeLanguageClick(object sender, EventArgs e)
        {
            var mi = sender as MenuItem;
            if (mi?.Tag is CultureInfo lang)
            {
                App.Language = lang;
                Properties.Settings.Default.DefaultLanguage = App.Language;
                Properties.Settings.Default.Save();
            }
        }


    }
}

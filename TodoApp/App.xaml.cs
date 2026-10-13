using System.Globalization;

namespace TodoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string savedLanguage = Preferences.Default.Get("selected_language", CultureInfo.CurrentCulture.Name);

            var culture = new CultureInfo(savedLanguage);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "TodoApp" };
        }
    }
}

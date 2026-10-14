using System.Globalization;

namespace TodoApp
{
    public partial class App : Application
    {
        public App()
        {

            string savedLanguage = Preferences.Default.Get("selected_language",CultureInfo.CurrentUICulture.Name);

            var culture = new CultureInfo(savedLanguage);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "TodoApp" };
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using System.Windows;
using XamlLocalizationEditor.Services;
using XamlLocalizationEditor.ViewModels;

namespace XamlLocalizationEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            Services = serviceCollection.BuildServiceProvider();
            var mainVM = Services.GetRequiredService<MainViewModel>();

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = mainVM;
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDialogService, DialogService>();

            services.AddSingleton<AppState>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();

            services.AddTransient<MenuViewModel>();
            services.AddTransient<EditorViewModel>();
        }
    }

}

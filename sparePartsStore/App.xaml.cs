using Microsoft.Extensions.DependencyInjection;
using sparePartsStore.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace sparePartsStore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            // создание экземплятор сервисов
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Регистрация MainHeadViewModel как зависимость
            services.AddTransient<MainHeadViewModel>();
        }

        // переопределение GetService для получения экземпляра зависимостей
        protected object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }
    }

}

using Caliburn.Micro;
using DesktopAssignment.Data;
using DesktopAssignment.Services;
using DesktopAssignment.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DesktopAssignment
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer container;

        public Bootstrapper()
        {
            Initialize();
        }

        //Configure the IoC container
        protected override void Configure()
        {
            container = new SimpleContainer();
            //Register services
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IGeolocationService, GeolocationService>();

            // DbContext with options
            var options = new DbContextOptionsBuilder<GeolocationDbContext>()
                .UseSqlite($"Data Source={System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "geolocation.db")}")
                .Options;
            container.Instance(options);
            container.Singleton<GeolocationDbContext>();

            container.PerRequest<ShellViewModel>();
        }

        protected override object GetInstance(System.Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(System.Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override async void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            await DisplayRootViewForAsync<ShellViewModel>();
        }
    }
}

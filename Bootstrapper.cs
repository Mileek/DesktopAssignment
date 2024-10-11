using Caliburn.Micro;
using DesktopAssignment.ViewModels;

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
            container.Singleton<IEventAggregator, EventAggregator>();
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

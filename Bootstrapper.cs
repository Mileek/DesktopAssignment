using Caliburn.Micro;
using DesktopAssignment.ViewModels;

namespace DesktopAssignment
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewForAsync<ShellViewModel>();
        }
    }
}

using Caliburn.Micro;

namespace DesktopAssignment.ViewModels
{
    public class WarningDialogViewModel : Screen
    {
        private string warningMessage;

        public string WarningMessage
        {
            get { return warningMessage; }
            set
            {
                warningMessage = value;
                NotifyOfPropertyChange(() => WarningMessage);
            }
        }

        public WarningDialogViewModel(string warningMessage)
        {
            WarningMessage = warningMessage;
        }

        public async Task Ok()
        {
            await TryCloseAsync(true);
        }
    }
}
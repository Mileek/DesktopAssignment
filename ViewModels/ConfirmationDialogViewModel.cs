using Caliburn.Micro;

namespace DesktopAssignment.ViewModels
{
    public class ConfirmationDialogViewModel : Screen
    {
        private bool isConfirmed;

        public bool IsConfirmed
        {
            get { return isConfirmed; }
            set
            {
                isConfirmed = value;
                NotifyOfPropertyChange(() => IsConfirmed);
            }
        }

        public async Task Confirm()
        {
            IsConfirmed = true;
            await TryCloseAsync(true);
        }

        public async Task Decline()
        {
            IsConfirmed = false;
            await TryCloseAsync(true);
        }
    }
}

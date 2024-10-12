using Caliburn.Micro;
using DesktopAssignment.Data;
using DesktopAssignment.Models;
using DesktopAssignment.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Data.Common;

/*
 Summary
The aim of this task is to build a desktop application (backed by any kind of database). The application should be able to store geolocation data in the database, based on IP address or URL - you can use https://ipstack.com/ to get geolocation data. The application should be able to add, delete or provide geolocation data on the base of ip address or URL.

Application specification
It should be implemented using WPF
You can use https://ipstack.com/ for the geolocation of IP addresses and URLs
The application can be built in .net framework
Usage of any free library which will help implement solution is acceptable (e.g. Google material design Rx.Net, Caliburn.micro any MVVM library)
The solution should also include base specs/tests coverage

How to submit
Create a public Git repository and share the link with us

Notes:
We will run the application on our local machines for testing purposes. This implies that the solution should provide a quick and easy way to get the system up and running, including test data
We will test the behavior of the system under various "unfortunate" conditions (hint: How will the app behave when we take down the DB? How about the IPStack API?)
After we finish reviewing the solution, we'll invite you to Sofomo's office (or to a Zoom call) for a short discussion about the provided solution. We may also use that as an opportunity to ask questions and drill into the details of your implementation.
 */

/*
 Hi,

I would like to provide some context before we start the task. I Used .NET8 instead of .NET Framework. I have decided to use the following technologies and frameworks for this project:

1. **MVVM Framework**: Caliburn.Micro
2. **Database Provider**: SQLite
3. **UI Framework**: Material Design
4. **Testing Framework**: XUnit and Moq
5. **Object-Relational Mapping**: Entity Framework Core
6. **Dependency Injection**: SimpleContainer class provided by Caliburn.Micro

Areas for potential improvement:
1. I have not implemented a logging system (e.g., NLog).
2. Validation could be improved using regex.
3. I have not implemented a translation manager for the UI; all strings are hardcoded.
4. I could have used AutoFac for better dependency injection handling.
5. I should have used pull requests and feature branches instead of committing directly to the master branch.

Thank you.
 */

namespace DesktopAssignment.ViewModels
{
    public class ShellViewModel : Screen
    {
        //Default IP address, may work as a Tip
        private const string DEFAULT_IP_ADDRESS = "134.201.250.155";

        private readonly IDatabaseService _databaseService;
        private readonly IWindowManager _windowManager;
        private readonly GeolocationDbContext dbContext;
        private IGeolocationService _geolocationService;
        private string apiKey = string.Empty;
        private ObservableCollection<GeolocationModel> geolocations = new ObservableCollection<GeolocationModel>();
        private string ipAddressOrUrl = DEFAULT_IP_ADDRESS;

        public ShellViewModel(IWindowManager windowManager, IGeolocationService geolocationService, GeolocationDbContext dbContext, IDatabaseService databaseService)
        {
            _geolocationService = geolocationService ?? throw new ArgumentNullException(nameof(geolocationService));
            _windowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }

        public string ApiKey
        {
            get { return apiKey; }
            set
            {
                apiKey = value;
                NotifyOfPropertyChange(() => ApiKey);
                _geolocationService.SetApiKey(apiKey);
            }
        }

        public ObservableCollection<GeolocationModel> Geolocations
        {
            get { return geolocations; }
            set
            {
                geolocations = value;
                NotifyOfPropertyChange(() => Geolocations);
            }
        }

        public string IpAddressOrUrl
        {
            get { return ipAddressOrUrl; }
            set
            {
                ipAddressOrUrl = value;
                NotifyOfPropertyChange(() => IpAddressOrUrl);
            }
        }

        /// <summary>
        /// Add geolocation data to the database
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddGeolocationAsync()
        {
            try
            {
                if (!await ValidateGeolocationServiceAsync())
                {
                    return;
                }
                await _databaseService.EnsureDatabaseExistsAsync();
                var geolocation = await _geolocationService.GetGeolocationDataAsync(IpAddressOrUrl);
                dbContext.Geolocation.Add(geolocation);
                await dbContext.SaveChangesAsync();
                Geolocations.Add(geolocation);
            }
            catch (Exception ex)
            {
                //Handle db exceptions
                await HandleDbExceptionAsync(ex, $"An unexpected error occurred:");
            }
        }

        /// <summary>
        /// Deletes geolocation data from the database.
        /// </summary>
        /// <param name="geolocationModel">The geolocation model to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteItemAsync(GeolocationModel geolocationModel)
        {
            try
            {
                if (geolocationModel != null)
                {
                    await _databaseService.EnsureDatabaseExistsAsync();
                    Geolocations.Remove(geolocationModel);
                    dbContext.Geolocation.Remove(geolocationModel);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                //Handle db exceptions
                await HandleDbExceptionAsync(ex, $"An unexpected error occurred:");
            }
        }

        /// <summary>
        /// Read geolocation data from the database
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ReadGeolocationAsync()
        {
            try
            {
                Geolocations.Clear();
                await _databaseService.EnsureDatabaseExistsAsync();
                var geolocationsFromDb = await Task.Run(() => dbContext.Geolocation.ToList());
                if (geolocationsFromDb.Count == 0)
                {
                    await ShowWarningAsync("Database is empty.");
                    return;
                }

                foreach (var geolocation in geolocationsFromDb)
                {
                    Geolocations.Add(geolocation);
                    await Task.Delay(50); //Small delay to improve user experience
                }
            }
            catch (Exception ex)
            {
                //Handle db exceptions
                await HandleDbExceptionAsync(ex, $"An unexpected error occurred:");
            }
        }

        /// <summary>
        /// Remove all geolocation data from the database
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RemoveAllAsync()
        {
            var confirmationDialogViewModel = new ConfirmationDialogViewModel();
            var result = await _windowManager.ShowDialogAsync(confirmationDialogViewModel);

            if (result == true && confirmationDialogViewModel.IsConfirmed)
            {
                dbContext.Geolocation.RemoveRange(dbContext.Geolocation);
                await dbContext.SaveChangesAsync();
                Geolocations.Clear();
            }
        }

        /// <summary>
        /// Displays a warning dialog with the specified message.
        /// </summary>
        /// <param name="warningMessage">The message to display in the warning dialog.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ShowWarningAsync(string warningMessage)
        {
            var warningDialog = new WarningDialogViewModel(warningMessage);
            await _windowManager.ShowDialogAsync(warningDialog);
        }

        /// <summary>
        /// Handles database-related exceptions and displays a warning dialog with a custom message.
        /// </summary>
        /// <param name="ex">The exception that was thrown.</param>
        /// <param name="customMessage">The custom message to display in the warning dialog.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandleDbExceptionAsync(Exception ex, string customMessage)
        {
            if (ex is DbUpdateException || ex is DbException)
            {
                await ShowWarningAsync($"{customMessage}: {ex.Message}");
            }
            else
            {
                await ShowWarningAsync($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates the geolocation service by checking if the service instance is available and if the API key is set.
        /// </summary>
        /// <returns>Returns true if the geolocation service is valid; otherwise, false.</returns>
        private async Task<bool> ValidateGeolocationServiceAsync()
        {
            if (_geolocationService == null)
            {
                await ShowWarningAsync("Cannot connect with web API, the service may be down or your API Key may not be valid. Try again later.");
                return false;
            }
            if (string.IsNullOrEmpty(ApiKey))
            {
                await ShowWarningAsync("API Key cannot be empty. Please provide a valid API Key.");
                return false;
            }

            return true;
        }
    }
}
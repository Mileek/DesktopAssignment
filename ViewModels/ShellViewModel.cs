using Caliburn.Micro;
using DesktopAssignment.Data;
using DesktopAssignment.Models;
using DesktopAssignment.Services;
using DesktopAssignment.Views;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
namespace DesktopAssignment.ViewModels
{
    public class ShellViewModel : Screen
    {
        private string ipAddressOrUrl;
        private string apiKey;
        private ObservableCollection<GeolocationModel> geolocations;
        private readonly GeolocationDbContext dbContext;
        private GeolocationService geolocationService;

        public ShellViewModel()
        {
            IpAddressOrUrl = "134.201.250.155";
            dbContext = new GeolocationDbContext();
            Geolocations = new ObservableCollection<GeolocationModel>(/*dbContext.Geolocation.ToList()*/);
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

        public string ApiKey
        {
            get { return apiKey; }
            set
            {
                apiKey = value;
                NotifyOfPropertyChange(() => ApiKey);
                geolocationService = new GeolocationService(apiKey);
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

        public async Task AddGeolocation()
        {
            if (geolocationService == null)
            {
                ShowWarning("Cannot connect with web API, the service may be down or your API Key may not be valid. Try again later.");
                return;
            }
            if (string.IsNullOrEmpty(ApiKey))
            {
                ShowWarning("API Key cannot be empty. Please provide a valid API Key.");
                return;
            }

            var geolocation = await geolocationService.GetGeolocationDataAsync(IpAddressOrUrl);
            dbContext.Geolocation.Add(geolocation);
            await dbContext.SaveChangesAsync();
            Geolocations.Add(geolocation);
        }

        public async Task ReadGeolocation()
        {
            Geolocations.Clear();

            var geolocationsFromDb = await Task.Run(() => dbContext.Geolocation.ToList());

            foreach (var geolocation in geolocationsFromDb)
            {
                Geolocations.Add(geolocation);
                await Task.Delay(100); //Small delay to simulate processing time
            }
        }

        public async Task DeleteItem(GeolocationModel geolocationModel)
        {   
            if (geolocationModel != null)
            {
                Geolocations.Remove(geolocationModel);
                dbContext.Geolocation.Remove(geolocationModel);
                await dbContext.SaveChangesAsync();
                Geolocations.Remove(geolocationModel);
            }
        }

        public async Task RemoveAll()
        {
            var confirmationDialog = new ConfirmationDialog();
            confirmationDialog.Owner = Application.Current.MainWindow;
            confirmationDialog.ShowDialog();

            if (confirmationDialog.IsConfirmed)
            {
                dbContext.Geolocation.RemoveRange(dbContext.Geolocation);
                await dbContext.SaveChangesAsync();
                Geolocations.Clear();
            }
        }
        public void ShowWarning(string warningMessage)
        {
            var warningDialog = new WarningDialog(warningMessage);
            warningDialog.Owner = Application.Current.MainWindow;
            warningDialog.ShowDialog();
        }
    }
}

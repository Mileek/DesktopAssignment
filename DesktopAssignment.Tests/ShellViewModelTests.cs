using Moq;
using Caliburn.Micro;
using DesktopAssignment.ViewModels;
using DesktopAssignment.Services;
using DesktopAssignment.Data;
using DesktopAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace DesktopAssignment.Tests.ViewModels
{
    public class ShellViewModelTests : IAsyncLifetime
    {
        private readonly Mock<IWindowManager> _windowManagerMock;
        private readonly Mock<IGeolocationService> _geolocationServiceMock;
        private readonly GeolocationDbContext _dbContext;
        private readonly ShellViewModel _viewModel;
        private readonly Mock<IDatabaseService> _databaseServiceMock;

        public ShellViewModelTests()
        {
            _windowManagerMock = new Mock<IWindowManager>();
            _geolocationServiceMock = new Mock<IGeolocationService>();
            _databaseServiceMock = new Mock<IDatabaseService>();
            //Use SQLite in-memory database for testing
            var options = new DbContextOptionsBuilder<GeolocationDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            _dbContext = new GeolocationDbContext(options);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();

            _viewModel = new ShellViewModel(_windowManagerMock.Object, _geolocationServiceMock.Object, _dbContext, _databaseServiceMock.Object)
            {
                ApiKey = "fake_ApiKey"
            };
        }

        public async Task InitializeAsync()
        {
            //Clean up the database before each test
            _dbContext.Geolocation.RemoveRange(_dbContext.Geolocation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            //Dispose the database connection after all tests are done
            await _dbContext.Database.CloseConnectionAsync();
        }

        [Fact]
        public void ShellViewModel_Constructor_ShouldThrowArgumentNullException_WhenWindowManagerIsNull()
        {
            // arragnge & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ShellViewModel(null!, _geolocationServiceMock.Object, _dbContext, _databaseServiceMock.Object));
        }

        [Fact]
        public void ShellViewModel_Constructor_ShouldThrowArgumentNullException_WhenGeolocationServiceIsNull()
        {
            // Arrange % Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ShellViewModel(_windowManagerMock.Object, null!, _dbContext, _databaseServiceMock.Object));
        }

        [Fact]
        public void ShellViewModel_Constructor_ShouldThrowArgumentNullException_WhenDbContextIsNull()
        {
            // Arragnge & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ShellViewModel(_windowManagerMock.Object, _geolocationServiceMock.Object, null!, _databaseServiceMock.Object));
        }

        [Fact]
        public void ShellViewModel_Constructor_ShouldThrowArgumentNullException_WhenDatabaseServiceIsNull()
        {
            // Arragnge & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ShellViewModel(_windowManagerMock.Object, _geolocationServiceMock.Object, _dbContext, null!));
        }

        [Fact]
        public async Task AddGeolocation_ShouldAddGeolocationToCollection()
        {
            // Arrange
            var geolocation = CreateSampleGeolocation();
            _geolocationServiceMock.Setup(s => s.GetGeolocationDataAsync(It.IsAny<string>()))
                                   .ReturnsAsync(geolocation);

            // Act
            await _viewModel.AddGeolocationAsync();

            // Assert
            Assert.Contains(geolocation, _viewModel.Geolocations);
        }

        [Fact]
        public async Task ReadGeolocation_ShouldPopulateGeolocations()
        {
            // Arrange
            var geolocations = CreateSampleGeolocations();
            _dbContext.Geolocation.RemoveRange(_dbContext.Geolocation);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Geolocation.AddRangeAsync(geolocations);
            await _dbContext.SaveChangesAsync();

            // Act
            await _viewModel.ReadGeolocationAsync();

            // Assert
            Assert.Equal(geolocations.Count, _viewModel.Geolocations.Count);
        }

        [Fact]
        public async Task DeleteItem_ShouldRemoveGeolocationFromCollection()
        {
            // Arrange
            var geolocation = CreateSampleGeolocation();
            await _dbContext.Geolocation.AddAsync(geolocation);
            await _dbContext.SaveChangesAsync();
            _viewModel.Geolocations.Add(geolocation);

            // Act
            await _viewModel.DeleteItemAsync(geolocation);

            // Assert
            Assert.DoesNotContain(geolocation, _viewModel.Geolocations);
        }

        [Fact]
        public async Task RemoveAll_ShouldClearGeolocationsCollection()
        {
            // Arrange
            var geolocation = CreateSampleGeolocation();
            await _dbContext.Geolocation.AddAsync(geolocation);
            await _dbContext.SaveChangesAsync();
            _viewModel.Geolocations.Add(geolocation);

            // Mock the confirmation dialog to return true
            _windowManagerMock.Setup(wm => wm.ShowDialogAsync(It.IsAny<ConfirmationDialogViewModel>(), null, null))
                      .ReturnsAsync((ConfirmationDialogViewModel vm, object _, object _) =>
                      {
                          vm.IsConfirmed = true;
                          return true;
                      });

            // Act
            await _viewModel.RemoveAllAsync();

            // Assert
            Assert.Empty(_viewModel.Geolocations);
            Assert.Empty(await _dbContext.Geolocation.ToListAsync());
        }

        [Fact]
        public async Task AddGeolocation_ShouldShowWarning_WhenApiKeyIsEmpty()
        {
            // Arrange
            _viewModel.ApiKey = string.Empty;

            // Act
            await _viewModel.AddGeolocationAsync();

            // Assert
            _windowManagerMock.Verify(wm => wm.ShowDialogAsync(It.IsAny<WarningDialogViewModel>(), null, null), Times.Once);
        }

        [Fact]
        public async Task AddGeolocation_ShouldShowWarning_WhenGeolocationServiceFails()
        {
            // Arrange
            _geolocationServiceMock.Setup(s => s.GetGeolocationDataAsync(It.IsAny<string>()))
                                   .ThrowsAsync(new Exception("Service error"));

            // Act
            await _viewModel.AddGeolocationAsync();

            // Assert
            _windowManagerMock.Verify(wm => wm.ShowDialogAsync(It.IsAny<WarningDialogViewModel>(), null, null), Times.Once);
        }

        [Fact]
        public async Task AddGeolocation_ShouldShowWarning_WhenGeolocationServiceIsNull()
        {
            // Arrange
            var viewModel = new ShellViewModel(_windowManagerMock.Object, _geolocationServiceMock.Object, _dbContext, _databaseServiceMock.Object);

            // Act
            await viewModel.AddGeolocationAsync();

            // Assert
            _windowManagerMock.Verify(wm => wm.ShowDialogAsync(It.IsAny<WarningDialogViewModel>(), null, null), Times.Once);
        }

        [Fact]
        public async Task ShowWarning_ShouldDisplayWarningMessage()
        {
            // Arrange
            var warningMessage = "Test warning message";

            // Act
            await _viewModel.ShowWarningAsync(warningMessage);

            // Assert
            _windowManagerMock.Verify(wm => wm.ShowDialogAsync(It.Is<WarningDialogViewModel>(vm => vm.WarningMessage == warningMessage), null, null), Times.Once);
        }

        // Helper method to create a sample geolocation model
        private GeolocationModel CreateSampleGeolocation()
        {
            return new GeolocationModel
            {
                Ip = "134.201.250.155",
                Type = "IPv4",
                ContinentName = "North America",
                CountryName = "USA",
                RegionName = "Huston",
                City = "Washington",
                Zip = "111111",
                Latitude = 12.345,
                Longitude = 345.678
            };
        }

        // Helper method to create a list of sample geolocation models
        private List<GeolocationModel> CreateSampleGeolocations()
        {
            return new List<GeolocationModel>
            {
                new GeolocationModel
                {
                    Ip = "134.201.250.155",
                    Type = "IPv4",
                    ContinentName = "North America",
                    CountryName = "USA",
                    RegionName = "Huston",
                    City = "Washington",
                    Zip = "111111",
                    Latitude = 12.345,
                    Longitude = 345.678
                },
                new GeolocationModel
                {
                    Ip = "134.201.250.155",
                    Type = "IPv4",
                    ContinentName = "North America",
                    CountryName = "USA",
                    RegionName = "Huston",
                    City = "Washington",
                    Zip = "111111",
                    Latitude = 12.345,
                    Longitude = 345.678
                }
            };
        }
    }
}
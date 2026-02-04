using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Buffers.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using TesterBudAutomationFramework.Core.Config;
using TesterBudAutomationFramework.Core.Controls;
using TesterBudAutomationFramework.Core.Drivers;
using TesterBudAutomationFramework.Core.Waits;
using TesterBudAutomationFramework.Pages;
using TesterBudAutomationFramework.Services;

namespace TesterBudAutomationFramework.Tests
{
    [TestFixture]
    [Category("UI")]
    internal class FlightBookingTests
    {
        private IWebDriver _driver;
        private FlightBookingPage _flightPage;
        private FlightBookingService _flightService;
        private PaymentModalService _paymentService;

        private const string DefaultFrom = "New York";
        private const string DefaultTo = "London";

        private DateTime _departureDate;
        private DateTime _returnDate;

        [SetUp]
        public void SetUp()
        {
            var config = TestConfig.CurrentSetting;
            _driver = WebDriverFactory.CreateWebDriver(config.Browser, config.PageLoadSec);
            _flightPage = new FlightBookingPage(_driver).GoToFlightBookingPage();
            _flightService = new FlightBookingService(_driver);
            _paymentService = new PaymentModalService(_driver);
            _departureDate = DateTime.Today.AddDays(7);
            _returnDate = DateTime.Today.AddDays(8);
        }

        [Test]
        public void N01_Search_OneWay_MinimalRequiredData_DisplaysAvailableFlights()
        {
            var results = _flightService.SearchOneWay(DefaultFrom, DefaultTo, _departureDate);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_flightService.HasFlights(results), "Expected available flights, but none were found.");
                Assert.That(_flightService.ShowsNoFlightsMessage(results), Is.False, "Unexpected 'No flights found' message.");

                Assert.That(_flightService.AllFlightsMatchRoute(DefaultFrom), "From city do not match.");
                Assert.That(_flightService.AllFlightsMatchRoute(DefaultTo), "To city do not match.");
                Assert.That(_flightService.AllFlightsMatchDate(_departureDate), "Departure date do not match.");
            });
        }

        [Test]
        public void N02_Search_RoundTrip_WithValidDates_DisplaysMatchingFlights()
        {
            var results = _flightService.SearchRoundWay(DefaultFrom, DefaultTo, _departureDate, _returnDate);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_flightService.HasFlights(results), "Expected flights for round trip.");
                Assert.That(_flightService.ShowsNoFlightsMessage(results), Is.False, "Unexpected 'No flights found'.");

                Assert.That(_flightService.AllFlightsMatchRoute(DefaultFrom), "From city do not match.");
                Assert.That(_flightService.AllFlightsMatchRoute(DefaultTo), "To city do not match.");
                Assert.That(_flightService.AllFlightsMatchDate(_departureDate), "Departure date do not match.");
                Assert.That(_flightService.AllFlightsMatchDate(_returnDate), "Return date do not match.");
            });
        }

        [Test]
        public void N03_Search_SinglePassenger_DefaultAdult_DisplaysFlights()
        {
            // Arrange
            var adults = 1;

            var results = _flightService.SearchWithPassengers(DefaultFrom, DefaultTo, _departureDate, adults);

            // Assert
            Assert.That(_flightService.HasFlights(results), "Expected flights for 1 adult.");
        }


        [Test]
        public void N04_Search_MultiplePassengers_DisplaysFlightsWithSufficientSeats()
        {
            var adults = 3;

            var results = _flightService.SearchWithPassengers(DefaultFrom, DefaultTo, _departureDate, adults);

            Assert.That(_flightService.HasFlights(results), "Expected flights for 3 passengers.");
        }

        [Test]
        public void N05_SelectFlight_FromSearchResults_ShowsPaymentModal()
        {
            var results = _flightService.SearchOneWay(DefaultFrom, DefaultTo, _departureDate);
            var paymentModal = _flightService.SelectFirstFlight(results);

            Assert.That(_paymentService.IsPaymentModalPresented(paymentModal), Is.True, "Payment modal is not opened.");
        }

        [Test]
        public void N06_Booking_WithValidPassengerAndPaymentData_Succeeds()
        {
            var results = _flightService.SearchOneWay(DefaultFrom, DefaultTo, _departureDate);
            var paymentModal = _flightService.SelectFirstFlight(results);
            var completed = _paymentService
                .CompleteBookingWithValidData(paymentModal, "4242424242424242", "12/30", "123");

            Assert.That(_flightService.IsBookingSuccessfulMessagePresented(completed), Is.True, "Booking confirmation ID not found.");
        }

        //// ---------- Negative ----------

        [Test]
        public void N07_Search_EmptyOrigin_ShowsRequiredFieldError()
        {
            var results = _flightService.SearchOneWayWithoutOriginCity(DefaultTo, _departureDate);

            Assert.Multiple(() =>
            {
                Assert.That(_flightService.ShowsOriginRequired(results), Is.True, "Expected origin required error.");
                Assert.That(_flightService.GetOriginRequiredErrorMessage(results), Is.EqualTo("Please select a departure city."));
            });
        }

        [Test]
        public void N08_Search_EmptyDestination_ShowsRequiredFieldError()
        {
            var results = _flightService.SearchOneWayWithoutDestinationCity(DefaultFrom, _departureDate);

            Assert.Multiple(() =>
            {
                Assert.That(_flightService.ShowsDestinationRequired(results), Is.True, "Expected destination required error.");
                Assert.That(_flightService.GetDestinationRequiredErrorMessage(results), Is.EqualTo("Please select a destination city."));
            });
        }

        [Test]
        public void N09_Search_EmptyDepartureDate_ShowsRequiredFieldError()
        {
            var results = _flightService.SearchOneWayWithoutDepartureDate(DefaultFrom, DefaultTo);

            Assert.Multiple(() =>
            {
                Assert.That(_flightService.ShowsDepartureDateRequired(results), Is.True, "Expected date required error.");
                Assert.That(_flightService.GetDepartureRequiredErrorMessage(results), Is.EqualTo("Departure date cannot be in the past."));
            });
        }

        [Test]
        public void N010_Search_DepartureDateInPast_BlockedWithError()
        {
            var results = _flightService.SearchOneWayWithoutReturnDate(DefaultFrom, DefaultTo, _departureDate);

            Assert.Multiple(() =>
            {
                Assert.That(_flightService.ShowsReturnDateRequired(results), Is.True, "Expected validation for past date (or date error).");
                Assert.That(_flightService.GetReturnRequiredErrorMessage(results), Is.EqualTo("Return date must be after departure date."));
            });
        }

        [Test]
        public void N011_Search_SameOriginAndDestination_OneWay_ShowsReturnDestinationConflict()
        {
            var results = _flightService.SearchOneWay(DefaultFrom, DefaultFrom, _departureDate);

            Assert.Multiple(() =>
            {
                Assert.That(_flightService.ShowsDestinationRequired(results), Is.True, "Expected destination required error.");
                Assert.That(_flightService.GetDestinationRequiredErrorMessage(results), Is.EqualTo("Departure and destination cities cannot be the same."));
            });
        }

        public void N012_Search_SameOriginAndDestination_RoundWay_ShowsReturnDestinationConflict()
        {
            var results = _flightService.SearchRoundWay(DefaultFrom, DefaultFrom, _departureDate, _returnDate);

            Assert.Multiple(() =>
            {
                Assert.That(_flightService.ShowsDestinationRequired(results), Is.True, "Expected destination required error.");
                Assert.That(_flightService.GetDestinationRequiredErrorMessage(results), Is.EqualTo("Departure and destination cities cannot be the same."));
            });
        }

        public void N013_Search_OneWay_EmptyFieldsShowsConflicts()
        {
            var results = _flightService.SearchOneWayFlightsWithEmptyFields();

            Assert.Multiple(() =>
            {
                Assert.That(_flightService.ShowsOriginRequired(results), Is.True, "Expected origin required error.");
                Assert.That(_flightService.GetOriginRequiredErrorMessage(results), Is.EqualTo("Please select a departure city."));
                Assert.That(_flightService.ShowsDestinationRequired(results), Is.True, "Expected destination required error.");
                Assert.That(_flightService.GetDestinationRequiredErrorMessage(results), Is.EqualTo("Please select a destination city."));
                Assert.That(_flightService.ShowsDepartureDateRequired(results), Is.True, "Expected date required error.");
                Assert.That(_flightService.GetDepartureRequiredErrorMessage(results), Is.EqualTo("Departure date cannot be in the past."));
            });
        }

        [Test]
        public void N014_Search_RoundWay_EmptyFieldsShowsConflicts()
        {
            var results = _flightService.SearchRoundWayFlightsWithEmptyFields();

            Assert.Multiple(() =>
            {
                Assert.That(_flightService.ShowsOriginRequired(results), Is.True, "Expected origin required error.");
                Assert.That(_flightService.GetOriginRequiredErrorMessage(results), Is.EqualTo("Please select a departure city."));
                Assert.That(_flightService.ShowsDestinationRequired(results), Is.True, "Expected destination required error.");
                Assert.That(_flightService.GetDestinationRequiredErrorMessage(results), Is.EqualTo("Please select a destination city."));
                Assert.That(_flightService.ShowsDepartureDateRequired(results), Is.True, "Expected date required error.");
                Assert.That(_flightService.GetDepartureRequiredErrorMessage(results), Is.EqualTo("Departure date cannot be in the past."));
                Assert.That(_flightService.ShowsReturnDateRequired(results), Is.True, "Expected validation for past date (or date error).");
                Assert.That(_flightService.GetReturnRequiredErrorMessage(results), Is.EqualTo("Return date must be after departure date."));
            });
        }

        //[Test]
        //public void Payment_EmptyCardNumber_ShowsPopUp()
        //{
        //    var search = new FlightSearchService(Driver, Wait, BaseUrl);
        //    var results = search.SearchOneWay("NYC", "LAX", DateTime.Today.AddDays(7));
        //    var booking = results.SelectFirstFlight();

        //    var payment = booking.FillPassenger("John", "Doe", "AB1234567").ContinueToPayment();
        //    payment.EnterCard("4111111111111110", "10/30", "123").SubmitExpectingStayOnPage();

        //    Assert.IsTrue(payment.IsDeclined(), "");
        //}

        [TearDown]
        public void TearDown()
        {
            _driver.Dispose();
        }
    }
}

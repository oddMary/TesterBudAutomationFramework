using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using Shouldly;
using System.IO;
using TesterBudAutomationFramework.Core.Config;
using TesterBudAutomationFramework.Core.Constants;
using TesterBudAutomationFramework.Core.Drivers;
using TesterBudAutomationFramework.Services;

namespace TesterBudAutomationFramework.Tests
{
    [AllureNUnit]
    [AllureSuite("Flights")]
    [AllureFeature("Booking")]
    [TestFixture]
    [Category("UI")]
    public class FlightBookingTests : BaseTest
    {
        private IWebDriver _driver;
        private FlightBookingService _flightService;
        private PaymentModalService _paymentService;

        private DateTime _departureDate;
        private DateTime _returnDate;

        [SetUp]
        public void SetUp()
        {
            var config = TestConfig.CurrentSetting;
            _driver = WebDriverFactory.CreateWebDriver(config.Browser, config.PageLoadSec);
            _flightService = new FlightBookingService(_driver);
            _paymentService = new PaymentModalService(_driver);
            _departureDate = DateTime.Today.AddDays(7);
            _returnDate = DateTime.Today.AddDays(8);
        }

        [Test]
        [AllureStory("Search One-Way")]
        [AllureSeverity(SeverityLevel.critical)]
        public void N01_Search_OneWay_MinimalRequiredData_DisplaysAvailableFlights()
        {
            var results = _flightService.SearchOneWay(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate);

            _flightService.HasFlights(results).ShouldBeTrue("Expected available flights, but none were found.");
            _flightService.NoFlightsMessageNotShown(results).ShouldBeTrue("Unexpected 'No flights found' message.");
            _flightService.AllFlightsMatchRoute(TestConstants.DEFAULT_FROM).ShouldBeTrue("From city do not match.");
            _flightService.AllFlightsMatchRoute(TestConstants.DEFAULT_TO).ShouldBeTrue("To city do not match.");
            _flightService.DepartureFlightDateMatch(_departureDate).ShouldBeTrue("Departure date do not match.");
        }

        [Test]
        public void N02_Search_RoundTrip_WithValidDates_DisplaysMatchingFlights()
        {
            var results = _flightService.SearchRoundWay(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate, _returnDate);

            _flightService.HasFlights(results).ShouldBeTrue("Expected available flights, but none were found.");
            _flightService.NoFlightsMessageNotShown(results).ShouldBeTrue("Unexpected 'No flights found' message.");
            _flightService.AllFlightsMatchRoute(TestConstants.DEFAULT_FROM).ShouldBeTrue("From city do not match.");
            _flightService.AllFlightsMatchRoute(TestConstants.DEFAULT_TO).ShouldBeTrue("To city do not match.");
            _flightService.DepartureFlightDateMatch(_departureDate).ShouldBeTrue("Departure date do not match.");
            _flightService.ReturnFlightDateMatch(_returnDate).ShouldBeTrue("Return date do not match.");
        }

        [Test]
        public void N03_Search_SinglePassenger_DefaultAdult_DisplaysFlights()
        {
            var adults = 1;

            var results = _flightService.SearchWithPassengers(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate, adults);

            _flightService.HasFlights(results).ShouldBeTrue("Expected flights for 1 adult.");
        }


        [Test]
        public void N04_Search_MultiplePassengers_DisplaysFlightsWithSufficientSeats()
        {
            var adults = 3;

            var results = _flightService.SearchWithPassengers(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate, adults);

            _flightService.HasFlights(results).ShouldBeTrue("Expected flights for 3 passengers.");
        }

        [Test]
        public void N05_SelectFlight_FromSearchResults_ShowsPaymentModal()
        {
            var results = _flightService.SearchOneWay(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate);
            var paymentModal = _flightService.SelectFirstFlight(results);

            _paymentService.IsPaymentModalPresented(paymentModal).ShouldBeTrue("Payment modal is not opened.");
        }

        [Test]
        public void N06_Booking_WithValidPassengerAndPaymentData_Succeeds()
        {
            var results = _flightService.SearchOneWay(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate);
            var paymentModal = _flightService.SelectFirstFlight(results);
            var completed = _paymentService
                .CompleteBookingWithValidData(paymentModal, "4242424242424242", "12/30", "123");

            _flightService.IsBookingSuccessfulMessagePresented(completed).ShouldBeTrue("Booking confirmation ID not found.");
        }

        // ---------- Negative ----------

        [Test]
        public void N07_Search_EmptyOrigin_ShowsRequiredFieldError()
        {
            var results = _flightService.SearchOneWayWithoutOriginCity(TestConstants.DEFAULT_TO, _departureDate);

            _flightService.ShowsOriginRequired(results).ShouldBeTrue("Expected origin required error.");
            _flightService.GetOriginRequiredErrorMessage(results).ShouldBeEquivalentTo("Please select a departure city.");
        }

        [Test]
        public void N08_Search_EmptyDestination_ShowsRequiredFieldError()
        {
            var results = _flightService.SearchOneWayWithoutDestinationCity(TestConstants.DEFAULT_FROM, _departureDate);

            _flightService.ShowsDestinationRequired(results).ShouldBeTrue("Expected destination required error.");
            _flightService.GetDestinationRequiredErrorMessage(results).ShouldBeEquivalentTo("Please select a destination city.");
        }

        [Test]
        public void N09_Search_EmptyDepartureDate_ShowsRequiredFieldError()
        {
            var results = _flightService.SearchOneWayWithoutDepartureDate(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO);

            _flightService.ShowsDepartureDateRequired(results).ShouldBeTrue("Expected date required error.");
            _flightService.GetDepartureRequiredErrorMessage(results).ShouldBeEquivalentTo("Departure date cannot be in the past.");
        }

        [Test]
        public void N10_Search_DepartureDateInPast_BlockedWithError()
        {
            var results = _flightService.SearchOneWayWithoutReturnDate(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate);

            _flightService.ShowsReturnDateRequired(results).ShouldBeTrue("Expected validation for past date (or date error).");
            _flightService.GetReturnRequiredErrorMessage(results).ShouldBeEquivalentTo("Return date must be after departure date.");
        }

        [Test]
        public void N11_Search_SameOriginAndDestination_OneWay_ShowsReturnDestinationConflict()
        {
            var results = _flightService.SearchOneWay(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_FROM, _departureDate);

            _flightService.ShowsDestinationRequired(results).ShouldBeTrue("Expected destination required error.");
            _flightService.GetDestinationRequiredErrorMessage(results).ShouldBeEquivalentTo("Departure and destination cities cannot be the same.");
        }

        [Test]
        public void N12_Search_SameOriginAndDestination_RoundWay_ShowsReturnDestinationConflict()
        {
            var results = _flightService.SearchRoundWay(TestConstants.DEFAULT_TO, TestConstants.DEFAULT_TO, _departureDate, _returnDate);

            _flightService.ShowsDestinationRequired(results).ShouldBeTrue("Expected destination required error.");
            _flightService.GetDestinationRequiredErrorMessage(results).ShouldBeEquivalentTo("Departure and destination cities cannot be the same.");
        }

        [Test]
        public void N13_Search_OneWay_EmptyFieldsShowsConflicts()
        {
            var results = _flightService.SearchOneWayFlightsWithEmptyFields();

            _flightService.ShowsOriginRequired(results).ShouldBeTrue("Expected origin required error.");
            _flightService.GetOriginRequiredErrorMessage(results).ShouldBeEquivalentTo("Please select a departure city.");
            _flightService.ShowsDestinationRequired(results).ShouldBeTrue("Expected destination required error.");
            _flightService.GetDestinationRequiredErrorMessage(results).ShouldBeEquivalentTo("Please select a destination city.");
            _flightService.ShowsDepartureDateRequired(results).ShouldBeTrue("Expected date required error.");
            _flightService.GetDepartureRequiredErrorMessage(results).ShouldBeEquivalentTo("Departure date cannot be in the past.");
        }

        [Test]
        public void N14_Search_RoundWay_EmptyFieldsShowsConflicts()
        {
            var results = _flightService.SearchRoundWayFlightsWithEmptyFields();

            _flightService.ShowsOriginRequired(results).ShouldBeTrue("Expected origin required error.");
            _flightService.GetOriginRequiredErrorMessage(results).ShouldBeEquivalentTo("Please select a departure city.");
            _flightService.ShowsDestinationRequired(results).ShouldBeTrue("Expected destination required error.");
            _flightService.GetDestinationRequiredErrorMessage(results).ShouldBeEquivalentTo("Please select a destination city.");
            _flightService.ShowsDepartureDateRequired(results).ShouldBeTrue("Expected date required error.");
            _flightService.GetDepartureRequiredErrorMessage(results).ShouldBeEquivalentTo("Departure date cannot be in the past.");
            _flightService.ShowsReturnDateRequired(results).ShouldBeTrue("Expected validation for past date (or date error).");
            _flightService.GetReturnRequiredErrorMessage(results).ShouldBeEquivalentTo("Return date must be after departure date.");
        }

        [TearDown]
        public void AfterEach()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Passed)
            {
                try
                {
                    var testName = TestContext.CurrentContext.Test.Name;
                    var path = _shots.Save(_driver, testName, "teardown");
                    AllureApi.AddAttachment(testName, "image/png", path);
                }
                catch { }
            }
            _driver.Dispose();
        }
    }
};

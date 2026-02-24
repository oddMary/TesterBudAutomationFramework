using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Shouldly;
using TesterBudAutomationFramework.Core.Constants;

namespace TesterBudAutomationFramework.Tests
{
    [AllureNUnit]
    [AllureSuite("Flights")]
    [AllureFeature("Booking")]
    [TestFixture]
    [Category("UI")]
    public class FlightBookingTests : BaseTest
    {
        [Test]
        [AllureStory("Search One-Way")]
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        public void N01_Search_OneWay_MinimalRequiredData_DisplaysAvailableFlights()
        {
            var results = _flightService.SearchOneWay(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate);

            _flightService.HasFlights(results).ShouldBeTrue(TestConstants.NO_AWAILABLE_FLIGHTS);
            _flightService.NoFlightsMessageNotShown(results).ShouldBeTrue(TestConstants.UNEXPECTED_NO_FLIGHTS_FOUND);
            _flightService.AllFlightsMatchRoute(TestConstants.DEFAULT_FROM).ShouldBeTrue();
            _flightService.AllFlightsMatchRoute(TestConstants.DEFAULT_TO).ShouldBeTrue(TestConstants.FROM_CITY_NOT_MATCH);
            _flightService.DepartureFlightDateMatch(_departureDate).ShouldBeTrue(TestConstants.TO_CITY_NOT_MATCH);
        }

        [Test]
        public void N02_Search_RoundTrip_WithValidDates_DisplaysMatchingFlights()
        {
            var results = _flightService.SearchRoundWay(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate, _returnDate);

            _flightService.HasFlights(results).ShouldBeTrue(TestConstants.NO_AWAILABLE_FLIGHTS);
            _flightService.NoFlightsMessageNotShown(results).ShouldBeTrue(TestConstants.UNEXPECTED_NO_FLIGHTS_FOUND);
            _flightService.AllFlightsMatchRoute(TestConstants.DEFAULT_FROM).ShouldBeTrue(TestConstants.FROM_CITY_NOT_MATCH);
            _flightService.AllFlightsMatchRoute(TestConstants.DEFAULT_TO).ShouldBeTrue(TestConstants.TO_CITY_NOT_MATCH);
            _flightService.DepartureFlightDateMatch(_departureDate).ShouldBeTrue(TestConstants.DEPARTURE_DATE_NOT_MATCH);
            _flightService.ReturnFlightDateMatch(_returnDate).ShouldBeTrue(TestConstants.RETURN_DATE_NOT_MATCH);
        }

        [Test]
        public void N03_Search_SinglePassenger_DefaultAdult_DisplaysFlights()
        {
            var adults = 1;

            var results = _flightService.SearchWithPassengers(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate, adults);

            var message = string.Format(TestConstants.EXPECTED_FLIGHTS, adults);
            _flightService.HasFlights(results).ShouldBeTrue(message);
        }


        [Test]
        public void N04_Search_MultiplePassengers_DisplaysFlightsWithSufficientSeats()
        {
            var adults = 3;

            var results = _flightService.SearchWithPassengers(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate, adults);

            var message = string.Format(TestConstants.EXPECTED_FLIGHTS, adults);
            _flightService.HasFlights(results).ShouldBeTrue(message);
        }

        [Test]
        public void N05_SelectFlight_FromSearchResults_ShowsPaymentModal()
        {
            var results = _flightService.SearchOneWay(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate);
            var paymentModal = _flightService.SelectFirstFlight(results);

            _paymentService.IsPaymentModalPresented(paymentModal).ShouldBeTrue(TestConstants.PAYMENT_MODAL_NOT_OPENED);
        }

        [Test]
        public void N06_Booking_WithValidPassengerAndPaymentData_Succeeds()
        {
            var results = _flightService.SearchOneWay(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate);
            var paymentModal = _flightService.SelectFirstFlight(results);
            var completed = _paymentService
                .CompleteBookingWithValidData(paymentModal, "4242424242424242", "12/30", "123");

            _flightService.IsBookingSuccessfulMessagePresented(completed).ShouldBeTrue(TestConstants.BOOKING_CONFIRMATION_ID_NOT_FOUND);
        }

        // ---------- Negative ----------

        [Test]
        public void N07_Search_EmptyOrigin_ShowsRequiredFieldError()
        {
            var results = _flightService.SearchOneWayWithoutOriginCity(TestConstants.DEFAULT_TO, _departureDate);

            _flightService.ShowsOriginRequired(results).ShouldBeTrue(TestConstants.ORIGIN_REQUIRED_ERROR);
            _flightService.GetOriginRequiredErrorMessage(results).ShouldBeEquivalentTo(TestConstants.SELECT_DEPARTURE_CITY);
        }

        [Test]
        public void N08_Search_EmptyDestination_ShowsRequiredFieldError()
        {
            var results = _flightService.SearchOneWayWithoutDestinationCity(TestConstants.DEFAULT_FROM, _departureDate);

            _flightService.ShowsDestinationRequired(results).ShouldBeTrue(TestConstants.DESTINATION_REQUIRED_ERROR);
            _flightService.GetDestinationRequiredErrorMessage(results).ShouldBeEquivalentTo(TestConstants.SELECT_DESTINATION_CITY);
        }

        [Test]
        public void N09_Search_EmptyDepartureDate_ShowsRequiredFieldError()
        {
            var results = _flightService.SearchOneWayWithoutDepartureDate(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO);

            _flightService.ShowsDepartureDateRequired(results).ShouldBeTrue(TestConstants.DATE_REQUIRED_ERROR);
            _flightService.GetDepartureRequiredErrorMessage(results).ShouldBeEquivalentTo(TestConstants.DEPARTURE_DATE_IN_THE_PAST);
        }

        [Test]
        public void N10_Search_DepartureDateInPast_BlockedWithError()
        {
            var results = _flightService.SearchOneWayWithoutReturnDate(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_TO, _departureDate);

            _flightService.ShowsReturnDateRequired(results).ShouldBeTrue(TestConstants.PAST_DATE_OR_ERROR);
            _flightService.GetReturnRequiredErrorMessage(results).ShouldBeEquivalentTo(TestConstants.RETURN_DATE_AFTER_DEPARTURE);
        }

        [Test]
        public void N11_Search_SameOriginAndDestination_OneWay_ShowsReturnDestinationConflict()
        {
            var results = _flightService.SearchOneWay(TestConstants.DEFAULT_FROM, TestConstants.DEFAULT_FROM, _departureDate);

            _flightService.ShowsDestinationRequired(results).ShouldBeTrue(TestConstants.DESTINATION_REQUIRED_ERROR);
            _flightService.GetDestinationRequiredErrorMessage(results).ShouldBeEquivalentTo(TestConstants.SAME_DERARTURE_AND_DESTINATION);
        }

        [Test]
        public void N12_Search_SameOriginAndDestination_RoundWay_ShowsReturnDestinationConflict()
        {
            var results = _flightService.SearchRoundWay(TestConstants.DEFAULT_TO, TestConstants.DEFAULT_TO, _departureDate, _returnDate);

            _flightService.ShowsDestinationRequired(results).ShouldBeTrue(TestConstants.DESTINATION_REQUIRED_ERROR);
            _flightService.GetDestinationRequiredErrorMessage(results).ShouldBeEquivalentTo(TestConstants.SAME_DERARTURE_AND_DESTINATION);
        }

        [Test]
        public void N13_Search_OneWay_EmptyFieldsShowsConflicts()
        {
            var results = _flightService.SearchOneWayFlightsWithEmptyFields();

            _flightService.ShowsOriginRequired(results).ShouldBeTrue(TestConstants.ORIGIN_REQUIRED_ERROR);
            _flightService.GetOriginRequiredErrorMessage(results).ShouldBeEquivalentTo(TestConstants.SELECT_DEPARTURE_CITY);
            _flightService.ShowsDestinationRequired(results).ShouldBeTrue(TestConstants.DESTINATION_REQUIRED_ERROR);
            _flightService.GetDestinationRequiredErrorMessage(results).ShouldBeEquivalentTo(TestConstants.SELECT_DESTINATION_CITY);
            _flightService.ShowsDepartureDateRequired(results).ShouldBeTrue(TestConstants.DATE_REQUIRED_ERROR);
            _flightService.GetDepartureRequiredErrorMessage(results).ShouldBeEquivalentTo(TestConstants.DEPARTURE_DATE_IN_THE_PAST);
        }

        [Test]
        public void N14_Search_RoundWay_EmptyFieldsShowsConflicts()
        {
            var results = _flightService.SearchRoundWayFlightsWithEmptyFields();

            _flightService.ShowsOriginRequired(results).ShouldBeTrue(TestConstants.ORIGIN_REQUIRED_ERROR);
            _flightService.GetOriginRequiredErrorMessage(results).ShouldBeEquivalentTo(TestConstants.SELECT_DEPARTURE_CITY);
            _flightService.ShowsDestinationRequired(results).ShouldBeTrue(TestConstants.DESTINATION_REQUIRED_ERROR);
            _flightService.GetDestinationRequiredErrorMessage(results).ShouldBeEquivalentTo(TestConstants.SELECT_DESTINATION_CITY);
            _flightService.ShowsDepartureDateRequired(results).ShouldBeTrue(TestConstants.DATE_REQUIRED_ERROR);
            _flightService.GetDepartureRequiredErrorMessage(results).ShouldBeEquivalentTo(TestConstants.DEPARTURE_DATE_IN_THE_PAST);
            _flightService.ShowsReturnDateRequired(results).ShouldBeTrue(TestConstants.PAST_DATE_OR_ERROR);
            _flightService.GetReturnRequiredErrorMessage(results).ShouldBeEquivalentTo(TestConstants.RETURN_DATE_AFTER_DEPARTURE);
        }
    }
};

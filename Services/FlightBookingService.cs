using OpenQA.Selenium;
using Serilog;
using System.Text.RegularExpressions;
using TesterBudAutomationFramework.Core.Controls;
using TesterBudAutomationFramework.Pages;

namespace TesterBudAutomationFramework.Services
{
    public class FlightBookingService
    {
        IWebDriver _driver;
        FlightBookingPage _flightBookingPage;

        public FlightBookingService(IWebDriver driver)
        {
            Log.Debug("FlightBookingService.ctor: initializing with driver={driver}", driver);
            _driver = driver;
            _flightBookingPage = new FlightBookingPage(driver);
            Log.Debug("FlightBookingService.ctor: navigating to FlightBookingPage");
            _flightBookingPage.GoToFlightBookingPage();
            Log.Debug("FlightBookingService.ctor: initialized");
        }

        public FlightBookingPage SearchOneWay(string from, string to, DateTime date)
        {
            Log.Debug("SearchOneWay: from={from}, to={to}, date={date}", from, to, date);
            _flightBookingPage
                .GoToFlightBookingPage()
                .SetTripTypeOneWay()
                .SetFromCity(from)
                .SetToCity(to)
                .SetDepartureDate(date);

            var page = _flightBookingPage.SearchFlights();
            Log.Debug("SearchOneWay: search submitted");
            return page;
        }

        public bool HasFlights(FlightBookingPage flightPage)
        { 
            var list = flightPage.GetListOfAwailableFlights();
            var count = list?.Count ?? 0;
            var result = list?.Any() == true;
            Log.Debug("HasFlights: count={count}, result={result}", count, result);
            return result;
        }

        public bool NoFlightsMessageNotShown(FlightBookingPage flightPage)
        {
            var list = flightPage.GetNoFlightsMessage();
            var count = list?.Count ?? 0;
            var result = list?.Any() == true;
            Log.Debug("NoFlightsMessageNotShown: count={count}, result={result}", count, result);
            return result;
        }

        public FlightBookingPage SearchRoundWay(string from, string to, DateTime date, DateTime returnDate)
        {
            Log.Debug("SearchRoundWay: from={from}, to={to}, depart={depart}, return={return}", from, to, date, returnDate);
            SearchOneWay(from, to, date);
            _flightBookingPage
                .SetTripTypeRoundWay()
                .SetReturnDate(returnDate);

            var page = _flightBookingPage.SearchFlights();
            Log.Debug("SearchRoundWay: search submitted");
            return page;
        }

        public FlightBookingPage SearchOneWayFlightsWithEmptyFields()
        {
            Log.Debug("SearchOneWayFlightsWithEmptyFields: submitting empty search (one-way)");
            var page = _flightBookingPage.SearchFlights();
            return page;
        }

        public FlightBookingPage SearchRoundWayFlightsWithEmptyFields()
        {
            Log.Debug("SearchRoundWayFlightsWithEmptyFields: submitting empty search (round-way)");
            _flightBookingPage.SetTripTypeRoundWay();
            var page = _flightBookingPage.SearchFlights();
            return page;
        }

        public bool AllFlightsMatchRoute(string city)
        {
            Log.Debug("AllFlightsMatchRoute: city={city}", city);
            var flightsInfo = _flightBookingPage.GetFlightsTextInfo();
            var count = flightsInfo?.Count ?? 0;

            if (flightsInfo == null || count == 0)
            {
                Log.Debug("AllFlightsMatchRoute: no items -> false");
                return false;
            }
            var target = city ?? string.Empty;
            var result = flightsInfo.All(f =>
                    (f?.Text ?? string.Empty).Contains(target, StringComparison.OrdinalIgnoreCase));

            Log.Debug("AllFlightsMatchRoute: result={result}", result);
            return result;
        }

        public bool DepartureFlightDateMatch(DateTime date)
        {
            Log.Debug("DepartureFlightDateMatch: date={date}", date);
            var flightsDateInfo = _flightBookingPage.GetDepartureFlightsDateTextInfo();
            var result = AllFlightsMatchDate(flightsDateInfo, date);
            var count = flightsDateInfo?.Count ?? 0;
            Log.Debug("DepartureFlightDateMatch: items={count}, result={result}", count, result);
            return result;
        }

        public bool ReturnFlightDateMatch(DateTime date)
        {
            Log.Debug("ReturnFlightDateMatch: date={date}", date);
            var flightsDateInfo = _flightBookingPage.GetReturnFlightsDateTextInfo();
            var result = AllFlightsMatchDate(flightsDateInfo, date);
            Log.Debug("ReturnFlightDateMatch: items={count}, result={result}", flightsDateInfo?.Count ?? 0, result);
            return result;
        }

        public bool AllFlightsMatchDate(List<Label> flightsDateInfo, DateTime date)
        {
            Log.Debug("AllFlightsMatchDate: target date={date}, items={count}", date, flightsDateInfo?.Count ?? 0);

            if (flightsDateInfo?.Count == 0)
            {
                Log.Debug("AllFlightsMatchDate: no items -> false");
                return false;
            }

            var d = date.Day;   
            var m = date.Month;  
            var y = date.Year;

            var pattern = $@"\b0?{m}/0?{d}/{y}\b";
            var regex = new Regex(pattern);
            Log.Debug("AllFlightsMatchDate: regex pattern={pattern}", pattern);

            var result = flightsDateInfo?.All(f => regex.IsMatch(f.Text ?? string.Empty));
            Log.Debug("AllFlightsMatchDate: result={result}", result);
            return result ?? false;
        }

        public FlightBookingPage SearchWithPassengers(string from, string to, DateTime date, int adults)
        {
            Log.Debug("SearchWithPassengers: from={from}, to={to}, date={date}, adults={adults}", from, to, date, adults);
            SearchOneWay(from, to, date);
            _flightBookingPage.SetPassengers(adults.ToString());

            var page = _flightBookingPage.SearchFlights();
            Log.Debug("SearchWithPassengers: search submitted");
            return page;
        }

        public PaymentModal SelectFirstFlight(FlightBookingPage flights)
        {
            Log.Debug("SelectFirstFlight: clicking first available flight");
            flights.GetListOfAwailableFlights().First().Click();
            Log.Debug("SelectFirstFlight: opening PaymentModal");
            return new PaymentModal(_driver);
        }

        public bool IsBookingSuccessfulMessagePresented(FlightBookingPage flightPage)
        {
            Log.Debug("IsBookingSuccessfulMessagePresented: checking success message (note: calling .Any() on string per original logic)");
            var result = flightPage.GetBookingSuccessfulMessage()?.Any() == true;
            Log.Debug("IsBookingSuccessfulMessagePresented: result={result}", result);
            return result;
        }

        public bool ShowsOriginRequired(FlightBookingPage flight)
        {
            Log.Debug("ShowsOriginRequired: checking origin required message (note: .Any() on string per original logic)");
            var result = flight.GetOriginRequiredErrorMessage()?.Any() == true;
            Log.Debug("ShowsOriginRequired: result={result}", result);
            return result;
        }

        public bool ShowsDestinationRequired(FlightBookingPage flight)
        {
            Log.Debug("ShowsDestinationRequired: checking destination required message (note: .Any() on string per original logic)");
            var result = flight.GetDestinationRequiredErrorMessage()?.Any() == true;
            Log.Debug("ShowsDestinationRequired: result={result}", result);
            return result;
        }

        public bool ShowsDepartureDateRequired(FlightBookingPage flight)
        {
            Log.Debug("ShowsDepartureDateRequired: checking departure date required message (note: .Any() on string per original logic)");
            var result = flight.GetDepartureDateRequiredErrorMessage()?.Any() == true;
            Log.Debug("ShowsDepartureDateRequired: result={result}", result);
            return result;
        }

        public bool ShowsReturnDateRequired(FlightBookingPage flight)
        {
            Log.Debug("ShowsReturnDateRequired: checking return date required message (note: .Any() on string per original logic)");
            var result = flight.GetReturnDateRequiredErrorMessage().Any();
            Log.Debug("ShowsReturnDateRequired: result={result}", result);
            return result;
        }

        public string GetOriginRequiredErrorMessage(FlightBookingPage flightPage)
        {
            Log.Debug("GetOriginRequiredErrorMessage: fetching text");
            var text = flightPage.GetOriginRequiredErrorMessage();
            Log.Debug("GetOriginRequiredErrorMessage: value=\"{text}\"", text);
            return text;
        }

        public string GetDestinationRequiredErrorMessage(FlightBookingPage flightPage)
        {
            Log.Debug("GetDestinationRequiredErrorMessage: fetching text");
            var text = flightPage.GetDestinationRequiredErrorMessage();
            Log.Debug("GetDestinationRequiredErrorMessage: value=\"{text}\"", text);
            return text;
        }

        public string GetDepartureRequiredErrorMessage(FlightBookingPage flightPage)
        {
            Log.Debug("GetDepartureRequiredErrorMessage: fetching text");
            var text = flightPage.GetDepartureDateRequiredErrorMessage();
            Log.Debug("GetDepartureRequiredErrorMessage: value=\"{text}\"", text);
            return text;
        }

        public string GetReturnRequiredErrorMessage(FlightBookingPage flightPage)
        {
            Log.Debug("GetReturnRequiredErrorMessage: fetching text");
            var text = flightPage.GetReturnDateRequiredErrorMessage();
            Log.Debug("GetReturnRequiredErrorMessage: value=\"{text}\"", text);
            return text;
        }

        public FlightBookingPage SearchOneWayWithoutOriginCity(string to, DateTime date)
        {
            Log.Debug("SearchOneWayWithoutOriginCity: to={to}, date={date}", to, date);
            _flightBookingPage
                .GoToFlightBookingPage()
                .SetTripTypeOneWay()
                .SetToCity(to)
                .SetDepartureDate(date);

            var page = _flightBookingPage.SearchFlights();
            Log.Debug("SearchOneWayWithoutOriginCity: search submitted");
            return page;
        }

        public FlightBookingPage SearchOneWayWithoutDestinationCity(string from, DateTime date)
        {
            Log.Debug("SearchOneWayWithoutDestinationCity: from={from}, date={date}", from, date);
            _flightBookingPage
                .GoToFlightBookingPage()
                .SetTripTypeOneWay()
                .SetFromCity(from)
                .SetDepartureDate(date);

            var page = _flightBookingPage.SearchFlights();
            Log.Debug("SearchOneWayWithoutDestinationCity: search submitted");
            return page;
        }

        public FlightBookingPage SearchOneWayWithoutDepartureDate(string from, string to)
        {
            Log.Debug("SearchOneWayWithoutDepartureDate: from={from}, to={to}", from, to);
            _flightBookingPage
                .GoToFlightBookingPage()
                .SetTripTypeOneWay()
                .SetFromCity(from)
                .SetToCity(to);

            var page = _flightBookingPage.SearchFlights();
            Log.Debug("SearchOneWayWithoutDepartureDate: search submitted");
            return page;
        }

        public FlightBookingPage SearchOneWayWithoutReturnDate(string from, string to, DateTime date)
        {
            Log.Debug("SearchOneWayWithoutReturnDate: from={from}, to={to}, depart={date}", from, to, date);
            _flightBookingPage
                .GoToFlightBookingPage()
                .SetFromCity(from)
                .SetToCity(to)
                .SetDepartureDate(date);

            var page = _flightBookingPage.SearchFlights();
            Log.Debug("SearchOneWayWithoutReturnDate: search submitted");
            return page;
        }
    }
};

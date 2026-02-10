using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Input;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TesterBudAutomationFramework.Core.Controls;
using TesterBudAutomationFramework.Pages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TesterBudAutomationFramework.Services
{
    public class FlightBookingService
    {
        IWebDriver _driver;
        FlightBookingPage _flightBookingPage;

        public FlightBookingService(IWebDriver driver)
        {
            _driver = driver;
            _flightBookingPage = new FlightBookingPage(driver);
            _flightBookingPage.GoToFlightBookingPage();
        }

        public FlightBookingPage SearchOneWay(string from, string to, DateTime date)
        {
            _flightBookingPage
                .GoToFlightBookingPage()
                .SetTripTypeOneWay()
                .SetFromCity(from)
                .SetToCity(to)
                .SetDepartureDate(date);

            return _flightBookingPage.SearchFlights();
        }

        public bool HasFlights(FlightBookingPage flightPage) =>
            flightPage.GetListOfAwailableFlights()?.Any() == true;

        public bool NoFlightsMessageNotShown(FlightBookingPage flightPage) => 
            flightPage.GetNoFlightsMessage()?.Any() == true;

        public FlightBookingPage SearchRoundWay(string from, string to, DateTime date, DateTime returnDate)
        {
            SearchOneWay(from, to, date);
            _flightBookingPage
                .SetTripTypeRoundWay()
                .SetReturnDate(returnDate);

            return _flightBookingPage.SearchFlights();
        }

        public FlightBookingPage SearchOneWayFlightsWithEmptyFields()
        {
            return _flightBookingPage.SearchFlights();
        }

        public FlightBookingPage SearchRoundWayFlightsWithEmptyFields()
        {
            _flightBookingPage
                .SetTripTypeRoundWay();
            return _flightBookingPage.SearchFlights();
        }

        public bool AllFlightsMatchRoute(string city)
        {
            var flightsInfo = _flightBookingPage.GetFlightsTextInfo();
            if (flightsInfo.Count == 0) return false;

            return flightsInfo.All(f => f.Text.Contains(city));
        }

        public bool DepartureFlightDateMatch(DateTime date)
        {
            var flightsDateInfo = _flightBookingPage.GetDepartureFlightsDateTextInfo();
            return AllFlightsMatchDate(flightsDateInfo, date);
        }

        public bool ReturnFlightDateMatch(DateTime date)
        {
            var flightsDateInfo = _flightBookingPage.GetReturnFlightsDateTextInfo();
            return AllFlightsMatchDate(flightsDateInfo, date);
        }

        public bool AllFlightsMatchDate(List<Label> flightsDateInfo, DateTime date)
        {
            if (flightsDateInfo.Count == 0) return false;

            var d = date.Day;   
            var m = date.Month;  
            var y = date.Year;

            var pattern = $@"\b0?{m}/0?{d}/{y}\b";
            var regex = new Regex(pattern);

            foreach(var ttt in flightsDateInfo)
            {
                var rrr = ttt.Text;
            }

            return flightsDateInfo.Any(f => regex.IsMatch(f.Text));
        }

        public FlightBookingPage SearchWithPassengers(string from, string to, DateTime date, int adults)
        {
            SearchOneWay(from, to, date);
            _flightBookingPage.SetPassengers(adults.ToString());

            return _flightBookingPage.SearchFlights();
        }

        public PaymentModal SelectFirstFlight(FlightBookingPage flights)
        {
            flights.GetListOfAwailableFlights().First().Click();
            return new PaymentModal(_driver);
        }

        public bool IsBookingSuccessfulMessagePresented(FlightBookingPage flightPage)
        {
            return flightPage.GetBookingSuccessfulMessage()?.Any() == true;
        }

        public bool ShowsOriginRequired(FlightBookingPage flight)
        {
            return flight.GetOriginRequiredErrorMessage()?.Any() == true;
        }

        public bool ShowsDestinationRequired(FlightBookingPage flight)
        {
            return flight.GetDestinationRequiredErrorMessage()?.Any() == true;
        }

        public bool ShowsDepartureDateRequired(FlightBookingPage flight)
        {
            return flight.GetDepartureDateRequiredErrorMessage()?.Any() == true;
        }

        public bool ShowsReturnDateRequired(FlightBookingPage flight)
        {
            return flight.GetReturnDateRequiredErrorMessage().Any();
        }

        public string GetOriginRequiredErrorMessage(FlightBookingPage flightPage)
        {
            return flightPage.GetOriginRequiredErrorMessage();
        }

        public string GetDestinationRequiredErrorMessage(FlightBookingPage flightPage)
        {
            return flightPage.GetDestinationRequiredErrorMessage();
        }

        public string GetDepartureRequiredErrorMessage(FlightBookingPage flightPage)
        {
            return flightPage.GetDepartureDateRequiredErrorMessage();
        }

        public string GetReturnRequiredErrorMessage(FlightBookingPage flightPage)
        {
            return flightPage.GetReturnDateRequiredErrorMessage();
        }

        public FlightBookingPage SearchOneWayWithoutOriginCity(string to, DateTime date)
        {
            _flightBookingPage
                .GoToFlightBookingPage()
                .SetTripTypeOneWay()
                .SetToCity(to)
                .SetDepartureDate(date);

            return _flightBookingPage.SearchFlights();
        }

        public FlightBookingPage SearchOneWayWithoutDestinationCity(string from, DateTime date)
        {
            _flightBookingPage
                .GoToFlightBookingPage()
                .SetTripTypeOneWay()
                .SetFromCity(from)
                .SetDepartureDate(date);

            return _flightBookingPage.SearchFlights();
        }

        public FlightBookingPage SearchOneWayWithoutDepartureDate(string from, string to)
        {
            _flightBookingPage
                .GoToFlightBookingPage()
                .SetTripTypeOneWay()
                .SetFromCity(from)
                .SetToCity(to);

            return _flightBookingPage.SearchFlights();
        }

        public FlightBookingPage SearchOneWayWithoutReturnDate(string from, string to, DateTime date)
        {
            _flightBookingPage
                .GoToFlightBookingPage()
                .SetFromCity(from)
                .SetToCity(to)
                .SetDepartureDate(date);

            return _flightBookingPage.SearchFlights();
        }
    }
};

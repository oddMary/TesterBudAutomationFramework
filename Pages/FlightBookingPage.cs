using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework.Constraints;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Input;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using TesterBudAutomationFramework.Core.Config;
using TesterBudAutomationFramework.Core.Controls;
using TesterBudAutomationFramework.Core.Waits;
using static System.Collections.Specialized.BitVector32;

namespace TesterBudAutomationFramework.Pages
{
    public class FlightBookingPage : BasePage
    {
        private Checkbox OneWayRadio => FindComponent<Checkbox>(By.Id("oneWay"));
        private Select FromSelect => FindComponent<Select>(By.Id("from"));
        private Select ToSelect => FindComponent<Select>(By.Id("to"));
        private TextInput DepartureDateInput => FindComponent<TextInput>(By.Id("departureDate"));
        private TextInput ReturnDateInput => FindComponent<TextInput>(By.Id("returnDate"));
        private Button SearchFlightsButton => FindComponent<Button>(By.XPath("//button[contains(text(), 'Search Flights')]"));
        private TextInput PassengersInput => FindComponent<TextInput>(By.Id("passengers"));
        private List<Label> AvailableFlightsList => FindComponents<Label>(By.CssSelector(".list-group button"));
        private List<Label> FlightInfoTextList => FindComponents<Label>(By.CssSelector("p.text-muted"));
        private List<Label> FlightDepartureDateInfoTextList => FindComponents<Label>(By.XPath("//h6[contains(normalize-space(.), 'Departure')]/following-sibling::small[1]"));
        private List<Label> FlightReturnDateInfoTextList => FindComponents<Label>(By.XPath("//h6[contains(normalize-space(.), 'Return')]/following-sibling::small[1]"));
        private Label BookingSuccessfulMessage => FindComponent<Label>(By.CssSelector("h4.text-success"));
        private Label FromRequiredErrorMessage => FindComponent<Label>(By.XPath("//select[@id='from']/following-sibling::small"));
        private Label ToRequiredErrorMessage => FindComponent<Label>(By.XPath("//select[@id='to']/following-sibling::small"));
        private Label DepartureDateRequiredErrorMessage => FindComponent<Label>(By.XPath("//input[@id='departureDate']/following-sibling::small"));
        private Label ReturnDateRequiredErrorMessage => FindComponent<Label>(By.XPath("//input[@id='returnDate']/following-sibling::small"));

        public FlightBookingPage(IWebDriver driver) : base(driver) { }

        public FlightBookingPage SetTripTypeOneWay()
        {
            OneWayRadio.ScrollToCenterAndClick();
            return this;
        }

        public FlightBookingPage SetTripTypeRoundWay()
        {
            if (OneWayRadio.Element.Selected)
            {
                OneWayRadio.Click();
            }             
            return this;
        }        

        public FlightBookingPage SetFromCity(string city)
        {
            FromSelect.SelectOption(city);
            return this;
        }

        public FlightBookingPage SetToCity(string destination)
        {
            ToSelect.SelectOption(destination);
            return this;
        }

        public FlightBookingPage SetDepartureDate(DateTime date)
        {
            DepartureDateInput.SetFormattedDate(date);
            return this;
        }

        public FlightBookingPage SetReturnDate(DateTime date)
        {
            ReturnDateInput.SetFormattedDate(date);
            return this;
        }

        public FlightBookingPage SearchFlights()
        {
            SearchFlightsButton.ScrollToCenterAndClick();
            return this;
        }

        public List<Label> GetListOfAwailableFlights()
        {
            return AvailableFlightsList;
        }

        public List<Label> GetListOfAwailableFlightsDeparture()
        {
            return FlightDepartureDateInfoTextList;
        }

        public List<Label> GetListOfAwailableFlightsReturn()
        {
            return FlightReturnDateInfoTextList;
        }

        public FlightBookingPage SetPassengers(string passengers)
        {
            PassengersInput.SetText(passengers);
            return this;
        }

        public List<Label> GetNoFlightsMessage() => FlightInfoTextList;

        public List<Label> GetFlightsTextInfo() => FlightInfoTextList;

        public List<Label> GetFlightsDateTextInfo() => GetListOfAwailableFlights();

        public List<Label> GetDepartureFlightsDateTextInfo() => GetListOfAwailableFlightsDeparture();
        public List<Label> GetReturnFlightsDateTextInfo() => GetListOfAwailableFlightsReturn();

        public string GetBookingSuccessfulMessage() => FlightInfoTextList.FirstOrDefault().Text;

        public string GetOriginRequiredErrorMessage() => FromRequiredErrorMessage.Text;

        public string GetDestinationRequiredErrorMessage() => ToRequiredErrorMessage.Text;

        public string GetDepartureDateRequiredErrorMessage() => DepartureDateRequiredErrorMessage.Text;

        public string GetReturnDateRequiredErrorMessage() => ReturnDateRequiredErrorMessage.Text;

        //private List<Label> GetVisibleElements()
        //{
        //    var visible = WaitUntilAllVisible(locator);
        //    return visible.ToList();
        //}

        //private List<Label> GetDatesFromSections()
        //{
        //    var visibleSections = GetListOfAwailableFlights();
        //    foreach (var card in visibleSections)
        //    {
        //        WaitUntilCardHasReturnDate(card, TimeSpan.FromSeconds(TestConfig.CurrentSetting.TimeoutSec));
        //    }

        //    BaseControl.ScrollToCenter(_driver, (IWebElement)visibleSections.Last());

        //    return visibleSections;
        //}

        //private bool WaitUntilCardHasReturnDate(Label? card, TimeSpan timeout)
        //{
        //    var wait = new WebDriverWait(_driver, timeout);
        //    return wait.Until(_ =>
        //    {
        //        try
        //        {
        //            var dates = ;
        //            return dates.Count >= 2 && dates.All(d => d.Element.Displayed && !string.IsNullOrWhiteSpace(d.Text));
        //        }
        //        catch (StaleElementReferenceException)
        //        {
        //            return false;
        //        }
        //    });
        //}
    }
};

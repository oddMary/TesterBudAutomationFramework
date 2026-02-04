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

        private By AvailableFlightButtonLocator => By.CssSelector(".list-group button");
        private By FlightInfoTextLocator => By.CssSelector("p.text-muted");
        private By FlightDateInfoTextLocator => By.XPath("//p[contains(@class,'text-muted')]/following-sibling::small[1]");
        private By NoFlightsMessageLocator => By.ClassName("text-muted");
        private By BookingSuccessfulMessage => By.CssSelector("h4.text-success");
        private By FromRequiredErrorMessage => By.XPath("//select[@id='from']/following-sibling::small");
        private By ToRequiredErrorMessage => By.XPath("//select[@id='to']/following-sibling::small");
        private By DepartureDateRequiredErrorMessage => By.XPath("//input[@id='departureDate']/following-sibling::small");
        private By ReturnDateRequiredErrorMessage => By.XPath("//input[@id='returnDate']/following-sibling::small");

        public FlightBookingPage(IWebDriver driver) : base(driver) { }

        public FlightBookingPage SetTripTypeOneWay()
        {
            OneWayRadio.Click();
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
            DepartureDateInput.SetDate(date);
            return this;
        }

        public FlightBookingPage SetReturnDate(DateTime date)
        {
            ReturnDateInput.SetDate(date);
            return this;
        }

        public FlightBookingPage SearchFlights()
        {
            SearchFlightsButton.Click();
            return this;
        }

        public List<IWebElement> GetListOfAwailableFlights()
        {
            return _driver.FindElements(AvailableFlightButtonLocator).ToList();
        }
        private List<IWebElement> WaitUntilAllVisible(By locator)
        {
            WaitForDocumentReady();
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(TestConfig.CurrentSetting.TimeoutSec));
            return wait.Until(driver =>
            {
                var elements = driver.FindElements(locator).ToList();
                if (elements.Count == 0) return null; 
                return elements.All(e => e.Displayed) ? elements : null;
            })!;
        }

        private void WaitForDocumentReady()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(25));
            wait.Until(d =>
                ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState")?.ToString() == "complete"
            );
        }

        public FlightBookingPage SetPassengers(string passengers)
        {
            PassengersInput.SetText(passengers);
            return this;
        }

        public List<IWebElement> GetNoFlightsMessage()
            => GetVisibleElements(NoFlightsMessageLocator);

        public List<IWebElement> GetFlightsTextInfo()
            => GetVisibleElements(FlightInfoTextLocator);

        public List<IWebElement> GetFlightsDateTextInfo()
            => GetDatesFromSections(FlightDateInfoTextLocator);

        public List<IWebElement> GetBookingSuccessfulMessage()
            => GetVisibleElements(FlightInfoTextLocator);

        public List<IWebElement> GetOriginRequiredErrorMessage()
            => GetVisibleElements(FromRequiredErrorMessage);

        public List<IWebElement> GetDestinationRequiredErrorMessage()
            => GetVisibleElements(ToRequiredErrorMessage);

        public List<IWebElement> GetDepartureDateRequiredErrorMessage()
            => GetVisibleElements(DepartureDateRequiredErrorMessage);

        public List<IWebElement> GetReturnDateRequiredErrorMessage()
            => GetVisibleElements(ReturnDateRequiredErrorMessage);

        private List<IWebElement> GetVisibleElements(By locator)
        {
            var visible = WaitUntilAllVisible(locator);
            return visible.ToList();
        }

        private List<IWebElement> GetDatesFromSections(By locator)
        {
            var visibleSections = GetListOfAwailableFlights();
            foreach(var card in visibleSections)
            {
                WaitUntilCardHasReturnDate(card, TimeSpan.FromSeconds(TestConfig.CurrentSetting.TimeoutSec));
            }

            BaseControl.ScrollToCenter(_driver, visibleSections.Last());
            var visibleDates = WaitUntilAllVisible(locator);

            return visibleDates.ToList();
        }

        private bool WaitUntilCardHasReturnDate(IWebElement card, TimeSpan timeout)
        {
            var wait = new WebDriverWait(_driver, timeout);
            return wait.Until(_ =>
            {
                try
                {
                    var dates = card.FindElements(FlightDateInfoTextLocator);
                    return dates.Count >= 2 && dates.All(d => d.Displayed && !string.IsNullOrWhiteSpace(d.Text));
                }
                catch (StaleElementReferenceException)
                {
                    return false; 
                }
            });
        }
    }
}

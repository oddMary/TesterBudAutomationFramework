using OpenQA.Selenium;
using Serilog;
using TesterBudAutomationFramework.Core.Controls;

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
            Log.Debug("Setting trip type: One Way");
            OneWayRadio.ScrollToCenterAndClick();
            return this;
        }

        public FlightBookingPage SetTripTypeRoundWay()
        {
            Log.Debug("Setting trip type: Round Trip");
            if (OneWayRadio.Element.Selected)
            {
                Log.Debug("OneWay radio was selected – switching to Round Trip");
                OneWayRadio.Click();
            }             
            return this;
        }        

        public FlightBookingPage SetFromCity(string city)
        {
            Log.Debug("Selecting origin city: {city}", city);
            FromSelect.SelectOption(city);
            return this;
        }

        public FlightBookingPage SetToCity(string destination)
        {
            Log.Debug("Selecting destination city: {destination}", destination);
            ToSelect.SelectOption(destination);
            return this;
        }

        public FlightBookingPage SetDepartureDate(DateTime date)
        {
            Log.Debug("Setting departure date: {date}", date);
            DepartureDateInput.SetFormattedDate(date);
            return this;
        }

        public FlightBookingPage SetReturnDate(DateTime date)
        {
            Log.Debug("Setting return date: {date}", date);
            ReturnDateInput.SetFormattedDate(date);
            return this;
        }

        public FlightBookingPage SearchFlights()
        {
            Log.Debug("Clicking Search Flights button");
            SearchFlightsButton.ScrollToCenterAndClick();
            return this;
        }

        public List<Label> GetListOfAwailableFlights()
        {
            Log.Debug("Fetching list of available flights");
            return AvailableFlightsList;
        }

        public List<Label> GetListOfAwailableFlightsDeparture()
        {
            Log.Debug("Fetching list of available flights (departure dates)");
            return FlightDepartureDateInfoTextList;
        }

        public List<Label> GetListOfAwailableFlightsReturn()
        {
            Log.Debug("Fetching list of available flights (return dates)");
            return FlightReturnDateInfoTextList;
        }

        public FlightBookingPage SetPassengers(string passengers)
        {
            Log.Debug("Setting number of passengers: {passengers}", passengers);
            PassengersInput.SetText(passengers);
            return this;
        }
        public List<Label> GetNoFlightsMessage()
        {
            Log.Debug("Fetching 'no flights' info text");
            return FlightInfoTextList;
        }

        public List<Label> GetFlightsTextInfo()
        {
            Log.Debug("Fetching flights info text");
            return FlightInfoTextList;
        }

        public List<Label> GetFlightsDateTextInfo()
        {
            Log.Debug("Fetching flights date info text");
            return GetListOfAwailableFlights();
        }

        public List<Label> GetDepartureFlightsDateTextInfo()
        {
            Log.Debug("Fetching flights departure date text");
            return GetListOfAwailableFlightsDeparture();
        }

        public List<Label> GetReturnFlightsDateTextInfo()
        {
            Log.Debug("Fetching flights return date text");
            return GetListOfAwailableFlightsReturn();
        }

        public string? GetBookingSuccessfulMessage()
        {
            Log.Debug("Fetching booking success message");
            return BookingSuccessfulMessage.Text;
        }
        public string? GetOriginRequiredErrorMessage()
        {
            Log.Debug("Fetching 'origin required' validation message");
            return FromRequiredErrorMessage.Text;
        }

        public string? GetDestinationRequiredErrorMessage()
        {
            Log.Debug("Fetching 'destination required' validation message");
            return ToRequiredErrorMessage.Text;
        }

        public string? GetDepartureDateRequiredErrorMessage()
        {
            Log.Debug("Fetching 'departure date required' validation message");
            return DepartureDateRequiredErrorMessage.Text;
        }

        public string? GetReturnDateRequiredErrorMessage()
        {
            Log.Debug("Fetching 'return date required' validation message");
            return ReturnDateRequiredErrorMessage.Text;
        }
    }
};

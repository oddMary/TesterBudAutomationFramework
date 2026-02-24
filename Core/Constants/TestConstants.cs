
namespace TesterBudAutomationFramework.Core.Constants
{
    public static class TestConstants
    {
        public const string DEFAULT_FROM = "New York";
        public const string DEFAULT_TO = "London";

        public const string DEFAULT_URL = "https://testerbud.com/practice-page-selection";
        public const string DEFAULT_BROWSER = "chrome";
        public const string SCREENSHOTS_PATH = "\"Artifacts\\\\Screenshots\";";
        public const string LOG_PATH = "logs/test.log";
        public const string JSON_FILE_NAME = "appsettings.json";
        public const string DATE_FORMAT = "MM-dd-yyyy";

        public const string SCREENSHOTS_ATTACHMENT_TYPE = "image/png";
        public const string LOGS_FOLDER = "logs";
        public const string ALLURE_RESULTS_FOLDER = "allure-results";
        public const string ALLURE_REPORT_FOLDER = "allure-report";
        public const string SCREENSHOTS_FOLDER = "Screenshots";

        public const string SCREENSHOT_DATE_FORMAT = "yyyy-MM-dd";
        public const string SCREENSHOT_TIME_FORMAT = "HHmmssfff";
        public const string IMG_FORMAT = "png";

        #region Error test messages
        public const string NO_AWAILABLE_FLIGHTS = "Expected available flights, but none were found.";
        public const string UNEXPECTED_NO_FLIGHTS_FOUND = "Unexpected 'No flights found' message.";
        public const string FROM_CITY_NOT_MATCH = "From city do not match.";
        public const string TO_CITY_NOT_MATCH = "To city do not match.";
        public const string DEPARTURE_DATE_NOT_MATCH = "Departure do not match.";
        public const string RETURN_DATE_NOT_MATCH = "Return date do not match.";
        public const string EXPECTED_FLIGHTS = "Expected flights for {0} passenger(s).";
        public const string PAYMENT_MODAL_NOT_OPENED = "Payment modal is not opened.";
        public const string BOOKING_CONFIRMATION_ID_NOT_FOUND = "Booking confirmation ID not found.";
        public const string ORIGIN_REQUIRED_ERROR = "Expected origin required error.";
        public const string SELECT_DEPARTURE_CITY = "Please select a departure city.";
        public const string DESTINATION_REQUIRED_ERROR = "Expected destination required error.";
        public const string SELECT_DESTINATION_CITY = "Please select a destination city.";
        public const string DATE_REQUIRED_ERROR = "Expected date required error.";
        public const string DEPARTURE_DATE_IN_THE_PAST = "Departure date cannot be in the past.";
        public const string PAST_DATE_OR_ERROR = "Expected validation for past date (or date error).";
        public const string RETURN_DATE_AFTER_DEPARTURE = "Return date must be after departure date.";
        public const string SAME_DERARTURE_AND_DESTINATION = "Departure and destination cities cannot be the same.";
        #endregion
    }
}

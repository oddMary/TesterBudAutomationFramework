using OpenQA.Selenium;
using Serilog;
using TesterBudAutomationFramework.Core.Controls;
using TesterBudAutomationFramework.Core.Waits;

namespace TesterBudAutomationFramework.Pages
{
    public class PaymentModal : BasePage
    {
        private TextInput CardNumber => FindComponent<TextInput>(By.Id("cardNumber"));
        private TextInput ExpiryDate => FindComponent<TextInput>(By.Id("expiryDate"));
        private TextInput CVV => FindComponent<TextInput>(By.Id("cvv"));
        private Button SubmitPaymentButton => FindComponent<Button>(By.CssSelector("div.modal-body button"));

        private By PaymentModalWindowLocator => By.CssSelector(".modal-header");

        public PaymentModal(IWebDriver driver) : base(driver) { }

        public bool PaymentModalWindowAppeared()
        {
            var isVisible = Wait.WaitUntilVisible(_driver, PaymentModalWindowLocator, Wait.DefaultTimeout).Displayed;

            Log.Debug("Payment Modal visibility = {isVisible}", isVisible);
            return isVisible;
        }

        public PaymentModal EnterCard(string cardNumber, string expiryMmYy, string cvv)
        {

            Log.Debug("Entering payment data: cardNumber=****{last4}, expiry={expiry}, cvv=***",
                            cardNumber?.Substring(Math.Max(0, cardNumber.Length - 4)),
                            expiryMmYy,
                            "***");

            CardNumber.SetText(cardNumber);
            ExpiryDate.SetText(expiryMmYy);
            CVV.SetText(cvv);
            Log.Debug("Payment data entered");
            return this;
        }

        public FlightBookingPage SubmitPayment()
        {
            SubmitPaymentButton.ScrollToCenterAndClick();

            Log.Debug("Payment submitted — navigating back to FlightBookingPage");
            return new FlightBookingPage(_driver);
        }
    }
};

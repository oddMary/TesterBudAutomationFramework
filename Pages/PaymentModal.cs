using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TesterBudAutomationFramework.Core.Config;
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
            return Wait.WaitUntilVisible(_driver, PaymentModalWindowLocator, Wait.DefaultTimeout).Displayed;
        }

        public PaymentModal EnterCard(string cardNumber, string expiryMmYy, string cvv)
        {
            CardNumber.SetText(cardNumber);
            ExpiryDate.SetText(expiryMmYy);
            CVV.SetText(cvv);
            return this;
        }

        public FlightBookingPage SubmitPayment()
        {
            SubmitPaymentButton.ScrollToCenterAndClick();
            return new FlightBookingPage(_driver);
        }
    }
};

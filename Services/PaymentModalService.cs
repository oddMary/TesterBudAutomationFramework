using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TesterBudAutomationFramework.Pages;

namespace TesterBudAutomationFramework.Services
{
    public class PaymentModalService
    {
        IWebDriver _driver;
        PaymentModal _paymentModal;

        public PaymentModalService(IWebDriver driver)
        {
            _driver = driver;
            _paymentModal = new PaymentModal(driver);
        }

        public bool IsPaymentModalPresented(PaymentModal paymentModal)
        {
            return paymentModal.PaymentModalWindowAppeared();
        }

        public FlightBookingPage CompleteBookingWithValidData(PaymentModal payment, string cardNumber, string expiryMmYy, string cvv)
        {
            return payment
                .EnterCard(cardNumber, expiryMmYy, cvv)
                .SubmitPayment();
        }
    }
};

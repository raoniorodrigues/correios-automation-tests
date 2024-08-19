using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BuscaCepCorreios.Pages
{
    public class TrackPackagePage
    {
        private IWebDriver _driver;

        public TrackPackagePage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement RastreioInput
        {
            get
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                return wait.Until(ExpectedConditions.ElementIsVisible(By.Id("objeto")));
            }
        }

        private IWebElement CaptchaInput => _driver.FindElement(By.Id("captcha"));
        private IWebElement ConsultarButton => _driver.FindElement(By.Id("b-pesquisar"));

        public void SearchTrackingCode(string trackingCode)
        {
            RastreioInput.Clear();
            RastreioInput.SendKeys(trackingCode);

            // Captcha handling might be required here
            // CaptchaInput.SendKeys(captchaText);

            ConsultarButton.Click();
        }

        public bool VerifyTrackingCodeNotFound()
        {
            // Espera até que o alerta esteja presente e verifique se a mensagem de erro correta está presente
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement alertMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#alerta .msg")));
            return alertMessage.Text.Contains("Objeto não encontrado na base de dados dos Correios.");
        }
    }
}

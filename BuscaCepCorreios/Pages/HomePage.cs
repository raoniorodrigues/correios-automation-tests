using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace BuscaCepCorreios.Pages
{
    public class HomePage
    {
        private IWebDriver _driver;

        public HomePage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Input do código de rastreamento na home page
        private IWebElement CodigoRastreamentoInput => _driver.FindElement(By.Id("objetos"));

        // Botão de busca na home page
        private IWebElement BotaoPesquisaHomePage => _driver.FindElement(By.XPath("/html/body/div[1]/div/div/div[5]/div[2]/section/div/form/div/div/button"));

        public void ClickBuscaCep()
        {
            // Espera até que o link com o img 'Busca CEP ou Endereço' esteja visível e clicável
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement buscaCepLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//li[@data-item-acesso-rapido='itemAcessoRapido']//img[@alt='Busca CEP ou Endereço']/parent::a")));

            // Clica no link
            buscaCepLink.Click();
        }

        public void ClickRastrearCodigo()
        {
            // Espera até que o link para rastreamento de código esteja visível e clicável
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement rastrearCodigoLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//li[@data-item-acesso-rapido='itemAcessoRapido']//span[text()='Rastrear Objetos']/parent::a")));

            // Clica no link
            rastrearCodigoLink.Click();
        }

        // Novo método para digitar o código de rastreamento
        public void EnterTrackingCode(string trackingCode)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement trackingInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("objetos")));

            trackingInput.Clear();
            trackingInput.SendKeys(trackingCode);
        }

        // Novo método para clicar no botão de rastreamento
        public void ClickTrackButton()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement trackButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.bt-link-ic")));

            trackButton.Click();
        }

        public void DigitarCodigoRastreamento(string codigoRastreamento)
        {
            CodigoRastreamentoInput.SendKeys(codigoRastreamento);
        }

        // Método para clicar no botão de pesquisa na home page
        public void ClicarBotaoPesquisa()
        {
            BotaoPesquisaHomePage.Click();
        }
    }
}

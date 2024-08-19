using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BuscaCepCorreios.Pages
{
    public class RastreamentoPage
    {
        private readonly IWebDriver _driver;

        public RastreamentoPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Input do código de rastreamento
        private IWebElement CodigoRastreamentoInput => _driver.FindElement(By.Id("objeto"));

        // Botão Consultar
        private IWebElement ConsultarButton => _driver.FindElement(By.Id("b-pesquisar"));

        // Método para verificar se o valor do input está conforme esperado
        public bool VerificarCodigoRastreamento(string codigoEsperado)
        {
            return CodigoRastreamentoInput.GetAttribute("value") == codigoEsperado;
        }

        // Método para clicar no botão Consultar
        public void ClicarConsultar()
        {
            ConsultarButton.Click();
        }

        // Método para obter a mensagem de erro no alerta
        public string ObterMensagemErro()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            IWebElement mensagemErroElemento = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#alerta .msg")));

            return mensagemErroElemento.Text.Trim();
        }
    }
}

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using BuscaCepCorreios.Pages;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BuscaCepCorreios.Steps
{
    [Binding]
    public class BuscaCepSteps
    {
        private IWebDriver _driver;
        private HomePage _homePage;
        private SearchCepPage _searchCepPage;
        private TrackPackagePage _trackPackagePage;
        private RastreamentoPage _rastreamentoPage;

        [BeforeScenario]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _homePage = new HomePage(_driver);
            _searchCepPage = new SearchCepPage(_driver);
            _trackPackagePage = new TrackPackagePage(_driver);
        }

        [AfterScenario]
        public void TearDown()
        {
            _driver.Quit();
        }

        [Given(@"que estou na página inicial dos Correios")]
        public void GivenQueEstouNaPaginaInicialDosCorreios()
        {
            _driver.Navigate().GoToUrl("https://www.correios.com.br/");
        }

        [When(@"eu procuro pelo CEP (.*)")]
        public void WhenEuProcuroPeloCEP(string cep)
        {
            _homePage.ClickBuscaCep();
            _searchCepPage.SearchCep(cep);
        }

        [Then(@"eu confirmo que o logradouro seja ""(.*)""")]
        public void ThenEuConfirmoQueOLogradouroSeja(string expectedLogradouro)
        {
            Assert.AreEqual(expectedLogradouro, _searchCepPage.GetLogradouro(), "O logradouro retornado não corresponde ao esperado.");
        }

        [Then(@"eu confirmo que o bairro seja ""(.*)""")]
        public void ThenEuConfirmoQueOBairroSeja(string expectedBairro)
        {
            Assert.AreEqual(expectedBairro, _searchCepPage.GetBairro(), "O bairro retornado não corresponde ao esperado.");
        }

        [Then(@"eu confirmo que o estado seja ""(.*)""")]
        public void ThenEuConfirmoQueOEstadoSeja(string expectedEstado)
        {
            Assert.AreEqual(expectedEstado, _searchCepPage.GetEstado(), "O estado retornado não corresponde ao esperado.");
        }

        [Then(@"eu confirmo que o CEP seja ""(.*)""")]
        public void ThenEuConfirmoQueOCEPSeja(string expectedCep)
        {
            Assert.AreEqual(expectedCep, _searchCepPage.GetCep(), "O CEP retornado não corresponde ao esperado.");
        }

        [Then(@"eu confirmo que o CEP não existe")]
        public void ThenEuConfirmoQueOCEPNaoExiste()
        {
            Assert.IsTrue(_searchCepPage.VerifyCepNotFound(), "CEP foi encontrado, mas não deveria.");
        }

        [When(@"eu procuro pelo código de rastreamento SS(.*)BR")]
        public void WhenEuProcuroPeloCodigoDeRastreamentoSSBR(string codigo)
        {
            // 1. Digitar o código de rastreamento na home page
            _homePage.DigitarCodigoRastreamento($"SS{codigo}BR");

            // 2. Clicar no botão de busca na home page
            _homePage.ClicarBotaoPesquisa();

            // 3. Aguardar a mudança de aba (esperar até que haja mais de uma janela aberta)
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); // Ajuste o tempo de espera se necessário
            wait.Until(d => d.WindowHandles.Count > 1);

            // 4. Trocar para a nova aba
            _driver.SwitchTo().Window(_driver.WindowHandles.Last());

            // 5. Verificar se a URL é https://rastreamento.correios.com.br/app/index.php
            Assert.AreEqual("https://rastreamento.correios.com.br/app/index.php", _driver.Url);

            // 6. Inicializar a página de rastreamento
            _rastreamentoPage = new RastreamentoPage(_driver);

            // 7. Verificar se o código de rastreamento está preenchido corretamente no elemento id=objeto
            Assert.IsTrue(_rastreamentoPage.VerificarCodigoRastreamento($"SS{codigo}BR"));

            // 8. Aguardar 10 segundos para digitar o captcha manualmente
            Thread.Sleep(10000);

            // 9. Clicar no botão Consultar (id=b-pesquisar)
            _rastreamentoPage.ClicarConsultar();
            Thread.Sleep(5000);
        }

        [Then(@"eu confirmo que o código de rastreamento está incorreto com a mensagem ""(.*)""")]
        public void ThenEuConfirmoQueOCodigoDeRastreamentoEstaIncorretoComAMensagem(string mensagemEsperada)
        {
            // Aguarda a mensagem de erro aparecer
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#alerta .msg")));

            // Obtém o texto da mensagem de erro
            string mensagemErroText = _rastreamentoPage.ObterMensagemErro();

            // Verifica se o texto do elemento contém a mensagem esperada (case-insensitive)
            Assert.AreEqual(mensagemEsperada, mensagemErroText, "A mensagem de erro exibida não corresponde à esperada.");
        }
    }
}

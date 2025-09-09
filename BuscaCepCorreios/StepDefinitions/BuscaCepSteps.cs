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
        // FIX: inicializações com null! para satisfazer NRT (evita CS8618)
        private IWebDriver _driver = null!;
        private HomePage _homePage = null!;
        private SearchCepPage _searchCepPage = null!;
        private TrackPackagePage _trackPackagePage = null!;
        private RastreamentoPage _rastreamentoPage = null!;

        [BeforeScenario]
        public void Setup()
        {
            var options = new ChromeOptions();
            // opcional: rodar sem UI
            // options.AddArgument("--headless=new");

            _driver = new ChromeDriver(options);
            _driver.Manage().Window.Maximize();

            _homePage = new HomePage(_driver);
            _searchCepPage = new SearchCepPage(_driver);
            _trackPackagePage = new TrackPackagePage(_driver);
            // _rastreamentoPage é criado só quando você abre a aba de rastreamento
        }

        [AfterScenario]
        public void TearDown()
        {
            try { _driver?.Quit(); } catch { /* ignora erro de teardown */ }
        }

        [Given(@"que estou na página inicial dos Correios")]
        public void GivenQueEstouNaPaginaInicialDosCorreios()
        {
            _driver.Navigate().GoToUrl("https://www.correios.com.br/");
        }

        [When(@"eu procuro pelo CEP (.*)")]
        [When(@"eu procuro pelo CEP (.*)")]
        public void WhenEuProcuroPeloCEP(string cep)
        {
            _homePage.ClickBuscaCep();

            // 1) Espera abrir nova aba
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(d => d.WindowHandles.Count > 1);

            // 2) Troca para a aba recém-aberta
            _driver.SwitchTo().Window(_driver.WindowHandles.Last());

            // 3) Garante que a página terminou de carregar
            wait.Until(d =>
                ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState")?.ToString() == "complete"
            );

            // 4) Recria a Page agora apontando para a aba correta
            _searchCepPage = new SearchCepPage(_driver);

            // 5) Agora sim, pesquisa o CEP
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
            _homePage.DigitarCodigoRastreamento($"SS{codigo}BR");
            _homePage.ClicarBotaoPesquisa();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.WindowHandles.Count > 1);

            _driver.SwitchTo().Window(_driver.WindowHandles.Last());

            // às vezes tem redireciono/complemento de URL, então StartsWith é mais resiliente
            StringAssert.StartsWith("https://rastreamento.correios.com.br/app/index.php", _driver.Url);

            _rastreamentoPage = new RastreamentoPage(_driver);

            Assert.IsTrue(_rastreamentoPage.VerificarCodigoRastreamento($"SS{codigo}BR"));

            // captcha manual mesmo (não automatize). Se quiser, aumente o tempo.
            Thread.Sleep(10000);

            _rastreamentoPage.ClicarConsultar();
            Thread.Sleep(5000);
        }

        [Then(@"eu confirmo que o código de rastreamento está incorreto com a mensagem ""(.*)""")]
        public void ThenEuConfirmoQueOCodigoDeRastreamentoEstaIncorretoComAMensagem(string mensagemEsperada)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#alerta .msg")));

            string mensagemErroText = _rastreamentoPage.ObterMensagemErro();
            Assert.AreEqual(mensagemEsperada, mensagemErroText, "A mensagem de erro exibida não corresponde à esperada.");
        }
    }
}

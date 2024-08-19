using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace BuscaCepCorreios.Pages
{
    public class SearchCepPage
    {
        private IWebDriver _driver;

        public SearchCepPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement CepInput
        {
            get
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                return wait.Until(ExpectedConditions.ElementIsVisible(By.Id("endereco")));
            }
        }

        private IWebElement BuscarButton => _driver.FindElement(By.Id("btn_pesquisar"));

        // Localiza a tabela de resultados quando o CEP existe
        private IWebElement ResultadoTabela => _driver.FindElement(By.Id("resultado-DNEC"));

        // Localiza as mensagens de erro quando o CEP não existe
        private IWebElement MensagemResultado => _driver.FindElement(By.CssSelector("#mensagem-resultado"));
        private IWebElement MensagemResultadoAlerta => _driver.FindElement(By.CssSelector("#mensagem-resultado-alerta h6"));

        public void SearchCep(string cep)
        {
            // Limpa o campo antes de enviar o valor
            CepInput.Clear();

            // Envia o valor do CEP
            CepInput.SendKeys(cep);

            // Por conta do Captcha deve aguardar 10 segundos
            Thread.Sleep(10000);

            // Clica no botão de busca
            BuscarButton.Click();
        }

        public string GetLogradouro()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("resultado-DNEC")));

            // Retorna o logradouro da primeira célula da primeira linha da tabela
            return ResultadoTabela.FindElement(By.XPath("//tbody/tr/td[1]")).Text;
        }

        public string GetBairro()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("resultado-DNEC")));

            // Retorna o bairro da segunda célula da primeira linha da tabela
            return ResultadoTabela.FindElement(By.XPath("//tbody/tr/td[2]")).Text;
        }

        public string GetEstado()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("resultado-DNEC")));

            // Retorna o estado (localidade/UF) da terceira célula da primeira linha da tabela
            return ResultadoTabela.FindElement(By.XPath("//tbody/tr/td[3]")).Text;
        }

        public string GetCep()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("resultado-DNEC")));

            // Retorna o CEP da quarta célula da primeira linha da tabela
            return ResultadoTabela.FindElement(By.XPath("//tbody/tr/td[4]")).Text;
        }

        public bool VerifyCepNotFound()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#mensagem-resultado")));

                string mensagem1 = MensagemResultado.Text;
                string mensagem2 = MensagemResultadoAlerta.Text;

                return mensagem1.Contains("Não há dados a serem exibidos") && mensagem2.Contains("Dados não encontrado");
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}

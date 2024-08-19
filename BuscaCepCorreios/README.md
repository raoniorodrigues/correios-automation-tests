



### Projeto de Automação de Testes - Busca de CEP e Rastreamento de Código nos Correios

Este projeto é uma automação de testes de integração para o site dos Correios, especificamente para as funcionalidades de busca de CEP e rastreamento de códigos. Ele utiliza C#, NUnit, SpecFlow, Selenium WebDriver e ChromeDriver para executar testes automatizados. Este README serve como guia para instalação, configuração, execução dos testes e proposta de melhorias para o projeto.

---

## Sumário

- [Sumário](#sumário)
- [Instalação e Configuração](#instalação-e-configuração)
  - [Pré-requisitos](#pré-requisitos)
  - [Instalação](#instalação)
  - [Explicação:](#explicação)
- [Dependências](#dependências)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Funcionalidades Testadas](#funcionalidades-testadas)
- [Execução dos Testes](#execução-dos-testes)
  - [ATENÇÃO:](#atenção)
- [Desafios e Soluções Implementadas](#desafios-e-soluções-implementadas)
- [Propostas de Melhoria](#propostas-de-melhoria)
- [Conclusão](#conclusão)

---

## Instalação e Configuração

### Pré-requisitos

1. **.NET SDK**: Certifique-se de que o SDK do .NET está instalado em sua máquina. O projeto foi desenvolvido usando .NET Core, e você pode baixá-lo em [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download).

2. **Visual Studio**: Recomenda-se usar o Visual Studio 2019 ou superior com suporte ao desenvolvimento em C#. Certifique-se de ter as extensões de teste NUnit e SpecFlow instaladas.

3. **ChromeDriver**: O ChromeDriver deve estar instalado e ser compatível com a versão do Google Chrome instalada no sistema. O caminho do `ChromeDriver` deve estar incluído no `PATH` do sistema ou estar configurado corretamente no projeto.

### Instalação

1. **Clonar o Repositório**:
   ```sh
   git clone https://github.com/seu-usuario/seu-repositorio.git
   cd seu-repositorio
   ```

2. **Restaurar Dependências**:
   No Visual Studio, abra a solução e restaure as dependências do projeto. Isso pode ser feito automaticamente ao abrir o projeto ou manualmente via CLI:
   ```sh
   dotnet restore
   ```

---
3. **Estrutura de Projeto**:

Aqui está a estrutura de pastas limpa, mantendo apenas os arquivos importantes para o desenvolvimento, manutenção e execução dos testes automatizados:

```
BuscaCepCorreios
├─ Features
│  ├─ BuscaCep.feature
│  └─ BuscaCep.feature.cs
├─ Pages
│  ├─ HomePage.cs
│  ├─ RastreamentoPage.cs
│  ├─ SearchCepPage.cs
│  └─ TrackPackagePage.cs
├─ StepDefinitions
│  └─ BuscaCepSteps.cs
├─ Tests
│  └─ TestRun.cs
├─ BuscaCepCorreios.csproj
└─ README.md
```

### Explicação:
- **Features/**: Contém os arquivos `.feature` do SpecFlow, que descrevem os cenários de teste BDD.
- **Pages/**: Contém as classes de Page Object Model (POM) que mapeiam os elementos das páginas do site dos Correios.
- **StepDefinitions/**: Contém as classes que implementam os steps definidos nos arquivos `.feature`.
- **Tests/**: Contém classes adicionais de testes ou configurações específicas.
- **BuscaCepCorreios.csproj**: Arquivo de configuração do projeto no .NET.
---

## Dependências

As principais dependências do projeto incluem:

- **Selenium WebDriver**: Para controle e automação do navegador.
- **NUnit**: Framework de teste utilizado para executar e validar os testes.
- **SpecFlow**: Ferramenta BDD usada para definir e organizar os cenários de teste.
- **ChromeDriver**: Driver específico para o navegador Google Chrome.
- **SeleniumExtras.WaitHelpers**: Biblioteca auxiliar para gerenciamento de esperas explícitas no Selenium.

Essas dependências estão listadas no arquivo `*.csproj` do projeto e são gerenciadas automaticamente pelo NuGet.

---

## Estrutura do Projeto

- **Pages/**: Contém as classes de Page Object para mapear os elementos e ações das páginas do site dos Correios.
  - `HomePage.cs`: Mapeia a página inicial dos Correios.
  - `SearchCepPage.cs`: Mapeia a página de busca de CEP.
  - `RastreamentoPage.cs`: Mapeia a página de rastreamento de código.

- **Features/**: Contém os arquivos `.feature` do SpecFlow, que definem os cenários de teste BDD.
  - `BuscaCep.feature`: Cenários de teste para a busca de CEP.
  - `Rastreamento.feature`: Cenários de teste para rastreamento de código.

- **Steps/**: Contém as classes de definição de steps (passos) do SpecFlow.
  - `BuscaCepSteps.cs`: Implementa os steps para os cenários de busca de CEP.
  - `BuscaRastreamentoStep.cs`: Implementa os steps para os cenários de rastreamento de código.

- **Hooks/**: Contém os hooks de configuração e teardown (se necessário).

- **Tests/**: Pode conter classes adicionais de testes ou configurações específicas (exemplo: `TestRun.cs`).

---

## Funcionalidades Testadas

1. **Busca de CEP**:
   - Verifica se o logradouro, bairro, estado e CEP retornados são corretos ao buscar um CEP existente.
   - Valida que um CEP inexistente não retorna resultados, exibindo uma mensagem apropriada.

2. **Rastreamento de Código**:
   - Valida que um código de rastreamento inexistente retorna uma mensagem de erro correta.
   - Testa a navegação entre páginas ao rastrear um código e a correta inserção e verificação do código rastreado.

---

## Execução dos Testes

1. **Compilar o Projeto**:
   Certifique-se de que o projeto está compilado sem erros antes de executar os testes.

2. **Executar os Testes**:
   Os testes podem ser executados diretamente no Visual Studio usando a janela de Test Explorer ou via linha de comando:
   ```sh
   dotnet test
   ```

### ATENÇÃO:

- **Interação com CAPTCHA**:
  Como o CAPTCHA não pode ser automatizado, foi inserido um `Thread.Sleep(10000)` de 10 segundos para permitir que o usuário insira manualmente o CAPTCHA durante a execução dos testes de rastreamento. Essa é uma limitação temporária, discutida na seção de propostas de melhoria.

---

## Desafios e Soluções Implementadas

1. **CAPTCHA nos Testes de Rastreamento**:
   - **Desafio**: O CAPTCHA impede a automação completa do fluxo de rastreamento de código.
   - **Solução**: Implementação de uma pausa (`Thread.Sleep`) para permitir a interação humana.

2. **Gerenciamento de Dependências**:
   - **Desafio**: Problemas com a injeção de dependências e resolução do `IWebDriver` entre as classes.
   - **Solução**: Centralização das instâncias do driver e das páginas na classe de steps (`BuscaCepSteps`).

---

## Propostas de Melhoria

1. **Ambiente de Sandbox sem CAPTCHA**:
   - **Proposta**: Criar ou utilizar um ambiente de QA/Sandbox dos Correios onde o CAPTCHA esteja desabilitado, permitindo automação completa dos testes.
   - **Benefício**: Facilita a execução de testes end-to-end sem necessidade de interação humana, melhorando a eficiência dos testes automatizados.

2. **Melhoria no Gerenciamento de Dependências**:
   - **Proposta**: Refatorar o projeto para utilizar injeção de dependências de forma mais robusta, evitando centralizar toda a lógica numa única classe.
   - **Benefício**: Melhora a modularidade e escalabilidade do código, permitindo adições e manutenção mais fáceis.

3. **Parallelização dos Testes**:
   - **Proposta**: Configurar os testes para rodar em paralelo, especialmente em cenários onde o CAPTCHA não é um fator limitante.
   - **Benefício**: Reduz o tempo total de execução dos testes, aumentando a eficiência do processo de CI/CD.

---

## Conclusão

Este projeto demonstrou a automação de testes básicos e intermediários nas funcionalidades de busca de CEP e rastreamento de códigos no site dos Correios. A implementação atual atende às necessidades, mas há espaço para melhorias significativas, especialmente em termos de automação de CAPTCHA e refatoração de código. As propostas de melhoria sugeridas visam aumentar a eficiência, escalabilidade e robustez dos testes automatizados.
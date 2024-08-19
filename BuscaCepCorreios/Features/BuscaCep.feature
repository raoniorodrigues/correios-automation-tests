Feature: Busca CEP e Rastreamento de Código nos Correios

  # Este cenário verifica se um CEP inexistente é tratado corretamente.
  Scenario: Buscar CEP inexistente
    Given que estou na página inicial dos Correios
    When eu procuro pelo CEP 80700000
    Then eu confirmo que o CEP não existe

  # Este cenário verifica se um CEP existente retorna o endereço esperado.
   Scenario: Buscar CEP existente
    Given que estou na página inicial dos Correios
    When eu procuro pelo CEP 01013001
    Then eu confirmo que o logradouro seja "Rua Quinze de Novembro - lado ímpar"
    Then eu confirmo que o bairro seja "Centro"
    Then eu confirmo que o estado seja "São Paulo/SP"
    Then eu confirmo que o CEP seja "01013-001"

  # Este cenário verifica se um código de rastreamento inexistente é tratado corretamente.
  Scenario: Rastrear código inexistente
    Given que estou na página inicial dos Correios
    When eu procuro pelo código de rastreamento SS987654321BR
    Then eu confirmo que o código de rastreamento está incorreto com a mensagem "Objeto não encontrado na base de dados dos Correios."

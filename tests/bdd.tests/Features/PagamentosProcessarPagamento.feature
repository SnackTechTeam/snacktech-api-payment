Feature: PagamentosProcessarPagamento
    Como usuário do serviço de pagamentos
    Quero que a aplicação receba alteração de status de pagamento
    Atualizando o registro de pagamento e notificando isso para outros
    Serviços dentro da Arquitetura

@Finalizacao
Scenario: Pagamento processado na plataforma
    Given um pagamento retornado pela plataforma
    When for chamado o método PagamentoHook
    Then deve retornar um objeto OkResult

@FinalizacaoMock
Scenario: Pagamento Finalizado Via Mock
    Given um Id de pedido válido
    When for chamado o método PagamentoMock
    Then deve retornar um objeto OkResult
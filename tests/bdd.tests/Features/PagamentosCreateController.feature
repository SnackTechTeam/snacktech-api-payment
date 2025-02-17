Feature: PagamentosCreateController
    Como usuário do serviço de pagamentos
    Quero criar pagamentos para pedidos de forma eficiente
    Para garantir que esses pedidos sejam pagos corretamente através da plataforma de pagamento

@Criacao
Scenario: Iniciar um pagamento
    Given eu tenho um Pedido válido
    When eu chamar o método CriarPagamentoParaPedido
    Then eu devo receber um IActionResult do tipo OkObjectResult
    And a resposta deve conter os campos id,qrCode e valorTotal preenchido


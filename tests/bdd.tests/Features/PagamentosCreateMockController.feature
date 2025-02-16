Feature: PagamentosCreateMockController
    Como usuário do serviço de pagamentos
    Quero criar pagamentos com mock para pedidos de forma eficiente
    Para em alguns cenários poder simular comportamentos sem precisar depender da plataforma de pagamento
    

@CriacaoMock
Scenario: Iniciar um pagamento com mock
    Given eu tenho um Pedido preenchido corretamente
    When eu chamar o método CriarPagamentoMock
    Then eu devo receber um retorno IActionResult do tipo OkObjectResult
    And a resposta deve ser do tipo PagamentoDto com os campos preenchidos
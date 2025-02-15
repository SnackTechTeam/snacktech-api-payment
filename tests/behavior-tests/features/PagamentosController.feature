Feature: PagamentosController
    Como usuário do serviço de pagamentos
    Quero criar pagamentos para pedidos de forma eficiente
    Para garantir que esses pedidos sejam pagos corretamente através da plataforma de pagamento

Background: 
    Given um pedido válido

@Criacao
Scenario: Iniciar um pagamento
    When eu envio uma solicitação para criar um pagamento
    Then eu devo receber um dado de pagamento válido

@Finalizacao
Scenario: Pagamento processado na plataforma
    Given um pagamento retornado pela plataforma
    When eu envio um evento para indicar que o pagamento foi feito
    Then eu devo receber uma resposta de sucesso
    
using common.Interfaces;
using common.Options;

namespace core.gateways
{
    internal class RabbitMqGateway(IRabbitIntegration rabbitIntegration, RabbitMqPublishValues rabbitMqPublishValues)
    {
        internal async Task PublicarMensagemPagamentoRealizado<T>(T mensagem){
            await rabbitIntegration.Publish(rabbitMqPublishValues.ExchangeName,rabbitMqPublishValues.RouteKeySucesso,mensagem);
        }

        internal async Task PublicarMensagemPagamentoCancelado<T>(T mensagem){
            await rabbitIntegration.Publish(rabbitMqPublishValues.ExchangeName,rabbitMqPublishValues.RouteKeyFalha,mensagem);
        }
    }
}
using common.ExternalSource.Sqs;
using common.Interfaces;

namespace core.gateways
{
    internal class SqsGateway(ISqsIntegration sqsIntegration)
    {
        internal async Task PublicarMensagemPagamentoNoSqs(PagamentoMessageDto pagamentoMessageDto){
            await sqsIntegration.SendMessageAsync(pagamentoMessageDto);
        }
    }
}
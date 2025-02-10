using System.Diagnostics.CodeAnalysis;
using common.ExternalSource.Sqs;
using common.Interfaces;
using core.interfaces.gateways;

namespace core.gateways
{
    internal class SqsGateway(ISqsIntegration sqsIntegration) : ISqsGateway
    {
        internal async Task PublicarMensagemPagamentoNoSqs(PagamentoMessageDto pagamentoMessageDto){
            await sqsIntegration.SendMessageAsync(pagamentoMessageDto);
        }

        [ExcludeFromCodeCoverage]
        Task ISqsGateway.PublicarMensagemPagamentoNoSqs(PagamentoMessageDto pagamentoMessageDto)
        {
            return PublicarMensagemPagamentoNoSqs(pagamentoMessageDto);
        }
    }
}
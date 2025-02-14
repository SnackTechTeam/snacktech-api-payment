using common.ExternalSource.Sqs;

namespace core.interfaces.gateways
{
    public interface ISqsGateway
    {
        Task PublicarMensagemPagamentoNoSqs(PagamentoMessageDto pagamentoMessageDto);
    }
}
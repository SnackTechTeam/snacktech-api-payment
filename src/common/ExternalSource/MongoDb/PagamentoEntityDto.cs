using common.ApiSource.MercadoPago;
using common.DataSource;

namespace common.ExternalSource.MongoDb
{
    public class PagamentoEntityDto
    {
        public PedidoDto pedidoDto {get; set;} = default!;
        public MercadoPagoQrCodeDto pagamentoDto {get; set;} = default!;
    }
}
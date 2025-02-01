using common.Enums;
using common.ExternalSource.MongoDb;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace driver.database.mongo.Entities
{
    public class Pagamento
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {get; set;} = default!;

        public string PedidoId {get; set;} = default!;
        public Cliente Cliente {get; set;} = default!;

        public DateTime DataCriacao {get; set;}

        public string LojaPedidoId {get; set;} = default!;
        public string QrCodePagamento {get; set;} = default!;
        public decimal Valor {get; set;}
        public string Status {get; set;} = default!;
        public DateTime? DataUltimaAtualizacao {get; set;}

        public static Pagamento ConverterParaPagamento(PagamentoEntityDto pagamentoEntityDto, StatusPagamento statusPagamento){
            return new Pagamento{
                PedidoId = pagamentoEntityDto.pedidoDto.PedidoId.ToString(),
                Cliente = pagamentoEntityDto.pedidoDto.Cliente,
                DataCriacao = DateTime.Now,
                LojaPedidoId = pagamentoEntityDto.pagamentoDto.LojaPedidoId,
                QrCodePagamento = pagamentoEntityDto.pagamentoDto.DadoDoCodigo,
                Valor = pagamentoEntityDto.pagamentoDto.ValorPagamento,
                Status = statusPagamento.ToString(),
                DataUltimaAtualizacao = null
            };
        }

        public static implicit operator BuscaPagamentoDto(Pagamento pagamento){
            return new BuscaPagamentoDto{
                ClienteId = pagamento.Cliente.ClienteId,
                DataCriacao = pagamento.DataCriacao,
                DataUltimaAtualizacao = pagamento.DataUltimaAtualizacao,
                PagamentoId = pagamento.Id,
                PedidoId = pagamento.PedidoId,
                Status = pagamento.Status,
                Valor = pagamento.Valor
            };
        }
    }
}
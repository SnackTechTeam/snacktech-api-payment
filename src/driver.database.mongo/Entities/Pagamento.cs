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

        public string InStoreOrderId {get; set;} = default!;
        public string QrData {get; set;} = default!;
        public string Status {get; set;} = default!;
    }
}
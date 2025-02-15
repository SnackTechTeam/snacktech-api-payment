using Newtonsoft.Json;

namespace behavior_tests.helpers
{
    public static class StringContentBuilder
    {
        public static string CriarPagamentoBody(){
            var objeto = new {
                pedidoId = Guid.NewGuid(),
                cliente = new {
                    id = Guid.NewGuid(),
                    nome = "NomeCliente",
                    email = "email@gmail.com"
                },
                itens = new List<object>(){
                    new {
                        pedidoItemId = Guid.NewGuid(),
                        valor = 20.0
                    }
                },
            };
            return JsonConvert.SerializeObject(objeto);
        }

        public static string CriarPagamentoResponse(){
            var objeto = new {
                id = Guid.NewGuid(),
                qrCode = $"00020101021243650016COM.FAKESITE020130636{Guid.NewGuid()}5204000053039865802BR5909Test Test6009SAO PAULO72570553***63046F20",
                valorTotal = 20.0
            };
            return JsonConvert.SerializeObject(objeto);
        }

        public static string CriarFinalizacaoBody(){
            var objeto = new {
                action = "update",
                application_id = "7910800073137785",
                data = new {
                    currency_id = "",
                    marketplace = "NONE",
                    status = "closed"
                    },
                date_created = "2024-10-06T17:42:13.510-04:00",
                id = "23603350837",
                live_mode = false,
                status = "closed",
                type = "topic_merchant_order_wh",
                user_id = 2012037660,
                version = 1
                };

            return JsonConvert.SerializeObject(objeto);
        }
    }
}
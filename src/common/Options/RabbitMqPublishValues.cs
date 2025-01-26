namespace common.Options
{
    public class RabbitMqPublishValues
    {
        public string ExchangeName {get; set;} = default!;
        public string RouteKeySucesso {get; set;} = default!;
        public string RouteKeyFalha {get; set;} = default!;
    }
}
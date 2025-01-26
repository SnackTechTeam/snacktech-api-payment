namespace common.Options
{
    public class MongoDbOptions
    {
        public string Endpoint {get; set;} = default!;
        public string Port {get; set;} = default!;
        public string UserName {get; set;} = default!;
        public string Password {get; set;} = default!;
        public bool SSL {get; set;}
        public string SslCertificatePath {get; set;} = default!;
        public string DatabaseName {get; set;} = default!;
    }
}
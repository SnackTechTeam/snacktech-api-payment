namespace common.Options
{
    public class SqsOptions
    {
        public string ServiceUrl {get; set;} = default!;
        public string QueueUrl {get; set;} = default!;
        public string AwsAccessKeyId {get; set;} = default!;
        public string AwsSecretAccessKey {get; set;} = default!;
        public string AwsSecretAccessToken {get; set;} = default!;
    }
}
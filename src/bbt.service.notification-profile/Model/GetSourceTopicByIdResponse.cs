

public class GetSourceTopicByIdResponse
{

        public int Id { get; set; }
        public string Topic { get; set; }
        public string PushServiceReference { get; set; }
        public string SmsServiceReference { get; set; }
        public string EmailServiceReference { get; set; }

        public class TitleLabel
        {
            public string EN { get; set; }
            public string TR { get; set; }

        }
    }


//TODO: Display diger modellerede yansimali

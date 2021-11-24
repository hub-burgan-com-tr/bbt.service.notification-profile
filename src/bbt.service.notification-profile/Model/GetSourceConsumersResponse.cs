

public class GetSourceConsumersResponse
{
    public List<Consumer> Consumers { get; set; }

    public class Consumer
    {
       public bool IsPushEnabled { get; set; }
        public string DeviceKey { get; set; }
        public bool IsSmsEnabled { get; set; }
        public Phone Phone { get; set; }
        public bool IsMailEnabled { get; set; }
        public string Email { get; set; }
    }
}

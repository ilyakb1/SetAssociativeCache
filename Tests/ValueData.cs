namespace Tests
{
    public class ValueData
    {
        public ValueData(int id, string payload)
        {
            Id = id;
            Payload = payload;
        }

        public int Id { get; }

        public string Payload { get; }
    }
}

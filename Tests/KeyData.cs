namespace Tests
{
    public class KeyData
    {
        public KeyData(int id, string payload)
        {
            Id = id;
            Payload = payload;
        }

        public int Id { get; }

        public string Payload { get; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}

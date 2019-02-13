namespace Chronicle.Persistence
{
    internal sealed class SagaContextMetadata : ISagaContextMetadata
    {
        public string Key { get; }
        public object Value { get; }

        public SagaContextMetadata(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}

namespace Chronicle
{
    public interface ISagaContextMetadata
    {
        string Key { get; }
        object Value { get; }
    }
}

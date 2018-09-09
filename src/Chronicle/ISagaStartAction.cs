namespace Chronicle
{
    public interface ISagaStartAction<in TMessage> : ISagaAction<TMessage>
    {
    }
}

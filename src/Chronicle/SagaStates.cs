namespace Chronicle
{
    public enum SagaStates : byte
    {
        Pending = 0,
        Completed = 1,
        Rejected = 3
    }
}

namespace Chronicle
{

    public class ChronicleConfiguration : IChronicleConfiguration
    {
        public bool AllowConcurrentWrites { get; set; } = true;
    }
}
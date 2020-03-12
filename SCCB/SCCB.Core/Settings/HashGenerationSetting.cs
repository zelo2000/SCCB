namespace SCCB.Core.Settings
{
    public class HashGenerationSetting
    {
        public string Salt { get; set; }

        public int IterationCount { get; set; }

        public int BytesNumber { get; set; }
    }
}

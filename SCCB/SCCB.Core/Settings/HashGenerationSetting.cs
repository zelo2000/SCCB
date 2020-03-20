namespace SCCB.Core.Settings
{
    public class HashGenerationSetting
    {
        /// <summary>
        /// Get or set the hash generating salt
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// Get or set the pseudo random iteration count
        /// </summary>
        public int IterationCount { get; set; }

        /// <summary>
        /// Get or set the key length in bytes
        /// </summary>
        public int BytesNumber { get; set; }
    }
}

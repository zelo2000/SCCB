namespace SCCB.Core.Infrastructure
{
    public interface IIdentifiable<TKey>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        TKey Id { get; }
    }
}

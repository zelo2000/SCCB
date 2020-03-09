namespace SCCB.Core.Helpers
{
    public interface IIdentifiable<TKey>
    {
        TKey Id { get; }
    }
}

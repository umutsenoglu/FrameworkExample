namespace Common.Validation
{
    public interface IValidator
    {
        bool Validate<T>(T item);
    }
}

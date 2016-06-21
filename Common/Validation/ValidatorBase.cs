
namespace Common.Validation
{
    public abstract class ValidatorBase : IValidator
    {
        public abstract bool Validate<T>(T item);
    }
    
}

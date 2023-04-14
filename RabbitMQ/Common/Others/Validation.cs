using Common.Intefaces;

namespace Common.Others;

public class Validation : IValidation
{
    public void CheckNull(object? obj, string errorMessage = "cannot be null!")
    {
        if (obj is null)
        {
            string error = $"{nameof(obj)} {errorMessage}";
            throw new ArgumentNullException(nameof(obj), error);
        }
    }
}
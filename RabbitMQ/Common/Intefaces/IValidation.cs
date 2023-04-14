namespace Common.Intefaces;

public interface IValidation
{
    void CheckNull(object? obj, string errorMessage = "cannot be null!");
}
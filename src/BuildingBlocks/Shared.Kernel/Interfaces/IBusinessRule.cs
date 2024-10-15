using AWC.Shared.Kernel.Utilities;

namespace AWC.Shared.Kernel.Interfaces;

public interface IBusinessRule<T>
{
    void SetNext(IBusinessRule<T> next);

    Task<Result> Validate(T request);
}

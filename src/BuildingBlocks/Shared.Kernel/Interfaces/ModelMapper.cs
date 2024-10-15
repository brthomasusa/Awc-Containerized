using AWC.Shared.Kernel.Utilities;

namespace AWC.Shared.Kernel.Interfaces;

public abstract class ModelMapper<TSource, TDestination>
{
    public abstract Result<TDestination> Map(TSource dataModel);
}

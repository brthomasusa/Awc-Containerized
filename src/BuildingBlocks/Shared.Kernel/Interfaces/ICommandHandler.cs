using AWC.Shared.Kernel.Utilities;

namespace AWC.Shared.Kernel.Interfaces;

public interface ICommandHandler<TCommand>
{
    Task<Result<bool>> Handle(TCommand command);

    void SetNext(ICommandHandler<TCommand> next);
}

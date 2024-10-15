using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWC.Shared.Kernel.Utilities;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        "ValidationError",
        "A validation problem occurred.");

    Error[] Errors { get; }
}

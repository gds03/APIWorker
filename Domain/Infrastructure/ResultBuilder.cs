using FluentResults;

namespace Domain.Infrastructure;

public class ResultBuilder<T>
{
    private readonly List<string> _errors = [];
    
    public ResultBuilder<T> Error(string error)
    {
        _errors.Add(error);
        return this;
    }
    
    public Result<T> Build(Func<T> instance) => _errors.Any()
        ? Result.Fail<T>(_errors)
        : Result.Ok(instance());
}
using Ardalis.Result;

namespace WebApi.Extensions
{
    public static class ResultExtensions
    {
        public static Result ToResult<T>(this Result<T> result)
        {
            return result.Status switch
            {
                ResultStatus.Conflict => Result.Success(),
                ResultStatus.Ok => Result.Success(),
                ResultStatus.NotFound => Result.NotFound(result.Errors.ToArray()),
                ResultStatus.Invalid => Result.Invalid(result.ValidationErrors),
                ResultStatus.Forbidden => Result.Forbidden(),
                ResultStatus.Unauthorized => Result.Unauthorized(),
                ResultStatus.Unavailable => Result.Unavailable(),
                _ => Result.Error(new ErrorList(result.Errors, result.CorrelationId)),
            };
        }
    }
}
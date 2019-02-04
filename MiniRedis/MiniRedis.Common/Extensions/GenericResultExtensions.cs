using MiniRedis.Common.Model;
using System.Linq;

namespace MiniRedis.Common.Extensions
{
    public static class GenericResultExtensions
    {
        public static T Valid<T>(this T result)
            where T : GenericResult
        {
            result.IsValid = true;
            return result;
        }

        public static T Valid<T>(this T result, bool valid)
            where T : GenericResult
        {
            result.IsValid = valid;
            return result;
        }

        public static T Invalid<T>(this T result)
            where T : GenericResult
        {
            result.IsValid = false;
            return result;
        }

        public static T WithError<T>(this T result, string error)
            where T : GenericResult
        {
            result.Errors.Add(new ResultError()
            {
                Message = error,
            });

            result.IsValid = false;

            return result;
        }

        public static T WithMessage<T>(this T result, string message)
            where T : GenericResult
        {
            result.Message = message;
            return result;
        }

        public static GenericResult<T> WithData<T>(this GenericResult<T> result, T data)
            where T : GenericResult<T>
        {
            result.Data = data;
            return result;
        }

        public static T Merge<T>(this T result, GenericResult value)
            where T : GenericResult
        {
            result.IsValid = result.IsValid && value.IsValid;
            result.Message = string.Concat(result.Message, " ", value.Message).Trim();
            result.Errors = result.Errors.Concat(value.Errors).ToList();

            return result;
        }
    }
}

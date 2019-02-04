using System;
using System.Collections.Generic;

namespace MiniRedis.Common.Model
{
    public class GenericResult
    {
        public bool IsValid { get; internal set; } = true;

        public List<ResultError> Errors { get; internal set; } = new List<ResultError>();

        public string Message { get; internal set; }
    }

    public class ResultError
    {
        public string Message { get; internal set; }
    }


    public class GenericResult<T> : GenericResult
    {
        public T Data { get; set; }

        public GenericResult()
        {
        }
        
        public GenericResult(T data)
        {
            Data = data;
        }
    }
}

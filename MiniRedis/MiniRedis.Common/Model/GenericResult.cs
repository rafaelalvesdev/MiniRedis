using System;
using System.Collections.Generic;

namespace MiniRedis.Common.Model
{
    public class GenericResult
    {
        public bool IsValid { get; internal set; }

        public List<ResultError> Errors { get; set; } = new List<ResultError>();

        public string Message { get; set; }
    }

    public class ResultError
    {
        public string Message { get; set; }
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

using MiniRedis.Common.Model;
using MiniRedis.Services.Commands.Enums;

namespace MiniRedis.Services.Commands
{
    public class EvaluationResult : GenericResult
    {
        public ResultValueType ValueType { get; }

        public long? Number { get; }
        public string String { get; }
        public string[] StringArray { get; }

        public EvaluationResult()
        {
            ValueType = ResultValueType.None;
        }

        public EvaluationResult(long number)
        {
            ValueType = ResultValueType.Long;
            Number = number;
        }

        public EvaluationResult(string str)
        {
            ValueType = ResultValueType.String;
            String = str;
        }

        public EvaluationResult(string[] array)
        {
            ValueType = ResultValueType.StringArray;
            StringArray = array;
        }
    }
}

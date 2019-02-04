using MiniRedis.Common.Model;
using MiniRedis.Services.Commands.Enums;
using System.Linq;

namespace MiniRedis.Services.Commands
{
    public class EvaluationResult : GenericResult
    {
        public ResultValueType ValueType { get; }

        public long Number { get; }
        public string String { get; }
        public string[] StringArray { get; }

        public EvaluationResult()
        {
            ValueType = ResultValueType.None;
        }

        public EvaluationResult(long number)
        {
            ValueType = ResultValueType.Number;
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

        public override string ToString()
        {
            switch (ValueType)
            {
                case ResultValueType.Number:
                    return $"(integer) {Number}";

                case ResultValueType.String:
                    if (String == null)
                        return "(nil)";

                    return $"\"{String}\"";

                case ResultValueType.StringArray:
                    if ((StringArray?.LongLength ?? 0) == 0)
                        return "(empty list or set)";

                    var i = 0;
                    return StringArray.Aggregate((a, b) => $"{++i}) {a}\n{++i}) {b}");
            }

            return string.Empty;
        }
    }
}

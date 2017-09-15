using System.Collections.Generic;
using matchmaking;

namespace HistoryBattlesServer.Requests
{
    public abstract class Request : Message<HBPlayer>
    {
        protected Request(int id, HBPlayer player) : base(id, player) { }

        public abstract Result Valdate();

        public abstract Result Process();
    }

    public class Result
    {
        public bool Success { get; protected set; }
        public string ErrorMessage { get; protected set; }

        public Result(string errorMessage) {
            ErrorMessage = errorMessage;
            Success = false;
        }

        protected Result() {
            Success = true;
        }

        public static readonly Result Ok = new Result();
    }

    public class ValueResult<TValue> : Result
    {
        public TValue Value { get; private set; }

        protected ValueResult() { }

        public ValueResult(TValue val) {
            Value = val;
        }

        public static ValueResult<TValue> BuildResult(bool success, TValue value) {
            return new ValueResult<TValue> {
                Success = success,
                Value = value
            };
        }
    }
}
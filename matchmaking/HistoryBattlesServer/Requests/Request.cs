using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using matchmaking;

namespace HistoryBattlesServer.Requests
{
    public abstract class Request : Message<HBPlayer>
    {
        protected Request(int id, HBPlayer player) : base(id, player) { }

        public abstract void Deserialize(List<string> parameters);

        public abstract Result Valdate();

        public abstract Result Process();
    }

    public class Result
    {
        public readonly bool Success;
        public readonly string ErrorMessage;

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
        public readonly TValue Value;

        public ValueResult(TValue val) {
            Value = val;
        }

        public ValueResult(string errorMessage) : base(errorMessage) { }
    }
}
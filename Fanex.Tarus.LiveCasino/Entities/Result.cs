using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Fanex.Data.DNAide;

namespace Fanex.Tarus.LiveCasino.Entities
{
    [Serializable]
    [DataContract]
    public class Result
    {
        public Result()
        {
            Key = "0";
            Value = "Successful";
        }

        public Result(string errorCode, string errorMessage)
        {
            Key = errorCode;
            Value = errorMessage;
        }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Value { get; set; }

        private bool _success;

        [DataMember]
        public bool IsSuccess
        {
            get { return Key == "0" || DNAide.Core.CastAide.AsInt32(Key) < 0 || Key.Length == 0; }
            set { _success = value; }
        }
    }

    public class ResultDTO
    {
        [DataMember]
        public List<Result> ErrorHandler { get; set; }

        [DataMember]
        public bool IsSuccess { get; set; }
    }
}

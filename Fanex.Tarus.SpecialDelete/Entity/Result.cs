// -----------------------------------------------------------------------
// <copyright file="Result.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.SpecialDelete.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
   
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

        public string Key { get; set; }

        public string Value { get; set; }

        private bool _success;

        public bool IsSuccess
        {
            get { return Key == "0" || DNAide.Core.CastAide.AsInt32(Key) < 0 || Key.Length == 0; }
            set { _success = value; }
        }
    }

    public class ResultModel
    {
        public List<Result> ErrorHandler { get; set; }

        public bool IsSuccess { get; set; }
    }
}

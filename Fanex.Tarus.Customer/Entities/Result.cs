// -----------------------------------------------------------------------
// <copyright file="Result.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Customer.Entities
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
            ErrorCode = 0;
            ErrorMessage = "Successful";
        }

        public Result(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

       
        public int ErrorCode { get; set; }

       
        public string ErrorMessage { get; set; }
    }

    public class ResultCasinoComm : Result
    {
        public string valueComm { get; set; }
    }
}

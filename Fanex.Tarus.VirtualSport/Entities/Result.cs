// -----------------------------------------------------------------------
// <copyright file="Result.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.VirtualSport.Entities
{
    using System.Runtime.Serialization;
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

        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}

using System.Collections;

namespace Fanex.Tarus.WhiteLabel.Models
{
    public class Result
    {
        public int ErrCode { get; private set; }

        public string ErrMsg { get; private set; }

        public Hashtable Outputs { get; private set; }

        public bool IsSuccess => ErrCode == 0;

        public Result(int errCode, string errMsg)
        {
            ErrCode = errCode;
            ErrMsg = errMsg;
        }

        public Result(int errCode, string errMsg, Hashtable outPuts)
            : this(errCode, errMsg)
        {
            Outputs = outPuts;
        }
    }
}

using System;

namespace Fanex.Tarus.Customer.Requests
{
    public class GetMessageLogsQuery
    {
        public int UserId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int FunctionId { get; set; }
    }
}
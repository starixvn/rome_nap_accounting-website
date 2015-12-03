namespace Fanex.Tarus.Statements.Models
{
    using System;

    [Serializable]
    public class OldStatement
    {
        public string Date { get; set; }

        public string Remark { get; set; }

        public double Total { get; set; }

        public double AccumulatedTotal { get; set; }

        public string Creator { get; set; }

        public string VerifiedBy { get; set; }

        public string CheckedTime { get; set; }

        public string AccumulatedTotalText { get; set; }
    }
}

namespace Fanex.Tarus.Statements.Models
{
    using System;

    [Serializable]
    public class DetailStatement
    {
        public string Date { get; set; }

        public string RefNo { get; set; }

        public string Remark { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        public decimal Total { get; set; }

        public string Creator { get; set; }
    }
}

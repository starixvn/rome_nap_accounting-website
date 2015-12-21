namespace Fanex.Tarus.Statements.Models
{
    using System;

    [Serializable]
    public class AuditStatement
    {
        public string Date { get; set; }

        public string Remark { get; set; }

        public string Type { get; set; }

        public decimal Total { get; set; }

        public decimal AccumulatedTotal { get; set; }
    }
}

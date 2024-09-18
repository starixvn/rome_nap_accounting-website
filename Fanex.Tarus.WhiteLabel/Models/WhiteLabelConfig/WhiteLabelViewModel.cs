using System;

namespace Fanex.Tarus.WhiteLabel.Models.WhiteLabelConfig
{
    public class WhiteLabelViewModel
    {
        public int CustId { get; set; }

        public string WhiteLabel { get; set; }

        public string Headline { get; set; }

        public decimal HeadlineRate { get; set; }

        public string Super { get; set; }

        public decimal SuperRate { get; set; }

        public string Master { get; set; }

        public decimal MasterRate { get; set; }

        public string UserName { get; set; }

        public decimal AgentRate { get; set; }

        public DateTime? LaunchedDate { get; set; }

        public DateTime? TerminatedDate { get; set; }

        public string Remark { get; set; }

        public string Site { get; set; }

        public string SubsidiaryName { get; set; }
    }
}

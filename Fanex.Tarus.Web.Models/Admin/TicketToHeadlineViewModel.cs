using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Web.Models.Admin
{
    public class TicketToHeadlineViewModel
    {
        public string ErrorMessage { get; set; }

        public int TotalRows { get; set; }

        public List<TicketToHeadline> TicketToHeadlineCollection { get; set; }

        public TicketToHeadlineViewModel()
        {
            this.TicketToHeadlineCollection = new List<TicketToHeadline>();
        }
    }
}

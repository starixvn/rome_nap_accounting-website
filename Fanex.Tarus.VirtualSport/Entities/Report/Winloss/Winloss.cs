
using System;
using System.Collections.Generic;
namespace Fanex.Tarus.VirtualSport.Entities.Report.Winloss
{

    [Serializable]
    public class Winloss
    {
        public List<WinlossByDate> WLByDateList { get; set; }
        public List<WinlossDetail> Total { get; set; }
        public List<WinlossDetail> GrandTotal { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fanex.Tarus.LiveCasino.Entities.Report
{
    [Serializable]
    [DataContract]
    public class GameStatisticsBO
    {
        [DataMember]
        public List<GameStatisticsDTO> Items { get; set; }
        [DataMember]
        public int total { get; set; }
        
        [DataMember]
        public List<GameStatisticsDTO> TotalItems { get; set; }

        [DataMember]
        public List<GameStatisticsDTO> GrandTotal { get; set; }
    }

    [Serializable]
    [DataContract]
    public class GameStatisticsParams
    {
        [DataMember]
        public string fromdate { get; set; }
        [DataMember]
        public string todate { get; set; }
        [DataMember]
        public string movebetsday { get; set; }
        [DataMember]
        public string balanceupday { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public bool excludetest { get; set; }
        [DataMember]
        public int site { get; set; }
        [DataMember]
        public int gametype { get; set; }
        [DataMember]
        public int gametable { get; set; }
        [DataMember]
        public int currency { get; set; }
        [DataMember]
        public int reporttype { get; set; }
        [DataMember]
        public int pagesize { get; set; }
        [DataMember]
        public int pageindex { get; set; }
        [DataMember]
        public bool fromcache { get; set; }
        [DataMember]
        public string betLess { get; set; }
        [DataMember]
        public string betMore { get; set; }
        [DataMember]
        public string winLess { get; set; }
        [DataMember]
        public string winMore { get; set; }
        [DataMember]
        public Sorting sort { get; set; }
        [DataMember]
        public bool showrefund { get; set; }        
        
    }

    [Serializable]
    [DataContract]
    public class Category
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Parent { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public bool Selected { get; set; }
    }

    [Serializable]
    [DataContract]
    public class GameSearchBO
    {
        [DataMember]
        public List<Category> Company { get; set; }
        [DataMember]
        public List<Category> GameType { get; set; }
        [DataMember]
        public List<Category> GameTable { get; set; }
        [DataMember]
        public List<Category> ReportType { get; set; }
        [DataMember]
        public List<Category> Currency { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Sorting
    {
        [DataMember]
        public string SortType { get; set; }
        [DataMember]
        public string SortCol { get; set; }
        [DataMember]
        public bool IsDecimal { get; set; }
        [DataMember]
        public bool IsDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Sunplus.Accounting.Report.MatchWL
{
    [ServiceContract]
    public interface IServiceReport2a
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string MatchWL2a(string db, string type, string fdate);
    }

    [DataContract]
    public class Report2aResult
    {
        [DataMember]
        public List<Sport> Sports { get; set; }

        [DataMember]
        public GrandTotal GrandTotal { get; set; }
    }

    public class Sport
    {
        public List<Bettypes> Bettypes;
        public GrandTotal GrandTotal;
    }

    public class Bettypes
    {
        public GrandTotal GrandTotal;
        public List<Match> Matchs;
    }

    public class Match
    {
        public string Bettype { get; set; }
        public DateTime EventDate { get; set; }
        public string Home { get; set; }
        public string Away { get; set; }
        public decimal WinLoss { get; set; }
        public decimal Position { get; set; }
        public decimal Commission { get; set; }
        public decimal VirtualCommission { get; set; }
        public decimal LineTotal { get; set; }
        public decimal VirtualLineTotal { get; set; }
        public decimal Variance { get; set; }
    }

    public class GrandTotal
    {
        public decimal CommGarndTotal { get; set; }
        public decimal VirtualCommGrandTotal { get; set; }
        public decimal LineTotalGrandTotal { get; set; }
        public decimal VirtualLineTotalGrandTotal { get; set; }
        public decimal VarianceGrandTotal { get; set; }
    }
}
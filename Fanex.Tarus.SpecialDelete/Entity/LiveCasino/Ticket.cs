using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fanex.Tarus.SpecialDelete.Entity.LiveCasino
{
    public class Tickets
    {
        public Tickets()
        {
            Licensees = new List<LicenseeTickets>();
        }
        
        public int MatchId { get; set; }
        
        public string MatchCode { get; set; }
        
        public string MatchEvent { get; set; }
        
        public string EventDate { get; set; }
        
        public decimal FromTransID { get; set; }
        
        public decimal ToTransID { get; set; }
        
        public List<LicenseeTickets> Licensees { get; set; }
    }

    
    public class LicenseeTickets
    {
        public string Name { get; set; }
        
        public bool IsAllowOptionDelete { get; set; }
        
        public bool DeleteOtionDefault { get; set; }
        
        public List<TicketDetail> TicketDetails { get; set; }
    }
        
    public class TicketDetail
    {
        public TicketDetail(){}

        public int Index { get; set; }
        
        public decimal RefNo { get; set; }
        
        public string UserName { get; set; }
        
        public string TransTime { get; set; }
        
        public string BetID { get; set; }
        
        public int BetType { get; set; }
        
        public string BetTypeName { get; set; }
        
        public string Stake { get; set; }
        
        public string CurrencyName { get; set; }
        
        public string Status { get; set; }

        public string WinLoss { get; set; }

        public string Comm { get; set; }
        
        public string SuperPT { get; set; }
        
        public string MasterPT { get; set; }
        
        public string AgentPT { get; set; }
        
        public string ChoiceStr { get; set; }

        public int cntChoice { get; set; }
    }
    
    public class Choice
    {
        public string Betteam { get; set; }
        
        public string Stake { get; set; }
        
        public string Odds { get; set; }
    }
}

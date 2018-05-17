namespace Fanex.Tarus.BetList.DataAccess
{
    using Constants;
    using Criteria;
    using Fanex.BetList.Core.Entities;
    using Fanex.NAP.Framework.Data;
    using Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    
    public interface IBetListRepository
    {
        IList<S> FetchMemberWinLoss<S>(ICriteria criteria, Func<DataRow, S> mapObjectFunction, ref List<ITicketData> outputList) where S : class;

        IList<TicketDetail> Fetch(dynamic criteria, CurrencyContants baseCurrency);

        IList<S> Fetch<S>(dynamic criteria, Func<DataRow, S> mapObjectFunction, ref Hashtable outputParams) where S : class;

        IList<S> Fetch<S>(dynamic criteria, Func<DataRow, S> mapObjectFunction) where S : class;

        IList<S> Fetch<S>(dynamic criteria, Func<DataRow, S> mapObjectFunction, ref List<ITicketData> outputList) where S : class;

        SystemParlayModel FetchSystemParlay(BetListSystemParlayByRefnoCriteria criteria);

        CombinationParlay BetList_SelDetailCombinationMP(ICriteria criteria);

        string GetSiteNameByCustId(ICriteria criteria);
    }
}
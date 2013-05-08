using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.SpecialDelete.TicketsListing;
using Sunplus.Accounting.Entities.SpecialDelete.TicketsListing;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Accounting.WcfServiceLibrary.SpecialDelete.TicketsListing
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TicketsListingService:ITicketsListingService
    {
        public LicenseeDTO SelAllLicensee()
        {
            var ex = CommonBootStrapper.Locator.GetInstance<ITicketsListing>();
            return ex.SelAllLicensee();
        }


        public BettypeDTO SelBettype()
        {
            var ex = CommonBootStrapper.Locator.GetInstance<ITicketsListing>();
            return ex.SelBettype();
        }

        public ProductListingDTO SelTicketsListing(int matchID, int licenseeID, string betTypes, string betTypesLive, int Type, int GroupBy, string LiveType, string HomeScore, string AwayScore, string FromTransDate, string ToTransDate, int SAFilter)
        {
            var ex = CommonBootStrapper.Locator.GetInstance<ITicketsListing>();
            
            return ex.SelTicketsListing(matchID, licenseeID, betTypes, betTypesLive, Type, GroupBy, LiveType, HomeScore, AwayScore, FromTransDate, ToTransDate , SAFilter);
        }

        public TicketsListingBetlistDTO GetTicketsListingBetlist(bool fromCache, int pageSize, int pageIndex, bool isMain, int Type, int matchID, string Licensees, string bettypes, string bettypesLive, int groupBy, string LiveType, string HomeScore, string AwayScore, string FromTransDate, string ToTransDate, int SAFilter)
        {
            var rs = new TicketsListingBetlistDTO();
            var ex = CommonBootStrapper.Locator.GetInstance<ITicketsListing>();
            rs= ex.GetTicketsListingBetlist(fromCache, pageSize, pageIndex, isMain, Type, matchID, Licensees, bettypes, bettypesLive, groupBy, LiveType, HomeScore, AwayScore, FromTransDate, ToTransDate, SAFilter);
            return rs;
        }
       
    }
}

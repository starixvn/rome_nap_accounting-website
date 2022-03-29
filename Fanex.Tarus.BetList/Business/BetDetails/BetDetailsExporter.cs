namespace Fanex.Tarus.BetList.Business.BetDetails
{
    using Builders.ExcelBuilders;
    using Constants;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Module.RNGCasino.Criteria;
    using Fanex.Module.RNGCasino.Repositories;
    using Fanex.RemotingRef;
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.DataAccess;
    using Fanex.Voidbridge.BetDetail.Criteria;
    using Fanex.Voidbridge.BetDetail.Repositories;
    using Models;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using Resources;
    using Sunplus.Accounting.Business;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Configurations;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class BetDetailsExporter : IBetDetailsExporter
    {
        private readonly IRemotingRefService _remotingRefService;

        public BetDetailsExporter()
        {
            _remotingRefService = new RemotingRefService();
        }

        public string GetFileNameWithoutExtension(int sportType)
        {
            var fileNameMapper = new Dictionary<int, string>
            {
                {SportContants.AllBet, "Allbet_Details"},
                {SportContants.SaGaming, "SAGaming_Details"},
                {SportContants.SexyGaming, "AESexy_Details"},
                {SportContants.GTCasino, "IBCBetLiveCasino_Details"},
                {SportContants.Voidbridge, "MacauGames_Details"},
                {SportContants.FishingWorld, "FishingWorld_Details"},
                {SportContants.PP, "PP_Details"},
                {SportContants.CardClub, "ArcadiaGaming_Details"},
                {SportContants.Spadegaming, "Spadegaming_Details"},
                {SportContants.RNG_RNGGame, "RNGCasino_Details"},
                {SportContants.RNG_MiniGame, "RNGCasino_Details"},
                {SportContants.RNG_Mobile, "RNGCasino_Details"},
                {SportContants.Sabaclub, "Saba.games_Details"},
                {SportContants.IonCasino, "IONLiveCasino_Details"},
                {SportContants.GDG, "GDG_Details"},
                {SportContants.PPLiveCasino, "PPLiveCasino_Details"},
                {SportContants.FGG, "FGG_Details"}
            };

            return fileNameMapper.ContainsKey(sportType) ?
                fileNameMapper[sportType] :
                $"{_remotingRefService.GetSportName(sportType.ToString(), "en-US")}_Details";
        }

        public IEnumerable<TicketDetail> GetTicketDetails(
            int sportType,
            long refNo,
            int custId,
            byte transType)
        {
            if (sportType == SportContants.AllBet ||
                sportType == SportContants.SaGaming ||
                sportType == SportContants.SexyGaming ||
                sportType == SportContants.GTCasino ||
                sportType == SportContants.IonCasino ||
                sportType == SportContants.WM ||
                sportType == SportContants.GDG ||
                sportType == SportContants.BigGame ||
                sportType == SportContants.PPLiveCasino)
            {
                return GetLiveCasinoTickets(refNo, custId, transType);
            }

            if (sportType == SportContants.BBin)
            {
                return GetBBinTickets(refNo, custId, transType);
            }

            if (sportType == SportContants.Voidbridge ||
                sportType == SportContants.PP ||
                sportType == SportContants.CardClub ||
                sportType == SportContants.Spadegaming ||
                sportType == SportContants.PGSoft ||
                sportType == SportContants.RNGWar)
            {
                return GetMacauGamesTickets(refNo, custId, transType);
            }

            if (sportType == SportContants.FishingWorld ||
                sportType == SportContants.RNG_RNGGame ||
                sportType == SportContants.RNG_MiniGame ||
                sportType == SportContants.RNG_Mobile ||
                sportType == SportContants.Joker ||
                sportType == SportContants.Sabaclub ||
                sportType == SportContants.MaxGame ||
                sportType == SportContants.Habanero ||
                sportType == SportContants.CG ||
                sportType == SportContants.VGaming ||
                sportType == SportContants.AdvantPlay ||
                sportType == SportContants.AdvantPlayMini)
            {
                return GetRNGCasinoOrFishingWorldTickets(refNo, custId, transType);
            }

            return Enumerable.Empty<TicketDetail>();
        }

        public IEnumerable<TicketDetail> GetJackpotDetails(long transId, int custId, int betType)
        {
            var criteria = new GetJackpotDetailsCriteria
            {
                TransId = transId,
                CustId = custId,
                BetType = betType
            };

#pragma warning disable CS0612 // Type or member is obsolete
            var repository = new RNGCasinoRepository();
#pragma warning restore CS0612 // Type or member is obsolete

            return repository.JackpotDetails(criteria).Select(t => new TicketDetail { Ticket = t });
        }

        public void Export(IEnumerable<TicketDetail> ticketDetails, Stream stream, int sportType)
        {
            short excelColumns = 10;

            var factory = new BetListExcelFactory();
            var builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper();
            builder.BuildWorksheet("BetDetails", excelColumns);
            AddHeader(builder, sportType);

            if (ticketDetails.Any())
            {
                int index = 0;
                GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;
                ticketHelper.IsVnIp = IpManagement.IsVNIP;
                var ticketData = new List<ITicketData>();
                ITicket ticket;

                foreach (var item in ticketDetails)
                {
                    ticket = item.Ticket;
                    ticketHelper.Index = index++;

                    builder
                        .AddNo(index)
                        .AddTrans(ticket, ticketHelper)
                        .AddChoice(ticket, ticketHelper, ticketData)
                        .AddOdds(ticket, ticketData, funcGetOddsTypeName)
                        .AddStake(ticket, ticketHelper)
                        .AddWinLoss(ticket)
                        .AddStatus(ticket, ticketHelper, ticketData, AppSession.ViewIPVN || WebSettings.IPVNReplace == "1")
                        .AddPTComm(ticket, ticketData, RoleIdConstants.Super)
                        .AddPTComm(ticket, ticketData, RoleIdConstants.Master)
                        .AddPTComm(ticket, ticketData, RoleIdConstants.Agent);
                }
            }
            else
            {
                AddNoData(builder, excelColumns);
            }

            var workbook = builder.GetBetList();
            workbook.Write(stream);
        }

        private static void AddHeader(IBetListExcelBuilder builder, int sportType)
        {
            var headerStyle = builder.CreateCellStyle();
            headerStyle.CloneStyleFrom(builder.EvenCellStyle);
            headerStyle.FillForegroundColor = 44;
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.SetFont(builder.RTFHelper.PosFont);

            CellAddress cellAddr;
            cellAddr = builder.AddCell(new HSSFRichTextString(BetListResources.No), headerStyle);
            builder.SetColumnWidth(cellAddr.ColumnIndex, 1200);

            cellAddr = builder.AddCell(new HSSFRichTextString(sportType == SportContants.AllBet || sportType == SportContants.SaGaming || sportType == SportContants.GDG ? BetListResources.Trans_Time : BetListResources.Date), headerStyle);
            builder.SetColumnWidth(cellAddr.ColumnIndex, 5400);

            cellAddr = builder.AddCell(new HSSFRichTextString(BetListResources.Choice), headerStyle);
            builder.SetColumnWidth(cellAddr.ColumnIndex, 15500);

            cellAddr = builder.AddCell(new HSSFRichTextString(BetListResources.Odds), headerStyle);
            builder.SetColumnWidth(cellAddr.ColumnIndex, 2400);

            cellAddr = builder.AddCell(new HSSFRichTextString(BetListResources.Stake), headerStyle);
            builder.SetColumnWidth(cellAddr.ColumnIndex, 2400);

            cellAddr = builder.AddCell(new HSSFRichTextString(BetListResources.Win_Loss), headerStyle);
            builder.SetColumnWidth(cellAddr.ColumnIndex, 3400);

            cellAddr = builder.AddCell(new HSSFRichTextString(BetListResources.Status), headerStyle);
            builder.SetColumnWidth(cellAddr.ColumnIndex, 3400);

            cellAddr = builder.AddCell(
                new HSSFRichTextString(string.Format("{0}\n{1}/{2}", BetListResources.Super, BetListResources.PT, BetListResources.Comm)),
                headerStyle);
            builder.SetColumnWidth(cellAddr.ColumnIndex, 3400);

            cellAddr = builder.AddCell(
                new HSSFRichTextString(string.Format("{0}\n{1}/{2}", BetListResources.Master, BetListResources.PT, BetListResources.Comm)),
                headerStyle);
            builder.SetColumnWidth(cellAddr.ColumnIndex, 3400);

            cellAddr = builder.AddCell(
                new HSSFRichTextString(string.Format("{0}\n{1}/{2}", BetListResources.Agent, BetListResources.PT, BetListResources.Comm)),
                headerStyle);
            builder.SetColumnWidth(cellAddr.ColumnIndex, 3400);
        }

        private static void AddNoData(IBetListExcelBuilder builder, int columnsNum)
        {
            ICellStyle captionStyle = builder.CreateCellStyle();
            captionStyle.CloneStyleFrom(builder.EvenCellStyle);
            captionStyle.Alignment = HorizontalAlignment.Center;
            captionStyle.SetFont(builder.RTFHelper.PosFont);

            IRichTextString rtfCaption = new HSSFRichTextString(BetListResources.ThereIsNoData);

            CellAddress headerCellAddress = builder.AddCell(rtfCaption, captionStyle);
            CellRangeAddress mergeRange = new CellRangeAddress(headerCellAddress.RowIndex, headerCellAddress.RowIndex, 0, columnsNum - 1);
            builder.MergeCell(mergeRange, captionStyle);
        }

        #region Get Data functions

        private static IEnumerable<TicketDetail> GetLiveCasinoTickets(long refNo, int custId, byte transType)
        {
            var criteria = new GetAGCasinoBetDetailsCriteria
            {
                Refno = refNo,
                CustId = custId,
                TransType = transType,
                PageIndex = 1,
                PageSize = int.MaxValue
            };

            IAGCasinoRepository repository = new AGCasinoRepository();

            return repository.BetDetails(criteria);
        }

        private static IEnumerable<TicketDetail> GetBBinTickets(long refNo, int custId, byte transType)
        {
            var criteria = new GetBBinBetDetailsCriteria
            {
                Refno = refNo,
                CustId = custId,
                TransType = transType,
                PageIndex = 1,
                PageSize = int.MaxValue
            };

            IAGCasinoRepository repository = new AGCasinoRepository();

            return repository.BetDetails(criteria);
        }

        private static IEnumerable<TicketDetail> GetMacauGamesTickets(long refNo, int custId, byte transType)
        {
            var criteria = new GetVoidbridgeBetDetailsCriteria
            {
                TransId = refNo,
                CustId = custId,
                TransType = transType,
                PageIndex = 1,
                PageSize = int.MaxValue
            };

            IVoidbridgeRepository repository = new VoidbridgeRepository();

            return repository.BetDetails(criteria).Select(t => new TicketDetail { Ticket = t });
        }

        private static IEnumerable<TicketDetail> GetRNGCasinoOrFishingWorldTickets(long refNo, int custId, byte transType)
        {
#pragma warning disable CS0612 // Type or member is obsolete
            var criteria = new GetRNGCasinoBetDetailsCriteria
#pragma warning restore CS0612 // Type or member is obsolete
            {
                TransId = refNo,
                CustId = custId,
                TransType = transType,
                PageIndex = 1,
                PageSize = int.MaxValue
            };

#pragma warning disable CS0612 // Type or member is obsolete
            IRNGCasinoRepository repository = new RNGCasinoRepository();
#pragma warning restore CS0612 // Type or member is obsolete

            return repository.BetDetails(criteria).Select(t => new TicketDetail { Ticket = t });
        }

        #endregion Get Data functions
    }
}
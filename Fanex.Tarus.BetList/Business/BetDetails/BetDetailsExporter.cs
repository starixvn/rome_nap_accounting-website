namespace Fanex.Tarus.BetList.Business.BetDetails
{
    using Builders.ExcelBuilders;
    using Constants;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Module.RNGCasino.Criteria;
    using Fanex.Module.RNGCasino.Services;
    using Fanex.RemotingRef;
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.DataAccess;
    using Models;
    using BetListNPOI.HSSF.UserModel;
    using BetListNPOI.SS.UserModel;
    using BetListNPOI.SS.Util;
    using Resources;
    using Sunplus.Accounting.Business;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Configurations;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CellAddress = Fanex.BetList.Core.Entities.CellAddress;
    using Fanex.Tarus.BetList.Extensions;

    public class BetDetailsExporter : IBetDetailsExporter
    {
        private readonly IRemotingRefService _remotingRefService;

        public BetDetailsExporter()
        {
            _remotingRefService = new RemotingRefService();
        }

        public string GetFileNameWithoutExtension(int sportType, string sportName)
        {
            var fileNameMapper = new Dictionary<int, string>
            {
                {SportContants.AllBet, "Allbet_Details"},
                {SportContants.SaGaming, "SAGaming_Details"},
                {SportContants.SexyGaming, "AESexy_Details"},
                {SportContants.GTCasino, "IBCBetLiveCasino_Details"},
                {SportContants.Voidbridge, "MacauGames_Details"},
                {SportContants.PP, "PP_Details"},
                {SportContants.CardClub, "ArcadiaGaming_Details"},
                {SportContants.Spadegaming, "Spadegaming_Details"},
                {SportContants.RNG_RNGGame, "SabaCasino_Details"},
                {SportContants.RNG_MiniGame, "SabaCasino_Details"},
                {SportContants.RNG_Mobile, "SabaCasino_Details"},
                {SportContants.Sabaclub, "Saba.games_Details"},
                {SportContants.IonCasino, "IONLiveCasino_Details"},
                {SportContants.PPLiveCasino, "PPLiveCasino_Details"}
            };
            var name = string.IsNullOrEmpty(sportName) ? _remotingRefService.GetSportName(sportType.ToString(), "en-US") : sportName;
            return fileNameMapper.ContainsKey(sportType) ?
                fileNameMapper[sportType] :
                $"{name}_Details";
        }

        public IEnumerable<TicketDetail> GetTicketDetails(
            int sportType,
            long refNo,
            int custId,
            byte transType,
            DateTime winlostDate)
        {
            if (sportType == SportContants.AllBet ||
                sportType == SportContants.SaGaming ||
                sportType == SportContants.SexyGaming ||
                sportType == SportContants.GTCasino ||
                sportType == SportContants.IonCasino ||
                sportType == SportContants.WM ||
                sportType == SportContants.BigGame ||
                sportType == SportContants.PPLiveCasino ||
                sportType == SportContants.FGG ||
                sportType == SportContants.YeeBet ||
                sportType == SportContants.WE)
            {
                return GetLiveCasinoTickets(refNo, custId, transType, winlostDate);
            }

            if (sportType == SportContants.BBin)
            {
                return GetBBinTickets(refNo, custId, transType);
            }

            if (sportType == SportContants.RNG_RNGGame ||
                sportType == SportContants.RNG_MiniGame ||
                sportType == SportContants.RNG_Mobile ||
                sportType == SportContants.Joker ||
                sportType == SportContants.Sabaclub ||
                sportType == SportContants.MaxGame ||
                sportType == SportContants.Habanero ||
                sportType == SportContants.CG ||
                sportType == SportContants.VGaming ||
                sportType == SportContants.AdvantPlay ||
                sportType == SportContants.AdvantPlayMini ||
                sportType == SportContants.Voidbridge ||
                sportType == SportContants.PP ||
                sportType == SportContants.CardClub ||
                sportType == SportContants.Spadegaming ||
                sportType == SportContants.PGSoft ||
                sportType == SportContants.RNGWar ||
                sportType == SportContants.SealEntertainment ||
                sportType == SportContants.MGRNG ||
                sportType == SportContants.Jili ||
                sportType == SportContants.Live22 ||
                sportType == SportContants.UUSlots ||
                sportType == SportContants.SabaxD)
            {
                return GetRNGCasinoTickets(refNo, custId, transType, winlostDate);
            }

            return Enumerable.Empty<TicketDetail>();
        }

        public IEnumerable<TicketDetail> GetJackpotDetails(long transId, int custId, int betType, DateTime winlostDate)
        {
            var criteria = new GetJackpotDetailsCriteria
            {
                TransId = transId,
                CustId = custId,
                BetType = betType,
                WinLostDate = winlostDate
            };

            var service = new RNGCasinoService();

            return service.JackpotDetails(criteria).Select(t => new TicketDetail { Ticket = t.Ticket, DeviceType = t.DeviceType });
        }

        public void Export(IEnumerable<TicketDetail> ticketDetails, Stream stream, int sportType, bool hideSabaText)
        {
            short excelColumns = 11;

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
                    ticketHelper.HideSabaText = hideSabaText;

                    builder
                        .AddNo(index)
                        .AddTrans(ticket, ticketHelper)
                        .AddChoice(ticket, ticketHelper, ticketData)
                        .AddOdds(ticket, ticketData, funcGetOddsTypeName)
                        .AddStake(ticket, ticketHelper)
                        .AddWinLoss(ticket)
                        .AddStatisWithIP(ticket, item, ticketHelper)
                        .AddPTComm(ticket, ticketData, RoleIdConstants.Super)
                        .AddPTComm(ticket, ticketData, RoleIdConstants.Master)
                        .AddPTComm(ticket, ticketData, RoleIdConstants.Agent)
                        .AddPlatform(ticketHelper, item.Platform);
                }
            }
            else
            {
                AddNoData(builder, excelColumns);
            }

            var workbook = builder.GetBetList();
            workbook.Write(stream, true);
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

            cellAddr = builder.AddCell(new HSSFRichTextString(sportType == SportContants.AllBet || sportType == SportContants.SaGaming ? BetListResources.Trans_Time : BetListResources.Date), headerStyle);
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

            cellAddr = builder.AddCell(new HSSFRichTextString(BetListResources.Platform), headerStyle);
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
        private static IEnumerable<TicketDetail> GetLiveCasinoTickets(long refNo, int custId, byte transType, DateTime winlostDate)
        {
            var criteria = new GetAGCasinoBetDetailsCriteria
            {
                Refno = refNo,
                CustId = custId,
                TransType = transType,
                PageIndex = 1,
                PageSize = int.MaxValue,
                WinLossDate = winlostDate
            };

            var service = new AGCasinoService();

            return service.BetDetails(criteria);
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

            var service = new AGCasinoService();

            return service.BetDetails(criteria);
        }


        private static IEnumerable<TicketDetail> GetRNGCasinoTickets(long refNo, int custId, byte transType, DateTime winlostDate)
        {
            var criteria = new GetRNGCasinoBetDetailsCriteria
            {
                TransId = refNo,
                CustId = custId,
                TransType = transType,
                PageIndex = 1,
                PageSize = int.MaxValue,
                WinLossDate = winlostDate
            };

            var service = new RNGCasinoService();

            return service.BetDetails(criteria).Select(t => new TicketDetail
            {
                Ticket = t.Ticket,
                IP = t.IP,
                Country = t.Country,
                DeviceType = t.DeviceType
            });
        }

        #endregion Get Data functions
    }
}
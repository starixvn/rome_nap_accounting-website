namespace Fanex.Tarus.BetList.Business.BetDetails
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Builders.ExcelBuilders;
    using Constants;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Module.RNGCasino.Criteria;
    using Fanex.Module.RNGCasino.Repositories;
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
    using Sunplus.Accounting.Core.Configurations;
    using Sunplus.Accounting.Core.Common;

    public class BetDetailsExporter : IBetDetailsExporter
    {
        public string GetFileNameWithoutExtension(int sportType)
        {
            if (sportType == SportContants.MicroGamingRNGCasino)
            {
                return "MicrogamingRNGCasino_Details";
            }

            if (sportType == SportContants.AllBet)
            {
                return "Allbet_Details";
            }

            if (sportType == SportContants.Voidbridge)
            {
                return "MacauGames_Details";
            }

            if (sportType == SportContants.FishingWorld)
            {
                return "FishingWorld_Details";
            }

            if (sportType == SportContants.PP)
            {
                return "PP_Details";
            }

            if (sportType == SportContants.RNG_RNGGame ||
                sportType == SportContants.RNG_MiniGame ||
                sportType == SportContants.RNG_Mobile)
            {
                return "RNGCasino_Details";
            }

            return null;
        }

        public IEnumerable<TicketDetail> GetTicketDetails(
            int sportType,
            long refNo,
            int custId,
            byte transType)
        {
            if (sportType == SportContants.MicroGamingRNGCasino)
            {
                return GetMicrogamingRNGTickets(refNo, custId, transType);
            }

            if (sportType == SportContants.AllBet)
            {
                return GetAllBetTickets(refNo, custId, transType);
            }

            if (sportType == SportContants.Voidbridge ||
                sportType == SportContants.PP)
            {
                return GetMacauGamesTickets(refNo, custId, transType);
            }

            if (sportType == SportContants.FishingWorld ||
                sportType == SportContants.RNG_RNGGame ||
                sportType == SportContants.RNG_MiniGame ||
                sportType == SportContants.RNG_Mobile)
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

            var repository = new RNGCasinoRepository();

            return repository.JackpotDetails(criteria).Select(t => new TicketDetail { Ticket = t });
        }

        public void Export(IEnumerable<TicketDetail> ticketDetails, Stream stream, int sportType)
        {
            short excelColumns = 10;

            var factory = new BetListExcelFactory();
            var builder = factory.CreateBuilder();

            builder.BuildWorksheet("BetDetails", excelColumns);
            AddHeader(builder, sportType);

            if (ticketDetails.Any())
            {
                int index = 0;
                GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
                ITicketHelper ticketHelper = new TicketHelper();
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
                        .AddStake(ticket)
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

        private static void AddHeader(BetListExcelBuilder builder, int sportType)
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

            cellAddr = builder.AddCell(new HSSFRichTextString(sportType == SportContants.AllBet ? BetListResources.Trans_Time : BetListResources.Date), headerStyle);
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

        private static void AddNoData(BetListExcelBuilder builder, int columnsNum)
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

        private static IEnumerable<TicketDetail> GetAllBetTickets(long refNo, int custId, byte transType)
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
            var criteria = new GetRNGCasinoBetDetailsCriteria
            {
                TransId = refNo,
                CustId = custId,
                TransType = transType,
                PageIndex = 1,
                PageSize = int.MaxValue
            };

            IRNGCasinoRepository repository = new RNGCasinoRepository();

            return repository.BetDetails(criteria).Select(t => new TicketDetail { Ticket = t });
        }

        private static IEnumerable<TicketDetail> GetMicrogamingRNGTickets(long refNo, int custId, byte transType)
        {
            var criteria = new GetMicroGamingBetDetailsCriteria
            {
                Refno = refNo,
                CustId = custId,
                TransType = transType,
                PageIndex = 1,
                PageSize = int.MaxValue
            };

            IMicroGamingRepository repository = new MicroGamingRepository();

            return repository.BetDetails(criteria);
        }



        #endregion
    }
}

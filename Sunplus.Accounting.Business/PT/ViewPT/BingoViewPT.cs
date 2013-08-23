using System.Collections.Generic;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Utils;
using DNAide.Web;
using Microsoft.Practices.Unity;
using Sunplus.Accounting.Core.Web;
using Sunplus.Bingo.Core;
using Sunplus.Bingo.Entities;
using Sunplus.Bingo.Services;

namespace Sunplus.Accounting.Business.PT.ViewPT
{
    /// <summary>
    /// Summary description for BingoViewPT
    /// </summary>
    public class BingoViewPT : IBingoViewPT
    {
        public BingoViewPT()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region IBingoViewPT Members

        public string GetListBingoGameKindGameType()
        {
            string ListGameKind = string.Empty;
            string ListGameType = string.Empty;
            var customerService = BingoContainer.Current.Resolve<ICustomerService>();
            IResult result = new Result();
            List<GameKind> tblGameKind = customerService.GetListGameKind(BingoSettings.BingoGame.CodeGameNo, ref  result);

            if (tblGameKind != null && tblGameKind.Count > 0)
            {
                //Load Game Type
                foreach (GameKind dr in tblGameKind)
                {
                    List<GameType> tblGameType = customerService.GetListGameType(dr.ID, ref result);
                    if (tblGameType != null && tblGameType.Count > 0)
                    {
                        ListGameKind += "<option value='-" + dr.ID.ToString() + "'>Group - " + dr.Name + "</option>";
                        foreach (GameType dr2 in tblGameType)
                        {
                            ListGameType += "<option value='" + dr2.ID.ToString() + "'>" + dr2.Name + "</option>";
                        }
                    }
                }
            }
            string ALL = "<option value='-1'>.: ALL - Game Kind :.</option>"
                         + "<option value='0'>.: ALL - Game Type :.</option></select>";
            return ListGameType + ListGameKind + ALL;
        }

        public string GetPTBingo(string username, int gameType)
        {
            var customerService = BingoContainer.Current.Resolve<ICustomerService>();
            IResult result = new Result();

            DataTable dtb = null;
            if (gameType < 0)
                dtb = customerService.GetCustomerPt(username, gameType * (-1), 0);
            else
                dtb = customerService.GetCustomerPt(username, 0, gameType);

            if (dtb == null) return "<b><br/>There is no information available.</b>";
            if (dtb.Rows.Count == 0) return "<b><br/>There is no information available.</b>";

            string _s = WebUtils.LoadTemplate("Customer/ViewPT_BingoPT.htm");
            DataRow rInfo = dtb.Rows[dtb.Rows.Count - 1];

            int roleID = CastAide.AsInt32(rInfo["roleid"], 1);

            // information
            string _sUpline = TemplateHandler.CopyBlock(_s, "upline");
            string curUpline = string.Empty;
            StringBuilder _sbUpline = new StringBuilder();
            if (roleID < 2)
            {
                curUpline = _sUpline;
                TemplateHandler.Assign(ref curUpline, "Role", "AGENT");
                TemplateHandler.Assign(ref curUpline, "Name", dtb.Rows[2]["username"].ToString());
                TemplateHandler.Assign(ref curUpline, "ID", dtb.Rows[2]["custid"].ToString());
                _sbUpline.Append(curUpline);
            }
            if (roleID < 3)
            {
                curUpline = _sUpline;
                TemplateHandler.Assign(ref curUpline, "Role", "MASTER");
                TemplateHandler.Assign(ref curUpline, "Name", dtb.Rows[1]["username"].ToString());
                TemplateHandler.Assign(ref curUpline, "ID", dtb.Rows[1]["custid"].ToString());
                _sbUpline.Append(curUpline);
            }
            if (roleID < 4)
            {
                curUpline = _sUpline;
                TemplateHandler.Assign(ref curUpline, "Role", "SUPER");
                TemplateHandler.Assign(ref curUpline, "Name", dtb.Rows[0]["username"].ToString());
                TemplateHandler.Assign(ref curUpline, "ID", dtb.Rows[0]["custid"].ToString());
                _sbUpline.Append(curUpline);
            }
            TemplateHandler.ReplaceBlock(ref _s, "upline", _sbUpline.ToString());
            TemplateHandler.Assign(ref _s, "CustName", rInfo["username"].ToString());
            TemplateHandler.Assign(ref _s, "CustID", rInfo["custid"].ToString());

            // position taking
            string sTable = TemplateHandler.CopyBlock(_s, "tbl");
            for (int i = 1; i < 5; i++)
            {
                if (roleID > i) TemplateHandler.RemoveBlock(ref sTable, "r" + i.ToString());
            }
            string curRow = string.Empty, _sTable = string.Empty;
            StringBuilder _mainTable = new StringBuilder();
            int curRoleID;
            foreach (DataRow r in dtb.Rows)
            {
                curRoleID = CastAide.AsInt16(r["roleid"].ToString());
                if (curRoleID == 4)
                {
                    _sTable = sTable;
                    TemplateHandler.Assign(ref _sTable, "sportName", "Bingo");
                    TemplateHandler.Assign(ref _sTable, "GameKind", r["gamekind"].ToString());
                    TemplateHandler.Assign(ref _sTable, "GameType", r["gametype"].ToString());
                }

                // super
                if (curRoleID == 4)
                {
                    TemplateHandler.Assign(ref _sTable, "s4", NumberFormat.DecFormat(r["sPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "m4", NumberFormat.DecFormat(r["mPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "a4", NumberFormat.DecFormat(r["aPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "MinPT4", NumberFormat.DecFormat(r["MinPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "isAutoPT4", (CastAide.AsBoolean(r["isAutoPT"], false)) ? " checked='checked'" : string.Empty);
                    TemplateHandler.Assign(ref _sTable, "AutoPT4", NumberFormat.DecFormat(r["AutoPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "Com4", IBCUtils.FormatHandler.FormatNumber4(r["RollingRate"]));
                }

                // master
                if (curRoleID == 3)
                {
                    TemplateHandler.Assign(ref _sTable, "s3", NumberFormat.DecFormat(r["sPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "m3", NumberFormat.DecFormat(r["mPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "a3", NumberFormat.DecFormat(r["aPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "MinPT3", NumberFormat.DecFormat(r["MinPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "isAutoPT3", (CastAide.AsBoolean(r["isAutoPT"], false)) ? " checked='checked'" : string.Empty);
                    TemplateHandler.Assign(ref _sTable, "AutoPT3", NumberFormat.DecFormat(r["AutoPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "Com3", IBCUtils.FormatHandler.FormatNumber4(r["RollingRate"]));
                }

                // agent
                if (curRoleID == 2)
                {
                    TemplateHandler.Assign(ref _sTable, "s2", NumberFormat.DecFormat(r["sPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "m2", NumberFormat.DecFormat(r["mPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "a2", NumberFormat.DecFormat(r["aPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "MinPT2", NumberFormat.DecFormat(r["MinPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "isAutoPT2", (CastAide.AsBoolean(r["isAutoPT"], false)) ? " checked='checked'" : string.Empty);
                    TemplateHandler.Assign(ref _sTable, "AutoPT2", NumberFormat.DecFormat(r["AutoPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "Com2", IBCUtils.FormatHandler.FormatNumber4(r["RollingRate"]));
                }

                // member
                if (curRoleID == 1)
                {
                    TemplateHandler.Assign(ref _sTable, "s1", NumberFormat.DecFormat(r["sPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "m1", NumberFormat.DecFormat(r["mPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "a1", NumberFormat.DecFormat(r["aPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "MinPT1", NumberFormat.DecFormat(r["MinPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "isAutoPT1", (CastAide.AsBoolean(r["isAutoPT"], false)) ? " checked='checked'" : string.Empty);
                    TemplateHandler.Assign(ref _sTable, "AutoPT1", NumberFormat.DecFormat(r["AutoPT"], 2));
                    TemplateHandler.Assign(ref _sTable, "Com1", IBCUtils.FormatHandler.FormatNumber4(r["RollingRate"]));
                }

                if (roleID == curRoleID)
                    _mainTable.Append(_sTable);
            }
            TemplateHandler.ReplaceBlock(ref _s, "tbl", _mainTable.ToString());
            return _s;
        }

        #endregion IBingoViewPT Members
    }
}
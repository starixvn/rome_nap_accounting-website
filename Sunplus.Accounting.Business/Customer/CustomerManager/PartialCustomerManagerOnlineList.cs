using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using Fanex.Common;
using Fanex.CustomerSetting.Module.Keno.KenoSetting.Configurations;
using Fanex.CustomerSetting.Module.ProgressiveBonus.Configurations;
using Fanex.CustomerSetting.Module.ProgressiveBonus.Criteria;
using Fanex.CustomerSetting.Module.ProgressiveBonus.Repositories.Impl;
using Fanex.LiveCasino.CoreSetting.Services;
using Fanex.Module.NG2.Entities;
using Fanex.Module.NG2.Services;
using Fanex.Tarus.Security.Criteria;
using Fanex.Tarus.Security.Domain;
using Fanex.Tarus.Security.Services;
using IBCUtils.Objects;
using RemotingRef;
using Sunplus.Accounting.Business.Criteria;
using Sunplus.Accounting.Business.Customer.ConfigureDirectAccount;
using Sunplus.Accounting.Business.CustSetting;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.AdoNet;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Entities.Customer;
using Sunplus.Bingo.Core;
using Result = Sunplus.Accounting.Entities.Result;

namespace Sunplus.Accounting.Business.Customer
{
    public partial class CustomerManager
    {
        public Hashtable GenDownlineList(int custId, int roleId, int pageIndex)
        {
            Hashtable returndata = new Hashtable();
            string content = "";
            string tplDownlineList = WebUtils.LoadTemplate("CustomerInfo/DownlineList.htm");
            string tplRowData = TemplateHandler.CopyBlock(tplDownlineList, "rowdata");
            if (roleId <= IBCUtils.Objects.UserInfo.ROLE_MEMBER || roleId > IBCUtils.Objects.UserInfo.ROLE_SUPER)
            {
                content = "<div style='width:250px;text-align:center;'>No downline list</div>";
                returndata.Add("Content", content);
                returndata.Add("Totalpages", 1);
                return returndata;
            }
            DbResult lsDownline = SiteProvider.Customer().GetDownLineList(custId, 100, pageIndex);
            DataRowCollection rows = lsDownline.ResultSet.Tables[0].Rows;
            int totalDownline = lsDownline.ResultSet.Tables[0].Rows.Count;
            int TotalPages = (int)lsDownline.Outputs["totalpages"];

            if (totalDownline == 0)
            {
                content = "<div style='width:250px;text-align:center;'>No downline list</div>";
                returndata.Add("Content", content);
                returndata.Add("Totalpages", 1);
                return returndata;
            }
            content = "";
            for (int i = 0; i < totalDownline; i = i + 2)
            {
                DataRow row = rows[i];
                string rowHtml = tplRowData;
                TemplateHandler.Assign(ref rowHtml, "{user1}", HttpUtility.HtmlDecode(row["username"] + ""));
                if (i + 1 < totalDownline)
                {
                    row = rows[i + 1];
                    TemplateHandler.Assign(ref rowHtml, "{user2}", HttpUtility.HtmlDecode(row["username"] + ""));
                }
                else
                {
                    TemplateHandler.Assign(ref rowHtml, "{user2}", "&nbsp;");
                }

                content += rowHtml;
            }
            TemplateHandler.ReplaceBlock(ref tplDownlineList, "rowdata", content);
            string strpager = "";
            if (TotalPages > 1)
            {
                strpager = Pager.InitPager(TotalPages, pageIndex, "pageritemclick");
            }

            returndata.Add("Content", tplDownlineList);
            returndata.Add("Totalpages", TotalPages);
            returndata.Add("Pager", strpager);
            return returndata;
        }
    }
}

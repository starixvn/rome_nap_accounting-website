using System;
using System.Data;
using System.Web.UI.WebControls;
using DNAide.Web;
using DNAide.Core;
using System.Text;
using DNAide.Utils;
using System.Collections.Generic;

using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess;
using Sunplus.Core.LoggingLib;

public partial class MinMaxExRate : PageBase
{
    protected override bool HasPermission()
    {
        return Functions.ExchangeRate.IsAllowExchangeRate && base.HasPermission();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        bool isHis =string.IsNullOrEmpty(Request["ishis"]) ? false : CastAide.AsBoolean(Request["ishis"]);
        MinMaxExchangeRate.Text = Resources.Language.MinMaxExchangeRate + " History";
        if (isHis)
        {
            int frmid = CastAide.AsInt32(Request["frmid"]);
            int toid = CastAide.AsInt32(Request["toid"]);
            MinMaxExRateListContentHtml.Text = ListExRateMinMaxHis(frmid, toid);
        }
        else
        {
            MinMaxExchangeRate.Text = Resources.Language.MinMaxExchangeRate ;
            MinMaxExRateListContentHtml.Text = ListExRateMinMax();
        }       

    }
    private string ListExRateMinMax()
    {
        string ListMain = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("ExRate/MinMaxExRate.html");
        string ListContent = TemplateHandler.CopyBlock(ListMain, "limit-content");
        
        string CopyContent = "";
        string bgcolor_row = "";
        string themePath = WebSettings.ThemePath;
        int counter = 0;
        StringBuilder Content = new StringBuilder();
        Dictionary<int, string> exchange = IBCProvider.Utils.GetExchange();//AppCache.Exchange;
        try
        {
            using (IDataReader dr =  Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Balance.ExChangeRateSel())
            {
                while (dr.Read())
                {
                    counter++;
                    bgcolor_row = (bgcolor_row == "") ? "evenrow" : "";
                    CopyContent = ListContent;
                    TemplateHandler.Assign(ref CopyContent, "themepath", themePath);
                    TemplateHandler.Assign(ref CopyContent, "odd_even", bgcolor_row);
                    TemplateHandler.Assign(ref CopyContent, "No_Value", counter.ToString());
                    TemplateHandler.Assign(ref CopyContent, "FromCurrency_Value", CacheUtils.GetExchangeName(exchange, DNAide.Core.CastAide.AsInt32(dr["fromexchangeId"])));
                    TemplateHandler.Assign(ref CopyContent, "ToCurrency_Value", CacheUtils.GetExchangeName(exchange, CastAide.AsInt32(dr["toexchangeId"])));
                    TemplateHandler.Assign(ref CopyContent, "Min_Value", NumberFormat.DecFormat(dr["minactualrate"],2));
                    TemplateHandler.Assign(ref CopyContent, "Max_Value", NumberFormat.DecFormat(dr["maxactualrate"], 2));
                    TemplateHandler.Assign(ref CopyContent, "LastUpdate_Value", dr["lastupdate"]);
                    TemplateHandler.Assign(ref CopyContent, "Creator_Value", dr["creator"]);
                    TemplateHandler.Assign(ref CopyContent, "frmid", dr["fromexchangeid"]);
                    TemplateHandler.Assign(ref CopyContent, "toid", dr["toexchangeid"]);
                    Content.Append(CopyContent);
                }
            }
            TemplateHandler.Assign(ref ListMain, "themepath", themePath);
            if (counter == 0)
            {
                TemplateHandler.ReplaceBlock(ref ListMain, "limit-content", AccCommon.GenNoInformation("9"));
            }
            else
            {
                TemplateHandler.ReplaceBlock(ref ListMain, "limit-content", Content.ToString());
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }      
        return ListMain;
    }
    private string ListExRateMinMaxHis(int fromid, int toid)
    {
        string ListMain = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("ExRate/MinMaxExRateHis.html");
        string ListContent = TemplateHandler.CopyBlock(ListMain, "limit-content");

        string CopyContent = "";
        string bgcolor_row = "oddrow";
        string themePath = WebSettings.ThemePath;
        int counter = 0;
        StringBuilder Content = new StringBuilder();
        Dictionary<int, string> exchange = CacheUtils.Exchange;
        try
        {
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Balance.ExChangeRateSelHis(fromid, toid))
            {
                while (dr.Read())
                {
                    counter++;
                    bgcolor_row = (bgcolor_row == "oddrow") ? "evenrow" : "oddrow";
                    CopyContent = ListContent;
                    TemplateHandler.Assign(ref CopyContent, "themepath", themePath);
                    TemplateHandler.Assign(ref CopyContent, "odd_even", bgcolor_row);
                    TemplateHandler.Assign(ref CopyContent, "No_Value", counter.ToString());
                    TemplateHandler.Assign(ref CopyContent, "FromCurrency_Value", CacheUtils.GetExchangeName(exchange, DNAide.Core.CastAide.AsInt32(dr["fromexchangeId"])));
                    TemplateHandler.Assign(ref CopyContent, "ToCurrency_Value", CacheUtils.GetExchangeName(exchange, CastAide.AsInt32(dr["toexchangeId"])));
                    TemplateHandler.Assign(ref CopyContent, "Min_Value", NumberFormat.DecFormat(dr["minactualrate"], 2));
                    TemplateHandler.Assign(ref CopyContent, "Max_Value", NumberFormat.DecFormat(dr["maxactualrate"], 2));
                    TemplateHandler.Assign(ref CopyContent, "LastUpdate_Value", dr["actiondate"]);
                    TemplateHandler.Assign(ref CopyContent, "Creator_Value", dr["username"]);                                       
                    Content.Append(CopyContent);
                }
            }
            TemplateHandler.Assign(ref ListMain, "themepath", themePath);
            if (counter == 0)
            {
                TemplateHandler.ReplaceBlock(ref ListMain, "limit-content", AccCommon.GenNoInformation("7"));
            }
            else
            {
                TemplateHandler.ReplaceBlock(ref ListMain, "limit-content", Content.ToString());
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex); 
        }      
        return ListMain;
    }
}

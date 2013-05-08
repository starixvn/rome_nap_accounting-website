using System;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;


public partial class Customer_ViewPT : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.sTitle.Text = "View Position Taking";
            string _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/ViewPT.htm");                               
            StringBuilder _sSport = new StringBuilder();
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.GetPTSportType())
            {
                while (dr.Read())
                {
                    string _sportName = (string)dr["sportname"];
                    string _sportType = dr["sporttype"].ToString();

                    if (CastAide.AsInt16(dr["sporttype"])== 161 && Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled)
                        _sSport.Append("<option value='" + "161" + "'>" + dr["sportname"].ToString() + "</option>");
                    else if ((!WebSettings.disabledHR || CastAide.AsInt16(dr["sporttype"]) != 151) && CastAide.AsInt16(dr["sporttype"]) != 161)
                        _sSport.Append("<option value='" + dr["sporttype"].ToString() + "'>" + dr["sportname"].ToString() + "</option>");
                }

                if (!WebSettings.disableCS)
                    _sSport.Append("<option value='" + "251" + "'>" + "Casino" + "</option>");
                
                //Kiet Mai - edit sporttype 252, rename Mini Game to Bingo; add Number game, parlay mixed sport
                if (Sunplus.Bingo.Core.BingoSettings.BingoGame.Enabled)
                    _sSport.Append("<option value='" + "252" + "'>" + "Bingo" + "</option>");

                _sSport.Append("<option value='" + "999" + "'>" + Resources.Language.lblMixedSports + "</option>");               


                
            }
            TemplateHandler.Assign(ref _s, "{ListSport}", _sSport.ToString());
            StringBuilder _sBet = new StringBuilder();
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.GetPTBetType(1))
            {
                while (dr.Read())
                {
                    _sBet.Append("<option value='" + dr["bettype"].ToString() + "'>" + dr["typenamee"].ToString() + "</option>");
                }
            }            
            TemplateHandler.Assign(ref _s, "{ListBet}", _sBet.ToString());

            sContent.Text = _s;
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
    }
}

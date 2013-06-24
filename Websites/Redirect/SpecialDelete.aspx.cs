using System;
using Fanex.Security;
using Sunplus.Accounting.Core.Common;
using System.Configuration;
using Fanex.Tarus.SpecialDelete.Core.Configurations;
using System.Collections.Generic;
using System.Linq;


public partial class SpecialDelete_Redirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["accspecialdeletekey"] != null && !string.IsNullOrEmpty(Request["accspecialdeletekey"]))
        {
            var userName = Cryptography.CfsEnCrypt(AppSession.AccUserName.ToString());
            var onuserId = Cryptography.CfsEnCrypt(AppSession.OnAccId.ToString());

            var accUrlIndentify = Request["accspecialdeletekey"];
            var mappingUrlSection = MappingSpecialDeleteUrlSection.Instance.SpecialDeleteUrl;
            if (mappingUrlSection != null)
            {
                var getMappingConfig = (from m in
                                            mappingUrlSection.Cast<SpecialDeleteUrlElement>().Where(
                                                m => m.AccUrl.ToLower().Equals(accUrlIndentify.ToLower()) && m.Active)
                                        select m);
                if (getMappingConfig != null && getMappingConfig.Count() > 0)
                {
                    var napUrl = getMappingConfig.First().NapUrl;
                    var url = string.Format("{0}?username={1}&onuserid={2}&redirectUrl={3}", ConfigurationManager.AppSettings["NapSpecialDeleteLoginUrl"], userName, onuserId, napUrl);
                    Response.Redirect(url);
                }
            }
        }
        else
        {
            Response.Redirect("Index.aspx");
        }
    }
}
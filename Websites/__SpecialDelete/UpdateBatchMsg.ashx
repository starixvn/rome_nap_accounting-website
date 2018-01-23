<%@ WebHandler Language="C#" Class="UpdateBatchMsg" %>

using System;
using System.Web;
using DNAide.Core;


public class UpdateBatchMsg : BaseHandler{

    public override void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        string msgEn = CastAide.AsString(context.Request["txtEn"]);
        string msgCs = CastAide.AsString(context.Request["txtCs"]);
        string msgCh = CastAide.AsString(context.Request["txtCh"]);
        string msgTh = CastAide.AsString(context.Request["txtTh"]);
        string msgIt = CastAide.AsString(context.Request["txtIt"]);
        string msgJp = CastAide.AsString(context.Request["txtJp"]);
        string msgEs = CastAide.AsString(context.Request["txtEs"]);
        string msgKo = CastAide.AsString(context.Request["txtKo"]);
        
        string TransId = CastAide.AsString(context.Request["TransId"]);
        var pmsg = new Sunplus.Accounting.Entities.SpecialDelete.Message() { 
            Content_en = msgEn,
            Content_cs= msgCs,
            Content_ch= msgCh,
            Content_it = msgIt,
            Content_jp = msgJp,
            Content_th = msgTh,
            Content_es = msgEs,
            Content_ko = msgKo
        };
        
        TransId = TransId.Replace("\n", "");
        Sunplus.Accounting.Business.SpecialDelete.UpdateBatchMsg batch = new Sunplus.Accounting.Business.SpecialDelete.UpdateBatchMsg();
        var rs = batch.UploadMsg(pmsg, TransId);
        context.Response.Write(rs);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}

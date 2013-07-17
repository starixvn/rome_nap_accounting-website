<%@ WebHandler Language="C#" Class="UploadSMAListHandler" %>

using System;
using System.Web;
using Fanex.Tarus.Customer.Business.SettingCustomer.SMATurnover;
using Fanex.Tarus.Customer.Criteria.SMATurnover;
using DNAide.Core;
using System.Web.Mvc;
using Newtonsoft.Json;

public class UploadSMAListHandler : IHttpHandler
{
    /// <summary>
    /// Using Handler to upload file because IE not support upload Jquery File Upload yet.
    /// </summary>
    /// <param name="context"></param>
    [AcceptVerbs(HttpVerbs.Post)]
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        var smaService = new SMATurnoverService();
        var addResult_Str = string.Empty;
        var uploadedFile = HttpContext.Current.Request.Files["file_UploadFile"];
        var isValidParams = true;
        if (uploadedFile == null)
        {
            addResult_Str = "{" +
                "\"ErrorCode\":\"" + 1 + "\"" +
                ",\"ErrorMessage\":\"" + "Please choose file to upload!" + "\"" +
                ",\"ActionResult\":\"\"" +
                "}";
            isValidParams = false;
        }
        else
        {
            var beginExtension = uploadedFile.FileName.LastIndexOf(".");
            var fileExtension = beginExtension != -1 ?
                uploadedFile.FileName.Substring(beginExtension) :
                string.Empty;
            if (beginExtension == -1 ||
                (fileExtension != ".xls" &&
                fileExtension != ".xlsx"))
            {
                addResult_Str = "{" +
                    "\"ErrorCode\":\"" + 1 + "\"" +
                    ",\"ErrorMessage\":\"" + "File type is not support!" + "\"" +
                    ",\"ActionResult\":\"\"" +
                    "}";
                isValidParams = false;
            }
            else if (uploadedFile.ContentLength / (1024 * 1024) > 0)
            {
                addResult_Str = "{" +
                    "\"ErrorCode\":\"" + 1 + "\"" +
                    ",\"ErrorMessage\":\"" + "File Size limit 1MB!" + "\"" +
                    ",\"ActionResult\":\"\"" +
                    "}";
                isValidParams = false;
            }
        }

        if (isValidParams)
        {
            HttpPostedFileBase filebase =
                new HttpPostedFileWrapper(HttpContext.Current.Request.Files["file_UploadFile"]);
            if (filebase.ContentLength > 0)
            {
                var addResult = smaService.UploadSMAList(filebase, new UploadSMAListCriteria
                {
                    UserID = CastAide.AsInt32(context.Request.Form["AccId"]),
                    SportId = CastAide.AsString(context.Request.Form["SportId"]),
                    LeagueId = CastAide.AsString(context.Request.Form["LeagueId"]),
                    FromDate = CastAide.AsString(context.Request.Form["FromDate"]),
                    ToDate = CastAide.AsString(context.Request.Form["ToDate"]),
                    SportName = CastAide.AsString(context.Request.Form["SportName"]),
                    LeagueName = CastAide.AsString(context.Request.Form["LeagueName"]),
                    MatchIds = CastAide.AsString(context.Request.Form["MatchIds"])
                });

                if (addResult != null)
                {
                    addResult_Str = "{" +
                       "\"ErrorCode\":\"" + 0 + "\"" +
                       ",\"ErrorMessage\":\"\"" +
                       ",\"ActionResult\":\"" + JsonConvert.SerializeObject(addResult).Replace("\"", "\\\"") + "\"" +
                       "}";
                    isValidParams = true;
                }
            }
        }

        context.Response.Write(addResult_Str);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
<%@ WebHandler Language="C#" Class="UploadHandler" %>

using System;
using System.Web;
using Fanex.Tarus.Customer.Business.SettingCustomer.SMATurnover;
using Fanex.Tarus.Customer.Criteria.SMATurnover;
using DNAide.Core;
using System.Web.Mvc;
using Newtonsoft.Json;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Business.ConfigureDirectAccount;
using Fanex.Tarus.Customer.Resources;

public class UploadHandler : IHttpHandler
{
    /// <summary>
    /// Using Handler to upload file because IE not support upload Jquery File Upload yet.
    /// </summary>
    /// <param name="context"></param>
    [AcceptVerbs(HttpVerbs.Post)]
    public void ProcessRequest(HttpContext context)
    {
        var importResultString = string.Empty;
        try
        {
            context.Response.ContentType = "text/plain";
            var uploadedFile = HttpContext.Current.Request.Files["file-upload-control"];
            var isValidParams = true;
            if (uploadedFile == null || string.IsNullOrEmpty(uploadedFile.FileName))
            {
                importResultString = GetImportResultString(new Result(1, Customer_Labels.ConfigureDirectAccount_Import_ChooseFile));
                isValidParams = false;
            }

            if (isValidParams)
            {
                var beginExtension = uploadedFile.FileName.LastIndexOf(".");
                var fileExtension = beginExtension != -1 ?
                    uploadedFile.FileName.Substring(beginExtension) :
                    string.Empty;
                if (beginExtension == -1 ||
                    (fileExtension != ".xls" && fileExtension != ".xlsx"))
                {
                    importResultString = GetImportResultString(new Result(1, Customer_Labels.ConfigureDirectAccount_Import_InvalidExtension));
                    isValidParams = false;
                }


                if (isValidParams && uploadedFile.ContentLength / (1024 * 1024) > 0)
                {
                    importResultString = GetImportResultString(new Result(1, Customer_Labels.ConfigureDirectAccount_Import_ExceedFileSize));
                    isValidParams = false;
                }
            }

            if (isValidParams)
            {
                HttpPostedFileBase filebase = new HttpPostedFileWrapper(HttpContext.Current.Request.Files["file-upload-control"]);
                if (filebase.ContentLength > 0)
                {
                    var userId = CastAide.AsInt32(context.Request.Form["AccId"]);
                    var importResult = (new ConfigureDirectAccountService()).CheckImportDirectAccount(filebase, userId);
                    if (importResult != null)
                    {
                        if (importResult.ImportResultCode == 0)
                        {
                            importResultString = JsonConvert.SerializeObject(new
                            {
                                ErrorCode = 0,
                                ErrorMessage = string.Empty,
                                ActionResult = JsonConvert.SerializeObject(importResult)
                            });
                        }
                        else
                        {
                            importResultString = JsonConvert.SerializeObject(new
                            {
                                ErrorCode = importResult.ImportResultCode,
                                ErrorMessage = importResult.ImportResultMessage,
                                ActionResult = string.Empty
                            });
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            importResultString = GetImportResultString(new Result(1, Customer_Labels.Error_SystemError));
        }

        context.Response.Write(importResultString);
    }

    public string GetImportResultString(Result importResult)
    {
        return "{" +
                    "\"ErrorCode\":\"" + importResult.ErrorCode + "\"" +
                    ",\"ErrorMessage\":\"" + importResult.ErrorMessage + "\"" +
                    ",\"ActionResult\":\"\"" +
                "}";
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
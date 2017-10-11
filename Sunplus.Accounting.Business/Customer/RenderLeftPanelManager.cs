namespace Sunplus.Accounting.Business.Customer
{
    using System.Data;
    using Core.Configurations;
    using DataAccess;
    using DNAide.Web;
    using Entities;
    using Fanex.Common;
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.Tarus.Infrastructure.Extensions;
    using RemotingRef;

    /// <summary>
    /// Render Left Panel Manager.
    /// </summary>
    public static class RenderLeftPanelManager
    {
        public static void RenderEnableLink(ref string htmlcontent, RenderLeftPanelParameter renderLeftPanelParameter)
        {
            if (!renderLeftPanelParameter.IsShowEnableBetTrade)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "BetTrade");
            }

            if (!renderLeftPanelParameter.IsShowEnableCashOut)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableCashOut");
            }

            if (!renderLeftPanelParameter.IsShowEnableMicrogaming)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "Microgaming");
            }

            if (!renderLeftPanelParameter.IsShowAllBetForCredit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableAllbet");
            }

            if (!renderLeftPanelParameter.IsShowMacau)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "Macau");
            }

            if (renderLeftPanelParameter.IsHideCricket)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableCricket");
            }

            if (renderLeftPanelParameter.IsShowAllBetForCredit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "EnableAllbet");
            }
        }

        public static void AssignCustomerInfo(ref string htmlcontent, CustomerDetail customerDetail)
        {
            TemplateHandler.Assign(ref htmlcontent, "{CustId}", customerDetail.CustID);
            TemplateHandler.Assign(ref htmlcontent, "{RoleId}", customerDetail.RoleId);
            TemplateHandler.Assign(ref htmlcontent, "{userId}", customerDetail.AccId);
            TemplateHandler.Assign(ref htmlcontent, "{HasDownline}", customerDetail.HasDownline ? 1 : 0);
        }

            if (!renderLeftPanelParameter.IsShowKeno && !renderLeftPanelParameter.IsShowGoldDeluxeForCredit)
        public static void RenderEditMicroGamingLink(ref string htmlcontent, RenderLeftPanelParameter renderLeftPanelParameter)
        {
            if (!WebSettings.EnableMicrogaming || !renderLeftPanelParameter.IsDeposit)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-MicroGaming");
                return;
            }

            var editSettingLink = "MicroGaming/EditSettingView?" +
                "parentId={0}&custId={1}&roleId={2}";
            editSettingLink = string.Format(
                editSettingLink,
                renderLeftPanelParameter.ParentId,
                renderLeftPanelParameter.CustId,
                renderLeftPanelParameter.RoleId);
            TemplateHandler.Assign(ref htmlcontent, "{microgamingLinkSetting}", editSettingLink);
        }

        public static void RenderEnableProductBox(
            DataRow custInfoRow,
            SettingGroup settingGroup,
            ref string htmlcontent,
            bool isManageCustomerAccess,
            bool disabledProductParent,
            bool hasDownline,
            string productName,
            bool isSupport,
            RenderLeftPanelParameter renderLeftPanelParameter,
            string title)
        {
            if (isSupport)
            {
                var columnName = "Disable" + productName;
                bool isDisabled;
                try
                {
                    isDisabled = Converter.ToBool(custInfoRow[columnName], false);
                }
                catch
                {
                    isDisabled = false;
                }

                columnName = "DisableDownlines" + productName;
                bool disableDownlines;
                try
                {
                    disableDownlines = Converter.ToBool(custInfoRow[columnName], false);
                }
                catch
                {
                    disableDownlines = false;
                }

                var isCustomerEnabled = custInfoRow["Enabled"].ToString();
                settingGroup.Add(GetProductSettingControl(
                    renderLeftPanelParameter,
                    isManageCustomerAccess,
                    "disabled" + productName,
                    !isDisabled,
                    hasDownline,
                    true,
                    Converter.ToBool(isCustomerEnabled),
                    disabledProductParent,
                    disableDownlines,
                    productName,
                    title));

                TemplateHandler.Assign(ref htmlcontent, "{chk" + productName + "}", isDisabled ? 0 : 1);
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "{chk" + productName + "}");
            }
        }

        /// <summary>
        /// Enable Status - New Direction Flow.
        /// </summary>
        /// <param name="custInfoRow"></param>
        /// <param name="settingGroup"></param>
        /// <param name="htmlcontent"></param>
        /// <param name="isManageCustomerAccess"></param>
        /// <param name="disabledProductParent"></param>
        /// <param name="hasDownline"></param>
        /// <param name="productName"></param>
        /// <param name="isSupport"></param>
        /// <param name="renderLeftPanelParameter"></param>
        /// <param name="title"></param>
        /// <param name="productId"></param>
        public static void RenderEnableProductBox2(
        DataRow custInfoRow,
        SettingGroup settingGroup,
        ref string htmlcontent,
        bool isManageCustomerAccess,
        bool disabledProductParent,
        bool hasDownline,
        string productName,
        bool isSupport,
        RenderLeftPanelParameter renderLeftPanelParameter,
        string title,
        int productId)
        {
            if (isSupport)
            {
                var columnName = "Disable" + productName;
                bool isDisabled;
                try
                {
                    isDisabled = Converter.ToBool(custInfoRow[columnName], false);
                }
                catch
                {
                    isDisabled = false;
                }

                columnName = "DisableDownlines" + productName;
                bool disableDownlines;
                try
                {
                    disableDownlines = Converter.ToBool(custInfoRow[columnName], false);
                }
                catch
                {
                    disableDownlines = false;
                }

                var isCustomerEnabled = custInfoRow["Enabled"].ToString();
                settingGroup.Add(GetProductSettingControl2(
                    renderLeftPanelParameter,
                    isManageCustomerAccess,
                    "disabled" + productName,
                    !isDisabled,
                    true,
                    Converter.ToBool(isCustomerEnabled),
                    disabledProductParent,
                    disableDownlines,
                    productName,
                    title,
                    productId));

                TemplateHandler.Assign(ref htmlcontent, "{chk" + productName + "}", isDisabled ? 0 : 1);
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "{chk" + productName + "}");
            }
        }

        public static void RenderEnableProductBoxDCS(
            DataRow custInfoRow,
            DataRow productInfo,
            SettingGroup settingGroup,
            ref string htmlcontent,
            bool isManageCustomerAccess,
            bool disabledProductParent,
            bool hasDownline,
            string productName,
            bool isSupport,
            RenderLeftPanelParameter renderLeftPanelParameter,
            string title,
            int productId = 0,
            int settingType = 0)
        {
            if (isSupport)
            {
                bool isDisabled = Utils.ToBoolean(productInfo["IsDisabled"]);
                bool disableDownlines = Utils.ToBoolean(productInfo["IsDisabledDownline"]);
                bool disableUplines = Utils.ToBoolean(productInfo["IsDisabledUpline"]);
                var isCustomerEnabled = custInfoRow["Enabled"].ToString();
                settingGroup.Add(GetProductSettingControl(
                    renderLeftPanelParameter,
                    isManageCustomerAccess,
                    "disabled" + productName,
                    !isDisabled,
                    hasDownline,
                    true,
                    Converter.ToBool(isCustomerEnabled),
                    disableUplines,
                    disableDownlines,
                    productName,
                    title,
                    productId,
                    settingType));

                TemplateHandler.Assign(ref htmlcontent, "{chk" + productName + "}", isDisabled ? 0 : 1);
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "{chk" + productName + "}");
            }
        }

        private static SettingControl GetProductSettingControl(
            RenderLeftPanelParameter renderLeftPanelParameter,
            bool isManageCustomerAccess,
            string statusName,
            bool currStatus,
            bool hasDownLine,
            bool isAllowEdit,
            bool isEnabled,
            bool disabledUpline,
            bool disableDownLine,
            string productName,
            string title,
            int productId = 0,
            int settingType = 0)
        {
            string classname = string.Empty;
            string jsonclick = string.Empty;
            string jsonmouseover = string.Empty;
            string jsonmouseout = string.Empty;
            if (isAllowEdit && isManageCustomerAccess)
            {
                var canChange = isEnabled && (4 == renderLeftPanelParameter.RoleId || !disabledUpline);
                classname = canChange ? "canchange" : "cannotchange";

                var parameters = new string[]
                {
                    renderLeftPanelParameter.CustId.ToString(),
                    "'" + renderLeftPanelParameter.Username + "'",
                    renderLeftPanelParameter.RoleId.ToString(),
                    hasDownLine ? "1" : "0",
                    renderLeftPanelParameter.AccId.ToString(),
                    currStatus ? "1" : "0",
                    disabledUpline ? "0" : "1",
                    disableDownLine ? "0" : "1",
                    "'" + title + "'",
                    "'" + productName + "'",
                    productId.ToString(),
                    settingType.ToString()
                };

                jsonclick = canChange ?
                    string.Format("return leftPanelManager.updateStatus({0});", string.Join(",", parameters)) :
                    string.Empty;
                jsonmouseout = canChange ? string.Empty : "UnTip()";
                jsonmouseover = canChange ? string.Empty :
                     string.Format(
                        "Tip('{0}', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)",
                        productId == 23 ? isEnabled ? RefData.GetResourceDataByRefIdAndResourceId("Product", "23", "en-US", "NG2") + " status has been disabled at upline" : ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") :
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                        isEnabled ? "lblDisabled" + productName + "Upline" : "lblDisabledAccount"));
            }

            return
                new SettingControl(
                    productId == 23 ? "Enable " + RefData.GetResourceDataByRefIdAndResourceId("Product", "23", "en-US", "NG2") :
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "enable" + productName),
                    isAllowEdit ? currStatus : false,
                    !isAllowEdit,
                    jsonclick,
                    jsonmouseout,
                    jsonmouseover,
                    classname,
                    statusName,
                    renderLeftPanelParameter.RoleId,
                    disableDownLine,
                    productName);
        }

        public static void RenderEnableProductBoxDCS2(
            DataRow custInfoRow,
            DataRow productInfo,
            SettingGroup settingGroup,
            ref string htmlcontent,
            bool isManageCustomerAccess,
            string productName,
            bool isSupport,
            RenderLeftPanelParameter renderLeftPanelParameter,
            string title,
            int productId = 0)
        {
            if (isSupport)
            {
                bool isDisabled = Utils.ToBoolean(productInfo["IsDisabled"]);
                bool disableDownlines = Utils.ToBoolean(productInfo["IsDisabledDownline"]);
                bool disableUplines = Utils.ToBoolean(productInfo["IsDisabledUpline"]);
                var isCustomerEnabled = custInfoRow["Enabled"].ToString();
                settingGroup.Add(GetProductSettingControl2(
                    renderLeftPanelParameter,
                    isManageCustomerAccess,
                    "disabled" + productName,
                    !isDisabled,
                    true,
                    Converter.ToBool(isCustomerEnabled),
                    disableUplines,
                    disableDownlines,
                    productName,
                    title,
                    productId));

                TemplateHandler.Assign(ref htmlcontent, "{chk" + productName + "}", isDisabled ? 0 : 1);
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "{chk" + productName + "}");
            }
        }

        public static void RenderOldProductEnableBoxLikeDCS(
            DataRow custInfoRow,
            SettingGroup settingGroup,
            ref string htmlcontent,
            bool isManageCustomerAccess,
            bool disabledProductParent,
            bool hasDownline,
            string productName,
            bool isSupport,
            RenderLeftPanelParameter renderLeftPanelParameter,
            string title,
            int productId = 0)
        {
            if (isSupport)
            {
                bool isDisabled = Utils.ToBoolean(custInfoRow["Disable" + productName]);
                bool disableDownlines = Utils.ToBoolean(custInfoRow["DisableDownlines" + productName]);
                bool disableUplines = Utils.ToBoolean(custInfoRow["DisabledUpline" + productName]);
                var isCustomerEnabled = custInfoRow["Enabled"].ToString();
                settingGroup.Add(GetProductSettingControl2(
                    renderLeftPanelParameter,
                    isManageCustomerAccess,
                    "disabled" + productName,
                    !isDisabled,
                    true,
                    Converter.ToBool(isCustomerEnabled),
                    disableUplines,
                    disableDownlines,
                    productName,
                    title,
                    productId));

                TemplateHandler.Assign(ref htmlcontent, "{chk" + productName + "}", isDisabled ? 0 : 1);
            }
            else
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "{chk" + productName + "}");
            }
        }

        private static SettingControl GetProductSettingControl2(
           RenderLeftPanelParameter renderLeftPanelParameter,
           bool isManageCustomerAccess,
           string statusName,
           bool currStatus,
           bool isAllowEdit,
           bool isEnabled,
           bool disabledUpline,
           bool disableDownLine,
           string productName,
           string title,
           int productId = 0)
        {
            string classname = string.Empty;
            string jsonclick = string.Empty;
            string jsonmouseover = string.Empty;
            string jsonmouseout = string.Empty;
            if (isAllowEdit && isManageCustomerAccess)
            {
                var canChange = isEnabled && (4 == renderLeftPanelParameter.RoleId || !disabledUpline);
                classname = canChange ? "canchange" : "cannotchange";
                jsonclick = canChange ?
                    string.Format("return leftPanelManager.updateThirdPartyProductStatus({0}, '{1}', {2}, {3}, '{4}', {5});", renderLeftPanelParameter.CustId, renderLeftPanelParameter.Username, renderLeftPanelParameter.RoleId, productId, title, renderLeftPanelParameter.IsDeposit.ToString().ToLower()) :
                    string.Empty;
                jsonmouseout = canChange ? string.Empty : "UnTip()";
                jsonmouseover = canChange ? string.Empty :
                     string.Format(
                        "Tip('{0}', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)",
                        productId == 23 ? isEnabled ? RefData.GetResourceDataByRefIdAndResourceId("Product", "23", "en-US", "NG2") + " status has been disabled at upline" : ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") :
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                        isEnabled ? "lblDisabled" + productName + "Upline" : "lblDisabledAccount"));
            }

            return
                new SettingControl(
                    productId == 23 ? "Enable " + RefData.GetResourceDataByRefIdAndResourceId("Product", "23", "en-US", "NG2") :
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "enable" + productName),
                    isAllowEdit ? currStatus : false,
                    !isAllowEdit,
                    jsonclick,
                    jsonmouseout,
                    jsonmouseover,
                    classname,
                    statusName,
                    renderLeftPanelParameter.RoleId,
                    disableDownLine,
                    productName);
        }
    }
}

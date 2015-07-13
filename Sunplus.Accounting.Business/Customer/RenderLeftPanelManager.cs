// --------------------------------------------------------------------------
// <copyright file="BetListAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Sunplus.Accounting.Business.Customer
{
    using DNAide.Web;
    using Sunplus.Accounting.Entities;
    using Fanex.Tarus.Infrastructure.Constants;
    using Sunplus.Accounting.Core.Configurations;

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

            if (!renderLeftPanelParameter.IsShowEnableMicrogaming)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "Microgaming");
            }
        }

        public static void AssignCustomerInfo(ref string htmlcontent, CustomerDetail customerDetail)
        {
            TemplateHandler.Assign(ref htmlcontent, "{CustId}", customerDetail.CustID);
            TemplateHandler.Assign(ref htmlcontent, "{RoleId}", customerDetail.RoleId);
            TemplateHandler.Assign(ref htmlcontent, "{userId}", customerDetail.AccId);
            TemplateHandler.Assign(ref htmlcontent, "{HasDownline}", customerDetail.HasDownline ? 1 : 0);
        }

        public static void RemoveEmptyBlock(ref string htmlcontent, RenderLeftPanelParameter renderLeftPanelParameter)
        {
            if (!renderLeftPanelParameter.IsShowKeno && !renderLeftPanelParameter.IsShowGoldDeluxe)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "Row_Keno_GoldDeluxe");
            }

            if (!renderLeftPanelParameter.IsShowAutoSuspend && !renderLeftPanelParameter.IsShowEnableBetTrade)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "Row_AutoSuspend_BetTrade");
            }

            if (!renderLeftPanelParameter.IsShowEnableMicrogaming)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "Row_Microgaming");
            }
        }

        public static void RenderEditMicroGamingLink(ref string htmlcontent, RenderLeftPanelParameter renderLeftPanelParameter)
        {
            if (!WebSettings.EnableMicrogaming)
            {
                TemplateHandler.RemoveBlock(ref htmlcontent, "edit-MicroGaming");
                return;
            }

            var editSettingLink = "MicroGaming/EditSetting?" +
                "parentId={0}&custId={1}&roleId={2}";
            editSettingLink = string.Format(
                editSettingLink,
                renderLeftPanelParameter.ParentId,
                renderLeftPanelParameter.CustId,
                renderLeftPanelParameter.RoleId);
            TemplateHandler.Assign(ref htmlcontent, "{microgamingLinkSetting}", editSettingLink);
        }
    }
}

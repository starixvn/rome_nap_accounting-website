namespace Sunplus.Accounting.Business.Customer
{
    using Core.Common;
    using Core.Configurations;
    using Core.Extensions;
    using Core.Web;
    using DNAide.Web;
    using Entities;
    using Security;
    using System;

    public partial class CustomerManager
    {
        private string StatusSpan(
            bool isManageCustomerAccess,
            string name,
            bool currStatus,
            bool hasDownLine)
        {

            string template = WebUtils.LoadTemplate("specialstatus.htm");
            TemplateHandler.Assign(ref template, "{name}", name);
            TemplateHandler.Assign(ref template, "{onmouseout}", string.Empty);
            TemplateHandler.Assign(ref template, "{onmouseover}", string.Empty);
            if (isManageCustomerAccess)
            {
                TemplateHandler.Assign(ref template, "{classname}", (currStatus) ? "chkchecked" : "chkuncheck");
                TemplateHandler.Assign(ref template, "{onclick}", "return changeStatus('" + name + "'," + ((hasDownLine) ? "true" : "false") + ");");
            }
            else
            {
                TemplateHandler.Assign(ref template, "{classname}", (currStatus) ? "chkchecked" : "chkuncheck");
                TemplateHandler.Assign(ref template, "{onclick}", string.Empty);
                TemplateHandler.Assign(ref template, "{onmouseout}", string.Empty);
                TemplateHandler.Assign(ref template, "{onmouseover}", string.Empty);
            }
            return template;
        }

        private string GetClosedStatus(
            bool isManageCustomerAccess,
            string name,
            bool currStatus,
            bool hasDownLine,
            bool uplineclosed,
            bool hasClearCreditPermission)
        {
            string template = WebUtils.LoadTemplate("SettingClosed.htm");
            TemplateHandler.Assign(ref template, "{name}", name);

            if (isManageCustomerAccess)
            {
                if (!uplineclosed)
                {
                    TemplateHandler.Assign(ref template, "{onmouseout}", string.Empty);
                    TemplateHandler.Assign(ref template, "{onmouseover}", string.Empty);
                    TemplateHandler.Assign(ref template, "{classname}", (currStatus) ? "chkchecked" : "chkuncheck");
                    TemplateHandler.Assign(ref template, "{onclick}", "return changeStatus('" + name + "'," + ((hasDownLine) ? "true" : "false") + ");");

                    //Check Clear Credit
                    if (!hasClearCreditPermission)
                    {
                        TemplateHandler.RemoveBlock(ref template, "ClearCredit");
                        TemplateHandler.Assign(ref template, "{isallowupdatecredit}", "0");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref template, "{btonclick}", "return updateCloseStatus('" + name + "'," + ((hasDownLine) ? "true" : "false") + ");");
                        TemplateHandler.Assign(ref template, "{UserName}", GetCustName(AppSession.CustRoleId));
                        TemplateHandler.Assign(ref template, "{isallowupdatecredit}", "1");
                    }
                }
                else
                {
                    TemplateHandler.Assign(ref template, "{classname}", (currStatus) ? "chkchecked" : "chkuncheck");
                    TemplateHandler.Assign(ref template, "{onclick}", string.Empty);
                    TemplateHandler.Assign(ref template, "{onmouseout}", "UnTip()");
                    TemplateHandler.Assign(ref template, "{onmouseover}", "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblUplineClosed") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)");
                }
            }
            else
            {
                TemplateHandler.Assign(ref template, "{classname}", (currStatus) ? "chkchecked" : "chkuncheck");
                TemplateHandler.Assign(ref template, "{onclick}", string.Empty);
                TemplateHandler.Assign(ref template, "{onmouseout}", string.Empty);
                TemplateHandler.Assign(ref template, "{onmouseover}", string.Empty);
            }
            return template;
        }

        private SettingControl GetSettingControlSelectedBetting(
            int roleID,
            bool parentstatus,
            bool isManageCustomerAccess,
            string statusName,
            bool currStatus)
        {
            string classname = string.Empty;
            string jsonclick = string.Empty;
            string jsonmouseover = string.Empty;
            string jsonmouseout = string.Empty;

            if (isManageCustomerAccess)
            {
                if (!parentstatus)
                {
                    classname = "canchange";
                    jsonclick = "return ShowDisablePop(this);";
                }
                else
                {
                    classname = "cannotchange";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSelectedBettingUL") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                }
            }

            return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSelectedBetting"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, false, statusName);
        }

        private string GetSettingControlDangerous(
            int roleId,
            bool isManageCustomerAccess,
            string[] dangeroustype,
            int[] levels,
            bool isBA)
        {
            try
            {
                DangerousSetting danger = new DangerousSetting();
                return danger.GenDangerous(roleId, isManageCustomerAccess, dangeroustype, levels, isBA);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        private SettingControl GetSettingControlSuspended(
            bool isManageCustomerAccess,
            string name,
            bool currStatus,
            bool hasDownLine,
            bool UpLineStatus)
        {
            string classname = string.Empty;
            string jsonclick = string.Empty;
            string jsonmouseover = string.Empty;
            string jsonmouseout = string.Empty;

            if (isManageCustomerAccess)
            {
                if (!UpLineStatus)
                {
                    classname = (currStatus) ? "chkchecked" : "chkuncheck";
                    jsonclick = "return changeStatus('" + name + "'," + ((hasDownLine) ? "true" : "false") + ");";
                }
                else
                {
                    classname = (currStatus) ? "chkchecked" : "chkuncheck";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSuspendedUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                }
            }
            else
            {
                classname = (currStatus) ? "chkchecked" : "chkuncheck";
            }

            return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSuspended"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, name, 0, false, string.Empty);
        }

        private SettingControl GetSettingControlNumberGame(
            int roleID,
            bool parentstatus,
            bool isManageCustomerAccess,
            string statusName,
            bool currStatus,
            bool isdisableNumberGameDL,
            bool isenabled,
            bool isDeposit)
        {

            string classname = string.Empty;
            string jsonclick = string.Empty;
            string jsonmouseover = string.Empty;
            string jsonmouseout = string.Empty;

            if (isManageCustomerAccess)
            {
                if (isenabled)
                {
                    if (4 == roleID || !parentstatus)
                    {
                        classname = "canchange";
                        jsonclick = isDeposit
                                ? "return ShowDisablePop(this);"
                                : "return leftPanelManager.viewProductStatusSetting({CustId}, '{Username}', {RoleId}, 3, 'Update Number Game Status');";
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledNumberGameUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }
                else
                {
                    classname = "cannotchange";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                }
            }
            return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledNumberGame"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, isdisableNumberGameDL, "NumberGame");

        }

        private SettingControl GetSettingControlCasino(
            int roleID,
            bool isManageCustomerAccess,
            string statusName,
            bool currStatus,
            bool syncCasino,
            bool isAllowEdit,
            bool isenabled,
            bool disabledCSParent)
        {
            if (WebSettings.disableCS || !isAllowEdit || !syncCasino)
            {
                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblEnabledCasino"), false, true, string.Empty, string.Empty, string.Empty, string.Empty, statusName, roleID, false, "CS");
            }
            else
            {
                string classname = string.Empty;
                string jsonclick = string.Empty;
                string jsonmouseover = string.Empty;
                string jsonmouseout = string.Empty;

                if (isManageCustomerAccess)
                {
                    if (isenabled)
                    {
                        if (4 == roleID || !disabledCSParent)
                        {
                            classname = "canchange";
                            jsonclick = "return leftPanelManager.viewProductStatusSetting({CustId}, '{Username}', {RoleId}, 6, 'Update Casino Status');";
                        }
                        else
                        {
                            classname = "cannotchange";
                            jsonmouseout = "UnTip()";
                            jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledCSUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                        }
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }

                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblEnabledCasino"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, false, "CS");
            }
        }

        private SettingControl GetSettingControlOutright(
            int roleID,
            bool parentstatus,
            bool isManageCustomerAccess,
            string statusName,
            bool currStatus,
            bool hasDownLine,
            bool isenalbed)
        {
            string classname = string.Empty;
            string jsonclick = string.Empty;
            string jsonmouseover = string.Empty;
            string jsonmouseout = string.Empty;
            if (isManageCustomerAccess)
            {
                if (isenalbed)
                {
                    if (4 == roleID || !parentstatus)
                    {
                        classname = "canchange";
                        jsonclick = "return changeStatus('" + "Outright" + "'," + ((hasDownLine) ? "true" : "false") + ");";
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledORUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }
                else
                {
                    classname = "cannotchange";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                }
            }
            return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAllowOutright"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, false, "OR");
        }

        private SettingControl GetSettingControlDoubleComm(
            int roleID,
            bool parentstatus,
            bool isManageCustomerAccess,
            string statusName,
            bool currStatus,
            bool hasDownLine,
            bool isenalbed)
        {
            string classname = string.Empty;
            string jsonclick = string.Empty;
            string jsonmouseover = string.Empty;
            string jsonmouseout = string.Empty;
            if (isManageCustomerAccess)
            {
                if (isenalbed)
                {
                    if (4 == roleID || parentstatus)
                    {
                        classname = "canchange";
                        jsonclick = "return updateDoubleCommLosing(" + ((hasDownLine) ? "true" : "false") + ");";
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledDoubleCommUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }
                else
                {
                    classname = "cannotchange";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                }
            }
            return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAllowDoubleComm"), currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, false, "DoubleComm");
        }

        private SettingControl GetSettingControlEnableAutoMaster(
            int roleID,
            bool isManageCustomerAccess,
            string statusName,
            bool currStatus,
            bool isenabled)
        {
            if (!Permissions.IsEnabledAutoMaster(true) || roleID != 3)
            {
                return new SettingControl(statusName, false, true, string.Empty, string.Empty, string.Empty, string.Empty, statusName, roleID, false, "AM");
            }
            else
            {
                string classname = string.Empty;
                string jsonclick = string.Empty;
                string jsonmouseover = string.Empty;
                string jsonmouseout = string.Empty;

                if (isManageCustomerAccess)
                {
                    if (isenabled)
                    {
                        classname = "canchange";
                        jsonclick = "return changeStatus('autompt',false)";
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }

                return new SettingControl(statusName, !currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, false, "AM");
            }
        }

        private SettingControl GetSettingControlInternalUser(
            int roleID,
            bool parentIsInternal,
            bool isManageCustomerAccess,
            string statusName,
            bool isInternalUser)
        {
            if (WebSettings.DisableInternal)
            {
                return new SettingControl("Internal", false, true, string.Empty, string.Empty, string.Empty, string.Empty, statusName, roleID, false, "INTERNALUSER");
            }
            else
            {
                string classname = string.Empty;
                string jsonclick = string.Empty;
                string jsonmouseover = string.Empty;
                string jsonmouseout = string.Empty;
                if (isManageCustomerAccess)
                {
                    if (roleID == 1)
                    {
                        classname = "cannotchangeinternal";
                        jsonmouseout = string.Empty;
                        jsonmouseover = string.Empty;
                    }
                    else
                    {
                        if (4 == roleID || !parentIsInternal)
                        {
                            classname = "canchange";
                            jsonclick = "return changeStatus('" + "Internal" + "', true);";
                        }
                        else
                        {
                            classname = "cannotchange";
                            jsonmouseout = "UnTip()";
                            jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblUplineIsInternalUser") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                        }
                    }
                }

                return new SettingControl(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "userInternal"), isInternalUser, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleID, false, "INTERNALUSER");
            }
        }

        private SettingControl GetSettingControlAllDayBalTransfer(
            int roleId,
            bool isManageCustomerAccess,
            string statusName, bool
            currentStatus)
        {
            string classname = string.Empty;
            string jsonclick = string.Empty;
            string jsonmouseover = string.Empty;
            string jsonmouseout = string.Empty;

            if (isManageCustomerAccess)
            {
                if (roleId == 4)
                {
                    classname = "canchange";
                    jsonclick = "return ChangeAllDayBalTransfer(this);";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = "Tip('Check to allow All-Day Balance Transfer mode', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                }
                else
                {
                    classname = "cannotchange";
                }
            }

            return new SettingControl("All-Day Bal Transfer", currentStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleId, false, statusName);
        }

        private SettingControl GetSettingControlEsports(
            int roleId,
            bool isManageCustomerAccess,
            string statusName,
            bool currStatus,
            bool isenabled,
            bool disabledUpline,
            bool disableDownLine)
        {
            if (!WebSettings.EnableVirtualSports)
            {
                return
                    new SettingControl(
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "disableEsports"),
                        false, true, string.Empty, string.Empty, string.Empty, string.Empty, statusName, roleId, disableDownLine, "ES");
            }
            else
            {
                string classname = string.Empty;
                string jsonclick = string.Empty;
                string jsonmouseover = string.Empty;
                string jsonmouseout = string.Empty;

                if (isManageCustomerAccess)
                {
                    if (isenabled)
                    {
                        if (4 == roleId || !disabledUpline)
                        {
                            classname = "canchange";
                            jsonclick = "return ShowDisablePop(this);";
                        }
                        else
                        {
                            classname = "cannotchange";
                            jsonmouseout = "UnTip()";
                            jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledESUpline") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                        }
                    }
                    else
                    {
                        classname = "cannotchange";
                        jsonmouseout = "UnTip()";
                        jsonmouseover = "Tip('" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDisabledAccount") + "', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)";
                    }
                }
                return
                    new SettingControl(
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblEsports"),
                        currStatus, false, jsonclick, jsonmouseout, jsonmouseover, classname, statusName, roleId,
                        disableDownLine, "ES");
            }
        }
    }
}

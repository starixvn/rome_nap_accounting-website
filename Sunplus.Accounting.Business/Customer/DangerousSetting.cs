using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;
using DNAide.Web;
using Sunplus.Accounting.Core.Web;

/// <summary>
/// Summary description for DangerousSetting
/// </summary>
namespace Sunplus.Accounting.Business.Customer
{
    public class DangerousSetting
    {
        private IList<DangerousDetail> dangerSettingOriginal = new List<DangerousDetail>();
        private IList<DangerousDetail> dangerSettingABT = new List<DangerousDetail>();
        private IList<DangerousDetail> dangerSettingOtherSports = new List<DangerousDetail>();
        private Hashtable idxDangerSettingOriginal = new Hashtable();
        private Hashtable idxDangerSettingABT = new Hashtable();
        private Hashtable idxDangerSettingOtherSports = new Hashtable();

        public DangerousSetting()
        {
            GetDangerSettingOriginal();
            GetDangerSettingABT();
            GetDangerSettingOtherSports();
        }

        private void GetDangerSettingOriginal()
        {
            try
            {
                string configFileName = HttpContext.Current.Server.MapPath("~/App_Data/DangerousSettingOriginal.xml");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configFileName);
                XmlNodeList levelList = xmlDoc.GetElementsByTagName("level");
                int i = 0;
                foreach (XmlNode level in levelList)
                {
                    try
                    {
                        idxDangerSettingOriginal.Add(level.Attributes["id"].Value, i++);
                        dangerSettingOriginal.Add(
                            new DangerousDetail(Convert.ToInt32(level.Attributes["id"].Value),
                                                 level.Attributes["name"].Value, level.Attributes["color"].Value));
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        private void GetDangerSettingABT()
        {
            try
            {
                string configFileName = HttpContext.Current.Server.MapPath("~/App_Data/DangerousSettingABT.xml");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configFileName);
                XmlNodeList levelList = xmlDoc.GetElementsByTagName("level");
                int i = 0;
                foreach (XmlNode level in levelList)
                {
                    try
                    {
                        idxDangerSettingABT.Add(level.Attributes["id"].Value, i++);
                        dangerSettingABT.Add(
                            new DangerousDetail(Convert.ToInt32(level.Attributes["id"].Value),
                                                 level.Attributes["name"].Value, level.Attributes["color"].Value));
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        private void GetDangerSettingOtherSports()
        {
            try
            {
                string configFileName = HttpContext.Current.Server.MapPath("~/App_Data/DangerousSettingOtherSports.xml");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configFileName);
                XmlNodeList levelList = xmlDoc.GetElementsByTagName("level");
                int i = 0;
                foreach (XmlNode level in levelList)
                {
                    try
                    {
                        idxDangerSettingOtherSports.Add(level.Attributes["id"].Value, i++);
                        dangerSettingOtherSports.Add(
                            new DangerousDetail(Convert.ToInt32(level.Attributes["id"].Value),
                                                 level.Attributes["name"].Value, level.Attributes["color"].Value));
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public string GetDangerousLevelName(int level, string dangeruostype, bool isBA)
        {
            switch (dangeruostype)
            {
                case "1":
                case "Original":
                    if ((level == 0 && isBA) || level == 100)
                    {
                        return "BA";
                    }
                    else
                        return dangerSettingOriginal[Convert.ToInt32(idxDangerSettingOriginal[Convert.ToString(level)])].name;
                case "2":
                case "ABT":
                    return dangerSettingABT[Convert.ToInt32(idxDangerSettingABT[Convert.ToString(level)])].name;
                case "3":
                case "OtherSports":
                    return dangerSettingOtherSports[Convert.ToInt32(idxDangerSettingOtherSports[Convert.ToString(level)])].name;
                default:
                    return string.Empty;
            }
        }

        public string GetDangerousLevelColor(int level, string dangeruostype, bool isBA)
        {
            switch (dangeruostype)
            {
                case "1":
                case "Original":
                    if ((level == 0 && isBA) || level == 100)
                    {
                        return "White";
                    }
                    else
                        return dangerSettingOriginal[Convert.ToInt32(idxDangerSettingOriginal[Convert.ToString(level)])].color;
                case "2":
                case "ABT":
                    return dangerSettingABT[Convert.ToInt32(idxDangerSettingABT[Convert.ToString(level)])].color;
                case "3":
                case "OtherSports":
                    return dangerSettingOtherSports[Convert.ToInt32(idxDangerSettingOtherSports[Convert.ToString(level)])].color;
                default:
                    return "white";
            }
        }

        public string GenDangerous(int roleId, bool isManageCustomerAccess, string[] names, int[] levels, bool isBA)
        {
            try
            {
                string dangerous = "";
                string s_ = "";
                string s = WebUtils.LoadTemplate("DangerousCust.htm");
                for (int i = 0; i < names.Length; i++)
                {
                    s_ = (string)s.Clone();
                    TemplateHandler.Assign(ref s_, "{name}", names[i].Replace(" ", ""));
                    TemplateHandler.Assign(ref s_, "{type}", names[i]);
                    TemplateHandler.Assign(ref s_, "{value}", GetDangerousLevelName(levels[i], names[i].Replace(" ", ""), isBA));
                    TemplateHandler.Assign(ref s_, "{dangerColor}", GetDangerousLevelColor(levels[i], names[i].Replace(" ", ""), isBA));
                    TemplateHandler.Assign(ref s_, "{currentLevel}", levels[i].ToString());
                    //Phuong.Nguyen
                    TemplateHandler.Assign(ref s_, "{onmouseout}", string.Empty);
                    TemplateHandler.Assign(ref s_, "{onmouseover}", string.Empty);
                    //End
                    if (isManageCustomerAccess && roleId < 3)
                    {
                        TemplateHandler.Assign(ref s_, "{onclick}", "OpenCloseMenuDangerous(this,'divLevel" + names[i].Replace(" ", "") + "'," + levels[i].ToString() + ");");
                        string levelItem = TemplateHandler.CopyBlock(s_, "LevelItem");
                        TemplateHandler.RemoveBlock(ref s_, "LevelItem");
                        StringBuilder content = new StringBuilder();
                        switch (names[i].Replace(" ", ""))
                        {
                            case "Original":
                                foreach (DangerousDetail st in dangerSettingOriginal)
                                {
                                    string row = Convert.ToString(levelItem.Clone());
                                    TemplateHandler.Assign(ref row, "{level_color}", st.color);
                                    TemplateHandler.Assign(ref row, "{LevelName}", st.name);
                                    TemplateHandler.Assign(ref row, "{LevelID}", st.id);
                                    //TemplateHandler.Assign(ref row, "{LevelName}", st.name);
                                    // TemplateHandler.Assign(ref row, "{}", st.color);
                                    // set popup downline for agent(roleid = 2)););
                                    TemplateHandler.Assign(ref row, "{type}", names[i]);
                                    var dangerousTpe = GetDanerousType(names[i]);
                                    TemplateHandler.Assign(ref row, "{dangerousType}", dangerousTpe);
                                    if (roleId == 1)
                                        TemplateHandler.Assign(ref row, "{level_onclick}", "return ChangeDangerousLevel('" + names[i] + "'," + st.id + ",'" + st.name + "','" + st.color + "');");
                                    else
                                    {
                                        //TemplateHandler.Assign(ref row, "{level_onclick}", string.Format("return ChangeDangerousLevelAgent('{0}','{1}')", dangerousTpe, st.name));
                                        TemplateHandler.Assign(ref row, "{level_onclick}", "return ChangeDangerousLevelAgent('" + names[i] + "'," + dangerousTpe + ",'" + st.id + "');");
                                    }
                                    content.Append(row);
                                }
                                break;

                            case "ABT":
                                foreach (DangerousDetail st in dangerSettingABT)
                                {
                                    string row = Convert.ToString(levelItem.Clone());
                                    TemplateHandler.Assign(ref row, "{level_color}", st.color);
                                    TemplateHandler.Assign(ref row, "{LevelName}", st.name);
                                    TemplateHandler.Assign(ref row, "{LevelID}", st.id);
                                    //TemplateHandler.Assign(ref row, "{LevelName}", st.name);
                                    //TemplateHandler.Assign(ref row, "{}", st.color);
                                    // set popup downline for agent(roleid = 2)););
                                    TemplateHandler.Assign(ref row, "{type}", names[i]);
                                    var dangerousTpe = GetDanerousType(names[i]);
                                    TemplateHandler.Assign(ref row, "{dangerousType}", dangerousTpe);
                                    if (roleId == 1)
                                        TemplateHandler.Assign(ref row, "{level_onclick}", "return ChangeDangerousLevel('" + names[i] + "'," + st.id + ",'" + st.name + "','" + st.color + "');");
                                    else
                                    {
                                        //TemplateHandler.Assign(ref row, "{level_onclick}", string.Format("return ChangeDangerousLevelAgent('{0}','{1}')", dangerousTpe, st.name));
                                        TemplateHandler.Assign(ref row, "{level_onclick}", "return ChangeDangerousLevelAgent('" + names[i] + "'," + dangerousTpe + ",'" + st.id + "');");
                                    }
                                    content.Append(row);
                                }
                                break;

                            case "OtherSports":
                                foreach (DangerousDetail st in dangerSettingOtherSports)
                                {
                                    string row = Convert.ToString(levelItem.Clone());
                                    TemplateHandler.Assign(ref row, "{level_color}", st.color);
                                    TemplateHandler.Assign(ref row, "{LevelName}", st.name);
                                    TemplateHandler.Assign(ref row, "{LevelID}", st.id);
                                    //TemplateHandler.Assign(ref row, "{LevelName}", st.name);
                                    //TemplateHandler.Assign(ref row, "{}", st.color);
                                    // set popup downline for agent(roleid = 2)););
                                    TemplateHandler.Assign(ref row, "{type}", names[i]);
                                    var dangerousTpe = GetDanerousType(names[i]);
                                    TemplateHandler.Assign(ref row, "{dangerousType}", dangerousTpe);
                                    if (roleId == 1)
                                        TemplateHandler.Assign(ref row, "{level_onclick}", "return ChangeDangerousLevel('" + names[i] + "'," + st.id + ",'" + st.name + "','" + st.color + "');");
                                    else
                                    {
                                        //TemplateHandler.Assign(ref row, "{level_onclick}", string.Format("return ChangeDangerousLevelAgent('{0}','{1}')", dangerousTpe, st.name));
                                        TemplateHandler.Assign(ref row, "{level_onclick}", "return ChangeDangerousLevelAgent('" + names[i] + "'," + dangerousTpe + ",'" + st.id + "');");
                                    }
                                    content.Append(row);
                                }
                                break;
                        }

                        TemplateHandler.Assign(ref s_, "{content}", content.ToString());
                    }
                    else
                    {
                        TemplateHandler.Assign(ref s_, "{onclick}", string.Empty);
                        TemplateHandler.RemoveBlock(ref s_, "ImgDropdownLevel");
                        TemplateHandler.RemoveBlock(ref s_, "DropdownLevel");
                    }
                    dangerous += s_;
                }
                return dangerous;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        public DangerousDetail CustomerUpdateDangerous(int accid, int custid, int dangerLevel, int dangertype)
        {
            try
            {
                bool isBA = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.CustomerUpdateDangerous(accid, custid, dangerLevel, dangertype);
                DangerousDetail dangerdetail = new DangerousDetail(dangerLevel, GetDangerousLevelName(dangerLevel, dangertype.ToString(), isBA), GetDangerousLevelColor(dangerLevel, dangertype.ToString(), isBA));
                return dangerdetail;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        //public Result UpdateLevelDangerousCust(int custId, int updateDownline, int dangerType, int dangerLevel, int userId, string ip)
        //{
        //    var result = new Result();
        //    result = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerUpdateStatus.UpdateLevelDangerous(custId,
        //                                                                                              updateDownline,
        //                                                                                              dangerType,
        //                                                                                              dangerLevel,
        //                                                                                              userId, ip);
        //    return result;

        //}

        private static int GetDanerousType(string dangerousName)
        {
            var dangerousType = 1;
            switch (dangerousName.ToLower())
            {
                case "original":
                    dangerousType = 1;
                    break;

                case "abt":
                    dangerousType = 2;
                    break;

                case "other sports":
                    dangerousType = 3;
                    break;

                default: break;
            }
            return dangerousType;
        }
    }

    public class DangerousDetail
    {
        public string color;
        public int id;
        public string name;

        public DangerousDetail(int _id, string _name, string _color)
        {
            this.id = _id;
            this.name = _name;
            this.color = _color;
        }
    }
}
// -----------------------------------------------------------------------
// <copyright file="AbnormalSetting.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Accounting.Business.Customer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;
    using System.Web;
    using System.Xml;
    using Sunplus.Accounting.Core.Web;
    using DNAide.Web;
    using Sunplus.Accounting.Business.BootStrappers;
    using Sunplus.Accounting.DataAccess;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AbnormalSetting
    {
        private IList<AbnormalDetail> abnormalSetting = new List<AbnormalDetail>();
        private Hashtable idxAbnormalSetting = new Hashtable();

        public AbnormalSetting()
        {
            GetAbnormalSetting();
        }

        public string GenAbnormal(int roleId, bool isManageCustomerAccess, int currentLevel)
        {
            string s = WebUtils.LoadTemplate("AbnormalCust.htm");
            string levelItem = TemplateHandler.CopyBlock(s, "LevelItem");
            TemplateHandler.RemoveBlock(ref s, "LevelItem");
            StringBuilder content = new StringBuilder();
            TemplateHandler.Assign(ref s, "{dangerColor}", GetAbnormalLevelColor(currentLevel));
            TemplateHandler.Assign(ref s, "{currentLevel}", currentLevel.ToString());
            TemplateHandler.Assign(ref s, "{value}", GetAbnormalLevelName(currentLevel));
            try
            {
                if (isManageCustomerAccess && roleId < 3)
                {
                    TemplateHandler.Assign(ref s, "{onclick}", "OpenCloseMenuAbnormal(this,'divLevel', " + currentLevel + ");");
                    foreach (AbnormalDetail st in abnormalSetting)
                    {
                        string row = Convert.ToString(levelItem.Clone());
                        TemplateHandler.Assign(ref row, "{level_color}", st.color);
                        TemplateHandler.Assign(ref row, "{LevelName}", st.name);
                        TemplateHandler.Assign(ref row, "{LevelID}", st.id);
                        if (roleId == 1)
                        {
                            TemplateHandler.Assign(ref row, "{level_onclick}", "return ChangeAbnormalLevel('" + st.id + "');");
                        }
                        else
                        {

                            TemplateHandler.Assign(ref row, "{level_onclick}", "return ChangeAbnormalLevelAgent('" + st.id + "');");
                        }
                        content.Append(row);
                    }
                    TemplateHandler.Assign(ref s, "{content}", content.ToString());
                }
                return s;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }
        public string GetAbnormalLevelColor(int level)
        {
            string color = "White";
            color =  abnormalSetting[Convert.ToInt32(idxAbnormalSetting[Convert.ToString(level)])].color;
            return color;
        }
        public string GetAbnormalLevelName(int level)
        {
            string name = "";
            name = abnormalSetting[Convert.ToInt32(idxAbnormalSetting[Convert.ToString(level)])].name;
            return name;
        }
     
        private void GetAbnormalSetting()
        {
            try
            {
                string configFileName = HttpContext.Current.Server.MapPath("~/App_Data/AbnormalSetting.xml");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configFileName);
                XmlNodeList levelList = xmlDoc.GetElementsByTagName("level");
                int i = 0;
                foreach (XmlNode level in levelList)
                {
                        idxAbnormalSetting.Add(level.Attributes["id"].Value, i++);
                        abnormalSetting.Add(
                            new AbnormalDetail(Convert.ToInt32(level.Attributes["id"].Value),
                                                 level.Attributes["name"].Value, level.Attributes["color"].Value));
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }
    }

    public class AbnormalDetail
    {
        public string color;
        public int id;
        public string name;

        public AbnormalDetail(int _id, string _name, string _color)
        {
            this.id = _id;
            this.name = _name;
            this.color = _color;
        }
    }
}

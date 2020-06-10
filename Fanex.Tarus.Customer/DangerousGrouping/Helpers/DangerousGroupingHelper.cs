namespace Fanex.Tarus.Customer.DangerousGrouping.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using Fanex.Tarus.Customer.DangerousGrouping.Models;

    public interface IDangerousGroupingHelper
    {
        IList<DangerDetail> GetDangerSettingOriginal();

        DangerDetail GetDangerDetailFromValue(int value);
    }

    public class DangerousGroupingHelper : IDangerousGroupingHelper
    {
        private IList<DangerDetail> DangerousLevels { get; set; }

        public DangerousGroupingHelper()
        {
            DangerousLevels = GetDangerSettingLevels();
        }

        public DangerDetail GetDangerDetailFromValue(int value)
        {
            return DangerousLevels.FirstOrDefault(level => level.Id == value);
        }

        public IList<DangerDetail> GetDangerSettingOriginal()
        {
            return DangerousLevels ?? GetDangerSettingLevels();
        }

        private IList<DangerDetail> GetDangerSettingLevels()
        {
            string configFileName = HttpContext.Current.Server.MapPath("~/App_Data/DangerousSetting.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configFileName);
            XmlNodeList levels = xmlDoc.GetElementsByTagName("level");

            IList<DangerDetail> settings = new List<DangerDetail>();
            foreach (XmlNode level in levels)
            {
                settings.Add(
                    new DangerDetail(
                        Convert.ToInt32(level.Attributes["id"].Value),
                        level.Attributes["name"].Value,
                        level.Attributes["color"].Value,
                        level.Attributes["background"].Value));
            }

            return settings;
        }
    }
}
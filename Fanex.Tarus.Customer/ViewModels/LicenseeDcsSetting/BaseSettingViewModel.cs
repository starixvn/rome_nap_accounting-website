using System.Collections.Generic;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting
{
    public abstract class BaseSettingViewModel
    {
        private const int MaxAgentNameLength = 5;

        public string Site { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int SuperId { get; set; }

        public string SuperName { get; set; }

        public string AgentIds { get; set; }

        public string AgentNames { get; set; }

        public virtual string ShowSuperOrAgentNames()
        {
            if (string.IsNullOrWhiteSpace(AgentNames))
            {
                return SuperName;
            }

            IList<int> indexOfCommas = new List<int>();

            for (int i = 0, max = AgentNames.Length; i < max; i++)
            {
                if (AgentNames[i] == ',')
                {
                    indexOfCommas.Add(i);
                }
            }

            if (indexOfCommas.Count > MaxAgentNameLength)
            {
                return AgentNames.Substring(0, indexOfCommas[MaxAgentNameLength]) + ", ...";
            }

            return AgentNames;
        }
    }
}

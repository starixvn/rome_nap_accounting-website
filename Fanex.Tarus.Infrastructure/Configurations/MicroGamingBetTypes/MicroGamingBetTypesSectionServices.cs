namespace Fanex.Tarus.Infrastructure.Configurations.MicroGamingBetTypes
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Tarus.Infrastructure.Models;

    public class MicroGamingBetTypesSectionServices
    {
        public MicroGamingBetTypesSectionServices()
        {
            GetActiveBetTypesDictionary();
            GetActiveBetTypes();
        }

        public Dictionary<int, string> BetTypesDictionary { get; set; }

        public List<BetTypeModel> BetTypes { get; set; }

        public void GetActiveBetTypesDictionary()
        {
            BetTypesDictionary = new Dictionary<int, string>();
            var bettypesConfig = MicroGamingBetTypesSection.Instance.BetTypes;
            if (bettypesConfig == null)
            {
                return;
            }

            foreach (MicroGamingBetTypesElement betType in bettypesConfig)
            {
                if (!betType.Active)
                {
                    continue;
                }

                BetTypesDictionary.Add(Converter.ToInt(betType.Id), betType.Name);
            }
        }

        public void GetActiveBetTypes()
        {
            BetTypes = new List<BetTypeModel>();
            var bettypesConfig = MicroGamingBetTypesSection.Instance.BetTypes;
            if (bettypesConfig == null)
            {
                return;
            }

            foreach (MicroGamingBetTypesElement betType in bettypesConfig)
            {
                BetTypes.Add(new BetTypeModel()
                {
                    Id = Converter.ToInt(betType.Id),
                    Name = betType.Name,
                    GameType = betType.GameType
                });
            }
        }

        public string GetGameTypeName(int betTypeId)
        {
            var gameTypeName = string.Empty;
            foreach (var betType in BetTypes)
            {
                if (betType.Id == betTypeId)
                {
                    gameTypeName = betType.GameType;
                    break;
                }
            }

            return gameTypeName;
        }
    }
}

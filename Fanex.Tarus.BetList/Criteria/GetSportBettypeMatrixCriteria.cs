namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class GetSportBettypeMatrixCriteria : CriteriaBase
    {
        public int Mode { get => 2; }

        public override string GetSettingKey()
        {
            return "Acc_BetList_SportBettypeMatrix_Sel";
        }
    }
}

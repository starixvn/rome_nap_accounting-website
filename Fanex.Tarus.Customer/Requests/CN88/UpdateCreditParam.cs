namespace Fanex.Tarus.Customer.Requests.CN88
{
    public class UpdateCreditParam
    {
        public UpdateCreditParam(
           int searchAccountId,
           byte searchAccountRoleId,
           decimal creditAmount,
           int actorId,
           int subAccountId,
           string language,
           string ip,
           bool isInternal)
        {
            SearchAccountId = searchAccountId;
            SearchAccountRoleId = searchAccountRoleId;
            ActorId = actorId;
            SubAccountId = subAccountId;
            Language = language;
            IP = ip;
            CreditAmount = creditAmount;
            IsInternal = isInternal;
        }

        public int SearchAccountId { get; }

        public byte SearchAccountRoleId { get; }

        public int ActorId { get; }

        public int SubAccountId { get; }

        public string Language { get; }

        public string IP { get; }

        public decimal CreditAmount { get; }

        public bool IsInternal { get; }
    }
}

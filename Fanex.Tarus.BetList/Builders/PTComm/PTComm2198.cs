namespace Fanex.Tarus.BetList.Builders.PTComm
{
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    public class PTComm2198 : PTComm
    {
        public PTComm2198(ITicket ticket, int roleId)
            : base(ticket, roleId)
        {
        }

        public override string PositionTaking => FormatPositionTaking(PositonTakingByRoleId[RoleId]);

        private static string FormatPositionTaking(object value)
        {
            var ptValue = Cast.AsDecimal(value);

            return ptValue < 0
                ? string.Format(ConvertByBetType.Minus, string.Format(Formatter.DECIMALFORMAT40, ptValue))
                : string.Format(Formatter.DECIMALFORMAT40, ptValue);
        }
    }
}

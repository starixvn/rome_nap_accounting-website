using System;
using System.Data;
using static Sunplus.Accounting.Core.Common.Enumerations;

namespace Sunplus.Accounting.Core.Helper
{
    public static class ClosedReasonUtils
    {
        public static string GetClassName(DataRow drow)
        {
            return GetClassName(drow["closedreason"]);
        }

        public static string GetClassName(IDataReader reader)
        {
            return GetClassName(reader["closedreason"]);
        }

        private static string GetClassName(object value)
        {
            CustomerClosedReason closedReason = value == null || value == DBNull.Value ?
                CustomerClosedReason.None :
                (CustomerClosedReason)Convert.ToByte(value);

            return MapClosedReasonToClassName(closedReason);
        }

        private static string MapClosedReasonToClassName(CustomerClosedReason closedReason)
        {
            switch (closedReason)
            {
                case CustomerClosedReason.WrongPasswordOrSCOrOTP:
                    return "closed-pwdscotp";

                case CustomerClosedReason.ByUplineOrAdmin:
                    return "closed-uplineadmin";

                case CustomerClosedReason.None:
                default:
                    return string.Empty;
            }
        }
    }
}

using System;
using System.Data;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Business.Security;
using Sunplus.Core.LoggingLib;

//using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Business.PositionTaking.SportBook
{
    /// <summary>
    /// Summary description for BasePT
    /// </summary>
    public class BasePT
    {
        protected static readonly double MaxPT = 0.9;
        protected static readonly double MinPT;
        protected JObject OutputData = new JObject();
        protected CustomerDetail _customerDetail = new CustomerDetail();

        public virtual string Insert(HttpRequest httpRequest)
        {
            try
            {
                return "";
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        public virtual string Update(HttpRequest httpRequest)
        {
            throw new NotImplementedException();
        }


        public virtual string Select(PtManager.PtType ptType)
        {
            OutputData["CreditPermission"] =
                new JValue(Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit));
            OutputData["CustomerDetail"] = new JValue(JsonConvert.SerializeObject(_customerDetail));
            return JsonConvert.SerializeObject(OutputData);
        }

        #region Sports

        protected virtual SportDetail GetSportPt(int sportId, PtManager.PtType ptType, DataTable data)
        {
            throw new NotImplementedException();
        }

        protected virtual SportDetail Soccer(PtManager.PtType ptType, DataTable data)
        {
            throw new NotImplementedException();
        }

        protected virtual SportDetail BasketBall(PtManager.PtType ptType, DataTable data)
        {
            throw new NotImplementedException();
        }

        protected virtual SportDetail FootBall(PtManager.PtType ptType, DataTable data)
        {
            throw new NotImplementedException();
        }

        protected virtual SportDetail Tennis(PtManager.PtType ptType, DataTable data)
        {
            throw new NotImplementedException();
        }

        protected virtual SportDetail BaseBall(PtManager.PtType ptType, DataTable data)
        {
            throw new NotImplementedException();
        }

        protected virtual SportDetail Golf(PtManager.PtType ptType, DataTable data)
        {
            throw new NotImplementedException();
        }

        protected virtual SportDetail MotoSports(PtManager.PtType ptType, DataTable data)
        {
            throw new NotImplementedException();
        }

        protected virtual SportDetail OtherSports(PtManager.PtType ptType, DataTable data)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
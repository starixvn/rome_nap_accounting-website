namespace Fanex.Tarus.Admin.Business.AutoFollowBet
{
    using Fanex.Tarus.Admin.DataAccess.AutoFollowBet;
    using Fanex.Tarus.Admin.Entities; 
    using System.Collections.Generic;
    using System.Data;
    using DNAide.Core;

    public class AutoFollowBetService : IAutoFollowBetService
    {
        private readonly IAutoFollowBetAccess _autoFollowBetAccess;

        public AutoFollowBetService():this(null)
        {
            
        }

        public AutoFollowBetService(IAutoFollowBetAccess crossTransferAccess)
        {
            _autoFollowBetAccess = crossTransferAccess ?? new AutoFollowBetAccess();
        }



        #region IAutoFollowBetService Members

        public List<FollowedCustomer> SelFollowedCustomer(int status)
        {
            var listFollowedCustomer = new List<FollowedCustomer>();
            DataTable dtb = _autoFollowBetAccess.AutoFollowBetSelCustomer(status);
            dtb.DefaultView.Sort = "username DESC";
            foreach (DataRow row in dtb.Rows)
            {
                listFollowedCustomer.Add(new FollowedCustomer(CastAide.AsString(row["username"]), CastAide.AsDouble(row["rate"]), CastAide.AsBoolean(row["IsBA"]))); 
            }
            return listFollowedCustomer;
        }

        public Result DelFollowedCustomer(string username)
        {
            var result = new Result();
            result = _autoFollowBetAccess.AutoFollowBetDelCustomer(username);
            return result;
        }

        public Entities.Result InsFollowedCustomer(string username, double rate)
        {
            var result = new Result();
            result = _autoFollowBetAccess.AutoFollowBetInsCustomer(username, rate);
            return result;
        }

        public Entities.Result UpdFollowedCustomer(string username, double rate)
        {
            var result = new Result();
            result = _autoFollowBetAccess.AutoFollowBetUpdCustomer(username, rate);
            return result;
        }

        #endregion
    }
}

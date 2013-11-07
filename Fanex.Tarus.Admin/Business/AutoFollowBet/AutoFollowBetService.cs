//Sample license text.

namespace Fanex.Tarus.Admin.Business.AutoFollowBet
{
    using System.Collections.Generic;
    using System.Data;
    using DNAide.Core;
    using Fanex.Tarus.Admin.DataAccess.AutoFollowBet;
    using Fanex.Tarus.Admin.Entities;

    /// <summary>
    /// Auto Follow Bet Service.
    /// </summary>
    public class AutoFollowBetService : IAutoFollowBetService
    {
        private readonly IAutoFollowBetAccess _autoFollowBetAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFollowBetService"/> class.
        /// </summary>
        public AutoFollowBetService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFollowBetService"/> class.
        /// </summary>
        /// <param name="crossTransferAccess">The cross transfer access.</param>
        public AutoFollowBetService(IAutoFollowBetAccess crossTransferAccess)
        {
            _autoFollowBetAccess = crossTransferAccess ?? new AutoFollowBetAccess();
        }

        #region IAutoFollowBetService Members

        /// <summary>
        /// Selects the followed customer.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>Return list of followed customer.</returns>
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

        /// <summary>
        /// Deletes the followed customer.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Return Result.</returns>
        public Result DelFollowedCustomer(string username)
        {
            var result = new Result();
            result = _autoFollowBetAccess.AutoFollowBetDelCustomer(username);
            return result;
        }

        /// <summary>
        /// Increases the followed customer.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="rate">The currency rate.</param>
        /// <returns></returns>
        public Entities.Result InsFollowedCustomer(string username, double rate)
        {
            var result = new Result();
            result = _autoFollowBetAccess.AutoFollowBetInsCustomer(username, rate);
            return result;
        }

        /// <summary>
        /// Updates the followed customer.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="rate">The currency rate.</param>
        /// <returns></returns>
        public Entities.Result UpdFollowedCustomer(string username, double rate)
        {
            var result = new Result();
            result = _autoFollowBetAccess.AutoFollowBetUpdCustomer(username, rate);
            return result;
        }

        #endregion IAutoFollowBetService Members
    }
}
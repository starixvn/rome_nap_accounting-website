// --------------------------------------------------------------------------
// <copyright file="IViewKenoPTRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>
namespace Sunplus.Accounting.DataAccess.IBCDLL.Customer
{
    using DNAide.Data;
    using Sunplus.Accounting.Entities.Criteria.ViewPT;

    public interface IViewKenoPTRepository
    {
        DbResult GetPT(ViewKenoPTCriteria criteria);
    }
}

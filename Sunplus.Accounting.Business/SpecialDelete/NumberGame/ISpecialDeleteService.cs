using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.SpecialDelete;

namespace Sunplus.Accounting.Business.SpecialDelete.NumberGame
{
    
    public interface ISpecialDeleteService
    {
    
        SpecialDeleteDTO SelSpecialDeleteNumberGame(int matchId, long fromRefNo, long toRefNo, string live, string dead , string userName);

        Result DeleteSpecialTransactionNumberGame(int matchId, long frefno, long trefno, string live, string dead,
                     int DelOption, string transids, 
            int DelOption_BD, string transids_BD, int DelOption_12EU, string transids_12EU, int DelOption_MHC, string transids_MHC,
            Message message);

        SpecialDeleteDTO BettransSelSpecialDel(SpecialDeleteParam param);

    }
}

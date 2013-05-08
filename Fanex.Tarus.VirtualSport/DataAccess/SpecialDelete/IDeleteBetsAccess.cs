


using System.Collections;
using System.Data;
using Fanex.Tarus.VirtualSport.Entities.SpecialDelete;

namespace Fanex.Tarus.VirtualSport.DataAccess.SpecialDelete
{
    public interface IDeleteBetsAccess
    {
        DataSet SelBettypes(int type);
        DataSet SelMatchInfo(int matchId, long transid);
        DataSet BettransSelSpecialDel(SpecialDeleteParam deleteParam);
        DataSet DeleteSpecialTransaction(SpecialDeleteParam deleteParam, ref Hashtable outputParam);
    }
}

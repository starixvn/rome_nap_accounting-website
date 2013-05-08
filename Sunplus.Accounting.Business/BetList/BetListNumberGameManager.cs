using System;
using System.Data;
using System.ServiceModel.Activation;
using DNAide.Core;
using Sunplus.Accounting.Entities;
using Sunplus.Bingo.Core;
using Sunplus.Accounting.Core.Extensions;

namespace Sunplus.Accounting.Business.BetList
{
    /// <summary>
    /// Summary description for DailyTransManager
    /// </summary>

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BetListNumberGameManager : IBetListNumberGameManager
    {
        #region IBetListNumberGameManager Members
        public NumbergameResult GetBetListNumberGame(int matchId)
        {
        
            DataTable dt;
            dt = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Betlist.GetNumberGameResult(matchId);
            var result = new NumbergameResult();            
            var totalrow = dt.Rows.Count;

            if (totalrow > 0)
            {
                result.Status = dt.Rows[0]["eventstatus"].ToString();
                if (result.Status.ToLowerInvariant() == "running")
                {
                    result.Status = "Pending";
                }
                result.KickOffTime = dt.Rows[0]["eventdate"].ToString();
                result.Match = string.Format("{0} {1}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblNumberGameNo"), dt.Rows[0]["matchcode"].ToString());

                result.FirstBall = "<img src='Images/" + (CastAide.AsInt16(dt.Rows[0]["number"], 0) < 10 ? "0" + dt.Rows[0]["number"].ToString() + ".jpg" : dt.Rows[0]["number"].ToString() + ".jpg") + "' border='0' hspace='1' vspace='1'>";
                
                int totalUnder = 0, secondBall = 0, thirdBall = 0;
                int totalOver = 0;
                int totalNumber = 0;
                string resultNumber = "";
                int totalseq = dt.Rows.Count;
                int number;
                string imgnumber;

                foreach (DataRow row in dt.Rows)
                {
                    number = CastAide.AsInt16(row["number"], 0);
                    imgnumber = number < 10 ? "0" + number.ToString() + ".jpg": number.ToString() + ".jpg";
               
                    if (number >= 38 )                    
                        totalOver++;                    
                    else                    
                        totalUnder++;                    

                    resultNumber += "<img src='Images/" + imgnumber + "' border='0' hspace='1' vspace='1'>";
                    totalNumber += number;
                }
            
                result.Result = resultNumber;
                secondBall = dt.Rows.Count >= 2 ?  CastAide.AsInt16(dt.Rows[1]["number"], 0) : 0;
                thirdBall = dt.Rows.Count == 3 ?  CastAide.AsInt16(dt.Rows[2]["number"], 0): 0;

                if (result.Status.IndexOf("completed", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    result.LastBall = "<img src='Images/" + (CastAide.AsInt16(dt.Rows[totalrow - 1]["number"], 0) < 10 ? "0" + dt.Rows[totalrow - 1]["number"].ToString() + ".jpg" : dt.Rows[totalrow - 1]["number"].ToString() + ".jpg") + "' border='0' hspace='1' vspace='1'>";
                    result.OUFT = (totalOver == totalUnder) ? "Draw" : (totalOver > totalUnder ? "OVER" : "UNDER");
                    result.OEFT = totalNumber % 2 == 0 ? "EVEN" : "ODD";
                    result.Warrior = secondBall > thirdBall ? "2nd ball Win" : "3rd ball Win";
                }
                else
                {
                    result.LastBall = "-";
                    result.OUFT = "-";
                    result.OEFT = "-";
                    result.Warrior = "-";
                }
            }
            return result;
        }
        #endregion
    }
}




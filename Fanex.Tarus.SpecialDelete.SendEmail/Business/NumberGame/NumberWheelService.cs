// -----------------------------------------------------------------------
// <copyright file="NumberWheelService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.SpecialDelete.SendEmail.Business.NumberGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Tarus.SpecialDelete.SendEmail.Entity.NumberGame;
    using System.Web;
    using System.IO;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Tarus.Business.Implement;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NumberWheelService : INumberWheelService 
    {
        private List<NumberWheelChoice> _listChoice;

        private const string NumberWheelChoiceKey = "Fanex.Tarus.BetlistNumberWheelChoices";

        public NumberWheelService()
        {
            _listChoice = new List<NumberWheelChoice>();

            var objListChoice = CacheService.Get(NumberWheelChoiceKey) as List<NumberWheelChoice>;

            if (objListChoice == null)
            {
                _listChoice = GetDefaultChoiceList();
                CacheService.Set(NumberWheelChoiceKey, _listChoice); 
            }
            else
            {
                _listChoice = objListChoice as List<NumberWheelChoice>;
            }
        }

        private static List<NumberWheelChoice> GetDefaultChoiceList()
        {
            var listChoice = new List<NumberWheelChoice>();


            var strNumberWheelDataPath = HttpContext.Current.Server.MapPath("~/App_Data/NumberWheelChoice.txt");
            try
            {
                if (File.Exists(strNumberWheelDataPath))
                {

                    using (var sr = new StreamReader(strNumberWheelDataPath))
                    {

                        while (sr.Peek() >= 0)
                        {
                            var strCurrent = sr.ReadLine();
                            if (string.IsNullOrEmpty(strCurrent))
                                continue;
                            var arrData = strCurrent.Split(' ');
                            if (arrData.Length < 3)
                                continue;
                            var choice = new NumberWheelChoice();
                            choice.Hdp1 = Convert.ToInt32(arrData[0]);
                            choice.Betteam = Convert.ToInt32(arrData[1]);
                            choice.Choice = arrData[2];

                            listChoice.Add(choice);

                        }

                    }

                }
            }
            catch (Exception ex)
            {

                LogExtensions.ExceptionHandler(ex);
            }



            return listChoice;
        }

        public List<NumberWheelChoice> GetChoiceList()
        {
            return _listChoice;
        }

        public string GetChoiceName(int hdp1, int betTeam)
        {
            if (_listChoice.Count.Equals(0)) return string.Empty;

            try
            {
                var objChoice = _listChoice.Where(item => item.Hdp1.Equals(hdp1) && item.Betteam.Equals(betTeam)).FirstOrDefault();

                if (objChoice == null)
                    return string.Empty;

                string [] arr = objChoice.Choice.Split(',');
                string str = "";
                int index = 0, i = 0;
                foreach (string s in arr)
                {
                    index++;

                    str += s + (i == arr.Length - 1 ? "" : ",");
                    if (index == 4)
                    {
                        str += "<br/>";
                        index = 0;
                    }
                    
                    i++;
                }

                return str;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);

            }

            return string.Empty;
        }
    }
}

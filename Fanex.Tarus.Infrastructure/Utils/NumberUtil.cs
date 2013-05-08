using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Infrastructure.Utils
{
    using System.Text.RegularExpressions;

    public class NumberUtil
    {
        public static double ExtractNumber(string value)
        {
            var result = 0.0;
            if (string.IsNullOrEmpty(value)) return result;

            var numberRegex = Regex.Match(value, @"\d{1,3}.\d{0,4}");

            if(numberRegex.Success)
            {
                try
                {
                    result = Convert.ToDouble(numberRegex.Value);
                }
                catch (Exception ex)
                {
                    result = .0;
                }
                
            }

            return result;
        }

        public static bool IsNumber(object objNumber)
        {
            try
            {
                Convert.ToDouble(objNumber);
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }
    }
}

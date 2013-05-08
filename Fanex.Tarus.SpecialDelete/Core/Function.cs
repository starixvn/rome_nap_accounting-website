// -----------------------------------------------------------------------
// <copyright file="Function.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.SpecialDelete.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Function
    {
        public enum ActionType
        {
            DeleteBet = 0,//Check negative balance for special delete funcion
            RecoverBet = 1//Check negative balance for recover funcion
        }
    }
}

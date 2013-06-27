// -----------------------------------------------------------------------
// <copyright file="INumberWheelService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.SpecialDelete.SendEmail.Business.NumberGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.SpecialDelete.SendEmail.Entity.NumberGame;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INumberWheelService
    {
        List<NumberWheelChoice> GetChoiceList();
        
        string GetChoiceName(int hdp1, int betTeam);
    }
}

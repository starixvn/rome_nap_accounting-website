// -----------------------------------------------------------------------
// <copyright file="Bettype.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.SpecialDelete.SendEmail.Entity.NumberGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Bettype
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class BettypeModel
    {

        public BettypeModel() 
        {
            Bettypes = new List<Bettype>();
        }

        public bool IsLive { get; set; }

        public List<Bettype> Bettypes { get; set; }
    }
}

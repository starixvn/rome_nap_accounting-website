// -----------------------------------------------------------------------
// <copyright file="AbnormalDetail.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Customer.Entities.SettingCustomer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AbnormalDetail
    {
        public string color;
        public int id;
        public string name;

        public AbnormalDetail() { }

        public AbnormalDetail(int _id, string _name, string _color)
        {
            this.id = _id;
            this.name = _name;
            this.color = _color;
        }
    }
}

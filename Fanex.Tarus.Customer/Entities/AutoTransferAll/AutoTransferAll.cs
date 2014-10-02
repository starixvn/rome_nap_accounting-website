namespace Fanex.Tarus.Customer.Entities.AutoTransferAll
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Auto Transfer All.
    /// </summary>
    public class AutoTransferAll
    {
        public AutoTransferAll()
        {
            this.AutoTransfers = new List<AutoTransfer>();
        }

        public List<AutoTransfer> AutoTransfers { get; set; }

        public int TotalRows { get; set; }
    }
}
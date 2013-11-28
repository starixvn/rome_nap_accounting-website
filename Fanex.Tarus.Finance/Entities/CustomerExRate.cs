namespace Fanex.Tarus.Finance.Entities
{
    using System;

    /// <summary>
    /// Summary description for CustDetail
    /// </summary>

    [Serializable]
    public class CustomerExRate
    {
        public int custid;

        public string username;

        public string currency;

        public double actualrate;
    }
}
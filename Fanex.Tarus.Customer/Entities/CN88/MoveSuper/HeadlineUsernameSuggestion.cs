namespace Fanex.Tarus.Customer.Entities.CN88.MoveSuper
{
    using System.Collections.Generic;

    public class HeadlineUsernameSuggestion
    {
        public string query;
        public IEnumerable<string> suggestions { get; set; }
        public string[] data = new string[] { "" };
    }
}

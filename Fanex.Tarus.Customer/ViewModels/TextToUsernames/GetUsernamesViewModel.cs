using System.Collections.Generic;

namespace Fanex.Tarus.Customer.ViewModels.TextToUsernames
{
    public class GetUsernamesViewModel
    {
        public string ValidUsernames { get; set; }
        public IEnumerable<UsernameViewModel> InvalidUsernames { get; set; }
    }
}

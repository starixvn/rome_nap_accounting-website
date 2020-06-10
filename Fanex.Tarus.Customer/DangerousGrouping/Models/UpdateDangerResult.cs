namespace Fanex.Tarus.Customer.DangerousGrouping.Models
{
    using Fanex.CustomerSetting.Module.Core.Models;

    public class UpdateDangerResult
    {
        public Result Result { get; set; }
        public DangerDetail CustomerDanger { get; set; }
    }
}
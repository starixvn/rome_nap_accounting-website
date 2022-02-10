namespace Fanex.Tarus.BetList.ViewModel
{
    public class ThirdPartyResultViewModel
    {
        public ThirdPartyResultViewModel(
               string productName,
               string resultAddress)
        {
            Title = $"{productName}";
            Result = resultAddress;
        }

        public string Title { get; }

        public string Result { get; }
    }
}

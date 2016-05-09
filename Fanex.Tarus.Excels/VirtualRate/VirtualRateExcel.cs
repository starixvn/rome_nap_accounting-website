namespace Fanex.Tarus.Excels.VirtualRate
{
    using System.Data;

    public class VirtualRateExcel : IVirtualRateExcel
    {
        private readonly ITarusExcel _tarusExcel;

        public VirtualRateExcel(string filePath)
        {
            _tarusExcel = new TarusExcel(filePath);
        }

        public DataTable ExcelToDataTable()
        {
            return _tarusExcel.ExcelToDataTable();
        }
    }
}

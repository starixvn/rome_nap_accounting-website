namespace Sunplus.Data.DataAbstract.DAO
{
    public class LogDAO : DataAccess
    {
        public LogDAO()
            : base()
        {
        }

        public void Insert<T>(T data)
        {
            DataStorage.Insert<T>(string.Empty, data);
        }
    }
}
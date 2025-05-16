namespace Fanex.Tarus.Customer.DataAccess.ManageSuperSettings
{
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.TextToUsernames;
    using Fanex.Tarus.Customer.ViewModels.TextToUsernames;

    public interface ITextToUsernamesDataAccess
    {
        GetUsernamesViewModel GetUsernames(GetUsernamesCriteria criteria);
    }

    public class TextToUsernamesDataAccess : BaseDataAccess, ITextToUsernamesDataAccess
    {
        public GetUsernamesViewModel GetUsernames(GetUsernamesCriteria criteria)
        {
            var validUsernames = new List<string>();
            var valdidNicknames = new List<string>();
            var invalidUsernames = new List<UsernameViewModel>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {

                    while (reader.Read())
                    {
                        var isValid = int.Parse(reader["ErrCode"].ToString()) == 0;
                        var username = reader["Username"].ToString();

                        if (isValid)
                        {                            
                            validUsernames.Add(username);
                            continue;
                        }

                        invalidUsernames.Add(new UsernameViewModel
                        {
                            Username = username,
                            Reason = reader["ErrMsg"].ToString()
                        });
                    }
                }

                var result = new GetUsernamesViewModel
                {
                    ValidUsernames = string.Join(", ", validUsernames),
                    ValidNicknames = string.Join(", ", valdidNicknames),
                    InvalidUsernames = invalidUsernames
                };

                return result;
            }
        }
    }
}

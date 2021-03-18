namespace Fanex.Tarus.Customer.Business.CopyVRSetting
{
    using Fanex.Tarus.Customer.Criteria.CopyVRSetting;
    using Fanex.Tarus.Customer.DataAccess.CopyVRSetting;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.CopyVRSetting;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ICopyVRSettingService
    {
        List<Result> Copy(CopyVRSettingCriteria criteria);
    }

    public class CopyVRSettingService : ICopyVRSettingService
    {
        private readonly ICopyVRSettingDataAccess _copyVRSettingDataAccess;

        public CopyVRSettingService() : this(null) { }

        public CopyVRSettingService(ICopyVRSettingDataAccess copyVRSettingDataAccess)
        {
            this._copyVRSettingDataAccess = copyVRSettingDataAccess ?? new CopyVRSettingDataAccess();
        }

        public List<Result> Copy(CopyVRSettingCriteria criteria) => ComposeErrorMessage(_copyVRSettingDataAccess.Copy(criteria), criteria.DestinationMembers);

        private List<Result> ComposeErrorMessage(IEnumerable<CopyResultEntity> copyResults, string destinationMembers)
        {
            //    if (copyResults == null || !copyResults.Any())
            //    {
            //        return new List<Result>() { new Result(0, "All accounts have been successfully copied.") };
            //    }

            //    // Source account is invalid
            //    if (copyResults.Any(e => e.ErrorCode == 1))
            //    {
            //        var copyResult = copyResults.Where(e => e.ErrorCode == 1).FirstOrDefault();
            //        return new List<Result>() { new Result(copyResult.ErrorCode, copyResult.ErrorMsg) };
            //    }

            //    var listResult = new List<Result>();
            //    List<string> listDestinationMembers = destinationMembers.Split(',').ToList();
            //    if (copyResults.Count() < listDestinationMembers.Count)
            //    {
            //        List<string> listCopySuccessMembers = listDestinationMembers.Except(copyResults.Select(e => e.UserName)).ToList();

            //        listResult.Add(new Result(0, "Successfully copied accounts: " + string.Join(", ", listCopySuccessMembers)));
            //    }

            //    foreach (var copyResult in copyResults)
            //    {
            //        listResult.Add(new Result(copyResult.ErrorCode, copyResult.ErrorCode == 1 ? copyResult.ErrorMsg : copyResult.ErrorMsg + copyResult.UserName));
            //    }

            //    return listResult;




            if (!copyResults.Any(e => e.ErrorCode != 0))
            {
                return new List<Result>() { new Result(0, "All accounts have been successfully copied.") };
            }

            var listResult = new List<Result>();
            var successCopyMembers = new StringBuilder();
            foreach (var copyResult in copyResults)
            {
                if (copyResult.ErrorCode == 0)
                {
                    successCopyMembers.Append($", {copyResult.UserName}");
                }
                else
                {
                    listResult.Add(new Result(copyResult.ErrorCode, copyResult.ErrorMsg + copyResult.UserName));
                }
            }

            if (successCopyMembers.Length > 0)
            {
                successCopyMembers.Remove(0, 2);
            }

            if (successCopyMembers.Length > 0)
            {
                listResult.Insert(0, new Result(0, $"Successfully copied accounts: {successCopyMembers.ToString()}"));
            }

            return listResult;
        }
    }
}

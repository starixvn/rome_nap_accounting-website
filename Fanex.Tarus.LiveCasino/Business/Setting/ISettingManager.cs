using Fanex.LiveCasinoSetting.Request;
using Fanex.LiveCasinoSetting.ViewModel;
using Fanex.Tarus.LiveCasino.Entities;

namespace Fanex.Tarus.LiveCasino.Business.Setting
{
    public interface ISettingManager
    {
        LiveCasinoSettingDTO GetSuperSetting(int custId, int exchangeId);
        LiveCasinoSettingDTO GetMasterSetting(int custId, int uplineId);
        LiveCasinoSettingDTO GetAgentSetting(int custId, int uplineId);
        LiveCasinoSettingDTO GetMemberSetting(int custId, int uplineId);
        LiveCasinoSettingDTO GetSuperOnMemberSetting(int custId, int uplineId);
        LiveCasinoSettingDTO GetMasterOnMemberSetting(int custId, int uplineId);
        
        ResultDTO UpdateSuperPTSetting(LiveCasinoSettingDTO setting);
        
        ResultDTO UpdateGameSetting(LiveCasinoSettingDTO setting);
        
        ResultDTO UpdateMasterPTSetting(LiveCasinoSettingDTO setting);
        
        ResultDTO UpdateAgentPTSetting(LiveCasinoSettingDTO setting);
        
        ResultDTO UpdatememberPTSetting(LiveCasinoSettingDTO setting);
        
        ResultDTO SuperUpdateMemberPTSetting(LiveCasinoSettingDTO setting);
        
        ResultDTO MasterUpdateMemberPTSetting(LiveCasinoSettingDTO setting);
        
        Result AutoInsertPtSetting(CustomerInfoDTO customerInfo, int adminUserId, string adminUserName, int signupFrom);
        
        Result UpdateCustomerSyncInfo(LiveCasinoSettingDTO currentSetting);
        
        bool GetSyncLiveCasinoStatus(int custId, string userName, int userId);

        SettingViewModel SelSuperSetting(int custId, int exchangeId);

        ResultDTO UpdateStatusLiveCasino(LiveCasinoSettingDTO setting);

        ResultDTO UpdateSuperLiveCasino(LiveCasinoSettingDTO setting);
        ResultDTO UpdateMasterLiveCasino(LiveCasinoSettingDTO setting);
        ResultDTO UpdateAgentLiveCasino(LiveCasinoSettingDTO setting);
        ResultDTO UpdateMemberLiveCasino(LiveCasinoSettingDTO setting);
        ResultDTO UpdateMasterOnMemberLiveCasino(LiveCasinoSettingDTO setting);
        ResultDTO UpdateSuperOnMemberLiveCasino(LiveCasinoSettingDTO setting);


    }
}
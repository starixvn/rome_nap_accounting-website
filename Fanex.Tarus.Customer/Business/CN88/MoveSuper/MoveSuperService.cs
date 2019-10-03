namespace Fanex.Tarus.Customer.Business.CN88.MoveSuper
{
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Tarus.Customer.Criteria.CN88.MoveSuper;
    using Fanex.Tarus.Customer.DataAccess.CN88.MoveSuper;
    using Fanex.Tarus.Customer.Entities.CN88.MoveSuper;
    using Fanex.Tarus.Customer.Requests.CN88.MoveSuper;
    using Fanex.Tarus.Customer.ViewModels.CN88.MoveSuper;
    using System;
    using System.Xml;

    public class MoveSuperService : IMoveSuperService
    {
        private IMoveSuperDataAccess _moveSuperDataAccess;

        public MoveSuperService() : this(null) { }

        public MoveSuperService(IMoveSuperDataAccess moveSuperDataAccess)
        {
            _moveSuperDataAccess = moveSuperDataAccess ?? new MoveSuperDataAccess();
        }

        public HeadlineUsernameSuggestion GetHeadlineUserNameSuggestion(string userName)
            => new HeadlineUsernameSuggestion
            {
                query = userName,
                suggestions = _moveSuperDataAccess.GetHeadlineUserNameSuggestion(new GetHeadlineUserNameSuggestionCriteria() { UserName = userName })
            };


        public MoveSuperViewModel SearchSuper(string userName)
        {
            var superModel = _moveSuperDataAccess.SearchSuper(new SearchSuperCriteria() { SuperUsername = userName });
            if (superModel == null)
            {
                return null;
            }

            var moveSuperViewModel = new MoveSuperViewModel()
            {
                SuperId = superModel.SuperId,
                CurrentHeadlineUsername = superModel.CurrentHeadlineUsername,
                NextHeadlineUsername = superModel.NextHeadlineUsername,
                EffectiveDate = superModel.NoOfMember == 0 ? DateTime.Today.ToString("MM/dd/yyyy") : DateTime.Today.AddDays(1).ToString("MM/dd/yyyy")
            };

            int currentSportsbookPossess = 0;
            int currentNonSportsbookPossess = 0;
            NewMethod(superModel.CurrentExtraPTSetting, ref currentSportsbookPossess, ref currentNonSportsbookPossess);
            if (!string.IsNullOrWhiteSpace(superModel.NextExtraPTSetting))
            {
                int nextSportsbookPossess = 0;
                int nextNonSportsbookPossess = 0;
                NewMethod(superModel.NextExtraPTSetting, ref nextSportsbookPossess, ref nextNonSportsbookPossess);
                moveSuperViewModel.NextSportsbookPossess = nextSportsbookPossess;
                moveSuperViewModel.NextNonSportPossess = nextNonSportsbookPossess;
            }

            moveSuperViewModel.CurrentSportsbookPossess = currentSportsbookPossess;
            moveSuperViewModel.CurrentNonSportPossess = currentNonSportsbookPossess;
            return moveSuperViewModel;
        }

        private static void NewMethod(string xml, ref int sportsbookPossess, ref int nonSportsbookPossess)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            if (doc.HasChildNodes)
            {
                var root = doc.FirstChild;
                if (root.HasChildNodes)
                {
                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        var node = root.ChildNodes[i];
                        var productId = Utils.ToString(node.Attributes["ProductId"].Value);
                        var currentExtraPT = Utils.ToDecimal(node.Attributes["ExtraPT"].Value) * 100;
                        if (productId.Equals("1"))
                        {
                            sportsbookPossess = (int)currentExtraPT;
                        }
                        else if (productId.Equals("3"))
                        {
                            nonSportsbookPossess = (int)currentExtraPT;
                        }
                    }
                }
            }
        }

        public Result MoveSuper(MoveSuperRequest request) => _moveSuperDataAccess.MoveSuper(new MoveSuperCriteria
        {
            SuperId = request.SuperId,
            UserId = request.UserId,
            IP = request.Ip,
            EffectiveDate = request.EffectiveDate,
            HeadlineUsername = request.HeadlineUsername,
            SportsbookPossess = request.SportsbookPossess,
            NonSportPossess = request.NonSportPossess
        });
    }
}

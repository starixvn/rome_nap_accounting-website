// --------------------------------------------------------------------------
// <copyright file="BetStatusParser.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Sunplus.Accounting.Business.BetList
{
    using System;
    using System.Data;

    public class TeamId
    {
        public static int HomeTeam = 1;
        public static int AwayTeam = 2;

        public static string HomeTeamString = "h";
        public static string AwayTeamString = "a";
    }

    /// <summary>
    /// Bet Status Parser.
    /// </summary>
    public class BetStatusParser
    {
        private string _betStatus;
        private string _goalRecord;
        private bool _hasFGLGInfo;

        private int _firstGoalValue;
        private int _lastGoalValue;
        private int _lastGoalValueHT;

        public bool IsHomeScoreHTFirstGoal;
        public bool IsAwayScoreHTFirstGoal;
        public bool IsHomeScoreHTLastGoal;
        public bool IsAwayScoreHTLastGoal;

        public bool IsHomeScoreFirstGoal;
        public bool IsAwayScoreFirstGoal;
        public bool IsHomeScoreLastGoal;
        public bool IsAwayScoreLastGoal;

        private int _finalHomeScore;
        private int _finalAwayScore;

        public BetStatusParser(DataRow betTransaction)
        {
            if (betTransaction == null)
            {
                return;
            }

            _betStatus = betTransaction["betstatus"].ToString().Trim();
            _goalRecord = betTransaction["goalrecord"].ToString().Trim();
            _finalHomeScore = Convert.IsDBNull(betTransaction["finalhomescore"]) ?
                0 :
                Convert.ToInt32(betTransaction["finalhomescore"]);
            _finalAwayScore = Convert.IsDBNull(betTransaction["finalawayscore"]) ?
                0 :
                Convert.ToInt32(betTransaction["finalawayscore"]);
            DetermineFGLG();
        }

        /// <summary>
        /// Determines the FGLG (include 1FGLG).
        /// </summary>
        private void DetermineFGLG()
        {
            if (string.IsNullOrEmpty(_betStatus))
            {
                return;
            }

            string[] betStatusList = _betStatus.Split(';');
            foreach (var status in betStatusList)
            {
                if (status.Contains("1FL:"))
                {
                    Parse1HFL(status);
                    continue;
                }

                if (status.Contains("FL:"))
                {
                    ParseFL(status);
                    continue;
                }
            }
        }

        private void Parse1HFL(string status)
        {
            _hasFGLGInfo = true;
            string fglgInfo = status.Split(':')[1];
            var goalInfoArray = fglgInfo.Split('-');

            _firstGoalValue = Convert.ToInt32(goalInfoArray[0]);
            _lastGoalValue = _lastGoalValueHT = Convert.ToInt32(goalInfoArray[1]);

            var goalRecord2T = string.IsNullOrWhiteSpace(_goalRecord) ?
                string.Empty :
                _goalRecord.Replace(";", string.Empty).ToLower();
            _lastGoalValueHT = Convert.ToInt32(goalInfoArray[1]);
            _lastGoalValue = Convert.ToInt32(goalInfoArray[1]);

            IsHomeScoreHTFirstGoal = _firstGoalValue == TeamId.HomeTeam;
            IsAwayScoreHTFirstGoal = _firstGoalValue == TeamId.AwayTeam;
            IsHomeScoreHTLastGoal = _lastGoalValueHT == TeamId.HomeTeam;
            IsAwayScoreHTLastGoal = _lastGoalValueHT == TeamId.AwayTeam;
            IsHomeScoreFirstGoal = IsHomeScoreHTFirstGoal;
            IsAwayScoreFirstGoal = IsAwayScoreHTFirstGoal;
        }

        private void ParseFL(string status)
        {
            _hasFGLGInfo = true;
            string fglgInfo = status.Split(':')[1];
            var goalInfoArray = fglgInfo.Split('-');
            _firstGoalValue = Convert.ToInt32(goalInfoArray[0]) == 0 ?
                0 :
                (_firstGoalValue == 0 ? Convert.ToInt32(goalInfoArray[0]) : _firstGoalValue);
            _lastGoalValue = _lastGoalValueHT = Convert.ToInt32(goalInfoArray[1]);

            IsHomeScoreFirstGoal = (_firstGoalValue != 0) && (IsHomeScoreFirstGoal || _firstGoalValue == TeamId.HomeTeam);
            IsAwayScoreFirstGoal = (_firstGoalValue != 0) && (IsAwayScoreFirstGoal || _firstGoalValue == TeamId.AwayTeam);

            IsHomeScoreLastGoal = _lastGoalValue == TeamId.HomeTeam;
            IsAwayScoreLastGoal = _lastGoalValue == TeamId.AwayTeam;
        }

        private void ParseGoalRecord()
        {
            if (string.IsNullOrEmpty(_goalRecord))
            {
                return;
            }

            var hasSemiColon = _goalRecord.IndexOf(";") != -1;
            var goalRecord1H = _goalRecord.Split(';')[0].ToLower();

            IsHomeScoreFirstGoal = goalRecord1H.StartsWith(TeamId.HomeTeamString);
            IsHomeScoreHTFirstGoal = IsHomeScoreFirstGoal && hasSemiColon;
            IsAwayScoreFirstGoal = !IsHomeScoreFirstGoal;
            IsAwayScoreHTFirstGoal = IsAwayScoreFirstGoal && hasSemiColon;

            IsHomeScoreHTLastGoal = goalRecord1H.ToLower().EndsWith(TeamId.HomeTeamString) && hasSemiColon;
            IsAwayScoreHTLastGoal = !IsHomeScoreHTLastGoal && hasSemiColon;

            if (_finalHomeScore != 0 &&
                _finalAwayScore != 0)
            {
                IsHomeScoreLastGoal = _goalRecord.Replace(";", string.Empty).ToLower().EndsWith(TeamId.HomeTeamString);
                IsAwayScoreLastGoal = !IsHomeScoreLastGoal;
            }
        }
    }
}

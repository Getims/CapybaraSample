using System.Collections.Generic;
using Main.Scripts.Configs.Cards;
using Main.Scripts.Core.Utilities;
using Main.Scripts.UI.MainMenu.LeaderBoard;
using UnityEngine;

namespace Main.Scripts.GameLogic.External
{
    public interface IRemoteDataService
    {
        int GetTimeNow();
        Sprite GetPlayerIcon();
        string GetPlayerName();
        List<LeaderBoardItemData> GetLeaderBoard(int accountLevel);
        bool TryToCompleteTask(string taskConfigTaskId);
        List<AccountData> GetFriendsData();
        long GetFriendsCount();
        IReadOnlyCollection<CardConfig> GetCardCombo();
    }

    public class RemoteDataServiceTest : IRemoteDataService
    {
        public int GetTimeNow()
        {
            return UnixTime.Now;
        }

        public Sprite GetPlayerIcon()
        {
            return null;
        }

        public string GetPlayerName()
        {
            return "Tester";
        }

        public List<LeaderBoardItemData> GetLeaderBoard(int accountLevel)
        {
            return null;
        }

        public List<AccountData> GetFriendsData()
        {
            return null;
        }

        public long GetFriendsCount()
        {
            return 0;
        }

        public bool TryToCompleteTask(string taskConfigTaskId)
        {
            return true;
        }

        public IReadOnlyCollection<CardConfig> GetCardCombo()
        {
            return null;
        }
    }
}
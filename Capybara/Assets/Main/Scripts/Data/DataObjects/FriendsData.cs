using System;
using System.Collections.Generic;
using Main.Scripts.Data.Core;

namespace Main.Scripts.Data.DataObjects
{
    [Serializable]
    public class FriendsData : GameData
    {
        public List<FriendAccountData> FriendList = new List<FriendAccountData>();
        public bool HasNotification;
        public bool LevelsCheckedToday;
    }
}
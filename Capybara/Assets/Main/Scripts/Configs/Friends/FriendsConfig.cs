using System.Collections.Generic;
using Main.Scripts.Configs.Core;
using Main.Scripts.GameLogic.External;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Friends
{
    public class FriendsConfig : ScriptableConfig
    {
        [SerializeField, MinValue(0)]
        private long _inviteReward = 0;

        [SerializeField, ListDrawerSettings(ShowIndexLabels = true), LabelWidth(65)]
        private List<long> _friendLevelReward = new List<long>();

        [SerializeField,
         ListDrawerSettings(Expanded = false, ShowIndexLabels = true, ListElementLabelName = "AccountName")]
        private List<AccountData> _fakeFriends = new List<AccountData>();

        public long InviteReward => _inviteReward;
        public IReadOnlyCollection<long> FriendLevelReward => _friendLevelReward;
        public IReadOnlyCollection<AccountData> FakeFriends => _fakeFriends;

        public long GetFriendLevelReward(int friendLevel)
        {
            if (friendLevel >= _friendLevelReward.Count)
                return 0;

            return _friendLevelReward[friendLevel];
        }
    }
}
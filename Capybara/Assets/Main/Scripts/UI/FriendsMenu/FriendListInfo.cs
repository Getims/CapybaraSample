using UnityEngine;

namespace Main.Scripts.UI.FriendsMenu
{
    public class FriendListInfo
    {
        public Sprite Icon { get; }
        public string Name { get; }
        public string AccountLevel { get; }
        public long Money { get; }
        public long Reward { get; }

        public FriendListInfo(Sprite icon, string name, string accountLevel, long money, long reward)
        {
            Icon = icon;
            Name = name;
            AccountLevel = accountLevel;
            Money = money;
            Reward = reward;
        }
    }
}
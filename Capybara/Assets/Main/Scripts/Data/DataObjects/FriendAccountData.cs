using System;

namespace Main.Scripts.Data.DataObjects
{
    [Serializable]
    public class FriendAccountData
    {
        public string AccountName;
        public int AccountLevel = 0;

        public FriendAccountData(string accountName, int accountLevel)
        {
            AccountName = accountName;
            AccountLevel = accountLevel;
        }
    }
}
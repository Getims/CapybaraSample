using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Data.Core;
using Main.Scripts.Data.DataObjects;

namespace Main.Scripts.Data.Services
{
    public interface IFriendsDataService
    {
        IReadOnlyCollection<FriendAccountData> FriendList { get; }
        bool LevelsCheckedToday { get; }
        bool HasNotification { get; }
        void SaveData();

        bool UpdateFriendAccount(FriendAccountData accountData, bool autosave = true);
        void AddFriendAccount(FriendAccountData accountData, bool autosave = true);
        void SetNotificationState(bool enabled, bool autosave = true);
        void SetLevelsCheckedState(bool enabled, bool autosave = true);
        long GetFriendsLevelsSum();
    }

    public class FriendsDataService : DataService, IFriendsDataService
    {
        private readonly FriendsData _friendsData;

        public IReadOnlyCollection<FriendAccountData> FriendList => _friendsData.FriendList;
        public bool LevelsCheckedToday => _friendsData.LevelsCheckedToday;
        public bool HasNotification => _friendsData.HasNotification;

        protected FriendsDataService(IDatabase database) : base(database)
        {
            _friendsData = database.GetData<FriendsData>();
        }

        public bool UpdateFriendAccount(FriendAccountData accountData, bool autosave = true)
        {
            FriendAccountData savedData =
                _friendsData.FriendList.FirstOrDefault(fd => fd.AccountName == accountData.AccountName);

            if (savedData == null)
                return false;

            savedData.AccountLevel = accountData.AccountLevel;
            TryToSave(autosave);
            return true;
        }

        public void AddFriendAccount(FriendAccountData accountData, bool autosave = true)
        {
            if (UpdateFriendAccount(accountData, autosave))
                return;

            _friendsData.FriendList.Add(accountData);
            TryToSave(autosave);
        }

        public void SetNotificationState(bool enabled, bool autosave = true)
        {
            if (_friendsData.HasNotification == enabled)
                return;

            _friendsData.HasNotification = enabled;
            TryToSave(autosave);
        }

        public void SetLevelsCheckedState(bool enabled, bool autosave = true)
        {
            if (_friendsData.LevelsCheckedToday == enabled)
                return;

            _friendsData.LevelsCheckedToday = enabled;
            TryToSave(autosave);
        }

        public long GetFriendsLevelsSum()
        {
            long result = 0;
            foreach (FriendAccountData accountData in _friendsData.FriendList)
                result += accountData.AccountLevel;

            return result;
        }

        public void SaveData() => TryToSave(true);
    }
}
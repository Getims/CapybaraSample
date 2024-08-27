using System.Collections.Generic;
using Main.Scripts.Configs.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Accounts
{
    public class AccountConfig : ScriptableConfig
    {
        [SerializeField, ListDrawerSettings(ShowIndexLabels = true), LabelWidth(40)]
        private List<AccountLevelConfig> _accountLevels = new List<AccountLevelConfig>();

        [SerializeField,
         ListDrawerSettings(Expanded = false, ShowIndexLabels = true, ListElementLabelName = "AccountName")]
        private List<FakeAccountData> _fakeAccountsData = new List<FakeAccountData>();

        public IReadOnlyCollection<AccountLevelConfig> AccountLevels => _accountLevels;
        public IReadOnlyCollection<FakeAccountData> FakeAccountsData => _fakeAccountsData;

        public AccountLevelConfig GetAccountLevelConfig(int id)
        {
            if (id >= _accountLevels.Count)
                return null;

            return _accountLevels[id];
        }
    }
}
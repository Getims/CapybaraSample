using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Accounts
{
    [Serializable]
    public class FakeAccountData
    {
        [SerializeField, PreviewField(ObjectFieldAlignment.Left, Height = 60)]
        private Sprite _accountIcon;

        [SerializeField]
        private string _accountName;

        public Sprite AccountIcon => _accountIcon;
        public string AccountName => _accountName;

        public FakeAccountData()
        {
        }

        public FakeAccountData(Sprite accountIcon, string accountName)
        {
            _accountIcon = accountIcon;
            _accountName = accountName;
        }
    }
}
using System;
using UnityEngine;

namespace Main.Scripts.Configs.Global
{
    [Serializable]
    public class LinksConfig
    {
        [SerializeField]
        private string _privacyPolicyLink;

        [SerializeField]
        private string _boostersInfoLink;

        public string PrivacyPolicyLink => _privacyPolicyLink;
        public string BoostersInfoLink => _boostersInfoLink;
    }
}
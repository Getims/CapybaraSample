using System;
using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Configs.Accounts;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.FriendsMenu.BonusInfo
{
    public class BonusInfoContainer : MonoBehaviour
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private List<LevelInfo> _levelInfos = new List<LevelInfo>();

        private bool _isInitialized = false;
        private Action _onCloseClick;

        public void Initialize(AccountConfig accountConfig, IReadOnlyCollection<long> rewards, Action onCloseClick)
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            _onCloseClick = onCloseClick;
            SetupInfo(accountConfig, rewards);
        }

        public void SetActive(bool isActive) =>
            gameObject.SetActive(isActive);

        private void Start() =>
            _closeButton.onClick.AddListener(OnCloseButtonClick);

        private void OnDestroy() =>
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);

        private void SetupInfo(AccountConfig accountConfig, IReadOnlyCollection<long> rewards)
        {
            int infoCount = _levelInfos.Count;
            int rewardsCount = rewards.Count;
            long[] rewardsArray = rewards.ToArray();
            int accountLevelsCount = accountConfig.AccountLevels.Count;

            for (int i = 0; i < infoCount; i++)
            {
                int id = i + 1;
                bool isActive = id < rewardsCount && id < accountLevelsCount;
                if (isActive)
                {
                    AccountLevelConfig levelConfig = accountConfig.GetAccountLevelConfig(id);
                    _levelInfos[i].Initialize(levelConfig.Icon, levelConfig.LevelName, rewardsArray[id],
                        levelConfig.UiGradient);
                }

                _levelInfos[i].SetActive(isActive);
            }
        }

        private void OnCloseButtonClick() =>
            _onCloseClick?.Invoke();
    }
}
using Main.Scripts.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.FriendsMenu
{
    public class FriendInfo : MonoBehaviour
    {
        [SerializeField]
        private Image _accountIcon;

        [SerializeField]
        private TextMeshProUGUI _accountName;

        [SerializeField]
        private TextMeshProUGUI _accountLevel;

        [SerializeField]
        private TextMeshProUGUI _accountMoney;

        [SerializeField]
        private TextMeshProUGUI _reward;

        public void UpdateInfo(FriendListInfo friendListInfo)
        {
            _accountIcon.sprite = friendListInfo.Icon;
            _accountName.text = friendListInfo.Name;
            _accountLevel.text = friendListInfo.AccountLevel;
            _accountMoney.text = $"{MoneyConverter.ConvertToShortValue(friendListInfo.Money)}";
            _reward.text = $"+{MoneyConverter.ConvertToShortValue(friendListInfo.Reward)}";
        }
    }
}
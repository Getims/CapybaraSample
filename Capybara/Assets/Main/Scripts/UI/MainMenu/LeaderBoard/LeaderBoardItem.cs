using Main.Scripts.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.MainMenu.LeaderBoard
{
    public class LeaderBoardItem : MonoBehaviour
    {
        [SerializeField]
        private Sprite _defaultIcon;

        [SerializeField]
        private GameObject _selectedBackground;

        [SerializeField]
        private Image _accountIcon;

        [SerializeField]
        private TextMeshProUGUI _accountName;

        [SerializeField]
        private Image _stockIcon;

        [SerializeField]
        private TextMeshProUGUI _moneyCounter;

        [SerializeField]
        private TextMeshProUGUI _placeCounter;

        public void UpdateInfo(LeaderBoardItemData leaderBoardItemData)
        {
            _selectedBackground.SetActive(leaderBoardItemData.IsCurrentPlayer);
            SetAccountIcon(leaderBoardItemData.AccountIcon);
            _accountName.text = leaderBoardItemData.AccountName;
            _stockIcon.sprite = leaderBoardItemData.StockIcon;
            _moneyCounter.text = MoneyConverter.ConvertToSpaceValue(leaderBoardItemData.Money);
            SetPlace(leaderBoardItemData.Place);
        }

        private void SetAccountIcon(Sprite icon) =>
            _accountIcon.sprite = icon != null ? icon : _defaultIcon;

        private void SetPlace(int place)
        {
            string result = place.ToString();
            if (place > 100)
                result = $"100+";
            if (place > 1000)
                result = $"1000+";
            if (place > 10000)
                result = $"10000+";
            if (place > 100000)
                result = $"100000+";

            _placeCounter.text = result;
        }
    }
}
using Main.Scripts.Configs.Cards;
using Main.Scripts.Core.Enums;
using Main.Scripts.Core.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.CardsMenu.Combo
{
    public class ComboCard : MonoBehaviour
    {
        [SerializeField]
        private Image _foundOutline;

        [SerializeField]
        private Image _closedIcon;

        [SerializeField]
        private Image _cardIconSimple;

        [SerializeField]
        private Image _cardIconFullCard;

        [SerializeField]
        private TextMeshProUGUI _cardTitle;

        public void Initialize(CardConfig cardConfig)
        {
            if (cardConfig == null)
                SetClose();
            else
                SetCard(cardConfig);
        }

        private void SetClose()
        {
            _foundOutline.enabled = false;
            _closedIcon.enabled = true;
            _cardIconSimple.enabled = false;
            _cardIconFullCard.enabled = false;
            _cardTitle.enabled = false;
        }

        private void SetCard(CardConfig cardConfig)
        {
            _foundOutline.enabled = true;
            _closedIcon.enabled = false;
            _cardTitle.enabled = true;
            _cardTitle.text = cardConfig.ShortName;

            if (cardConfig.IconType == CardIconType.JustIcon)
            {
                _cardIconSimple.enabled = true;
                _cardIconSimple.sprite = cardConfig.CardIcon;
                _cardIconFullCard.enabled = false;
            }
            else
            {
                _cardIconSimple.enabled = false;
                _cardIconFullCard.enabled = true;
                _cardIconFullCard.sprite = cardConfig.CardIcon;
                _cardIconFullCard.rectTransform.pivot = Utils.GetSpritePivot(cardConfig.CardIcon);
            }
        }
    }
}
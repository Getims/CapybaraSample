using Coffee.UIEffects;
using Main.Scripts.Core.Constants;
using TMPro;
using UnityEngine;

namespace Main.Scripts.UI.CardsMenu.Cards
{
    public class CardUpgradeButton : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _claimButtonCG;

        [SerializeField]
        private UIEffect _effect;

        [SerializeField]
        private TextMeshProUGUI _title;

        public void Initialize(bool isEnoughMoney, bool isMaxLevel, bool hasCondition, string conditionText)
        {
            if (isMaxLevel)
            {
                SetClaimState(false);
                _effect.enabled = true;
                _title.text = Phrases.CARD_MAX_LEVEL;
                return;
            }

            if (hasCondition)
            {
                SetClaimState(false);
                _effect.enabled = true;
                _title.text = conditionText;
                return;
            }

            if (isEnoughMoney)
            {
                SetClaimState(true);
                _effect.enabled = false;
                _title.text = Phrases.GET;
                return;
            }

            SetClaimState(false);
            _effect.enabled = true;
            _title.text = Phrases.NOT_ENOUGH_MONEY;
        }

        private void SetClaimState(bool enabled)
        {
            _claimButtonCG.alpha = enabled ? 1 : Numbers.NOT_ACTIVE_BUTTON_ALPHA;
            _claimButtonCG.interactable = enabled;
            _claimButtonCG.blocksRaycasts = enabled;
        }
    }
}
using Coffee.UIEffects;
using Main.Scripts.Configs.Accounts;
using Main.Scripts.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.MainMenu.LeaderBoard
{
    public class AccountLevelInfo : MonoBehaviour
    {
        [SerializeField]
        private Image _levelIcon;

        [SerializeField]
        private TextMeshProUGUI _levelName;

        [SerializeField]
        private Image _levelProgress;

        [SerializeField]
        private CanvasGroup _levelProgressCanvas;

        [SerializeField]
        private TextMeshProUGUI _moneyCounter;

        [SerializeField]
        private UIGradient _uiGradient;

        public void UpdateInfo(AccountLevelConfig currentAccountLevel, AccountLevelConfig nextAccountLevel,
            bool isCurrentLevel, long currentMoney)
        {
            SetName(currentAccountLevel.LevelName);
            SetIcon(currentAccountLevel.Icon);
            SetBackground(currentAccountLevel.UiGradient);

            if (!isCurrentLevel)
            {
                SetLevelProgress(-1);
                _moneyCounter.text = $"from {MoneyConverter.ConvertToShortValue(currentAccountLevel.NeedMoney)}";
                return;
            }

            if (nextAccountLevel == null)
            {
                SetLevelProgress(-1);
                SetMoney(currentMoney, -1);
            }
            else
            {
                long needMoney = nextAccountLevel.NeedMoney;
                float progress = 1f * currentMoney / needMoney;
                SetLevelProgress(progress);

                SetMoney(currentMoney, needMoney);
            }
        }

        private void SetName(string name) =>
            _levelName.text = name;

        private void SetIcon(Sprite icon) =>
            _levelIcon.sprite = icon;

        private void SetBackground(UIGradientConfig uiGradientConfig)
        {
            _uiGradient.direction = uiGradientConfig.Direction;
            _uiGradient.color1 = uiGradientConfig.Color1;
            _uiGradient.color2 = uiGradientConfig.Color2;
            _uiGradient.color3 = uiGradientConfig.TopLeft;
            _uiGradient.color4 = uiGradientConfig.TopRight;
            _uiGradient.rotation = uiGradientConfig.Rotation;
            _uiGradient.offset = uiGradientConfig.Offset1;
            _uiGradient.offset2 = new Vector2(uiGradientConfig.Offset2, uiGradientConfig.Offset1);
            _uiGradient.gradientStyle = uiGradientConfig.GradientStyle;
        }

        private void SetLevelProgress(float progress)
        {
            _levelProgress.fillAmount = progress;

            if (progress < 0 || progress > 1)
                _levelProgressCanvas.alpha = 0;
            else
                _levelProgressCanvas.alpha = 1;
        }

        private void SetMoney(long current, long next)
        {
            if (next < 0 || current > next)
            {
                _moneyCounter.text = $"{MoneyConverter.ConvertToShortValue(current)}";
                return;
            }

            _moneyCounter.text =
                $"{MoneyConverter.ConvertToShortValue(current)} / {MoneyConverter.ConvertToShortValue(next)}";
        }
    }
}
using Coffee.UIEffects;
using Main.Scripts.Configs.Accounts;
using Main.Scripts.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.FriendsMenu.BonusInfo
{
    public class LevelInfo : MonoBehaviour
    {
        [SerializeField]
        private Image _levelIcon;

        [SerializeField]
        private TextMeshProUGUI _levelName;

        [SerializeField]
        private TextMeshProUGUI _levelReward;

        [SerializeField]
        private UIGradient _uiGradient;

        public void Initialize(Sprite icon, string name, long reward, UIGradientConfig uiGradientConfig)
        {
            _levelIcon.sprite = icon;
            _levelName.text = name;
            _levelReward.text = $"+{MoneyConverter.ConvertToSpaceValue(reward)}";
            SetBackground(uiGradientConfig);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

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
    }
}
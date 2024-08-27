using Main.Scripts.Configs.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Accounts
{
    public class AccountLevelConfig : ScriptableConfig
    {
        [SerializeField, LabelWidth(120)]
        private string _levelName;

        [SerializeField, Min(0)]
        private long _needMoney;

        [SerializeField, LabelWidth(120), PreviewField(ObjectFieldAlignment.Left, Height = 120)]
        private Sprite _icon;

        [Title("Background Gradient")]
        [SerializeField, HideLabel, InlineProperty]
        private UIGradientConfig _uiGradient;

        public string LevelName => _levelName;
        public Sprite Icon => _icon;
        public long NeedMoney => _needMoney;
        public UIGradientConfig UiGradient => _uiGradient;
    }
}
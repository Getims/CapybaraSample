using System;
using Main.Scripts.Configs.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Global
{
    [Serializable]
    public class AudioClipsListConfig : ScriptableConfig
    {
        [Title("Background")]
        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_gameBackgroundMusic) + ")")]
        private AudioClipConfig _gameBackgroundMusic;

        [Title("UI")]
        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_addCoin) + ")")]
        private AudioClipConfig _addCoin;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_buttonClick) + ")")]
        private AudioClipConfig _buttonClick;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_taskComplete) + ")")]
        private AudioClipConfig _taskComplete;

        [Title("Tap Game")]
        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_tap) + ")")]
        private AudioClipConfig _tap;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_turboTap) + ")")]
        private AudioClipConfig _turboTap;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_turboPick) + ")")]
        private AudioClipConfig _turboPick;

        [Title("Cards")]
        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_cardBought) + ")")]
        private AudioClipConfig _cardBought;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_comboComplete) + ")")]
        private AudioClipConfig _comboComplete;

        public AudioClipConfig GameBackgroundMusic => _gameBackgroundMusic;
        public AudioClipConfig ButtonClick => _buttonClick;
        public AudioClipConfig AddCoin => _addCoin;
        public AudioClipConfig TurboTap => _turboTap;
        public AudioClipConfig Tap => _tap;
        public AudioClipConfig TurboPick => _turboPick;
        public AudioClipConfig TaskComplete => _taskComplete;
        public AudioClipConfig CardBought => _cardBought;
        public AudioClipConfig ComboComplete => _comboComplete;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.SOUND_CATEGORY;

        private string GetLabel(AudioClipConfig clipConfig)
        {
            if (clipConfig.IsDisabled)
                return "- Disabled";
            if (clipConfig.AudioClip == null)
                return "- No Audio";
            return clipConfig.AudioClip.name;
        }
#endif
    }
}
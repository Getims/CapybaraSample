using Main.Scripts.Configs.Global;

namespace Main.Scripts.GameLogic.Sound
{
    public interface ISoundService
    {
        bool IsSoundOn { get; }
        bool IsMusicOn { get; }


        void SwitchSoundState(bool isOn);
        void SwitchMusicState(bool isOn);
        void PlayGameBackgroundMusic();
        void PlayButtonClickSound();
        void PlayOneShot(AudioClipConfig clipConfig);
        void PlayTaskCompleteSound();
        void PlayAddCoinSound();
        void PlayTurboTapSound();
        void PlayTapSound();
        void PlayTurboPickSound();
        void PlayCardBoughtSound();
        void PlayComboCompleteSound();
    }
}
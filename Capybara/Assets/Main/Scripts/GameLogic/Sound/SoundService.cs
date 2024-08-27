using Main.Scripts.Configs.Global;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;

namespace Main.Scripts.GameLogic.Sound
{
    public class SoundService : ISoundService
    {
        private readonly SoundPlayer _soundPlayer;
        private readonly AudioClipsListConfig _audioClipsListConfig;
        private readonly GlobalEventProvider _globalEventProvider;

        public bool IsSoundOn => _soundPlayer.IsSoundOn;
        public bool IsMusicOn => _soundPlayer.IsMusicOn;

        public SoundService(SoundPlayer soundPlayer, ISoundConfigProvider soundConfigProvider,
            GlobalEventProvider globalEventProvider)
        {
            _soundPlayer = soundPlayer;
            _audioClipsListConfig = soundConfigProvider.AudioClipsListConfig;

            _globalEventProvider = globalEventProvider;
            _globalEventProvider.SoundSwitchEvent.Invoke(_soundPlayer.IsSoundOn);
            _globalEventProvider.MusicSwitchEvent.Invoke(_soundPlayer.IsMusicOn);
        }

        public void SwitchSoundState(bool isOn)
        {
            _soundPlayer.SwitchSoundState();
            _globalEventProvider.SoundSwitchEvent.Invoke(isOn);
        }

        public void SwitchMusicState(bool isOn)
        {
            _soundPlayer.SwitchMusicState();
            _globalEventProvider.MusicSwitchEvent.Invoke(isOn);
        }

        public void PlayGameBackgroundMusic() =>
            PlayMusic(_audioClipsListConfig.GameBackgroundMusic);

        public void PlayAddCoinSound() =>
            PlaySound(_audioClipsListConfig.AddCoin);

        public void PlayButtonClickSound() =>
            PlaySound(_audioClipsListConfig.ButtonClick);

        public void PlayTurboTapSound() =>
            PlaySound(_audioClipsListConfig.TurboTap);

        public void PlayTapSound() =>
            PlaySound(_audioClipsListConfig.Tap);

        public void PlayTurboPickSound() =>
            PlaySound(_audioClipsListConfig.TurboPick);

        public void PlayCardBoughtSound() =>
            PlaySound(_audioClipsListConfig.CardBought);

        public void PlayComboCompleteSound() =>
            PlaySound(_audioClipsListConfig.ComboComplete);

        public void PlayTaskCompleteSound() =>
            PlaySound(_audioClipsListConfig.TaskComplete);

        public void PlayOneShot(AudioClipConfig clipConfig) =>
            _soundPlayer.PlaySoundAndCreateAudioSource(clipConfig);

        private void PlayMusic(AudioClipConfig musicConfig) => _soundPlayer.PlayMusic(musicConfig);
        private void PlaySound(AudioClipConfig soundConfig) => _soundPlayer.PlaySound(soundConfig);
    }
}
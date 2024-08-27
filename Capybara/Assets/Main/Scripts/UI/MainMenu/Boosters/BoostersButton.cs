using System;
using Main.Scripts.Core.Constants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.MainMenu.Boosters
{
    public class BoostersButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private TextMeshProUGUI _info;

        [SerializeField]
        private TextMeshProUGUI _info2;

        public event Action OnClick;

        public void SetActive(bool isActive) =>
            gameObject.SetActive(isActive);

        public void SetState(bool interactable, bool fade = false)
        {
            _canvasGroup.interactable = interactable;
            _canvasGroup.blocksRaycasts = interactable;
            _canvasGroup.alpha = fade ? Numbers.NOT_ACTIVE_BUTTON_ALPHA : 1;
        }

        public void SetTitle(string title) =>
            _title.text = title;

        public void SetInfo1(string info) =>
            _info.text = info;

        public void SetInfo2(string info2) =>
            _info2.text = info2;

        private void Start() =>
            _button.onClick.AddListener(OnButtonClick);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClick);

        private void OnButtonClick() => OnClick?.Invoke();
    }
}
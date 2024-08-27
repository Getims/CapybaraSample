using Main.Scripts.Core.Constants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.MainMenu.Info
{
    public class AccountInfoPanel : MonoBehaviour
    {
        [SerializeField]
        private Image _accountIcon;

        [SerializeField]
        private TextMeshProUGUI _accountName;

        public void Initialize(Sprite icon, string name)
        {
            SetIcon(icon);
            SetName(name);
        }

        public void SetIcon(Sprite icon)
        {
            if (icon == null)
            {
                Debug.LogWarning("No account icon!");
                return;
            }

            _accountIcon.sprite = icon;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("No account name!");
                return;
            }

            _accountName.text = $"{name} {Phrases.ACCOUNT_PREFIX}";
        }
    }
}
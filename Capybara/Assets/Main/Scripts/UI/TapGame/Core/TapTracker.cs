using System;
using Main.Scripts.Core.Enums;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main.Scripts.UI.TapGame.Core
{
    public class TapTracker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private TapZone _zone = TapZone.Center;

        public event Action<TapZone> OnTapStart;
        public event Action<TapZone, Vector2> OnTapEnd;

        public void OnPointerDown(PointerEventData eventData) =>
            OnTapStart?.Invoke(_zone);

        public void OnPointerUp(PointerEventData eventData) =>
            OnTapEnd?.Invoke(_zone, eventData.position);
    }
}
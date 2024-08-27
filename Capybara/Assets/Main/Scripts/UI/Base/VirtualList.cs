using System.Collections.Generic;
using UIS;
using UnityEngine;

namespace Main.Scripts.UI.Base
{
    public abstract class VirtualList<T> : MonoBehaviour
    {
        [SerializeField]
        protected RectTransform _container;

        [SerializeField]
        protected Scroller _scroller = null;

        [SerializeField]
        protected int _itemHeight = 150;

        protected bool _wasStart = false;
        protected List<T> _itemsData { get; private set; } = new List<T>();

        public abstract void InitItems(List<T> itemData);

        public void ScrollTo(int index)
        {
            if (index < 0)
                index = 0;
            _scroller.ScrollTo(index);
        }

        protected void Start()
        {
            if (_wasStart)
                return;

            _wasStart = true;
            _scroller.OnFill += OnFillItem;
            _scroller.OnHeight += OnHeightItem;
        }

        protected abstract void OnFillItem(int index, GameObject item);

        protected virtual int OnHeightItem(int index)
        {
            return _itemHeight;
        }
    }
}
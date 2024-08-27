using System;
using System.Collections.Generic;
using Main.Scripts.UI.Base;
using UnityEngine;

namespace Main.Scripts.UI.CardsMenu.Combo
{
    [Serializable]
    public class DotsController
    {
        [SerializeField]
        private List<UIPanel> _dots;

        public void SetDots(int dotsCount)
        {
            for (int i = 0; i < _dots.Count; i++)
            {
                if (i < dotsCount)
                    _dots[i].Show();
                else
                    _dots[i].Hide();
            }
        }
    }
}
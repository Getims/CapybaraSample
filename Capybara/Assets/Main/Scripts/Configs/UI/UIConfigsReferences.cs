using System;
using System.Collections.Generic;
using Main.Scripts.Configs.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.UI
{
    [Serializable]
    public class UIConfigsReferences : ScriptableConfig
    {
        [SerializeField, Required, AssetsOnly]
        private List<ConfigReference> _references = new List<ConfigReference>();

        public List<ConfigReference> References => _references;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.UI_CATEGORY;
#endif
    }
}
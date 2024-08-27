using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Core
{
    public abstract class ScriptableConfig : ScriptableObject
    {
        [TitleGroup("Info")]
        [BoxGroup("Info/In", showLabel: false)]
        public string FileName = "Config";

#if UNITY_EDITOR
        public virtual string GetConfigCategory() =>
            ConfigsCategories.NO_CATEGORY;
#endif
    }
}
﻿using Main.Scripts.Editor.ConfigsEditor.Core;
using UnityEditor;

namespace Main.Scripts.Editor.ConfigsEditor
{
    [InitializeOnLoad]
    public class MenuItems
    {
        [MenuItem(Paths.MENU_PATH + "Open Editor")]
        private static void OpenWindow() =>
            ConfigEditor.OpenEditorWindow();
    }
}
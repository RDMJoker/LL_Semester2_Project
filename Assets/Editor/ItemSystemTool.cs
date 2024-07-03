using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ItemSystemTool : EditorWindow
    {
        int tabIndex;
        readonly string[] tabStrings = {"UniqueBuilder", "DropTableBuilder", "WeightEditor", "DropChanceView", "Simulator"};
        [MenuItem("Window/ItemSystemTool")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ItemSystemTool));
        }

        void OnGUI()
        {
            tabIndex = GUI.Toolbar(new Rect(25, 25, 650, 30), tabIndex, tabStrings);
        }
    }
}
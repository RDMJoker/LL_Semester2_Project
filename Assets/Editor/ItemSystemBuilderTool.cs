using System.Drawing;
using ItemSystem;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ItemSystemBuilderTool : EditorWindow
    {
        int tabIndex;
        readonly string[] tabStrings = { "UniqueBuilder", "DropTableBuilder" };

        [MenuItem("Window/ItemSystemBuilderTool")]
        public static void ShowWindow()
        {
            var window = GetWindow(typeof(ItemSystemBuilderTool));
            window.minSize = new Vector2(650, 500);
        }

        void OnGUI()
        {
            tabIndex = GUI.Toolbar(new Rect(25, 25, 500, 30), tabIndex, tabStrings);
            switch (tabIndex)
            {
                case 0:
                    GUI.Label(new Rect(25, 50, 250, 50), "UniqueBuilder chosen!");
                    break;
                case 1:
                    GUI.Label(new Rect(25, 50, 250, 50), "DropTableBuilder chosen!");
                    break;
                default:
                    break;
            }
        }
    }

    public class ItemSystemEditorTool : EditorWindow
    {
        int tabIndex;
        readonly string[] tabStrings = { "WeightEditor", "Simulator" };

        [MenuItem("Window/ItemSystemEditorTool")]
        public static void ShowWindow()
        {
            var window = GetWindow(typeof(ItemSystemEditorTool));
            window.minSize = new Vector2(650, 500);
        }

        void OnGUI()
        {
            tabIndex = GUI.Toolbar(new Rect(25, 25, 500, 30), tabIndex, tabStrings);
        }
    }
}
using System;
using Generation.DungeonGeneration;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class GenerationTool : EditorWindow
    {
        [SerializeField] VisualTreeAsset uxmlRef;
        DungeonGenerator dungeonGenerator;
        
        static GenerationTool window;

        [MenuItem("Window/GenerationTool %#g")]
        public static void ShowWindow()
        {
            window = GetWindow<GenerationTool>();
            window.minSize = new Vector2(650, 500);
            window.titleContent = new GUIContent("ItemSystemBuilderTool");
        }
        
        void OnEnable()
        {
            dungeonGenerator = FindObjectOfType<DungeonGenerator>();
            var generator = new SerializedObject(dungeonGenerator);
            rootVisualElement.Bind(generator);
        }

        void CreateGUI()
        {
            var root = rootVisualElement;
            uxmlRef.CloneTree(root);

            var propertyField = root.Q<PropertyField>("LevelList");
            propertyField.RegisterValueChangeCallback((_onValueChange => Debug.Log("Change") ));
            var staticSeedToggle = root.Q<Toggle>("StaticSeedToggle");
            staticSeedToggle.RegisterValueChangedCallback((_onValueChange) => Debug.Log("Change"));
            var generateButton = root.Q<Button>("GenerationButton");
            generateButton.RegisterCallback<ClickEvent>((_onClick => Debug.Log("click")));  
        }
    }
}
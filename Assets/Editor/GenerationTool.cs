using System;
using System.Linq;
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
        SerializedObject generator;
        bool overwriteStaticSeedSetting;
        
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
            generator = new SerializedObject(dungeonGenerator);
            rootVisualElement.Bind(generator);
        }

        void CreateGUI()
        {
            var root = rootVisualElement;
            uxmlRef.CloneTree(root);

            var propertyField = root.Q<PropertyField>("LevelList");
            propertyField.RegisterValueChangeCallback((_onValueChange => Debug.Log("Change")));
            var staticSeedToggle = root.Q<Toggle>("StaticSeedToggle");
            staticSeedToggle.RegisterValueChangedCallback((_onValueChange) =>
            {
                overwriteStaticSeedSetting = _onValueChange.newValue;
            });
            var generateButton = root.Q<Button>("GenerationButton");
            generateButton.RegisterCallback<ClickEvent>((_onClick => StartGeneration()));
        }

        void StartGeneration()
        {
            var errorBox = new HelpBox("The list of generation data is empty! Please add at least one generation data!", HelpBoxMessageType.Error);
            if (dungeonGenerator.GenerationData.Count == 0)
            {
                rootVisualElement.Add(errorBox);
            }
            else
            {
                if (overwriteStaticSeedSetting)
                {
                    foreach (var data in dungeonGenerator.GenerationData)
                    {
                        data.UseStaticSeed = true;
                    }
                }
                else
                {
                    foreach (var data in dungeonGenerator.GenerationData)
                    {
                        data.UseStaticSeed = false;
                    }
                }
                if(rootVisualElement.Children().Contains(errorBox)) rootVisualElement.Remove(errorBox);
                dungeonGenerator.GenerateDungeon();
            }
        }
    }
}
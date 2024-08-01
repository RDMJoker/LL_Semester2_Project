using System;
using System.Linq;
using Generation.DungeonGeneration;
using Generation.DungeonGeneration.DungeonGenerationScriptables;
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
        SerializedObject levelGenData;
        ScriptableObject creationInstance;
        Label filePathLabel;
        HelpBox errorBox;
        bool overwriteStaticSeedSetting;
        string generationDataName;

        static GenerationTool window;
        string savePath = "Assets/Scripts/Generation/DungeonGeneration/DungeonGenerationScriptables/";

        [MenuItem("Window/GenerationTool %#g")]
        public static void ShowWindow()
        {
            window = GetWindow<GenerationTool>();
            window.minSize = new Vector2(650, 400);
            window.titleContent = new GUIContent("GenerationTool");
        }

        void OnEnable()
        {
            dungeonGenerator = FindObjectOfType<DungeonGenerator>();
            generator = new SerializedObject(dungeonGenerator);
            rootVisualElement.Bind(generator);

            ResetToEmptyInstance();
        }

        void CreateGUI()
        {
            var root = rootVisualElement;
            uxmlRef.CloneTree(root);

            // var propertyField = root.Q<PropertyField>("LevelList");
            var staticSeedToggle = root.Q<Toggle>("StaticSeedToggle");
            staticSeedToggle.RegisterValueChangedCallback((_onValueChange) => { overwriteStaticSeedSetting = _onValueChange.newValue; });
            var generateButton = root.Q<Button>("GenerationButton");
            generateButton.RegisterCallback<ClickEvent>((_onClick => StartGeneration()));

            #region LevelGenDataEditor

            var editModeToggle = root.Q<Toggle>("EditModeToggle");
            var editObjectField = root.Q<ObjectField>("EditObjectField");
            // var seedField = root.Q<IntegerField>("SeedField");
            // var generationWidthField = root.Q<IntegerField>("GenerationWidthField");
            // var generationHeightField = root.Q<IntegerField>("GenerationHeightField");
            // var minRoomCountField = root.Q<IntegerField>("MinRoomCountField");
            // var gridCellSizeField = root.Q<IntegerField>("GridCellSizeField");
            // var roomIncrementField = root.Q<FloatField>("RoomIncrementField");
            // var tilesetField = root.Q<ObjectField>("TilesetField");
            var openFolderPathChoosing = root.Q<ToolbarButton>("OpenFolderPathChoosingButton");
            filePathLabel = root.Q<Label>("FilePathLabel");
            string cutString = savePath.Split("Assets")[1];
            savePath = "Assets" + cutString;
            filePathLabel.text = "Chosen save path: " + savePath;
            var generateLevelDataButton = root.Q<Button>("GenerateLevelDataButton");
            var generationDataNameField = root.Q<TextField>("GenerationDataNameField");
            generationDataNameField.RegisterValueChangedCallback((_onValueChange) => generationDataName = _onValueChange.newValue);
            editModeToggle.RegisterValueChangedCallback((_onValueChange =>
            {
                editObjectField.SetEnabled(_onValueChange.newValue);
                if (_onValueChange.newValue)
                {
                    generationDataNameField.style.display = DisplayStyle.None;
                    editObjectField.style.display = DisplayStyle.Flex;
                }
                else
                {
                    generationDataNameField.style.display = DisplayStyle.Flex;
                    editObjectField.style.display = DisplayStyle.None;
                }
                generateLevelDataButton.SetEnabled(!_onValueChange.newValue);
                if (_onValueChange.newValue) CopyDataFromGenDataToInstance((ScriptableObject)editObjectField.value);
                else ResetToEmptyInstance();
            }));
            generationDataNameField.style.display = DisplayStyle.Flex;
            editObjectField.style.display = DisplayStyle.None;
            editObjectField.RegisterValueChangedCallback((_onValueChange => CopyDataFromGenDataToInstance((ScriptableObject)_onValueChange.newValue)));
            openFolderPathChoosing.RegisterCallback<ClickEvent>((_ => SetFilePath() ));
            generateLevelDataButton.RegisterCallback<ClickEvent>((_ => CreateLevelGenData()));

            #endregion
        }

        void SetFilePath()
        {
            savePath = EditorUtility.OpenFolderPanel("Choose folder", "Assets", "");
            string cutString = savePath.Split("Assets")[1];
            savePath = "Assets" + cutString + "/";
            filePathLabel.text = "Chosen save path: " + savePath;
        }
        
        void CreateLevelGenData()
        {
            if (creationInstance == null)
            {
                ResetToEmptyInstance();
            }
            AssetDatabase.CreateAsset((LevelGenerationData)creationInstance, savePath + generationDataName + ".asset");
        }

        void CopyDataFromGenDataToInstance(ScriptableObject _object)
        {
            if (_object == null) return;
            creationInstance = _object;
            levelGenData = new SerializedObject(creationInstance);
            rootVisualElement.Bind(levelGenData);
        }

        void ResetToEmptyInstance()
        {
            if (creationInstance == null)
            {
                creationInstance = CreateInstance(typeof(LevelGenerationData));
                levelGenData = new SerializedObject(creationInstance);
                rootVisualElement.Bind(levelGenData);
                return;
            }

            string json = JsonUtility.ToJson((LevelGenerationData)creationInstance);
            creationInstance = CreateInstance(typeof(LevelGenerationData));
            JsonUtility.FromJsonOverwrite(json, (LevelGenerationData)creationInstance);
            levelGenData = new SerializedObject(creationInstance);
            rootVisualElement.Bind(levelGenData);
        }

        void StartGeneration()
        {
            errorBox = new HelpBox("The list of generation data is empty! Please add at least one generation data!", HelpBoxMessageType.Error);
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

                if (rootVisualElement.Children().Contains(errorBox)) rootVisualElement.Remove(errorBox);
                dungeonGenerator.GenerateDungeon();
            }
        }
    }
}
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
            window.minSize = new Vector2(650, 450);
            window.titleContent = new GUIContent("GenerationTool");
        }

        void OnEnable()
        {
            errorBox = new HelpBox("", HelpBoxMessageType.Error);
            dungeonGenerator = FindObjectOfType<DungeonGenerator>();
            if (dungeonGenerator == null)
            {
                errorBox.text = "No Dungeon Generator Found! Please switch to the correct scene and reopen the window! Correct scene: 'Generation' ";
                rootVisualElement.Add(errorBox);
                return;
            }
            generator = new SerializedObject(dungeonGenerator);
            rootVisualElement.Bind(generator);

            ResetToEmptyInstance();
        }

        void CreateGUI()
        {
            var root = rootVisualElement;
            uxmlRef.CloneTree(root);
            if (errorBox.text == "No Item Builder Found! Please switch to the correct scene and reopen the window! Correct scene: 'ItemSystem' ") return;
            
            // Get and Register the two VisualElements related to generating the dungeon
            var staticSeedToggle = root.Q<Toggle>("StaticSeedToggle");
            staticSeedToggle.RegisterValueChangedCallback((_onValueChange) => { overwriteStaticSeedSetting = _onValueChange.newValue; });
            var generateButton = root.Q<Button>("GenerationButton");
            generateButton.RegisterCallback<ClickEvent>((_onClick => StartGeneration()));

            #region LevelGenDataEditor

            // Get and Register all Visual Elements related to creating new LevelData ScriptableObjects
            var editModeToggle = root.Q<Toggle>("EditModeToggle");
            var editObjectField = root.Q<ObjectField>("EditObjectField");
            var openFolderPathChoosing = root.Q<ToolbarButton>("OpenFolderPathChoosingButton");
            
            // Cut the savePath to fit the required format for the AssetDatabase function
            filePathLabel = root.Q<Label>("FilePathLabel");
            string cutString = savePath.Split("Assets")[1];
            savePath = "Assets" + cutString;
            filePathLabel.text = "Chosen save path: " + savePath;
            
            // Continue to register Visual Elements.
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

        /// <summary>
        /// Function that prompts the user to choose a save file path. This path is then used to save created Assets.
        /// </summary>
        void SetFilePath()
        {
            savePath = EditorUtility.OpenFolderPanel("Choose folder", "Assets", "");
            string cutString = savePath.Split("Assets")[1];
            savePath = "Assets" + cutString + "/";
            filePathLabel.text = "Chosen save path: " + savePath;
        }
        
        /// <summary>
        /// Function to create a new Asset from the currently loaded instance.
        /// </summary>
        void CreateLevelGenData()
        {
            if (creationInstance == null)
            {
                ResetToEmptyInstance();
            }
            AssetDatabase.CreateAsset((LevelGenerationData)creationInstance, savePath + generationDataName + ".asset");
        }

        /// <summary>
        /// Function to copy the data of a existing ScriptableObject of the instance type to the current instance of the SerializedObject.
        /// </summary>
        /// <param name="_object"></param>
        void CopyDataFromGenDataToInstance(ScriptableObject _object)
        {
            if (_object == null) return;
            creationInstance = _object;
            levelGenData = new SerializedObject(creationInstance);
            rootVisualElement.Bind(levelGenData);
        }

        /// <summary>
        /// Resets the SerializedObject to a new, empty Instance. This should only be called everytime the old instance does no longer exist.
        /// </summary>
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
        
        /// <summary>
        /// Starts the generation.
        /// </summary>
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

                if (rootVisualElement.Contains(errorBox)) rootVisualElement.Remove(errorBox);
                dungeonGenerator.GenerateDungeon();
            }
        }
    }
}
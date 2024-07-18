using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemSystem;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class ItemSystemBuilderTool : EditorWindow
    {
        [SerializeField] VisualTreeAsset uxmlRef;

        UniqueItemPrefabBuilder builder;
        SerializedObject serializedBuilderObject;

        DropdownField uniqueDropDown;
        DropdownField typeDropDown;
        TextField textField;
        ObjectField meshObjectField;
        ObjectField materialObjectField;

        VisualElement firstTab;
        VisualElement secondTab;

        ToolbarButton firstTabButton;
        ToolbarButton secondTabButton;

        ScriptableObject dropTableInstance;
        SerializedObject dropTableObject;

        static ItemSystemBuilderTool window;

        const string SavePath = "Assets/ItemSystem/ItemSystemScriptables/ItemTypes/DropTables/";
        string dropTableName = "";

        List<Type> typeList = new();
        // ObjectField uniqueItemHolderObjectField = null;
        // Label uniqueItemHolderLabel = null;
        // ObjectField tierDataObjectField = null;
        // Label tierDataLabel = null;

        Dictionary<EItemStat, int> itemStats = new Dictionary<EItemStat, int>()
        {
            { EItemStat.Health, 0 },
            { EItemStat.Mana, 0 },
            { EItemStat.AttackSpeed, 0 },
            { EItemStat.DamageFlat, 0 },
            { EItemStat.DamagePercent, 0 },
            { EItemStat.MovementSpeed, 0 }
        };

        int baseDamage = 0;
        int baseAttackSpeed = 0;
        int baseArmor = 0;

        [MenuItem("Window/ItemSystemBuilderTool %#i")]
        public static void ShowWindow()
        {
            window = GetWindow<ItemSystemBuilderTool>();
            window.minSize = new Vector2(650, 500);
            window.titleContent = new GUIContent("ItemSystemBuilderTool");
        }

        void OnEnable()
        {
            builder = FindObjectOfType<UniqueItemPrefabBuilder>();
            typeList = new List<Type>(builder.itemTypes);
            serializedBuilderObject = new SerializedObject(builder);

            dropTableInstance = CreateInstance(typeof(ItemTypeDropTable));
            dropTableObject = new SerializedObject(dropTableInstance);
            rootVisualElement.Bind(dropTableObject);
        }

        void CreateGUI()
        {
            var root = rootVisualElement;
            uxmlRef.CloneTree(root);
            root.Bind(serializedBuilderObject);

            firstTab = root.Q<VisualElement>("ItemBuilderTab");
            secondTab = root.Q<VisualElement>("DroptableBuilderTab");

            firstTabButton = root.Q<ToolbarButton>("ItemBuilderTabButton");
            firstTabButton.style.color = Color.green;
            secondTabButton = root.Q<ToolbarButton>("DroptableBuilderTabButton");

            firstTabButton.RegisterCallback<ClickEvent>(_onClick => SwitchTabs(1));
            secondTabButton.RegisterCallback<ClickEvent>(_onClick => SwitchTabs(2));

            #region FirstTab

            textField = root.Q<TextField>("UniqueNameTextField");
            meshObjectField = root.Q<ObjectField>("MeshField");
            materialObjectField = root.Q<ObjectField>("MaterialField");

            var weaponStatsFoldout = root.Q<Foldout>("WeaponStats");
            var armorStatsFoldout = root.Q<Foldout>("ArmorStats");
            ToggleSpecificGroup(armorStatsFoldout, false);
            ToggleSpecificGroup(weaponStatsFoldout, false);


            var healthField = root.Q<IntegerField>("HealthTextField");
            var manaField = root.Q<IntegerField>("ManaTextField");
            var attackSpeedField = root.Q<IntegerField>("AttackSpeedField");
            var damageFlatField = root.Q<IntegerField>("FlatDamageField");
            var damagePercentField = root.Q<IntegerField>("PercentDamageField");
            var movementSpeedField = root.Q<IntegerField>("MovementSpeedField");
            var baseArmorField = root.Q<IntegerField>("BaseArmor");
            var baseDamageField = root.Q<IntegerField>("BaseDamage");
            var baseAttackSpeedField = root.Q<IntegerField>("BaseAttackSpeed");

            healthField.RegisterValueChangedCallback((_onValueChange) => ChangeDictionaryValue(EItemStat.Health, _onValueChange.newValue));
            manaField.RegisterValueChangedCallback((_onValueChange) => ChangeDictionaryValue(EItemStat.Mana, _onValueChange.newValue));
            attackSpeedField.RegisterValueChangedCallback((_onValueChange) => ChangeDictionaryValue(EItemStat.AttackSpeed, _onValueChange.newValue));
            damageFlatField.RegisterValueChangedCallback((_onValueChange) => ChangeDictionaryValue(EItemStat.DamageFlat, _onValueChange.newValue));
            damagePercentField.RegisterValueChangedCallback((_onValueChange) => ChangeDictionaryValue(EItemStat.DamagePercent, _onValueChange.newValue));
            movementSpeedField.RegisterValueChangedCallback((_onValueChange) => ChangeDictionaryValue(EItemStat.MovementSpeed, _onValueChange.newValue));
            baseArmorField.RegisterValueChangedCallback((_onValueChange) => baseArmor = _onValueChange.newValue);
            baseDamageField.RegisterValueChangedCallback((_onValueChange) => baseDamage = _onValueChange.newValue);
            baseAttackSpeedField.RegisterValueChangedCallback((_onValueChange) => baseAttackSpeed = _onValueChange.newValue);


            uniqueDropDown = root.Q<DropdownField>("UniqueTierDropDown");

            string[] test = AssetDatabase.FindAssets("t: UniqueItemHolder");
            for (int i = 0; i < test.Length; i++)
            {
                uniqueDropDown.choices.Add("Tier: " + i);
            }


            typeDropDown = root.Q<DropdownField>("ScriptTypeDropDown");
            foreach (var type in typeList)
            {
                typeDropDown.choices.Add(type.ToString().Replace("ItemSystem.", ""));
            }

            typeDropDown.RegisterValueChangedCallback((_onValueChange) =>
            {
                if (ItemTypeDictionaries.IsWeapon(typeList[typeDropDown.index]))
                {
                    ToggleSpecificGroup(armorStatsFoldout, false);
                    ToggleSpecificGroup(weaponStatsFoldout, true);
                }
                else
                {
                    ToggleSpecificGroup(weaponStatsFoldout, false);
                    ToggleSpecificGroup(armorStatsFoldout, true);
                }
            });

            var generationButton = root.Q<Button>("GenerationButton");
            generationButton.RegisterCallback<ClickEvent>((_onClick) => Generate());

            #endregion

            #region SecondTab

            var droptableField = root.Q<PropertyField>("DroptableField");

            var droptableButton = root.Q<Button>("DroptableButton");
            droptableButton.RegisterCallback<ClickEvent>((_onClick => CreateDropTable()));

            var deleteDroptableButton = root.Q<Button>("DeleteDroptableButton");
            deleteDroptableButton.RegisterCallback<ClickEvent>((_onClick => DeleteDropTable()));

            var droptableNameField = root.Q<TextField>("DroptableName");
            droptableNameField.RegisterValueChangedCallback((_onValueChange => dropTableName = _onValueChange.newValue));

            #endregion
        }

        void ChangeDictionaryValue(EItemStat _stat, int _value)
        {
            itemStats[_stat] = _value;
        }

        void Generate()
        {
            List<string> errorStringList = new();
            if (textField.value == string.Empty || textField.value.Trim() == string.Empty)
            {
                errorStringList.Add("Item name cannot be empty or just consist of spaces!");
            }

            if (meshObjectField.value == null)
            {
                errorStringList.Add("Mesh cannot be empty!");
            }

            if (materialObjectField.value == null)
            {
                errorStringList.Add("Material cannot be empty!");
            }

            if (typeDropDown.index < 0)
            {
                errorStringList.Add("Type cannot be empty!");
            }

            if (uniqueDropDown.index < 0)
            {
                errorStringList.Add("Unique Tier cannot be empty!");
            }

            if (errorStringList.Count > 0)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("Please fix the following errors: \n");
                foreach (string error in errorStringList)
                {
                    if (error == errorStringList.Last())
                    {
                        stringBuilder.Append(error);
                    }
                    else
                    {
                        stringBuilder.Append(error + "\n");
                    }
                }

                rootVisualElement.Add(new HelpBox(stringBuilder.ToString(), HelpBoxMessageType.Error));
            }
            else
            {
                builder.CreateItemData(typeList[typeDropDown.index], itemStats);
                if (ItemTypeDictionaries.IsWeapon(typeList[typeDropDown.index]))
                {
                    builder.CreatePrefab(textField.value, typeList[typeDropDown.index], (Mesh)meshObjectField.value, (Material)materialObjectField.value, uniqueDropDown.index, _baseDamage: baseDamage, _baseAttackSpeed: baseAttackSpeed);
                }
                else
                {
                    builder.CreatePrefab(textField.value, typeList[typeDropDown.index], (Mesh)meshObjectField.value, (Material)materialObjectField.value, uniqueDropDown.index, _baseDefence: baseArmor);
                }
            }
        }

        void CreateDropTable()
        {
            AssetDatabase.CreateAsset(dropTableInstance, SavePath + dropTableName + ".asset");
        }

        void DeleteDropTable()
        {
            rootVisualElement.Unbind();
            dropTableInstance = CreateInstance(typeof(ItemTypeDropTable));
            dropTableObject = new SerializedObject(dropTableInstance);
            rootVisualElement.Bind(dropTableObject);
            AssetDatabase.DeleteAsset(SavePath + dropTableName + ".asset");
        }

        void ToggleSpecificGroup(VisualElement _element, bool _show)
        {
            _element.style.display = _show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        void SwitchTabs(int _targetTab)
        {
            switch (_targetTab)
            {
                case 1:
                    ToggleSpecificGroup(secondTab, false);
                    ToggleSpecificGroup(firstTab, true);
                    secondTabButton.style.color = Color.white;
                    firstTabButton.style.color = Color.green;
                    break;
                case 2:
                    ToggleSpecificGroup(firstTab, false);
                    ToggleSpecificGroup(secondTab, true);
                    firstTabButton.style.color = Color.white;
                    secondTabButton.style.color = Color.green;
                    break;
            }
        }

        [Shortcut("Window/Close", KeyCode.W, ShortcutModifiers.Control)]
        static void CloseTab(ShortcutArguments _args)
        {
            if (window == null) return;
            window.Close();
        }

        // void InitCustomFieldGroupWithLabel(VisualElement _parentElement, VisualElement _root, Type _objectFieldType, ref ObjectField _field, ref Label _label)
        // {
        //     var childs = _parentElement.Children();
        //     foreach (var element in childs)
        //     {
        //         Debug.Log(element.name);
        //         switch (element)
        //         {
        //             case ObjectField:
        //                 _field = _root.Q<ObjectField>(element.name);
        //                 _field.objectType = _objectFieldType;
        //                 break;
        //             case Label:
        //                 _label = _root.Q<Label>(element.name);
        //                 break;
        //         }
        //     }
        // }

        // void SetTextElement(TextElement _label, string _value)
        // {
        //     _label.text = _value;
        // }
    }
}
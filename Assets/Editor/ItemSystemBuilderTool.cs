using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class ItemSystemBuilderTool : EditorWindow
    {
        //TODO: Summaries for every function(especially the last 3)
        [SerializeField] VisualTreeAsset uxmlRef;

        UniqueItemPrefabBuilder builder;
        SerializedObject serializedBuilderObject;

        DropdownField uniqueDropDown;
        DropdownField typeDropDown;
        TextField textField;
        ObjectField meshObjectField;
        ObjectField materialObjectField;

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

        [MenuItem("Window/ItemSystemBuilderTool")]
        public static void ShowWindow()
        {
            var window = GetWindow<ItemSystemBuilderTool>();
            window.minSize = new Vector2(650, 500);
            window.titleContent = new GUIContent("ItemSystemBuilderTool");
        }

        void OnEnable()
        {
            builder = FindObjectOfType<UniqueItemPrefabBuilder>();
            typeList = new List<Type>(builder.itemTypes);
            serializedBuilderObject = new SerializedObject(builder);
        }

        void CreateGUI()
        {
            var root = rootVisualElement;
            uxmlRef.CloneTree(root);
            root.Bind(serializedBuilderObject);


            textField = root.Q<TextField>("UniqueNameTextField");
            textField.Q("unity-text-input").style.unityTextAlign = TextAnchor.MiddleCenter;


            meshObjectField = root.Q<ObjectField>("MeshField");
            meshObjectField.Q<Label>().style.unityTextAlign = TextAnchor.MiddleRight;

            materialObjectField = root.Q<ObjectField>("MaterialField");
            materialObjectField.Q<Label>().style.unityTextAlign = TextAnchor.MiddleRight;
            var weaponStatsFoldout = root.Q<Foldout>("WeaponStats");
            var armorStatsFoldout = root.Q<Foldout>("ArmorStats");
            ToggleSpecificStatGroup(armorStatsFoldout, false);
            ToggleSpecificStatGroup(weaponStatsFoldout, false);

            
            //TODO: TextFields => IntFields (like wtf)
            var healthField = root.Q<TextField>("HealthTextField");
            var manaField = root.Q<TextField>("ManaTextField");
            var attackSpeedField = root.Q<TextField>("AttackSpeedField");
            var damageFlatField = root.Q<TextField>("FlatDamageField");
            var damagePercentField = root.Q<TextField>("PercentDamageField");
            var movementSpeedField = root.Q<TextField>("MovementSpeedField");
            var baseArmorField = root.Q<TextField>("BaseArmor");
            var baseDamageField = root.Q<TextField>("BaseDamage");
            var baseAttackSpeedField = root.Q<TextField>("BaseAttackSpeed");

            healthField.RegisterValueChangedCallback((_onValueChange) =>
            {
                int.TryParse(_onValueChange.newValue.ToString(), out int value);
                ChangeDictionaryValue(EItemStat.Health,value);
            });
            

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
                if (ItemTypeDictionary.IsWeapon(typeList[typeDropDown.index]))
                {
                    ToggleSpecificStatGroup(armorStatsFoldout, false);
                    ToggleSpecificStatGroup(weaponStatsFoldout, true);
                }
                else
                {
                    ToggleSpecificStatGroup(weaponStatsFoldout, false);
                    ToggleSpecificStatGroup(armorStatsFoldout, true);
                }
            });

            var generationButton = root.Q<Button>("GenerationButton");
            generationButton.RegisterCallback<ClickEvent>((_onClick) => Generate());
        }

        void ChangeDictionaryValue(EItemStat _stat,int _value)
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

        void ToggleSpecificStatGroup(VisualElement _element, bool _show)
        {
            _element.style.display = _show ? DisplayStyle.Flex : DisplayStyle.None;
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
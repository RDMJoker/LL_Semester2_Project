using System;
using System.Collections.Generic;
using System.Linq;
using ItemSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class ItemSystemSimulationTool : EditorWindow
    {
        [SerializeField] VisualTreeAsset uxmlRef;

        static ItemSystemSimulationTool window;

        ScrollView scrollView;
        ScrollView simulationScrollView;

        ItemDropper dropper;
        ItemTypeDropTable currentDropTable;

        HelpBox errorBox;
        
        WeightedListManager weightedListManager;
        
        [MenuItem("Window/ItemSystemSimulationTool")]
        public static void ShowWindow()
        {
            window = GetWindow<ItemSystemSimulationTool>();
            window.minSize = new Vector2(650, 300);
            window.titleContent = new GUIContent("ItemSystemBuilderTool");
        }

        void OnEnable()
        {
            dropper = FindObjectOfType<ItemDropper>();
            var dropperObject = new SerializedObject(dropper);
            rootVisualElement.Bind(dropperObject);

            weightedListManager = new WeightedListManager();
            errorBox = new HelpBox("Please first choose a drop table!", HelpBoxMessageType.Error);
        }

        void CreateGUI()
        {
            var root = rootVisualElement;
            uxmlRef.CloneTree(root);
            scrollView = root.Q<ScrollView>("ScrollView");
            simulationScrollView = root.Q<ScrollView>("SimulationScrollView");
            var dropTableField = root.Q<ObjectField>("DropTableField");
            var simulateButton = root.Q<Button>("SimulateButton");
            
            
            
            dropTableField.RegisterValueChangedCallback((_onValueChange => UpdateDisplayList((ItemTypeDropTable)_onValueChange.newValue)));
            simulateButton.RegisterCallback<ClickEvent>((_onClick) => SimulateItemDrops());
        }

        void SimulateItemDrops()
        {
            if (currentDropTable == null)
            {
                rootVisualElement.Add(errorBox);
                return;
            }
            simulationScrollView.Clear();
            var simulatedItems = dropper.SimulateItemDrop(10,currentDropTable);
            foreach (EItemType entry in Enum.GetValues(typeof(EItemType)))
            {
                if (entry == EItemType.Debug) continue;
                if (!simulatedItems.ContainsKey(entry)) continue;
                simulationScrollView.Add(new Label($"{entry} was dropped {simulatedItems[entry]} times!"));
            }
        }

        void UpdateDisplayList(ItemTypeDropTable _currentObject)
        {
            scrollView.Clear();
            if (rootVisualElement.Contains(errorBox)) rootVisualElement.Remove(errorBox);
            if (_currentObject == null) return;
            currentDropTable = _currentObject;
            weightedListManager.ResetWeights(_currentObject);
            var localList = new List<ItemType>();
            foreach (var data in _currentObject.DropTable)
            {
                if (data.Value == 0 || data.Value == data.Key.Weight) localList.Add(data.Key);
                else
                {
                    data.Key.Weight = data.Value;
                    localList.Add(data.Key);
                }
            }
            
            localList.Sort((_a,_b) => _a.Weight.CompareTo(_b.Weight));
            int totalWeight = localList.Sum(_entry => _entry.Weight);

            foreach (var entry in localList)
            {
                Debug.Log(entry.Weight + "|" + totalWeight);
                float decimalValue = (float)entry.Weight / totalWeight; 
                float percentageValue = decimalValue * 100;
                scrollView.Add(new Label("The type " + entry.Type + " has a drop chance of: " + percentageValue.ToString("0.00") + "%"));
            }
        }
    }
}
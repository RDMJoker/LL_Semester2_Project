using ItemSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class UIToolKitStarterTest : EditorWindow
    {
        [SerializeField] VisualTreeAsset uxmlRef;
        UniqueItemPrefabBuilder builder;
        SerializedProperty serializedProperty;
        SerializedProperty secondSerializedProperty;
        SerializedObject serializedObject;
        SerializedObject secondSerializedObject;

        [MenuItem("Window/UIToolkitTest")]
        public static void ShowWindow()
        {
            var window = GetWindow<UIToolKitStarterTest>();
            window.minSize = new Vector2(650, 500);
            window.titleContent = new GUIContent("Yay");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            uxmlRef.CloneTree(root);

            PropertyField field = root.Q<PropertyField>("PropertyField1");
            field.RegisterValueChangeCallback((_onChangeEvent) => TestMethod(_onChangeEvent.changedProperty.intValue));
            PropertyField field2 = root.Q<PropertyField>("PropertyField2");
            //   field2.Bind(secondSerializedObject);
            PropertyField field3 = root.Q<PropertyField>("PropertyField3");
            // field3.Bind(secondSerializedObject);

            PropertyField field4 = root.Q<PropertyField>("PropertyField3");
            GroupBox properties = root.Q<GroupBox>("PropertyFields");

            // Use Legacy Stuff
            IMGUIContainer container = root.Q<IMGUIContainer>("Container");
            // container.onGUIHandler = new Action(GUI CODE)
            
            Toggle toggle = root.Q<Toggle>("Toggle");
            toggle.RegisterValueChangedCallback((_onValueChanged) => DisableGroup(properties, _onValueChanged.newValue));
            
            root.Bind(serializedObject);
            root.Bind(secondSerializedObject);
        }

        void DisableGroup(VisualElement _element, bool _isOn)
        {
            switch (_isOn)
            {
                case true:
                    _element.style.display = DisplayStyle.Flex;
                    break;
                case false:
                    _element.style.display = DisplayStyle.None;
                    break;
            }
        }
        
        void OnEnable()
        {
            UniqueItemHolder holder = CreateInstance<UniqueItemHolder>();
            serializedObject = new SerializedObject(holder);

            serializedProperty = serializedObject.FindProperty("Tier");

            Debug.Log("Int: " + serializedProperty.intValue);

            builder = FindObjectOfType<UniqueItemPrefabBuilder>();
            secondSerializedObject = new SerializedObject(builder);
        }


        void TestMethod(int _int)
        {
            
        }
    }
}
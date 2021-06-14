using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

namespace GameFramework.Core
{
    [CustomPropertyDrawer(typeof(testEv),false)]
    public class EventSenderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);

            Debug.Log("OnGUI");
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {

            Debug.Log("CreatePropertyGUI");

            var container = new VisualElement();

            PropertyField eventTypeField = new PropertyField(property.FindPropertyRelative("eventType"));
            container.Add(eventTypeField);

            return container;
        }
    }
}

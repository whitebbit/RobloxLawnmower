﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace GBGamesPlugin
{
    [CustomPropertyDrawer(typeof(ConditionallyVisibleAttribute))]
    public class ConditionallyVisiblePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!ShouldDisplay(property)) return;
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUI.PropertyField(position, property, label, includeChildren: true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ShouldDisplay(property) 
                ? EditorGUI.GetPropertyHeight(property, label, includeChildren: true) 
                : 0;
        }

        private bool ShouldDisplay(SerializedProperty property)
        {
            var attr = (ConditionallyVisibleAttribute)attribute;
            var dependentProp = property.serializedObject.FindProperty(attr.propertyName);
            return dependentProp.boolValue;
        }
    }
}
#endif
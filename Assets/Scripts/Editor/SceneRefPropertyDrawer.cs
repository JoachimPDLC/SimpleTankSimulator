using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SceneRef))]
public class SceneRefPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        SerializedProperty sceneProperty = property.FindPropertyRelative("Scene");
        SerializedProperty nameProperty = property.FindPropertyRelative("Name");

        int indentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorGUI.ObjectField(position, sceneProperty, typeof(SceneAsset), GUIContent.none);
        nameProperty.stringValue = (sceneProperty.objectReferenceValue as SceneAsset)?.name ?? string.Empty;
        property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indentLevel;

        EditorGUI.EndProperty();
    }
}

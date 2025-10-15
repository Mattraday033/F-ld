using System.Collections;
using UnityEditor;
using static UnityEditor.PropertyDrawer;
using UnityEngine;

[CustomPropertyDrawer(typeof(InterfaceReference<>), true)]
public class InterfaceReferencePropertyDrawer : PropertyDrawer
{
    SerializedProperty property_Target;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        property_Target = property.FindPropertyRelative("m_Target");
        Rect rect = new(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(property_Target));
        EditorGUI.ObjectField(rect, property_Target, GetInterfaceType(), label);
        EditorGUI.EndProperty();
    }
   

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    System.Type GetInterfaceType()
    {
        System.Type type = fieldInfo.FieldType;
        System.Type[] typeArguments = type.GenericTypeArguments;
        if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            typeArguments = fieldInfo.FieldType.GenericTypeArguments[0].GenericTypeArguments;
        }
        if (typeArguments == null || typeArguments.Length == 0)
            return null;
        return typeArguments[0];
    }
}
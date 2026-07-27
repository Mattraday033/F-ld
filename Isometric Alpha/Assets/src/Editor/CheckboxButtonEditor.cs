using UnityEditor;

[CustomEditor(typeof(CheckboxButton))]
public class CheckboxButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CheckboxButton targetCheckboxButton = (CheckboxButton) target;

        // Show default inspector property editor
        DrawDefaultInspector();
    }
}
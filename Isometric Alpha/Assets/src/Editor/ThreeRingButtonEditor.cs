using UnityEditor;

[CustomEditor(typeof(ThreeRingButton))]
public class ThreeRingButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ThreeRingButton targetThreeRingButton = (ThreeRingButton) target;

        // targetThreeRingButton.bottomRingImage = EditorGUILayout.Toggle("Accepts pointer input", targetThreeRingButton.bottomRingImage);
        // targetThreeRingButton.middleRingImage;
        // targetThreeRingButton.topRingImage;

        // targetThreeRingButton.iconBackground;
        // targetThreeRingButton.icon;
        // targetThreeRingButton.iconText;

        // Show default inspector property editor
        DrawDefaultInspector();
    }
}
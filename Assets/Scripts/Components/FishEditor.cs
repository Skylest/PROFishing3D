using UnityEditor;

[CustomEditor(typeof(FishScriptableObject))]
public class FishEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Ваш текст напоминания
        EditorGUILayout.HelpBox("Coefficient:\nCommon = 1\nUncommon = 1.25,\nRare = 1.5,\nMythical = 2,\nLegendary = 3", MessageType.None);
        EditorGUILayout.HelpBox("Probability here is used for weighted random selection. Each item's probability weight contributes to its chance of being selected. Higher weights increase the likelihood of selection.", MessageType.None);

        // Отображение остальных полей объекта
        base.OnInspectorGUI();
    }
}

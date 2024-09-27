using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PickupRespawnerBroom))]
public class GameSettingsEditor : Editor
{
    // Serialized properties
    SerializedProperty gameModeProp;
    SerializedProperty scoreTextProp;
    SerializedProperty scoreTextMeshProp;
    SerializedProperty throwStartPositionProp;
    SerializedProperty isThrownProp;
    SerializedProperty scoreTextStringProp;

    private void OnEnable()
    {
        // Link the serialized properties to the actual fields
        gameModeProp = serializedObject.FindProperty("gameMode");
        scoreTextProp = serializedObject.FindProperty("scoreText");
        scoreTextMeshProp = serializedObject.FindProperty("scoreTextMesh");
        throwStartPositionProp = serializedObject.FindProperty("throwStartPosition");
        isThrownProp = serializedObject.FindProperty("isThrown");
        scoreTextStringProp = serializedObject.FindProperty("scoreTextString");
    }

    public override void OnInspectorGUI()
    {
        // Update the serialized object
        serializedObject.Update();

        // Draw the gameMode toggle
        EditorGUILayout.PropertyField(gameModeProp, new GUIContent("Game Mode"));

        // If gameMode is enabled, show additional properties
        if (gameModeProp.boolValue)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Game Mode Settings", EditorStyles.boldLabel);

            // Draw the scoreText field
            EditorGUILayout.PropertyField(scoreTextProp, new GUIContent("Score Text"));
            EditorGUILayout.PropertyField(scoreTextMeshProp, new GUIContent("Score Text Mesh"));

            // Optionally, include other related properties
            EditorGUILayout.PropertyField(throwStartPositionProp, new GUIContent("Throw Start Position"));
            EditorGUILayout.PropertyField(isThrownProp, new GUIContent("Is Thrown"));
            EditorGUILayout.PropertyField(scoreTextStringProp, new GUIContent("Score Text String"));
        }

        // Apply any changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
}

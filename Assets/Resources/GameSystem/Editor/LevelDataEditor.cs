using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor {

    public LevelData levelData;

    private void OnEnable()
    {
        levelData = (LevelData)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("--== Mode de jeu du niveau ==--");
        levelData.gameMode = (GameModeData)EditorGUILayout.ObjectField("Game Mode Data", levelData.gameMode, typeof(GameModeData), true);

        EditorGUILayout.LabelField("--== Scene correspondante au niveau ==--");
        //levelData.scene = (GameObject)EditorGUILayout.ObjectField("Scene", levelData.scene, typeof(GameObject), true);

        EditorGUILayout.LabelField("--== Nombre de joueurs ==--");
        EditorGUILayout.IntField("Minimum : ", levelData.levelMinPlayers);
        EditorGUILayout.IntField("Maximum : ", levelData.levelMaxPlayers);

        EditorGUILayout.LabelField("--== Image du niveau ==--");
        DrawImageLevel();

        EditorGUILayout.LabelField("--== Personnages de depart ==--");
        DrawCharacterArray();
    }

    void DrawImageLevel()
    {
        if (levelData.imageLevel == null)
        {
            EditorGUILayout.LabelField("Choisir une Texture de niveau");
        }
        else if (levelData.imageLevel != null){
            EditorGUILayout.LabelField("Sprite de niveau present. CHANGER SPRITE POUR TEXTURE DANS LE CODE");
        }

        levelData.textureLevel = (Texture)EditorGUILayout.ObjectField("Image", levelData.textureLevel, typeof(Texture), true);
        //levelData.imageLevel = (Sprite)EditorGUILayout.ObjectField("Image du niveau", levelData.gameMode, typeof(Sprite), true);

    }

    void DrawCharacterArray()
    {
        if (levelData.startCharacter.Length == 0)
        {
            levelData.startCharacter = new CharacterCard[4];
        }
        
        SerializedProperty prop = serializedObject.FindProperty("startCharacter");
        EditorGUILayout.PropertyField(prop, true);

        serializedObject.ApplyModifiedProperties();
    }

}

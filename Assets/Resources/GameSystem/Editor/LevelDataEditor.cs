using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor {

    public LevelData levelData;

    //Afficher le Custom Editor quand on clique sur un Level Data
    private void OnEnable()
    {
        levelData = (LevelData)target;
    }

    //Affichage des differents champs sur le formulaire
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("--== Mode de jeu du niveau ==--");
        levelData.gameMode = (GameModeData)EditorGUILayout.ObjectField("Game Mode Data", levelData.gameMode, typeof(GameModeData), true);

        EditorGUILayout.LabelField("--== Nom du niveau ==--");
        levelData.mapName = EditorGUILayout.TextField("Nom : ", levelData.mapName);

        EditorGUILayout.LabelField("--== Description du niveau ==--");
        levelData.levelDescription = EditorGUILayout.TextField("Description : ", levelData.levelDescription);
        
        EditorGUILayout.LabelField("--== Nombre de joueurs ==--");
        levelData.levelMinPlayers = EditorGUILayout.IntField("Minimum : ", levelData.levelMinPlayers);
        levelData.levelMaxPlayers = EditorGUILayout.IntField("Maximum : ", levelData.levelMaxPlayers);

        EditorGUILayout.LabelField("--== Image du niveau ==--");
        levelData.imageLevel = (Sprite)EditorGUILayout.ObjectField("Image du niveau", levelData.imageLevel, typeof(Sprite), true);

        EditorGUILayout.LabelField("--== Personnages de depart ==--");
        DrawCharacterArray();
        
    }

    //Affichage special du champ des personnages du niveau
    void DrawCharacterArray()
    {
        if (levelData.startCharacter.Length == 0)
        {
            levelData.startCharacter = new CharacterCard[4];
        }
        
        //Aller chercher la Property CharacterCard[] pour pouvoir l'afficher dans le Custom Object
        SerializedProperty prop = serializedObject.FindProperty("startCharacter");
        EditorGUILayout.PropertyField(prop, true);

        serializedObject.ApplyModifiedProperties();
    }

}

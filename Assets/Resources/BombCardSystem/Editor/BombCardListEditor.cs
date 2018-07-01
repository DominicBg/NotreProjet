using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(BombCardList))]
public class BombCardListEditor : Editor {

    public BombCardList bombCardList;

    public SerializedObject soTarget;

    public SerializedProperty soArray;

    private void OnEnable()
    {
        bombCardList = (BombCardList)target;
        ActualizeBombList();


    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("--== Liste des Bombes crees ==--");

        if (GUILayout.Button("Actualize List", GUILayout.Height(30)))
        {
            ActualizeBombList();
        }

        if (bombCardList.bombCardList.Length > 0)
        {
            EditorGUILayout.PropertyField(soArray, true);
        }
    }

    public void ActualizeBombList() {

        string[] foundGUIDAssets;

        foundGUIDAssets = AssetDatabase.FindAssets("t:BombCard");

        bombCardList.bombCardList = new BombCard[foundGUIDAssets.Length];

        for (int i = 0; i < foundGUIDAssets.Length; i++)
        {
            BombCard zzz = (BombCard)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(foundGUIDAssets[i]), typeof(BombCard));
            bombCardList.bombCardList[i] = zzz;
        }

        soTarget = new SerializedObject(this.target);
        soArray = soTarget.FindProperty("bombCardList");

        EditorUtility.SetDirty(bombCardList);
    }
}

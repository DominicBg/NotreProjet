using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterCardList))]
public class CharacterCardListEditor : Editor
{
    public CharacterCardList charCardList;

    public SerializedObject soTarget;

    public SerializedProperty soArray;

    private void OnEnable()
    {
        charCardList = (CharacterCardList)target;
        ActualizeCharacterList();
    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.LabelField("--== Liste des Character crees ==--");


        //EditorGUILayout.PropertyField(soArray, true); // True means show children



        if (GUILayout.Button("Actualize List", GUILayout.Height(30)))
        {
            ActualizeCharacterList();
        }



        if (charCardList.characterCardList.Length > 0)
        {
            //POUR AFFICHER UN TABLEAU DANS LE SCRIPT EDITOR
            // "target" can be any class derrived from ScriptableObject 
            // (could be EditorWindow, MonoBehaviour, etc)
            
            EditorGUILayout.PropertyField(soArray, true); // True means show children
            
            /*
            ScriptableObject target = this;
            Debug.Log("Serialization Test : " + target);
            SerializedObject so = new SerializedObject(target);
            Debug.Log("Serialization Test : " + so);
            SerializedProperty charProperty = so.FindProperty("characterCardList");
            Debug.Log("Target : " + so.targetObject);
            Debug.Log("Serialization Test : " + charProperty);
            */            
        }




    }

    void ActualizeCharacterList() {

        string[] foundGUIDAssets;

        foundGUIDAssets = AssetDatabase.FindAssets("t:CharacterCard");

        charCardList.characterCardList = new CharacterCard[foundGUIDAssets.Length];

        for (int i = 0; i < foundGUIDAssets.Length; i++)
        {
            CharacterCard zzz = (CharacterCard)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(foundGUIDAssets[i]), typeof(CharacterCard));
            charCardList.characterCardList[i] = zzz;
        }
        
        soTarget = new SerializedObject(this.target);
        soArray = soTarget.FindProperty("characterCardList");

        EditorUtility.SetDirty(charCardList);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PlayerStatSO))]
public class PlayerDataEditor : Editor
{
    PlayerStatSO Data;

    private SerializedProperty defaultData;


    void OnEnable()
    {
        defaultData = serializedObject.FindProperty("defaultData");
        Data = (PlayerStatSO)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(defaultData);

        base.OnInspectorGUI();

        if (GUILayout.Button("Setting Default"))
        {
            UpdateStats();
        }

        serializedObject.ApplyModifiedProperties();
    }

    //1.1 GSTU_Search 객체를 생성하는 부분
    void UpdateStats()
    {
        PlayerStatSO playerSO = (PlayerStatSO)target;
        playerSO.SetData(defaultData.objectReferenceValue as PlayerStatSO);
    }
}

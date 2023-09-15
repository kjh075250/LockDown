using GoogleSheetsToUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CustomEditor(typeof(UISO))]
public class DataEditor : Editor
{
    UISO Data;

    private SerializedProperty defaultData;


    void OnEnable()
    {
        defaultData = serializedObject.FindProperty("defaultData");
        Data = (UISO)target;
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
        UISO uiso = (UISO)target;
        uiso.SetData(defaultData.objectReferenceValue as UISO);
    }
}

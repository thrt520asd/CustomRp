using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEditor;
public class EditorShortcut 
{
    [MenuItem("GameObject/SetActive %w")]
    public static void SetActive()
    {
        var gos =  Selection.gameObjects;
        bool curActive = true;
        foreach (var go in gos)
        {
            curActive = curActive && go.activeSelf;
        }
        if (gos != null)
        {
            foreach (var go in gos)
            {
                go.SetActive(!curActive);
            }
        }
    }
    
}

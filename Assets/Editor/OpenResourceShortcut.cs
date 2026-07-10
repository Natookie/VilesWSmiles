using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;

public static class OpenResourceShortcut
{
    [Shortcut("Project/Open Resources", KeyCode.Slash, ShortcutModifiers.Action | ShortcutModifiers.Shift)]
    static void OpenResources(){
        var folder = AssetDatabase.LoadAssetAtPath<Object>("Assets/Resources");
        if(folder == null){
            Debug.LogError("Assets/Resources not found.");
            return;
        }

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = folder;
        EditorGUIUtility.PingObject(folder);
    }
}
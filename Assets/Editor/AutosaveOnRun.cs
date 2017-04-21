using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class AutosaveOnRun {
  static AutosaveOnRun() {
    EditorApplication.playmodeStateChanged = () =>
    {
      if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying) {
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
      }
    };
  }
}
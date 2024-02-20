using UnityEditor;
using UnityEditor.SceneManagement;

public static class EditorSwitchScene
{
    /// <summary>
    /// 打开场景
    /// </summary>
    /// <param name="filename">场景路径</param>
    public static void OpenScene(string filename)
    {
        // 询问是否保存对当前场景的修改
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(filename);
        }
    }

    [MenuItem("切换场景/泛型单例")]
    public static void SwitchSingleton()
    {
        OpenScene("Assets/Scenes/Singleton.unity");
    }

    [MenuItem("切换场景/事件系统")]
    public static void SwitchEvent()
    {
        OpenScene("Assets/Scenes/Event.unity");
    }
}
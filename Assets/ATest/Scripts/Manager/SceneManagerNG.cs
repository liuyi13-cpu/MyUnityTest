using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景管理器
/// </summary>
public class SceneManagerNG
{
    public static AsyncOperation LoadSceneAsync(string scenename)
    {
        return SceneManager.LoadSceneAsync(scenename);
    }

    public static void LoadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public static void LoadScene(string scenename, LoadSceneMode mode)
    {
        SceneManager.LoadScene(scenename, mode);
    }

    public static void UnloadScene(string name)
    {
        SceneManager.UnloadScene(name);
    }

    public static Scene GetActiveScene()
    {
        return SceneManager.GetActiveScene();
    }

    public static bool IsScene(string name)
    {
        return SceneManager.GetActiveScene().name.Equals(name);
    }
}

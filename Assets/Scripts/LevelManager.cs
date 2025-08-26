using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class LevelManager : MonoBehaviour
{
    // --- Simple (blocking) loads ---
    public void LoadScene0()
    {
        // Ensure scene 0 is added to Build Settings -> Scenes In Build
        SceneManager.LoadScene(0);
    }

    public void LoadScene1()
    {
        SceneManager.LoadScene(1);
    }

    // --- Recommended: asynchronous (non-blocking) loads ---
    public void LoadScene0Async()
    {
        StartCoroutine(LoadSceneAsyncCoroutine(0));
    }

    public void LoadScene1Async()
    {
        StartCoroutine(LoadSceneAsyncCoroutine(1));
    }

    private IEnumerator LoadSceneAsyncCoroutine(int sceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
        // optionally prevent auto-activation until you're ready:
        // op.allowSceneActivation = false;
        while (!op.isDone)
        {
            // op.progress goes 0..0.9 while loading, then 0.9->1 on activation
            yield return null;
        }
    }
}

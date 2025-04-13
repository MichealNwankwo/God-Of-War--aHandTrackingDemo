using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject Selection;
    [SerializeField] private Slider loadingSlider;

    public void LoadLevelButton(string levelToLoad)
    {
        if (Selection != null)
            Selection.SetActive(false);

        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASYNC(levelToLoad));
    }

    IEnumerator LoadLevelASYNC(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            if (loadingSlider != null)
                loadingSlider.value = progressValue;

            yield return null;
        }
    }
}

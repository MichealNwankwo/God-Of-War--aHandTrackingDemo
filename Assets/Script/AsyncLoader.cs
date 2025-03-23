using System.Collections;
using System.Collections.Generic;
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
        Selection.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASYNC(levelToLoad));
    }

    IEnumerator LoadLevelASYNC(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress/0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }
}

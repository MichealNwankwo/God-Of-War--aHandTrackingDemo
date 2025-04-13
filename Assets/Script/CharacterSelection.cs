using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characterModels;
    public GameObject lightningEffect;
    public ImageSwitcher imageSwitcher;
    public AsyncLoader asyncLoader; // Reference to AsyncLoader

    private int index;

    private void Start()
    {
        index = PlayerPrefs.GetInt("CharacterSelected", 0);

        DeactivateAllModels();
        ActivateModel(index);

        if (imageSwitcher != null)
            imageSwitcher.SetInstantIndex(index);

        StartCoroutine(WaitForCharacterActivation());
    }

    private void DeactivateAllModels()
    {
        foreach (GameObject model in characterModels)
        {
            if (model != null)
                model.SetActive(false);
        }
    }

    private void ActivateModel(int i)
    {
        if (i >= 0 && i < characterModels.Length && characterModels[i] != null)
            characterModels[i].SetActive(true);
    }

    private IEnumerator WaitForCharacterActivation()
    {
        yield return null;

        if (characterModels[index].activeSelf)
        {
            TriggerDissolver();
        }
    }

    public void ToggleLeft()
    {
        ResetLightningEffect();
        characterModels[index].SetActive(false);

        index--;
        if (index < 0)
            index = characterModels.Length - 1;

        characterModels[index].SetActive(true);

        if (imageSwitcher != null)
            imageSwitcher.SlideToIndex(index);

        StartCoroutine(WaitForCharacterActivation());
    }

    public void ToggleRight()
    {
        ResetLightningEffect();
        characterModels[index].SetActive(false);

        index++;
        if (index >= characterModels.Length)
            index = 0;

        characterModels[index].SetActive(true);

        if (imageSwitcher != null)
            imageSwitcher.SlideToIndex(index);

        StartCoroutine(WaitForCharacterActivation());
    }

    private void TriggerDissolver()
    {
        GameObject currentCharacter = characterModels[index];

        if (currentCharacter.activeSelf)
        {
            EnableLightningEffect();

            foreach (Transform child in currentCharacter.transform)
            {
                Dissolver dissolver = child.GetComponent<Dissolver>();
                if (dissolver != null && child.gameObject.activeSelf)
                {
                    dissolver.startDissolver();
                }
            }
        }
    }

    private void ResetLightningEffect()
    {
        if (lightningEffect != null)
            lightningEffect.SetActive(false);
    }

    private void EnableLightningEffect()
    {
        if (lightningEffect != null)
            lightningEffect.SetActive(true);
    }

    public void ConfirmButton()
    {
        PlayerPrefs.SetInt("CharacterSelected", index);

        if (asyncLoader != null)
        {
            asyncLoader.LoadLevelButton("test"); // Replace with your scene name
        }
        else
        {
            SceneManager.LoadScene("test"); // Fallback
        }
    }
}

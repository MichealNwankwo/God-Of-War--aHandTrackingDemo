using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    private GameObject[] characterList;
    private int index;

    public GameObject lightningEffect; // Lightning effect to enable when dissolving

    private void Start()
    {
        index = PlayerPrefs.GetInt("CharacterSelected");
        characterList = new GameObject[transform.childCount];

        // Fill the array with character models (children)
        for (int i = 0; i < transform.childCount; i++)
            characterList[i] = transform.GetChild(i).gameObject;

        // Turn off all characters initially
        foreach (GameObject go in characterList)
        {
            go.SetActive(false);
        }

        // Toggle on the selected character
        if (characterList[index])
            characterList[index].SetActive(true);

        // Start a coroutine to check the character's activation state
        StartCoroutine(WaitForCharacterActivation());
    }

    private IEnumerator WaitForCharacterActivation()
    {
        yield return null; // Wait for one frame to ensure activation is processed

        // Check if the selected character is active
        if (characterList[index].activeSelf)
        {
            TriggerDissolver();
        }
    }

    public void ToggleLeft()
    {
        ResetLightningEffect(); // Disable lightning before switching

        // Toggle off the current character
        characterList[index].SetActive(false);

        // Update index for the new character
        index--;
        if (index < 0)
            index = characterList.Length - 1;

        // Toggle on the new character
        characterList[index].SetActive(true);

        // Trigger Dissolver after character is fully active
        StartCoroutine(WaitForCharacterActivation());
    }

    public void ToggleRight()
    {
        ResetLightningEffect(); // Disable lightning before switching

        // Toggle off the current character
        characterList[index].SetActive(false);

        // Update index for the new character
        index++;
        if (index == characterList.Length)
            index = 0;

        // Toggle on the new character
        characterList[index].SetActive(true);

        // Trigger Dissolver after character is fully active
        StartCoroutine(WaitForCharacterActivation());
    }

    private void TriggerDissolver()
    {
        GameObject currentCharacter = characterList[index];

        if (currentCharacter.activeSelf)
        {
            EnableLightningEffect(); // Re-enable lightning after switch

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
        {
            lightningEffect.SetActive(false); // Disable before switching
            Debug.Log("Lightning effect disabled");
        }
    }

    private void EnableLightningEffect()
    {
        if (lightningEffect != null)
        {
            lightningEffect.SetActive(true); // Re-enable after switch
            Debug.Log("Lightning effect enabled");
        }
    }

    public void ConfirmButton()
    {
        PlayerPrefs.SetInt("CharacterSelected", index);
        SceneManager.LoadScene("test");
    }
}

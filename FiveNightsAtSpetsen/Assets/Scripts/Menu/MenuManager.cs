using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject Image;
    public GameObject Title;
    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject LevelSelect;
    public GameObject Confirm;
    public GameObject StaticEffect;

    [HideInInspector] public string SceneName;

    void Awake()
    {
        OptionsMenu.SetActive(false);
        LevelSelect.SetActive(false);
        Confirm.SetActive(false);
        StaticEffect.SetActive(true);
        MainMenu.SetActive(true);
        Title.SetActive(true);
        Image.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneName);
    }

    public void SetSceneName(string Name)
    {
        SceneName = Name;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OptionsMenu.activeSelf == true)
            {
                OptionsMenu.SetActive(false);
                MainMenu.SetActive(true);
                Title.SetActive(true);
                Image.SetActive(true);
            }
            else if (Confirm.activeSelf == true)
            {
                Confirm.SetActive(false);
            }
            else if (LevelSelect.activeSelf == true)
            {
                LevelSelect.SetActive(false);
                MainMenu.SetActive(true);
                Title.SetActive(true);
                Image.SetActive(true);
            }
        }
    }
}

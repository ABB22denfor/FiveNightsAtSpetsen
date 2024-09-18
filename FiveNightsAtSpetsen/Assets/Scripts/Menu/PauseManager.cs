using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public string MainMenuSceneName;
    public GameObject Canvas;
    public GameObject PauseMenu;
    public GameObject Confirm;
    public GameObject SettingsMenu;
    public GameObject Static;
    public GameObject Background;

    void Awake()
    {
        PauseMenu.SetActive(true);
        Background.SetActive(true);
        Static.SetActive(true);
        Confirm.SetActive(false);
        SettingsMenu.SetActive(false);
        Canvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Canvas.activeSelf == false)
            {
                Canvas.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
            else if (Canvas.activeSelf == true)
            {
                if (SettingsMenu.activeSelf == true)
                {
                    PauseMenu.SetActive(true);
                    SettingsMenu.SetActive(false);
                }
                else if (Confirm.activeSelf == true)
                {
                    PauseMenu.SetActive(true);
                    Confirm.SetActive(false);
                }
                else
                {
                    Resume();
                }
            }
        }
    }

    public void Resume()
    {
        Canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
    public void QuitMainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }
}

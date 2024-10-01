using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public string MainMenuSceneName;
    public GameObject Canvas;
    public GameObject PausePanel;
    public GameObject PauseMenu;
    public GameObject Confirm;
    public GameObject SettingsMenu;
    public GameObject Static;
    public GameObject Background;

    void Awake()
    {
        PauseMenu.SetActive(true);
        Background.SetActive(true);
        Static.SetActive(false);
        Confirm.SetActive(false);
        SettingsMenu.SetActive(false);
        PausePanel.SetActive(false);
        Canvas.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Static.activeSelf == false)
            {
                Static.SetActive(true);
                PausePanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
            else if (Static.activeSelf == true && PausePanel.activeSelf == true)
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
        PausePanel.SetActive(false);
        Static.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
    public void QuitMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(MainMenuSceneName);
    }
}

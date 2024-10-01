using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public string MainMenuSceneName;
    public GameObject Canvas;
    public GameObject WinPanel;
    public GameObject WinMenu;
    public GameObject Confirm;
    public GameObject Background;

    void Awake()
    {
        Background.SetActive(true);
        WinMenu.SetActive(true);
        Confirm.SetActive(false);
        WinPanel.SetActive(false);
    }

    void OnEnable()
    {
        EventsManager.Instance.taskEvents.OnAllTasksCompleted += WinGame;
    }

    void OnDisable()
    {
        EventsManager.Instance.taskEvents.OnAllTasksCompleted -= WinGame;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Canvas.activeSelf == true && WinPanel.activeSelf == true)
            {
                if (Confirm.activeSelf == true)
                {
                    WinMenu.SetActive(true);
                    Confirm.SetActive(false);
                }
            }
        }
    }

    void WinGame()
    {
        Canvas.SetActive(true);
        WinPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        WinMenu.SetActive(true);
    }

    public void NextDay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(MainMenuSceneName);
    }
}

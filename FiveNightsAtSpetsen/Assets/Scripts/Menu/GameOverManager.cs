using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public string MainMenuSceneName;
    public GameObject Canvas;
    public GameObject GameOverPanel;
    public GameObject Background;
    public GameObject GameOverMenu;
    public GameObject Confirm;
    public GameObject Image;

    void Awake()
    {
        Image.SetActive(true);
        Background.SetActive(true);
        GameOverMenu.SetActive(false);
        Confirm.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    void OnEnable()
    {
        EventsManager.Instance.teacherEvents.OnPlayerCaptured += StartGameOver;
    }

    void OnDisable()
    {
        EventsManager.Instance.teacherEvents.OnPlayerCaptured -= StartGameOver;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Canvas.activeSelf == true && GameOverPanel.activeSelf == true)
            {
                if (Confirm.activeSelf == true)
                {
                    GameOverMenu.SetActive(true);
                    Confirm.SetActive(false);
                }
            }
        }
    }

    void StartGameOver()
    {
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1);

        Canvas.SetActive(true);
        GameOverPanel.SetActive(true);

        yield return new WaitForSeconds(5);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        GameOverMenu.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(MainMenuSceneName);
    }
}

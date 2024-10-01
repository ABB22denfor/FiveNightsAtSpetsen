/*
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-09-24
 */

using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{

  private Coroutine processCoroutine;
  private bool      isProcessRunning = false;

  [SerializeField]
  private string backstoryText;
  private int backstoryIndex = 0;

  [SerializeField]
  private TextMeshProUGUI backstoryTextField  = null;

  [SerializeField]
  private string loadingText;
  private int loadingIndex = 0;
  private int loadingDots  = 3;

  [SerializeField]
  private TextMeshProUGUI loadingTextField = null;

  void Start()
  {
    loadingTextField.text   = "";
    backstoryTextField.text = "";
  
    StartCoroutine(AnimateBackstory());
  }

  /*
   *
   */
  private IEnumerator AnimateBackstory()
  {
    Debug.Log("Started animated backstory");

    isProcessRunning = true;

    if(backstoryTextField)
    {
      backstoryTextField.text = "";
      backstoryIndex          = 0;
    }

    while(isProcessRunning)
    {
      if(backstoryTextField && backstoryIndex < backstoryText.Length)
      {
        backstoryTextField.text += backstoryText[backstoryIndex++];
      }
      else
      {
        isProcessRunning = false;
      }

      yield return new WaitForSeconds(0.075f);
    }

    StartCoroutine(OnBackstoryStopped());
  }

  /*
   *
   */
  private IEnumerator OnBackstoryStopped()
  {
    Debug.Log("Done animating backstory");

    StartCoroutine(AnimateLoading());

    yield return new WaitForSeconds(2.0f);

    SceneManager.LoadScene("JonasNight");
  }

  /*
   *
   */
  private IEnumerator AnimateLoading()
  {
    Debug.Log("Started animating loading");

    while(true)
    {
      if(loadingTextField)
      {
        loadingTextField.text = loadingText + new string('.', loadingIndex);

        loadingIndex = (loadingIndex % loadingDots) + 1;

        Debug.Log("Loading Index: " + loadingIndex);
      }

      yield return new WaitForSeconds(0.5f);
    }
  }

}

/*
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-09-24
 */

using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class LoadingScene : MonoBehaviour
{
  [SerializeField]
  private float backstoryLetterDelay = 0.05f; // Delay between each letter

  [SerializeField]
  private TextMeshProUGUI backstoryTextField;
  private string          backstoryText;

  [SerializeField]
  private TextMeshProUGUI loadingTextField;
  private string          loadingText;

  [SerializeField]
  private AudioClip backstoryAudioClip;

  [SerializeField]
  private string playSceneName;

  [SerializeField]
  private Button skipButton;

  [SerializeField]
  private Button playButton;


  private AudioSource audioSource;

  private AsyncOperation sceneLoad;

  /*
   *
   */
  void OnEnable()
  {
    Debug.Log("LoadingScene.cs enabled");

    audioSource = gameObject.GetComponent<AudioSource>();

    // Add onClick events to buttons
    if(skipButton != null)
    {
      skipButton.onClick.AddListener(OnSkipClick);
      
      skipButton.gameObject.SetActive(false);
    }

    if(playButton != null)
    {
      playButton.onClick.AddListener(OnPlayClick);

      playButton.gameObject.SetActive(false);
    }

    loadingTextField?.gameObject.SetActive(false);
  }

  /*
   *
   */
  void OnDisable()
  {
    // Remove onClick events to buttons
    skipButton?.onClick.RemoveListener(OnSkipClick);

    playButton?.onClick.RemoveListener(OnPlayClick);
  }

  /*
   * When the scene starts, begin with:
   * start loading the play scene
   * start playing the audio
   */
  void Start()
  {
    Debug.Log("Start loading scene");

    // Start loading scene, but not activating it, yet
    if(playSceneName != null && playSceneName.Length > 0)
    {
      sceneLoad = SceneManager.LoadSceneAsync(playSceneName);
    }

    if(sceneLoad != null)
    {
      sceneLoad.allowSceneActivation = false;
    }

    // Store the texts
    backstoryText = backstoryTextField?.text;
    loadingText   = loadingTextField?.text;

    // If an AudioSource has been supplied, play the audio clip
    if(audioSource)
    {
      audioSource.clip = backstoryAudioClip;

      audioSource.Play();
    }
  
    if(backstoryTextField != null)
    {
      backstoryRoutine = StartCoroutine(ReadBackstory());
      
      skipButton?.gameObject.SetActive(true);
    }
    // If no backstory exists
    // continue like the backstory has been read
    else
    {
      AfterBackstoryHasBeenRead();
    }
  }

  /*
   *
   */
  private void OnSkipClick()
  {
    Debug.Log("Skipping backstory");

    if(backstoryRoutine != null)
    {
      StopCoroutine(backstoryRoutine);
    }

    backstoryTextField.text = backstoryText;

    AfterBackstoryHasBeenRead();
  }

  /*
   * Read the backstory, by:
   * playing the audio recording, and
   * revealing letter by letter
   */
  private Coroutine backstoryRoutine;

  private IEnumerator ReadBackstory()
  {
    Debug.Log("Started reading backstory");

    backstoryTextField.text = "";

    for(int index = 0; index < backstoryText.Length; index++)
    {
      backstoryTextField.text += backstoryText[index];

      yield return new WaitForSeconds(backstoryLetterDelay);
    }

    // Wait until the audio has stopped playing
    while(audioSource != null && (audioSource?.isPlaying ?? true))
    {
      yield return new WaitForSeconds(0.1f);
    }

    AfterBackstoryHasBeenRead();
  }

  /*
   * After the backstory has been read,
   * the audio should stop
   * the play button should appear
   * the skip button should disappear
   */
  private void AfterBackstoryHasBeenRead()
  {
    Debug.Log("After backstory has been read");

    skipButton?.gameObject.SetActive(false);

    if(audioSource != null && (audioSource?.isPlaying ?? true))
    {
      audioSource?.Stop();
    }

    if(playButton != null)
    {
      playButton.gameObject.SetActive(true);
    }
    else
    {
      StartCoroutine(LoadScene());
    }
  }

  /*
   * When the player clicks on the play button,
   * the play button should disappear
   * the scene should be loaded
   */
  private void OnPlayClick()
  {
    Debug.Log("Play button has been clicked");

    playButton?.gameObject.SetActive(false);

    StartCoroutine(LoadScene());
  }

  /*
   * Animate the loading text,
   * by cycling 3 dots after the text...
   */
  private IEnumerator LoadScene()
  {
    Debug.Log("Started animating loading");

    loadingTextField?.gameObject.SetActive(true);

    for(int index = 0;
        (sceneLoad == null) || (sceneLoad.progress < 0.9f);
        index = (index % 3) + 1)
    {
      if(loadingTextField)
      {
        loadingTextField.text = loadingText + new string('.', index);
      }

      yield return new WaitForSeconds(0.5f);
    }

    if(sceneLoad != null)
    {
      sceneLoad.allowSceneActivation = true;
    }
  }
}

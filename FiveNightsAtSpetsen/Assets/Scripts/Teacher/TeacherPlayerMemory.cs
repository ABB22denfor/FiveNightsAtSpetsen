/*
 * Keep track of what the teacher knows about the player
 * For making the teacher say suitable voicelines
 *
 * After some amount of time,
 * the teacher forgets things about the player
 *
 * In this script, the keyword "this." is used for booleans
 * This is to represent that the teacher.knowsAboutPlayer :D
 *
 * Written by Hampus Fridholm
 *
 * Last updated: 2024-10-01
 */

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherPlayerMemory : MonoBehaviour
{
  public bool knowsAboutPlayer   = false;
  public bool knowsWherePlayerIs = false;
  public bool hasCapturedPlayer  = false;

  // The amount of seconds the teacher remembers the player's position
  [SerializeField]
  private float trackingPlayerTime = 5.0f;

  // The amount of seconds the teacher remembers the player
  [SerializeField]
  private float forgetPlayerTime = 60.0f;

  /*
   * Make the teacher forget about the player
   *
   * When the teacher forgets the player,
   * he also forgets where the player is :D
   */
  private Coroutine forgetPlayerRoutine;

  private IEnumerator ForgetAboutPlayer()
  {
    yield return new WaitForSeconds(forgetPlayerTime);

    this.knowsAboutPlayer   = false;
    this.knowsWherePlayerIs = false;
  }

  /*
   * When the teacher hears a sound,
   * and already knows about the player,
   * he will remind himself that the player exists
   */
  public void HearSound()
  {
    Debug.Log("Teacher heard a sound");

    if(!this.knowsAboutPlayer) return;


    if(forgetPlayerRoutine != null)
    {
      StopCoroutine(forgetPlayerRoutine);
    }

    this.knowsAboutPlayer = true;

    forgetPlayerRoutine = StartCoroutine(ForgetAboutPlayer());
  }

  /*
   * The teacher loses track of the player after some time
   */
  private Coroutine losingTrackRoutine;

  private IEnumerator LoseTrackOfPlayer()
  {
    yield return new WaitForSeconds(trackingPlayerTime);

    this.knowsWherePlayerIs = false;
  }

  /*
   * When the teacher spotts the player,
   * he will know where the player is, and,
   * that the player exists
   */
  public void SpottPlayer()
  {
    Debug.Log("Teacher spotted player");

    if(forgetPlayerRoutine != null)
    {
      StopCoroutine(forgetPlayerRoutine);
    }

    this.knowsAboutPlayer = true;

    forgetPlayerRoutine = StartCoroutine(ForgetAboutPlayer());


    if(losingTrackRoutine != null)
    {
      StopCoroutine(losingTrackRoutine);
    }

    this.knowsWherePlayerIs = true;

    losingTrackRoutine = StartCoroutine(LoseTrackOfPlayer());
  }

  /*
   * 
   */
  public void CapturePlayer()
  {
    hasCapturedPlayer = true;
  }
}

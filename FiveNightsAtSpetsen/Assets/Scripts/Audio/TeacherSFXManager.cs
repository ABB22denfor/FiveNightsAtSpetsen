using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TeacherSFXManager : MonoBehaviour
{

    bool isWalking;
    bool noSound = true;

    public AudioClip walkingSFX;
    public AudioMixerGroup audioMixerGroup;


    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
       audioSource = gameObject.AddComponent<AudioSource>(); 
    }

    void OnEnable(){
       EventsManager.Instance.animationEvents.OnStartWalking += () => {
         isWalking = true;
       }; 
    }

    void OnDisable(){
       EventsManager.Instance.animationEvents.OnStartWalking -= () => {
         isWalking = false;
       }; 
    }

    // Update is called once per frame
    void Update()
    {
       if(isWalking && noSound){
         audioSource.clip = walkingSFX;
         audioSource.loop = true;
         audioSource.volume = 10f;
         audioSource.spatialBlend = 1f;
         audioSource.pitch = 1f;
         audioSource.minDistance = 5f;
         audioSource.outputAudioMixerGroup = audioMixerGroup;
         audioSource.Play();

         noSound = false;
       }
    }

    
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour {

  private List<EventInstance> eventInstances;
  public static AudioManager instance { get; private set; }

  //EventInstances for sounds that continues to loop for scenses
  private EventInstance musicEventInstance;

  private void Awake() {
    if (instance != null) {
      Debug.LogError("Found more than one Audio Manager in the scene");
    }

    instance = this;

    eventInstances = new List<EventInstance>();
  }

  private void Start() {
    InitializeMusic(FmodEvents.instance.MainBGM);
  }

  private void InitializeMusic(EventReference musicEventReference) {
    musicEventInstance = CreateInstance(musicEventReference);
    musicEventInstance.start();
  }

  public void SetMusicMach(int mach) {
    musicEventInstance.setParameterByName("Mach", (float)mach);
  }

  public void PlayTypeOnsehot(MonoBehaviour m) {
    RuntimeManager.PlayOneShot(FmodEvents.instance.TypeSound, m.transform.position);
  }

  public void PlayTypoOnsehot(MonoBehaviour m) {
    RuntimeManager.PlayOneShot(FmodEvents.instance.TypoSound, m.transform.position);
  }

  public void PlayPuncOnsehot(MonoBehaviour m) {
    RuntimeManager.PlayOneShot(FmodEvents.instance.PunctuationSound, m.transform.position);
  }

  public void PlayOneShot(EventReference sound, Vector3 worldPos) {
    RuntimeManager.PlayOneShot(sound, worldPos);
  }

  public EventInstance CreateInstance(EventReference eventReference) {
    EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
    eventInstances.Add(eventInstance);
    return eventInstance;
  }

  private void CleanUp() {
    foreach (EventInstance eventInstance in eventInstances) {
      eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
      eventInstance.release();
    }

  }

  private void OnDestroy() {
    CleanUp();
  }

}

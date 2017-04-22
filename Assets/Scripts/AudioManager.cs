using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

  public int audioSourceSize = 10;
  AudioSource[] sources;

  static AudioManager _instance;

  public AudioClip shoot;
  public AudioClip jump;

	// Use this for initialization
	void Start () {
    for (int i = 0; i < audioSourceSize; i++) {
      gameObject.AddComponent<AudioSource>();
    }
    sources = GetComponents<AudioSource>();
    foreach (AudioSource a in sources) {
      a.playOnAwake = false;
      a.loop = false;
    }

    _instance = this;
	}
	
  public static AudioManager Instance() {
    return _instance;
  }

  void PlaySFX(AudioClip clip) {
    for (int i = 0; i < sources.Length; i++) {
      if (!sources[i].isPlaying) {
        sources[i].clip = clip;
        sources[i].Play();
        return;
      }
    }
  }

  public void PlayFire() {
    PlaySFX(shoot);
  }

  public void PlayJump() {
    PlaySFX(jump);
  }
}

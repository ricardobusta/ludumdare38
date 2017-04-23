using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
  public AudioSource musicIntro;
  public AudioSource musicLoop;

  public void SetMusic(AudioClip intro, AudioClip loop) {
    musicIntro.clip = intro;
    musicLoop.clip = loop;
  }

  public void StopMusic() {
    musicIntro.Stop();
    musicLoop.Stop();
  }

  public float GetVolume() {
    return musicLoop.volume;
  }

  public void SetVolume(float volume) {
    musicIntro.volume = volume;
    musicLoop.volume = volume;
  }

  public void PlayMusic() {
    if (musicIntro.clip != null) {
      musicIntro.Play();
      if (musicLoop.clip != null) {
        musicLoop.PlayDelayed(musicIntro.clip.length / Mathf.Abs(musicIntro.pitch));
      }
    } else if (musicLoop.clip != null) {
      musicLoop.Play();
    } else {
      Debug.LogError("Music is null!");
    }
  }

  private void Start() {
    PlayMusic();
  }
}

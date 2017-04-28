using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class MusicManager : MonoBehaviour {
  public AudioSource musicIntro;
  public AudioSource musicLoop;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="intro"></param>
  /// <param name="loop"></param>
  public void SetMusic(AudioClip intro, AudioClip loop) {
    musicIntro.clip = intro;
    musicLoop.clip = loop;
  }

  /// <summary>
  /// 
  /// </summary>
  public void StopMusic() {
    musicIntro.Stop();
    musicLoop.Stop();
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public float GetVolume() {
    return musicLoop.volume;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="volume"></param>
  public void SetVolume(float volume) {
    musicIntro.volume = volume;
    musicLoop.volume = volume;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="pitch"></param>
  public void SetPitch(float pitch) {
    if (musicIntro.isPlaying) {
      musicIntro.Stop();
      musicLoop.Play();
    }
    musicLoop.pitch = pitch;
  }

  /// <summary>
  /// 
  /// </summary>
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

  /// <summary>
  /// 
  /// </summary>
  private void Start() {
    PlayMusic();
  }
}

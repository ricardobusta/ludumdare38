using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the sound effects. There should be only one instance per scene.
/// </summary>
public class AudioManager : MonoBehaviour {

  public int audioSourceSize = 10;
  AudioSource[] sources;

  static AudioManager _instance;

  public AudioClip shot;
  public AudioClip jump;
  public AudioClip hitHead;
  public AudioClip attack;
  public AudioClip landing;

  /// <summary>
  /// Creates the audio sources and initializes them with correct values.
  /// </summary>
  void Start() {
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

  /// <summary>
  /// Returns an Audio Manager instance. There should be only once in the scene.
  /// </summary>
  /// <returns></returns>
  public static AudioManager Instance() {
    return _instance;
  }

  /// <summary>
  /// Shortcut to play a sound without looking for the manager instance first.
  /// </summary>
  /// <param name="clip"></param>
  public static void Play(AudioClip clip) {
    _instance.PlaySFX(clip);
  }

  /// <summary>
  /// Plays a SFX given it's audio clip.
  /// </summary>
  /// <param name="clip">The audio clip that will be played.</param>
  void PlaySFX(AudioClip clip) {
    for (int i = 0; i < sources.Length; i++) {
      if (!sources[i].isPlaying) {
        sources[i].clip = clip;
        sources[i].Play();
        return;
      }
    }
  }

  /// <summary>
  /// Plays the gun fire sound.
  /// </summary>
  public void PlayFire() {
    PlaySFX(shot);
  }

  /// <summary>
  /// Plays the jumping sound.
  /// </summary>
  public void PlayJump() {
    PlaySFX(jump);
  }

  /// <summary>
  /// Plays the melee attack sound.
  /// </summary>
  public void PlayAttack() {
    PlaySFX(attack);
  }

  /// <summary>
  /// Plays the hit head sound.
  /// </summary>
  public void PlayHitHead() {
    PlaySFX(hitHead);
  }

  /// <summary>
  /// Plays the landing sound.
  /// </summary>
  public void PlayLanding() {
    PlaySFX(landing);
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnce : MonoBehaviour {
    private AudioSource _audio;

    void Start() {
        _audio = gameObject.GetComponent<AudioSource>();
        _audio.Play();
    }

    void Update()
    {
        if (!_audio.isPlaying) {
            Destroy(gameObject);
        }
    }
}

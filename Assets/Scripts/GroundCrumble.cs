using UnityEngine;
using System.Collections;

public class GroundCrumble : MonoBehaviour {

    public AudioClip[] clips;

    // Use this for initialization
    void Awake () {
        GetComponent<AudioSource>().clip = clips[Random.Range(0, clips.Length)];
        GetComponent<AudioSource>().Play();
    }
	
}

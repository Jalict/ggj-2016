using UnityEngine;
using System.Collections;

using XInputDotNetPure;

public class MenuManager : MonoBehaviour {
	public SpriteRenderer[] hangmanSprites;
	public Material[] playerMats;
	public ParticleSystem[] playerParticles;

	private bool[] isJoined;	
	private Color[] playerColours;

	// Use this for initialization
	void Start () {
		playerColours = new Color[playerMats.Length];

		for (int i = 0; i < playerMats.Length; i++) {
			playerColours [i] = playerMats [i].color;
			playerParticles[i].startColor = playerMats [i].color;	
			playerParticles [i].gameObject.SetActive (false);
		}

		isJoined = new bool[playerMats.Length];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("XboxOne_AButton") || Input.GetKeyDown(KeyCode.Alpha1)) {
			PlayerPressedA (0);
		}
		if (Input.GetButtonDown ("XboxTwo_AButton") || Input.GetKeyDown(KeyCode.Alpha2)) {
			PlayerPressedA (1);
		}
		if (Input.GetButtonDown ("XboxThree_AButton") || Input.GetKeyDown(KeyCode.Alpha3)) {
			PlayerPressedA (2);
		}
		if (Input.GetButtonDown ("XboxFour_AButton") || Input.GetKeyDown(KeyCode.Alpha4)) {
			PlayerPressedA (3);
		}
	}

	void PlayerPressedA(int i) {
		isJoined [i] = !isJoined [i];

		hangmanSprites [i].color = isJoined[i] ? playerMats[i].color : Color.white;

		if (isJoined[i]) {
			playerParticles [i].gameObject.SetActive (true);
		} else {
			playerParticles [i].gameObject.SetActive (false);
		}
	}
}

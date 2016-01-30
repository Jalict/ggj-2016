using UnityEngine;
using System.Collections;

public class DeleteYourself : MonoBehaviour {

	public float secondsUntilDelete = 5;

	// Use this for initialization
	void Start () {
		StartCoroutine (WaitUntil (secondsUntilDelete));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator WaitUntil(float s) {
		yield return new WaitForSeconds (s);

		Destroy (gameObject);
	}
}

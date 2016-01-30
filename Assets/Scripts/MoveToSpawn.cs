using UnityEngine;
using System.Collections;

public class MoveToSpawn : MonoBehaviour {
	private float t;
	public float spawnTime;
	private float distance;

	public Vector3 spawnPosition;
	private Vector3 deathPosition;

	// Use this for initialization
	void Start () {
		t = 0;
		spawnTime = 5;
		deathPosition = transform.position;
		distance = 1 / Vector3.Distance (deathPosition, spawnPosition);
	}
	
	// Update is called once per frame
	void Update() {
		if (t < 0.98f) {
			transform.position = Vector3.Lerp (deathPosition, spawnPosition, t);

			t += Time.deltaTime / (spawnTime/1.6f); //NO IDEA LOL
		} else {
			DestroyImmediate (gameObject);
		}
	}
}

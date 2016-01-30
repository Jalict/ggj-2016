using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody2D))]

public class Prisoner : MonoBehaviour {
	private Vector3 velocity;
	
	public Vector3 chainPoint;
	public float chainLength;
	public float speed;
	
	bool isPanicing;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void move(){
		transform.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Force);
	}
}

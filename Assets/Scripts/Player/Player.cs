using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody2D))]

public class Player : MonoBehaviour {
	
	Vector3 velocity;
	public float speed;
	public float maxSpeed;
	// Use this for initialization
	void Start () {
		velocity = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey (KeyCode.LeftArrow)){
			velocity = Vector3.left * speed;
			
			if(transform.GetComponent<Rigidbody2D>().velocity.magnitude < maxSpeed){
				Move();
			}
		}

		if (Input.GetKey (KeyCode.RightArrow)){
			velocity = Vector3.right * speed;

			if(transform.GetComponent<Rigidbody2D>().velocity.magnitude < maxSpeed){
				Move();
			}
		}
	}


	public void Move(){
		transform.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Force);
	}
}

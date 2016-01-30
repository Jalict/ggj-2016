using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody2D))]

public class Player : MonoBehaviour {
	
	Vector3 velocity;
	public float speed = 10;
	public float jumpSpeed = 10;
	public float maxSpeed = 10;
	// Use this for initialization
	void Start () {
		velocity = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
            velocity.x = speed * Input.GetAxis("Xbox_LeftThumbStickBackForward");
			
			if(transform.GetComponent<Rigidbody2D>().velocity.magnitude < maxSpeed){
				Move();
			}
	

		if(Input.GetKey (KeyCode.UpArrow)){
			velocity += Vector3.up * jumpSpeed;
		}
        Debug.Log(Input.GetAxis("Xbox_LeftThumbStickBackForward"));
	}


	public void Move(){
		transform.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Force);
	}
}

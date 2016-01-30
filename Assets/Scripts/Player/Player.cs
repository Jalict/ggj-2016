using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody2D))]

public class Player : MonoBehaviour {
	
	Vector3 velocity;
	bool onGround;
	public float speed = 10;
	public float jumpSpeed = 300;
	public float maxSpeed = 10;
	// Use this for initialization
	void Start () {
		velocity = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey (KeyCode.LeftArrow) && onGround){
			velocity = Vector3.left * speed;
			
			if(transform.GetComponent<Rigidbody2D>().velocity.magnitude < maxSpeed){
				Move();
			}
		}

		if (Input.GetKey (KeyCode.RightArrow) && onGround){
			velocity = Vector3.right * speed;

			if(transform.GetComponent<Rigidbody2D>().velocity.magnitude < maxSpeed){
				Move();
			}
		}

		if(Input.GetKeyDown (KeyCode.UpArrow)){
			if(onGround){
				velocity = Vector3.up * jumpSpeed;
				Move();
			}
		}
	}


	public void Move(){
		transform.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Force);
	}

	void OnCollisionEnter2D(Collision2D collision){
		if(collision.gameObject.tag == "Ground"){
			onGround = true;
		}
	}

	void OnCollisionExit2D(Collision2D collision){
		if(collision.gameObject.tag == "Ground"){
			onGround = false;
		}
	}
}

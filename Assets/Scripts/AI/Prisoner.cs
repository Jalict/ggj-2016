using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody2D))]

public class Prisoner : MonoBehaviour { //PURIPURI BLACK ANGEL STYLE!
	private Vector3 velocity;
	
	public Vector3 chainPoint;
	public float chainLength;
	public float speed;
	int random;
	
	//Control variables
	public float confusion; //how often will he turn around
	public float tired; //how often will he stand still
	public float restless; //how often will he begin moving
	public float decisionFrequency; //how often will he roll for decisions

	private float decisionTime;

	bool isMoving;
	bool isPanicking;

	// Use this for initialization
	void Start () {
		decisionTime = Time.time + decisionFrequency;
		isMoving = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(isMoving){
			Move();
			if(decisionTime < Time.time){ //Count untill next roll
				//Roll for chance of turning
				//Roll for chance of stopping (if stopping, then break)
				//If nothing has happened, check chainlength to distance to chainpoint

				if(Vector3.Distance(transform.position, chainPoint) >= chainLength){
					isMoving = false;
				}
			}
		}

		if(!isMoving){
			if(decisionTime < Time.time){ //Count untill next roll
				//Roll for chance to start moving
				/*if(){
					isMoving = true;
				}*/
			}
		}
	}

	public void CalculateChance(){

	}

	public void Move(){
		transform.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Force);
		isMoving = true;
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody2D))]

public class Prisoner : MonoBehaviour { //PURIPURI BLACK ANGEL STYLE!
	private Vector3 velocity;
	
	Vector3 chainPoint;
	public GameObject chainEnd;
	public float chainLength;
	public float moveSpeed;
	public float maxSpeed;
	public float health;
	int random;
	
	//Control variables
	public float confusion; //how often will he turn around. Needs to be value between 0-1
	public float tired; //how often will he stand still. Needs to be value between 0-1
	public float restless; //how often will he begin moving. Needs to be value between 0-1
	public float decisionFrequency; //how often will he roll for decisions. Is in seconds

	private float decisionTime;


	bool isMoving;
	bool movingBackToChain;
	bool isPanicking;


    private Rigidbody2D body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();

/*    moveSpeed = 100;
	chainPoint = chainEnd.transform.position;
	chainLength = 10;

	confusion = 0.5f;
	tired = 0.5f;
	restless = 0.5f;
	decisionFrequency = 3;
*/

		decisionTime = Time.time + decisionFrequency;
		isMoving = false;
		velocity = Vector3.left;
	}
	
	// Update is called once per frame
	void Update () {
		MakeDecision();
	}

	public void MakeDecision(){
		if(isMoving){
			Move();
			
			//If nothing has happened, check chainlength to distance to chainpoint
			if(Vector3.Distance(transform.position, chainPoint) >= chainLength && !movingBackToChain){
				movingBackToChain = true;
				velocity *= -1;
			}

			if(movingBackToChain && Vector3.Distance(transform.position, chainPoint) < chainLength-1){
				movingBackToChain = false;
			}
			
			if(decisionTime < Time.time && !movingBackToChain){ //Count untill next roll
				//Roll for chance of turning
				if(CalculateChance(confusion)){
					velocity *= -1;
				}
				//Roll for chance of stopping (if stopping, then break)
				if(CalculateChance(tired)){
					isMoving = false;
					decisionTime = Time.time + decisionFrequency;
					return;
				}

				decisionTime = Time.time + decisionFrequency; //Reset decision count
			}
		}

		if(!isMoving){
			if(decisionTime < Time.time){ //Count untill next roll
				//Roll for chance to start moving
				if(CalculateChance(restless)){
					isMoving = true;
				}

				decisionTime = Time.time + decisionFrequency; //Reset decision count
			}
		}
	}

	///<summary>When Player gets damaged</summary>
    ///<param>damage</param>
    ///<return>whether or not the player died from the hit</return>
    public bool OnHit(float damage){
        health -= damage;
        if(health <= 0){
            Die();
            return true;
        }
        
        CameraShake.Instance.start(.2f, .5f);

        return false;
    }

	public void Die(){
		//play death thing here
		//give blood to player
		Object.Destroy(this.gameObject);
	}

	public bool CalculateChance(float chance){
		float random = Random.Range(0, 10);
		random /= 10;
		if(chance < random)
			return true;
		else
			return false;
	}

	public void Move(){
		body.AddForce(velocity * moveSpeed * Time.deltaTime);
		if(body.velocity.magnitude > maxSpeed){
			body.velocity = body.velocity.normalized * maxSpeed;
		}
	}
}

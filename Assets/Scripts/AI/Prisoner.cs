using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody2D))]

public class Prisoner : MonoBehaviour { //PURIPURI BLACK ANGEL STYLE!
	private Vector3 velocity;
	
	Vector3 chainPoint;
    public PrisonerSpawner spawner;
    public GameObject chainEnd;
	public float chainLength;
	public float moveSpeed = 100;
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

    private Animator animator;


    private Rigidbody2D body;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();

		decisionTime = Time.time + decisionFrequency;
		isMoving = false;
        
		float r = (Random.Range(0, 2) * 2 - 1f);
        velocity = r * Vector3.left;
        
        transform.localScale = new Vector3(r, 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		MakeDecision();
	}

	public void MakeDecision(){
		if(isMoving){

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
            
            
			Move();
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

        animator.SetFloat("abs_vel_x",Mathf.Abs(body.velocity.x));
        
        if(body.velocity.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (body.velocity.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
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
        spawner.PrisonerDied();
        Object.Destroy(this.gameObject);
	}

	public bool CalculateChance(float chance){
		return chance > Random.Range(0f, 1f);
	}

	public void Move(){
		body.AddForce(velocity * moveSpeed * Time.deltaTime);
		if(body.velocity.magnitude > maxSpeed){
			body.velocity = body.velocity.normalized * maxSpeed;
		}
	}
}

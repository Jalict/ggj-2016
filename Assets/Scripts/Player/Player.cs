using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody2D))]

public class Player : MonoBehaviour {

    public IAbility leftTriggerAbility;
    public IAbility rightTriggerAbility;

    public int level = 0;

	private Vector3 velocity;
	public bool Controller;
	public float speed = 10;
	public float jumpSpeed = 300;
	public float maxSpeed = 10;


    public float health = 2;
    public float maxHealth = 2;
    public float regenHealth = .1f;

    bool doingRitual;
    bool atAltar;
    float altarTimeStamp;
    float altarWaitTime;

	// Use this for initialization
	void Start () {
		velocity = new Vector3(0,0,0);
        SetToLevel(level);
        altarWaitTime = 2.0f;
	}

	// Update is called once per frame
	void Update()
	{
        health += regenHealth * Time.deltaTime;
        if(health > maxHealth) 
            health = maxHealth;

		if (!Controller && !doingRitual)
		{
			if (Input.GetKey(KeyCode.X))
			{
				if(leftTriggerAbility != null)
                    leftTriggerAbility.Cast();
            }
            if (Input.GetKey(KeyCode.C))
			{
                if(rightTriggerAbility != null)
                    rightTriggerAbility.Cast();
			}
            
			if (Input.GetKey(KeyCode.LeftArrow) && OnGround())
			{
				velocity = Vector3.left * speed;

				if (transform.GetComponent<Rigidbody2D>().velocity.magnitude < maxSpeed)
				{
					Move();
				}
			}

			if (Input.GetKey(KeyCode.RightArrow) && OnGround())
			{
				velocity = Vector3.right * speed;

				if (transform.GetComponent<Rigidbody2D>().velocity.magnitude < maxSpeed)
				{
					Move();
				}
			}

			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				if (OnGround())
				{
					velocity = Vector3.up * jumpSpeed;
					Move();
				}
			}

			if(atAltar){
				if(altarTimeStamp < Time.time){
					if(Input.GetKeyDown(KeyCode.A))
						StartCoroutine(Ritual(4));
				}
			}
		}

		if (Controller && !doingRitual)
		{
			if ((int)Input.GetAxis("Xbox_LeftTrigger") == 1)
			{
				if(leftTriggerAbility != null)
                    leftTriggerAbility.Cast();
            }
            if ((int)Input.GetAxis("Xbox_RightTrigger") == 1)
			{
                if(rightTriggerAbility != null)
                    rightTriggerAbility.Cast();
			}
			


			if (OnGround())
			{
				velocity.x = speed * Input.GetAxis("Xbox_LeftThumbStickBackForward");
				if (transform.GetComponent<Rigidbody2D>().velocity.magnitude < maxSpeed)
				{
					Move();
				}
			}

			if (Input.GetButtonDown("Xbox_AButton") && OnGround())
			{
				velocity.y = jumpSpeed;
				Move();
				velocity = new Vector3(0,0,0);

			}

			if(atAltar){
				if(altarTimeStamp < Time.time){
					if(Input.GetButtonDown("Xbox_AButton"))
						StartCoroutine(Ritual(4));
				}
			}
		}
	}
    
    public void Die(){
        Respawn(3);
    }
    
    public void KilledPlayer(Player player){
    }
    
    public void Respawn(float time = 0){
        health = maxHealth;
        GameManager.Instance.RespawnPlayer(this,time);
	}

	public void RitualFail(){
		Debug.Log("YOU FAILED THE RITUAL");
		//TODO - Graphic feedback
		//TODO - Some drawback?
		doingRitual = false;
	}

	public void RitualSuccess(){
		Debug.Log("YOU SUCCEEDED THE RITUAL");
		//TODO - Graphic feedback
		//TODO - Transform
        SetToLevel(++level);
		doingRitual = false;
	}

	IEnumerator Ritual(int seqLength){
		doingRitual = true;
		
		string[] keys = new string[4] {"Xbox_AButton", "Xbox_BButton", "Xbox_XButton", "Xbox_YButton"};
		int randNum = Random.Range(0, keys.Length);
		
		bool ritualFail = false;

		for(int i = 0; i < seqLength; i++){
			Debug.Log(keys[randNum]);
			yield return new WaitForSeconds(0.001f);
			//TODO - Graphic feedback for button press
			while(true){
				if(Input.GetButtonDown(keys[0])){
					if(randNum == 0)
						break;
					else{
						RitualFail();
						ritualFail = true;
						break;
					}
				}

				if(Input.GetButtonDown(keys[1])){
					if(randNum == 1)
						break;
					else{
						RitualFail();
						ritualFail = true;
						break;
					}
				}
			
				if(Input.GetButtonDown(keys[2])){
					if(randNum == 2)
						break;
					else{
						RitualFail();
						ritualFail = true;
						break;
					}
				}

				if(Input.GetButtonDown(keys[3])){
					if(randNum == 3)
						break;
					else{
						RitualFail();
						ritualFail = true;
						break;
					}
				}

				yield return null;
			}
			if(ritualFail){
				break;
			}

			randNum = Random.Range(0, keys.Length);
		}

		if(!ritualFail){
			RitualSuccess();
		}
	}

	public void Move(){
		transform.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Force);
	}


	public bool OnGround()
	{
		//TODO
		RaycastHit2D hit = Physics2D.Raycast(transform.position + ((Vector3.down / 2) * 1.05f), Vector3.down, 0.2f);
		if (hit.collider != null && hit.collider.gameObject.CompareTag("Ground"))
		{
			return true;
		}

		return false;
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

        return false;
    }
    
    
     private void SetToLevel(int level){         
       //TODO: handle animation of levelup/down
       switch(level){
            case 0:
                {
                    speed = 10;
                    jumpSpeed = 300;
                    maxSpeed = 10;

                    health = 2;
                    maxHealth = 2;
                    regenHealth = .1f;

                 
                }
                break;
            case 1:
                {
                    speed = 8;
                    jumpSpeed = 400;
                    maxSpeed = 15;

                    health = 4;
                    maxHealth = 4;
                    regenHealth = 0f;

                }
                break;
            case 2:
                {
                    speed = 8;
                    jumpSpeed = 300;
                    maxSpeed = 17;

                    health = 5;
                    maxHealth = 5;
                    regenHealth = .1f;
                }
                break;
            case 3:
                {
                    speed = 20;
                    jumpSpeed = 300;
                    maxSpeed = 20;

                    health = 10;
                    maxHealth = 10;
                    regenHealth = .1f;
                }
                break;
            default:
                Debug.LogError("Level Index out of bounds");
                break;
        }
        this.level = level;
    }

    void OnTriggerEnter2D(Collider2D col){
		altarTimeStamp = altarWaitTime+Time.time;
    	if(col.gameObject.CompareTag("Altar")){
    		atAltar = true;
    	}
    }

    void OnTriggerExit2D(Collider2D col){
    	if(col.gameObject.CompareTag("Altar")){
    		atAltar = false;
    	}
    }
}

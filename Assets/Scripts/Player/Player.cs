using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[RequireComponent(typeof (Rigidbody2D))]

public class Player : MonoBehaviour {
	//raypoint positions
	public GameObject rayPoint1;
	public GameObject rayPoint2;

	public bool onGround;


    public GameObject bloodSpatterPrefab;
    //active abilities
    private IAbility leftTriggerAbility;
    private IAbility rightTriggerAbility;

    //referneces to palyer abilities
    private FireAbility fireAbility;
    private ShieldAbility shieldAbility;

    public int level = 0;

	private Vector3 velocity;
	public bool Controller;
	public float speed = 10;
	public float jumpSpeed = 300;
	public float maxJumpSpeed = 10;
	public float maxSpeed = 10;

    public float lastJumpTime = 0;
    public float jumpCooldown = 0.5f;

    //Controller
    public int playerNum;
    public PlayerIndex playerIndex; // DO NOT TOUCH
	private GamePadState state;

    public float health = 2;
    public float maxHealth = 2;
    public float regenHealth = .1f;

	private Animator animController;

    bool doingRitual;
    bool atAltar;
    AltarSprite Altar;
    float altarTimeStamp;
    float altarWaitTime;

	// Use this for initialization
	void Start () {
		playerIndex = (PlayerIndex)playerNum;
        fireAbility = GetComponent<FireAbility>();
        shieldAbility = GetComponent<ShieldAbility>();

		velocity = new Vector3(0,0,0);
        SetToLevel(level);
        altarWaitTime = 2.0f;


		animController = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update()
	{
        velocity = Vector3.zero;
        health += regenHealth * Time.deltaTime;

        onGround = OnGround();
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
            
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				velocity += Vector3.left * speed ;
			}

			if (Input.GetKey(KeyCode.RightArrow))
			{
				velocity += Vector3.right * speed ;
			}

			if (Input.GetKey(KeyCode.UpArrow) && lastJumpTime + jumpCooldown < Time.time && OnGround())
			{
                velocity += Vector3.up * jumpSpeed ;
                lastJumpTime = Time.time;
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
			// state = GamePad.GetState(playerIndex);
			if ((int)Input.GetAxis("Xbox"+playerIndex+"_LeftTrigger") == 1)
			{
				if(leftTriggerAbility != null)
                    leftTriggerAbility.Cast();
            }
            if ((int)Input.GetAxis("Xbox"+playerIndex+"_RightTrigger") == 1)
			{
                if(rightTriggerAbility != null)
                    rightTriggerAbility.Cast();
			}
				
			velocity.x += speed * Input.GetAxis("Xbox"+playerIndex+"_X_Axis_Left");

			if (Input.GetButton("Xbox"+playerIndex+"_AButton") && lastJumpTime + jumpCooldown < Time.time && OnGround())
			{
				if((atAltar && altarTimeStamp > Time.time) || !atAltar){
					velocity.y += jumpSpeed;
                    lastJumpTime = Time.time;
                }
			}

			if(atAltar){
				if(altarTimeStamp < Time.time){
					if(Input.GetButtonDown("Xbox"+playerIndex+"_AButton"))
						StartCoroutine(Ritual(4));
				}
			}
		}
		Debug.DrawLine(transform.position, new Vector3(transform.position.x+(float)Input.GetAxis("Xbox" + playerIndex + "_Look_X"),transform.position.y-
                                                               (float)Input.GetAxis("Xbox" + playerIndex + "_Look_Y"), 0), Color.red);
        //Debug.Log((float)Input.GetAxis("Xbox" + playerIndex + "_Look_Y") + " , " + (float)Input.GetAxis("Xbox" + playerIndex + "_Look_X") + "," + 0);

        Move();
        
		animController.SetFloat ("abs_velocity_x", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
		animController.SetFloat ("velocity_y", GetComponent<Rigidbody2D>().velocity.y);
		animController.SetBool ("OnGround", onGround);
		animController.SetBool ("IsSummoning", doingRitual);
        

        if(GetComponent<Rigidbody2D>().velocity.x < -0.01)
            transform.localScale = new Vector3(-1,1,1);        
        else if(GetComponent<Rigidbody2D>().velocity.x > 0.01)
            transform.localScale = new Vector3(1,1,1);


        Vector3 v = transform.GetComponent<Rigidbody2D>().velocity;
        
        if (Mathf.Abs(v.x) > maxSpeed){
        	Vector2 temp = v;
        	temp.x = Mathf.Sign(temp.x) * maxSpeed;
        	velocity = temp;
        }

        if(Mathf.Abs(v.y) > maxJumpSpeed){
        	Vector2 temp = v;
        	temp.y = Mathf.Sign(temp.y) * maxJumpSpeed;
        	velocity = temp;
        }
    }
    
    public void Die(){
        CameraShake.Instance.start(.5f, 1f);
        Respawn(3);
        
        Instantiate(bloodSpatterPrefab, transform.position, bloodSpatterPrefab.transform.rotation);
    }
    
    public void KilledPlayer(Player player){
    }  
    public void KilledPrisoner(Prisoner prisoner){
    }
    
    public void Respawn(float time = 0){
        health = maxHealth;
        GameManager.Instance.RespawnPlayer(this,time);
	}

	public void RitualFail(){
		Debug.Log("YOU FAILED THE RITUAL");
		//TODO - Graphic feedback
		//TODO - Some drawback?
        
        CameraShake.Instance.start(.2f, .2f);
		doingRitual = false;
	}

	public void RitualSuccess(){
		Debug.Log("YOU SUCCEEDED THE RITUAL");
		//TODO - Graphic feedback
		//TODO - Transform
        SetToLevel(++level);
        
        CameraShake.Instance.start(.5f, .5f);
		doingRitual = false;
	}

	IEnumerator Ritual(int seqLength){
		doingRitual = true;
		
		string[] keys = new string[4] {"Xbox"+playerIndex+"_AButton", "Xbox"+playerIndex+"_XButton", "Xbox"+playerIndex+"_YButton", "Xbox"+playerIndex+"_BButton"};
		int randNum = Random.Range(0, keys.Length);

		bool ritualFail = false;

		for(int i = 0; i < seqLength; i++){
			if(Altar != null)
				Altar.ChangeSprite(randNum);
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
                if(!ritualFail)
                    CameraShake.Instance.start(.05f, .05f * seqLength);
                    
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
		transform.GetComponent<Rigidbody2D>().AddForce(velocity * Time.deltaTime, ForceMode2D.Force);
	}


	public bool OnGround()
	{
		//TODO
		RaycastHit2D hit = Physics2D.Raycast(rayPoint1.transform.position, Vector3.down, 0.2f);
		if (hit.collider != null && hit.collider.gameObject.CompareTag("Ground"))
		{
			return true;
		}

		RaycastHit2D hit2 = Physics2D.Raycast(rayPoint2.transform.position, Vector3.down, 0.2f);
		if (hit2.collider != null && hit2.collider.gameObject.CompareTag("Ground"))
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

        animController.SetTrigger("IsHit");

        CameraShake.Instance.start(.2f, .5f);

        return false;
    }
    
    
     private void SetToLevel(int level){         
       //TODO: handle animation of levelup/down
       switch(level){
            case 0:
                {
                    speed = 10000;
                    jumpSpeed = 100000;
                    maxSpeed = 2;
                    maxJumpSpeed = 10;

                    health = 2;
                    maxHealth = 2;
                    regenHealth = .1f;

                    fireAbility.cooldown = 1;
                    fireAbility.damage = 1;
                    fireAbility.projectileSpeed = 10000;

                    rightTriggerAbility = fireAbility;

                    shieldAbility.cooldown = 1;
                    shieldAbility.activeTime = 1;

                    leftTriggerAbility = shieldAbility;
                }
                break;
            case 1:
                {
                    speed = 10000;
                    jumpSpeed = 100000;
                    maxSpeed = 2;
                    maxJumpSpeed = 10;

                    health = 4;
                    maxHealth = 4;
                    regenHealth = 0f;

                    fireAbility.cooldown = 1;
                    fireAbility.damage = 2;
                    fireAbility.projectileSpeed = 10000;

                    rightTriggerAbility = fireAbility;

                    shieldAbility.cooldown = .5f;
                    shieldAbility.activeTime = .5f;

                    leftTriggerAbility = shieldAbility;
                }
                break;
            case 2:
                {
                    speed = 10000;
                    jumpSpeed = 100000;
                    maxSpeed = 2;
                    maxJumpSpeed = 10;
                    
                    health = 5;
                    maxHealth = 5;
                    regenHealth = .1f;
                }
                break;
            case 3:
                {
                    speed = 10000;
                    jumpSpeed = 100000;
                    maxSpeed = 2;
                    maxJumpSpeed = 10;

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

    		if(Altar == null){
    			Altar = col.gameObject.GetComponent<AltarSprite>();
    			Altar.spriteRend.enabled = true;
    		}
    	}
    }

    void OnTriggerExit2D(Collider2D col){
    	if(col.gameObject.CompareTag("Altar")){
	    		atAltar = false;
    		if(Altar != null){
	    		Altar.spriteRend.enabled = false;
	    		Altar = null;
    		}
    	}
    }
}

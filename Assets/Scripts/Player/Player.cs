using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[RequireComponent(typeof (Rigidbody2D))]

public class Player : MonoBehaviour {
	//raypoint positions
	public GameObject rayPoint1;
	public GameObject rayPoint2;

	public GameObject sideRayPoint1;
	public GameObject sideRayPoint2;
	public GameObject sideRayPoint3;

    public GameObject overheadPoint;
    public GameObject groundCrumblePrefab;

    public bool onGround;
    private bool isShooting = false;


    public GameObject bloodSpatterPrefab;
    //active abilities
    private IAbility leftTriggerAbility;
    private IAbility rightTriggerAbility;

    //references to player abilities
    private FireAbility fireAbility;
    private ShieldAbility shieldAbility;
    private TentacleAbility tentacleAbility;

    public int level = 0;

	private Vector3 velocity;
	public bool Controller;
	public float speed = 10;
	public float jumpSpeed = 300;
	public float maxJumpSpeed = 10;
	public float maxSpeed = 10;
	public float maxAirSpeed = 10;

    public float lastJumpTime = 0;
    public float jumpCooldown = 0.5f;

	private float transferedJumpSpeed;
	private bool hasSavedJumpSpeed;

    //Controller
    public int playerNum;
    public PlayerIndex playerIndex; // DO NOT TOUCH
	private GamePadState state;
	public float vibration = 0;

    public float health = 2;
    public float maxHealth = 2;
    public float regenHealth = .1f;
	private Animator animController;

    public RuntimeAnimatorController[] animationControllers;
    public RuntimeAnimatorController[] spellAnimationControllers;

    public ParticleSystem footSteps;


    //Blood For Ritual
    public int Blood = 0;
    public int BloodGoal = 40;
    bool doingRitual;
    bool atAltar;
    AltarSprite Altar;
    float altarTimeStamp;
    float altarWaitTime;

    private Rigidbody2D body;

    public Material[] playerMaterials;

    void Start () {
		playerIndex = (PlayerIndex)playerNum;
        fireAbility = GetComponent<FireAbility>();
        shieldAbility = GetComponent<ShieldAbility>();
        tentacleAbility = GetComponent<TentacleAbility>();
		animController = GetComponent<Animator> ();

        body = GetComponent<Rigidbody2D>();

		velocity = new Vector3(0,0,0);
        SetToLevel(level);
        altarWaitTime = 2.0f;

        fireAbility.spellAnimationControllers = spellAnimationControllers[playerNum % 4]; 
        gameObject.GetComponent<SpriteRenderer>().material = playerMaterials[playerNum % 4];
		footSteps.startColor = playerMaterials [playerNum].color * 0.75f;
        
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
            
			if (Input.GetKey(KeyCode.LeftArrow) && !MidAirCollideCheck())
			{
				velocity += Vector3.left * speed ;
			}

			if (Input.GetKey(KeyCode.RightArrow) && !MidAirCollideCheck())
			{
				velocity += Vector3.right * speed ;
			}

			if (Input.GetKey(KeyCode.UpArrow) && lastJumpTime + jumpCooldown < Time.time && OnGround())
			{

                velocity = Vector3.up * jumpSpeed ;
                lastJumpTime = Time.time;
			}

			if(!doingRitual && atAltar){
                if(Input.GetKeyDown(KeyCode.A))
                    StartCoroutine(Ritual(4));
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
				isShooting = true;
                
			}
	if(isShooting && (int)Input.GetAxis("Xbox"+playerIndex + "_RightTrigger") ==0){
		if(rightTriggerAbility != null)
                    rightTriggerAbility.Cast();
                isShooting = false;
	}

			if(Mathf.Abs(Input.GetAxis("Xbox"+playerIndex+"_X_Axis_Left")) > 0.25f && !MidAirCollideCheck() && !isShooting)				
				velocity.x = speed * Input.GetAxis("Xbox"+playerIndex+"_X_Axis_Left");

			if (Input.GetButton("Xbox"+playerIndex+"_AButton") && lastJumpTime + jumpCooldown < Time.time && OnGround())
			{
					velocity = Vector2.up * jumpSpeed;
					lastJumpTime = Time.time;
			}

			if(!doingRitual && atAltar && this.Blood >= BloodGoal && this.level ==0){
                if(Input.GetButtonDown("Xbox"+playerIndex+"_YButton"))
                    StartCoroutine(Ritual(4));
				
			}
		}
		Debug.DrawLine(transform.position, new Vector3(transform.position.x+(float)Input.GetAxis("Xbox" + playerIndex + "_Look_X"),transform.position.y-
                                                               (float)Input.GetAxis("Xbox" + playerIndex + "_Look_Y"), 0), Color.red);
        //Debug.Log((float)Input.GetAxis("Xbox" + playerIndex + "_Look_Y") + " , " + (float)Input.GetAxis("Xbox" + playerIndex + "_Look_X") + "," + 0);

        Move();
      
		animController.SetFloat ("abs_velocity_x", Mathf.Abs(body.velocity.x));
		animController.SetFloat ("velocity_y", body.velocity.y);
		animController.SetBool ("OnGround", onGround);
		animController.SetBool ("IsSummoning", doingRitual);
        
        Vector3 v = body.velocity;
        

        if(body.velocity.x < -0.01){
        	Vector3 temp = transform.localScale;
        	temp.x = Mathf.Abs(temp.x)*-1;
            transform.localScale = temp;        
        }
        else if(body.velocity.x > 0.01){
        	Vector3 temp = transform.localScale;
        	temp.x = Mathf.Abs(temp.x);
            transform.localScale = temp;
        }

        if (Mathf.Abs(v.x) > maxSpeed && onGround){
        	Vector2 temp = v;
        	temp.x = Mathf.Sign(temp.x) * maxSpeed;
        	v = temp;
        	body.velocity = v;
        }

        if (Mathf.Abs(v.x) > maxAirSpeed && !onGround){
			Vector2 temp = v;
        	temp.x = Mathf.Sign(temp.x) * maxAirSpeed;
        	v = temp;
        	body.velocity = v;
        }

        if (Mathf.Abs(v.y) > maxJumpSpeed){
        	Vector2 temp = v;
        	temp.y = Mathf.Sign(temp.y) * maxJumpSpeed;
        	v = temp;
        	body.velocity = v;
        }
    }
    
    public void Die(){
        CameraShake.Instance.start(.5f, 1f);
        Respawn(3);
        
        Instantiate(bloodSpatterPrefab, transform.position, bloodSpatterPrefab.transform.rotation);
    }
    
    public void KilledPlayer(Player player){
    	this.Blood +=10;
    	if(this.Blood > 100){
    	this.Blood = 100;
    	if(this.level == 0)
    		GamePad.SetVibration(playerIndex,0.2f,0);
    	}
    }  
    public void KilledPrisoner(Prisoner prisoner){
        this.Blood += 100;
        	if(this.Blood > 100){
    	this.Blood = 100;
    	if(this.level == 0)
    		GamePad.SetVibration(playerIndex,0.2f,0);
    	}
    }
    
    public void Respawn(float time = 0){
        health = maxHealth;
        GameManager.Instance.RespawnPlayer(this,time);
	}

	public void RitualFail(){
		Debug.Log("YOU FAILED THE RITUAL");
		//TODO - Graphic feedback
		//TODO - Some drawback?
		vibration = 0;
        
        if(Controller)
		  GamePad.SetVibration(playerIndex, vibration, vibration);
        
        CameraShake.Instance.start(.2f, .2f);
        Altar.ChangeSprite(2);
        doingRitual = false;
		animController.SetBool ("IsSummoning", doingRitual);
        
	}

	public void RitualSuccess(){
		Debug.Log("YOU SUCCEEDED THE RITUAL");
		//TODO - Graphic feedback
		//TODO - Transform
        
        SetToLevel(++level);
        vibration = 0;
        
        if(Controller)
		  GamePad.SetVibration(playerIndex, vibration, vibration);
        
        this.Blood = 0;
        Altar.ChangeSprite(2);
        CameraShake.Instance.start(.5f, .5f);
		doingRitual = false;
		animController.SetBool ("IsSummoning", doingRitual);
        
	}

	IEnumerator Ritual(int seqLength){
		doingRitual = true;
        string[] keys;
        if(Controller){
		GamePad.SetVibration(playerIndex,0.2f,0.2f);
            keys = new string[4] {"Xbox"+playerIndex+"_AButton", "Xbox"+playerIndex+"_XButton", "Xbox"+playerIndex+"_YButton", "Xbox"+playerIndex+"_BButton"};
        }else
            keys = new string[4] {"k", "j", "i", "l"};
		
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
					if(randNum == 0){
						vibration += 0.2f;
						if(Controller)
                            GamePad.SetVibration(playerIndex, vibration, vibration);
						break;
					}
					else{
						RitualFail();
						ritualFail = true;
						break;
					}
				}

				if(Input.GetButtonDown(keys[1])){
					if(randNum == 1){
						vibration += 0.2f;
						if(Controller)
                            GamePad.SetVibration(playerIndex, vibration, vibration);
						break;
					}
					else{
						RitualFail();
						ritualFail = true;
						break;
					}
				}
			
				if(Input.GetButtonDown(keys[2])){
					if(randNum == 2){
						vibration += 0.2f;
						if(Controller)
                            GamePad.SetVibration(playerIndex, vibration, vibration);
						break;
					}
					else{
						RitualFail();
						ritualFail = true;
						break;
					}
				}

				if(Input.GetButtonDown(keys[3])){
					if(randNum == 3){
						vibration += 0.2f;
						if(Controller)
                            GamePad.SetVibration(playerIndex, vibration, vibration);
						break;
					}
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
		body.AddForce(velocity * Time.deltaTime, ForceMode2D.Force);
	}

	public bool MidAirCollideCheck(){
		if(!OnGround()){
            
			RaycastHit2D hit = Physics2D.Raycast(sideRayPoint1.transform.position, Vector3.left, 2f);
			if (hit.collider != null && hit.collider.gameObject.CompareTag("Ground"))
			{
				return true;
			}

			RaycastHit2D hit2 = Physics2D.Raycast(sideRayPoint2.transform.position, Vector3.left, 2f);
			if (hit2.collider != null && hit2.collider.gameObject.CompareTag("Ground"))
			{
				return true;
			}

			RaycastHit2D hit3 = Physics2D.Raycast(sideRayPoint3.transform.position, Vector3.left, 2f);
			if (hit3.collider != null && hit3.collider.gameObject.CompareTag("Ground"))
			{
				return true;
			}
		}
		return false;
	}

	public bool OnGround()
	{
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
    //make subroutine instance for vibration
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
                    speed = 7000;
                    jumpSpeed = 80000;
                    maxSpeed = 12f;
                    maxAirSpeed = 10f;
                    maxJumpSpeed = 28;

                    health = 2;
                    maxHealth = 2;
                    regenHealth = .1f;

                    fireAbility.cooldown = 1;
                    fireAbility.damage = 1;
                    fireAbility.projectileSpeed = 2000;

                    rightTriggerAbility = fireAbility;

                    shieldAbility.cooldown = 1;
                    shieldAbility.activeTime = 1;

                    leftTriggerAbility = shieldAbility;

                    animController.runtimeAnimatorController = animationControllers[0];
                    
                    rayPoint1.transform.position = new Vector2(transform.position.x + 0.34f, transform.position.y + -0.938f);
					rayPoint2.transform.position = new Vector2(transform.position.x + -0.365f, transform.position.y + -0.938f);

					sideRayPoint1.transform.position = new Vector2(transform.position.x + .65f, transform.position.y + -0.84f);
					sideRayPoint2.transform.position = new Vector2(transform.position.x + .65f, transform.position.y + 0);
					sideRayPoint3.transform.position = new Vector2(transform.position.x + .65f, transform.position.y + 0.68f);

                    ScreenWrapObject wrapObj = GetComponent<ScreenWrapObject>();
                    wrapObj.SetColliderSize(new Vector2(1.00962f, 1.58734f));
                    wrapObj.SetColliderOffset(new Vector2(-0.07074118f, -0.05305576f));
                    wrapObj.InitializeCollliders();
                }
                break;
            case 1:
                {
                    speed = 7000;
                    jumpSpeed = 80000;
                    maxSpeed = 12f;
                    maxAirSpeed = 10f;
                    maxJumpSpeed = 28;

                    health = 4;
                    maxHealth = 4;
                    regenHealth = 0f;

                    tentacleAbility.cooldown = 1;
                    tentacleAbility.damage = 2;
                    tentacleAbility.width = 1;
                    tentacleAbility.height = 1;
                    tentacleAbility.attackSpeed = 1.5f;

//                    rightTriggerAbility = tentacleAbility;

                    shieldAbility.cooldown = .5f;
                    shieldAbility.activeTime = .5f;

                    leftTriggerAbility = shieldAbility;
                    animController.runtimeAnimatorController = animationControllers[1];

                    //resetting raypoint positions
                    rayPoint1.transform.position = new Vector2(transform.position.x -0.223f, transform.position.y + -1.162f);
					rayPoint2.transform.position = new Vector2(transform.position.x + 0.477f, transform.position.y + -1.126f);

					sideRayPoint2.transform.position = new Vector2(transform.position.x + 0.993f, transform.position.y + 0);
					sideRayPoint3.transform.position = new Vector2(transform.position.x + 0.993f, transform.position.y + 0.741f);
					sideRayPoint1.transform.position = new Vector2(transform.position.x + 0.993f, transform.position.y -0.741f);

                    ScreenWrapObject wrapObj = GetComponent<ScreenWrapObject>();
                    wrapObj.SetColliderOffset(new Vector2(0f,0f));
                    wrapObj.SetColliderSize(new Vector2(1.5f, 2f));
                    wrapObj.InitializeCollliders();

                }
                break;
            case 2:
                {
                    speed = 7000;
                    jumpSpeed = 80000;
                    maxSpeed = 12f;
                    maxAirSpeed = 10f;
                    maxJumpSpeed = 28;
                    
                    health = 5;
                    maxHealth = 5;
                    regenHealth = .1f;
                }
                break;
            case 3:
                {
                    speed = 7000;
                    jumpSpeed = 80000;
                    maxSpeed = 12f;
                    maxAirSpeed = 10f;
                    maxJumpSpeed = 28;

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
    			Altar = col.gameObject.GetComponent<Altar>().altarSprite;
    			Altar.spriteRend.enabled = true;
                Altar.ChangeSprite(2);
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

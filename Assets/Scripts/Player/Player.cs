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

	public bool onGround;


    public GameObject bloodSpatterPrefab;
    //active abilities
    private IAbility leftTriggerAbility;
    private IAbility rightTriggerAbility;

    //references to player abilities
    private FireAbility fireAbility;
    private ShieldAbility shieldAbility;

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

    public BoxCollider2D box2D;

	//Blood For Ritual
	public int Blood = 0;
    bool doingRitual;
    bool atAltar;
    AltarSprite Altar;
    float altarTimeStamp;
    float altarWaitTime;

    private Rigidbody2D body;

    public Material[] playerMaterials;

    void Awake(){
    	box2D = GetComponent<BoxCollider2D>();
        gameObject.GetComponent<SpriteRenderer>().material = playerMaterials[playerNum % 4];
        
    }

    void Start () {
		playerIndex = (PlayerIndex)playerNum;
        fireAbility = GetComponent<FireAbility>();
        shieldAbility = GetComponent<ShieldAbility>();
		animController = GetComponent<Animator> ();

        body = GetComponent<Rigidbody2D>();

		velocity = new Vector3(0,0,0);
        SetToLevel(level);
        altarWaitTime = 2.0f;




        fireAbility.spellAnimationControllers = spellAnimationControllers[playerNum % 4];
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

                velocity = Vector3.up * jumpSpeed ;
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

			if(Mathf.Abs(Input.GetAxis("Xbox"+playerIndex+"_X_Axis_Left")) > 0.25f && !MidAirCollideCheck())				
				velocity.x = speed * Input.GetAxis("Xbox"+playerIndex+"_X_Axis_Left");

			if (Input.GetButton("Xbox"+playerIndex+"_AButton") && lastJumpTime + jumpCooldown < Time.time && OnGround())
			{
				if((atAltar && altarTimeStamp > Time.time) || !atAltar){
					velocity = Vector2.up * jumpSpeed;

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
    	if(this.Blood > 100)
    	this.Blood = 100;
    }  
    public void KilledPrisoner(Prisoner prisoner){
        SetToLevel(1);
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
		GamePad.SetVibration(playerIndex, vibration, vibration);
        
        CameraShake.Instance.start(.2f, .2f);
		doingRitual = false;
	}

	public void RitualSuccess(){
		Debug.Log("YOU SUCCEEDED THE RITUAL");
		//TODO - Graphic feedback
		//TODO - Transform
        SetToLevel(++level);
        vibration = 0;
		GamePad.SetVibration(playerIndex, vibration, vibration);
        
        this.Blood = 0;
        
        CameraShake.Instance.start(.5f, .5f);
		doingRitual = false;
	}

	IEnumerator Ritual(int seqLength){
		doingRitual = true;
		GamePad.SetVibration(playerIndex,0.2f,0.2f);
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
					if(randNum == 0){
						vibration += 0.2f;
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
			RaycastHit2D hit = Physics2D.Raycast(sideRayPoint1.transform.position, Vector3.left, 1.5f);
			if (hit.collider != null && hit.collider.gameObject.CompareTag("Ground"))
			{
				return true;
			}

			RaycastHit2D hit2 = Physics2D.Raycast(sideRayPoint2.transform.position, Vector3.left, 1.5f);
			if (hit2.collider != null && hit2.collider.gameObject.CompareTag("Ground"))
			{
				return true;
			}

			RaycastHit2D hit3 = Physics2D.Raycast(sideRayPoint3.transform.position, Vector3.left, 1.5f);
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
                    speed = 8000;
                    jumpSpeed = 100000;
                    maxSpeed = 12f;
                    maxAirSpeed = 8f;
                    maxJumpSpeed = 25;

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

                }
                break;
            case 1:
                {
                    speed = 8000;
                    jumpSpeed = 100000;
                    maxSpeed = 12f;
                    maxAirSpeed = 8f;
                    maxJumpSpeed = 25;

                    health = 4;
                    maxHealth = 4;
                    regenHealth = 0f;

                    fireAbility.cooldown = 1;
                    fireAbility.damage = 2;
                    fireAbility.projectileSpeed = 2000;

                    rightTriggerAbility = fireAbility;

                    shieldAbility.cooldown = .5f;
                    shieldAbility.activeTime = .5f;

                    leftTriggerAbility = shieldAbility;
                    animController.runtimeAnimatorController = animationControllers[1];

                    //resetting raypoint positions
                    rayPoint1.transform.position = new Vector2(transform.position.x + 0.82f, transform.position.y + -1.75f);
					rayPoint2.transform.position = new Vector2(transform.position.x + -0.56f, transform.position.y + -1.75f);

					sideRayPoint1.transform.position = new Vector2(transform.position.x + -1.83f, transform.position.y + -1.4f);
					sideRayPoint2.transform.position = new Vector2(transform.position.x + -1.83f, transform.position.y + 0);
					sideRayPoint3.transform.position = new Vector2(transform.position.x + -1.83f, transform.position.y + 1.56f);

                    //resizing boxcollider and character
                    transform.localScale = new Vector3(0.75f,0.75f,1);
                    box2D.size = new Vector2(2.8f, 3f);
                    box2D.offset = new Vector3(0,0);
                }
                break;
            case 2:
                {
                    speed = 8000;
                    jumpSpeed = 100000;
                    maxSpeed = 12f;
                    maxAirSpeed = 8f;
                    maxJumpSpeed = 25;
                    
                    health = 5;
                    maxHealth = 5;
                    regenHealth = .1f;
                }
                break;
            case 3:
                {
                    speed = 8000;
                    jumpSpeed = 100000;
                    maxSpeed = 12f;
                    maxAirSpeed = 8f;
                    maxJumpSpeed = 25;

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

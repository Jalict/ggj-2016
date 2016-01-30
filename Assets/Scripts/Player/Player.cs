using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody2D))]

public class Player : MonoBehaviour {

    public IAbility leftTriggerAbility;
    public IAbility rightTriggerAbility;

    private Vector3 velocity;
	public bool Controller;
	public float speed = 10;
	public float jumpSpeed = 300;
	public float maxSpeed = 10;
	// Use this for initialization
	void Start () {
		velocity = new Vector3(0,0,0);
	}

	// Update is called once per frame
	void Update()
	{
		if (Controller == false)
		{
			if (Input.GetKeyDown(KeyCode.X))
			{
				if(leftTriggerAbility != null)
                    leftTriggerAbility.Cast();
            }
            if (Input.GetKeyDown(KeyCode.C))
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
		}
		if (Controller)
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
		}
	}
    
    public void Respawn(float time = 0){
        GameManager.Instance.RespawnPlayer(this,time);
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
}

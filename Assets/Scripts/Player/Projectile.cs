using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public AudioClip[] castClip;
	public AudioClip hitClip;
	private AudioSource audSource;

    public Vector2 direction;
    public float speed;

    public float damage = 1;
    public Player sender;

    private Rigidbody2D body;

    private Animator animator;
    public bool isFinished;

    //Setters

    void Start(){
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
		audSource = GetComponent<AudioSource> ();

		audSource.PlayOneShot (castClip [Random.Range (0, castClip.Length)]);
    }
    
    void Update(){
    }

    public void Shoot(){
        body = GetComponent<Rigidbody2D>();
        body.AddForce(direction.normalized * speed * Time.deltaTime, ForceMode2D.Impulse);
    }
    
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            //Check if shield is active

            Player player = collider.gameObject.GetComponent<Player>();
            
            if(player == sender) return;

            if(player.GetComponent<ShieldAbility>() != null){
                ShieldAbility shield = player.GetComponent<ShieldAbility>();
                
                if(shield.isActivated){
                    bool playerFacingRight = Mathf.Sign(player.GetComponent<Rigidbody2D>().velocity.x) >= 0;
                    bool projectileFacingRight = Mathf.Sign(body.velocity.x) >= 0;
                    if(playerFacingRight != projectileFacingRight){ 
                        body.velocity = new Vector2(-body.velocity.x,body.velocity.y);
                        direction = new Vector2(-direction.x,direction.y);
                        
                        CameraShake.Instance.start(.1f, .1f);

                        return;
                    }
                }
            }

            if(player.OnHit(damage)){
                sender.KilledPlayer(player);
            }
            
        }

        if(collider.gameObject.tag == "Prisoner"){
            if (collider.GetComponent<Prisoner>().OnHit(damage))
            {
                sender.KilledPrisoner(collider.GetComponent<Prisoner>());
            }
        }
        body.isKinematic = true;

        animator.SetTrigger("Hit");
		audSource.clip = hitClip;
		audSource.volume = 0.6f;
		audSource.Play ();
        Destroy(this.gameObject,.2f);
    }
    
    void OnTriggerStawy2D(Collider2D collider){
        
    }
    void OnTriggerExit2D(Collider2D collider){
        
    }

}

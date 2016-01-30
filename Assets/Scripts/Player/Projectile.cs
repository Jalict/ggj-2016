using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public Vector2 direction;
    public float speed;
    public float maxSpeed = 10000;

    private Rigidbody2D body;

//Setters

    void Start(){
        body = GetComponent<Rigidbody2D>();
    }
    
    void Update(){
        body.AddForce(direction * speed * Time.deltaTime);
        if(body.velocity.magnitude > maxSpeed)
            body.velocity = body.velocity.normalized * maxSpeed;

        //TODO: remove projectile when out of bounds
    }
    
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Player"){
            //Check if shield is active
            
            if(collision.gameObject.GetComponent<ShieldAbility>() != null){
                ShieldAbility shield = collision.gameObject.GetComponent<ShieldAbility>();
                
                if(shield.isActivated){ //TODO: make shield only work in the "shielded" direction
                    body.velocity = new Vector2(-body.velocity.x,body.velocity.y);
                    direction = new Vector2(-direction.x,direction.y);
                }
            }
        }else
            Destroy(this.gameObject);
    }
    void OnCollisionStawy2D(Collision2D collision){
        
    }
    void OnCollisionExit2D(Collision2D collision){
        
    }

}

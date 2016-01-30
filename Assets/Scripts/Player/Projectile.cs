using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    private Vector2 direction;
    private float speed;

    private Rigidbody2D body;

//Setters
    public void SetDirection(Vector2 direction){
        this.direction = direction;
    }
    public void SetSpeed(float speed){
        this.speed = speed;
    }

    void Start(){
        body = GetComponent<Rigidbody2D>();
    }
    
    void Update(){
        body.AddForce(direction * speed * Time.deltaTime);
        
        //TODO: remove projectile when out of bounds
    }
    
    void OnCollisionEnter2D(Collision2D collision){
        Destroy(this.gameObject);
    }
    void OnCollisionStawy2D(Collision2D collision){
        
    }
    void OnCollisionExit2D(Collision2D collision){
        
    }

}

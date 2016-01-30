using UnityEngine;
using System.Collections;

public class FireAbility : IAbility {

    public float cooldown = 1;
    private float lastCast = 0;
    
    public float projectileSpeed = 10000;
    
    public GameObject projectile_prefab;
    
    public void Update(){
        //test
        if(Input.GetButton("Jump")){
            this.Cast();
        }
    }
    
    public override bool Cast(){
       if (lastCast + cooldown <= Time.time)
       {
            lastCast = Time.time;
            GameObject go = Object.Instantiate(projectile_prefab, transform.position, Quaternion.identity) as GameObject;
            Projectile projectile = go.GetComponent<Projectile>();
            projectile.direction = (transform.localScale.x == 1 ? 1:-1) * Vector2.right; //TODO: when sprite flipping is implemented this needs to be checked
            projectile.speed = projectileSpeed;
            


            Physics2D.IgnoreCollision(go.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            return true;
       }    
       return false;
    }
    
}

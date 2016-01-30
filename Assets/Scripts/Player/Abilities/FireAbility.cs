using UnityEngine;
using System.Collections;

public class FireAbility : IAbility {

    public float cooldown = 1;
    private float lastCast = 0;
    
    public float projectileSpeed = 10;
    
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
            GameObject go = Object.Instantiate(projectile_prefab, Vector2.zero, Quaternion.identity) as GameObject;
            Projectile projectile = go.GetComponent<Projectile>();
            projectile.SetDirection(Vector2.right);
            projectile.SetSpeed(projectileSpeed);

            return true;
       }
       return false;
    }
    
}

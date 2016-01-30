using UnityEngine;
using System.Collections;

public class FireAbility : IAbility {

    public float cooldown = 1;
    private float lastCast = 0;

    public float damage = 1;

    public float projectileSpeed = 10000;
    
    public GameObject projectile_prefab;
    
    public override bool Cast(){
       if (lastCast + cooldown <= Time.time)
       {
           
            lastCast = Time.time;
            GameObject obj = Object.Instantiate(projectile_prefab, transform.position, Quaternion.identity) as GameObject;
            Projectile projectile = obj.GetComponent<Projectile>();
            projectile.direction = (Mathf.Sign(owner.GetComponent<Rigidbody2D>().velocity.x) >= 0 ? 1:-1) * Vector2.right; //TODO: when sprite flipping is implemented this needs to be checked
            projectile.speed = projectileSpeed;
            projectile.sender = owner;
            projectile.damage = damage;
            CameraShake.Instance.start(.1f, .2f);

            ScreenWrap.IgnoreCollisions(gameObject, obj);

            return true;
       }    
       return false;
    }
    
}

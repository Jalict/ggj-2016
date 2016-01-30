using UnityEngine;
using System.Collections;
using XInputDotNetPure;


public class FireAbility : IAbility {

    public float cooldown = 1;
    private float lastCast = 0;

    public float damage = 1;

    public float projectileSpeed = 10000;
    
    private PlayerIndex playerIndexRef;
    private Player playerRef;
    
    public GameObject projectile_prefab;
    
    public override bool Cast(){
       if (lastCast + cooldown <= Time.time)
       {
            playerRef = GetComponent<Player>();
            playerIndexRef = playerRef.playerIndex;
            lastCast = Time.time;
             GameObject obj = Object.Instantiate(projectile_prefab, transform.position+new Vector3((float)Input.GetAxis("Xbox" + playerIndexRef + "_Look_X"),
                -(float)Input.GetAxis("Xbox" + playerIndexRef + "_Look_Y")), Quaternion.identity) as GameObject;
            Projectile projectile = obj.GetComponent<Projectile>();
            projectile.direction = new Vector2((float)Input.GetAxis("Xbox" + playerIndexRef + "_Look_X"), -(float)Input.GetAxis("Xbox" + playerIndexRef + "_Look_Y"));
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

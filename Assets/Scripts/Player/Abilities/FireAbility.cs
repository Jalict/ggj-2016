using UnityEngine;
using UnityEditor.Animations;
using System.Collections;
using XInputDotNetPure;


public class FireAbility : IAbility {

    public float cooldown = 1;
    private float lastCast = 0;

    public float damage = 1;

    public float projectileSpeed = 1;
    
    private PlayerIndex playerIndexRef;
    private Player playerRef;
    
    public GameObject projectile_prefab;

    [HideInInspector]
    public RuntimeAnimatorController spellAnimationControllers;

    public override bool Cast(){
       if (lastCast + cooldown <= Time.time)
       {
            playerRef = GetComponent<Player>();
            playerIndexRef = playerRef.playerIndex;
            lastCast = Time.time;
             GameObject obj = Object.Instantiate(projectile_prefab, transform.position+new Vector3((float)Input.GetAxis("Xbox" + playerIndexRef + "_X_Axis_Left"),
                -(float)Input.GetAxis("Xbox" + playerIndexRef + "_Y_Axis_Left")), Quaternion.identity) as GameObject;
            Projectile projectile = obj.GetComponent<Projectile>();
            projectile.direction = new Vector2((float)Input.GetAxis("Xbox" + playerIndexRef + "_X_Axis_Left"), -(float)Input.GetAxis("Xbox" + playerIndexRef + "_Y_Axis_Left"));
            projectile.speed = projectileSpeed;
            projectile.sender = owner;
            projectile.damage = damage;

            obj.GetComponent<Animator>().runtimeAnimatorController = spellAnimationControllers;

            CameraShake.Instance.start(.1f, .2f);
            projectile.Shoot();

            ScreenWrap.IgnoreCollisions(gameObject, obj);

            return true;
       }    
       return false;
    }
    
}

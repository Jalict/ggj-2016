﻿using UnityEngine;
using System.Collections;
using XInputDotNetPure;


public class FireAbility : IAbility {

    public float cooldown = 1;
    private float lastCast = 0;

    public float damage = 1;

    public float projectileSpeed = 1;

    public float XAxis;
    public float YAxis;
    
    private PlayerIndex playerIndexRef;
    private Player playerRef;
    
    public GameObject projectile_prefab;

    [HideInInspector]
    public RuntimeAnimatorController spellAnimationControllers;

    public override bool Cast(){
       if (lastCast + cooldown <= Time.time)
       {
            XAxis = (float)Input.GetAxis("Xbox"+playerIndexRef+"_X_Axis_Left");
            YAxis = -(float)Input.GetAxis("Xbox" + playerIndexRef + "_Y_Axis_Left");
            if(Mathf.Abs(XAxis) > 0.1f && Mathf.Abs(YAxis) < 0.1f){
                XAxis = Mathf.Sign(XAxis);
                YAxis = 0;
            }else if(Mathf.Abs(XAxis) < .1f && Mathf.Abs(YAxis) < 0.1f){
                YAxis = 0;
                XAxis = owner.transform.localScale.x;
            }

            playerRef = GetComponent<Player>();
            playerIndexRef = playerRef.playerIndex;
            lastCast = Time.time;
             GameObject obj = Object.Instantiate(projectile_prefab, transform.position+new Vector3(XAxis,
                YAxis), Quaternion.identity) as GameObject;
            Projectile projectile = obj.GetComponent<Projectile>();
            projectile.direction = new Vector2(XAxis,YAxis);
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

    public override bool CanCast(){
        return lastCast + cooldown <= Time.time;
    }
    
}

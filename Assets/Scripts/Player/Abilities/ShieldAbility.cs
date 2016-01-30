using UnityEngine;
using System.Collections;

public class ShieldAbility : IAbility {

    public float cooldown = 1;
    public float activeTime = 1;
    private float lastCast = 0;

    public bool isActivated = false;
    
    public override bool Cast(){
       if (!isActivated && lastCast + cooldown <= Time.time)
       {
            isActivated = true;
            Debug.Log("Shield Up!");
            CameraShake.Instance.start(.1f, .05f);
            StartCoroutine(stopShieldIn(activeTime));
            return true;
       }    
       return false;
    }
    
    IEnumerator stopShieldIn(float time){
        yield return new WaitForSeconds(time);
        lastCast = Time.time;
        CameraShake.Instance.start(.05f, .05f);
        isActivated = false;
        Debug.Log("Shield Down!");

    }
    
}

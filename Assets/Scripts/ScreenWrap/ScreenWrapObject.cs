using UnityEngine;
using System.Collections;

public class ScreenWrapObject : MonoBehaviour {

    public enum ColliderType{
        Circle,
        Box
        
    }

    public ColliderType colliderType = ColliderType.Circle;

    private Collider2D above;
    private Collider2D below;
    private Collider2D right;
    private Collider2D left;

    private Vector2 viewSize;

    // Use this for initialization
    void Start () {
        if (colliderType == ColliderType.Circle)
        {
            above = gameObject.AddComponent<CircleCollider2D>();
            below = gameObject.AddComponent<CircleCollider2D>();
            right = gameObject.AddComponent<CircleCollider2D>();
            left = gameObject.AddComponent<CircleCollider2D>();
        }else{
            above = gameObject.AddComponent<BoxCollider2D>();
            below = gameObject.AddComponent<BoxCollider2D>();
            right = gameObject.AddComponent<BoxCollider2D>();
            left = gameObject.AddComponent<BoxCollider2D>();
        }
        viewSize = ScreenWrap.GetSize();
        
        below.offset = new Vector2(0, viewSize.y);
        above.offset = new Vector2(0, -viewSize.y);
        
        right.offset = new Vector2(viewSize.x,0);        
        left.offset = new Vector2(-viewSize.x,0);

    }
    
    void Update(){
        Vector2 newPos = transform.position;
        if(transform.position.x < -viewSize.x/2)  
            newPos.Set(
                viewSize.x / 2 - Mathf.Abs(transform.position.x - viewSize.x / 2) % viewSize.x,
                transform.position.y);        
        else if(transform.position.x > viewSize.x/2)
            newPos.Set(
                -viewSize.x / 2 + (transform.position.x + viewSize.x / 2) % viewSize.x,
                transform.position.y);      
            
        if(transform.position.y < -viewSize.y/2)
            newPos.Set(
                transform.position.x,
                viewSize.y / 2 - Mathf.Abs(transform.position.y - viewSize.y / 2) % viewSize.y);         
        else if(transform.position.y > viewSize.y/2)
            newPos.Set(
                transform.position.x,
                -viewSize.y / 2 + (transform.position.y + viewSize.y / 2) % viewSize.y);
            
        transform.position = newPos;
    }
}

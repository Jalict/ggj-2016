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
        Vector2 viewSize = ScreenWrap.GetSize();
        
        below.offset = new Vector2(0, viewSize.y);
        above.offset = new Vector2(0, -viewSize.y);
        
        right.offset = new Vector2(viewSize.x,0);        
        left.offset = new Vector2(-viewSize.x,0);

    }
}

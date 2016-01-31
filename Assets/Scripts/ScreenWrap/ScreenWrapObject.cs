using UnityEngine;
using System.Collections;

public class ScreenWrapObject : MonoBehaviour
{

    public enum ColliderType
    {
        Circle,
        Box

    }

    public ColliderType colliderType = ColliderType.Circle;

    [HideInInspector]
    public Collider2D main;
    [HideInInspector]
    public Collider2D above;
    [HideInInspector]
    public Collider2D below;
    [HideInInspector]
    public Collider2D right;
    [HideInInspector]
    public Collider2D left;

    private Vector2 viewSize;

    void Awake()
    {
        main = GetComponent<Collider2D>();


        if (colliderType == ColliderType.Circle)
        {
            above = gameObject.AddComponent<CircleCollider2D>();
            below = gameObject.AddComponent<CircleCollider2D>();
            right = gameObject.AddComponent<CircleCollider2D>();
            left = gameObject.AddComponent<CircleCollider2D>();
        }
        else
        {
            above = gameObject.AddComponent<BoxCollider2D>();
            below = gameObject.AddComponent<BoxCollider2D>();
            right = gameObject.AddComponent<BoxCollider2D>();
            left = gameObject.AddComponent<BoxCollider2D>();
        }
    }

    // Use this for initialization
    void Start()
    {
        InitializeCollliders();
    }

    public void InitializeCollliders()
    {
        if (colliderType == ColliderType.Circle)
        {
            ((CircleCollider2D)above).radius = ((CircleCollider2D)main).radius;
            ((CircleCollider2D)below).radius = ((CircleCollider2D)main).radius;
            ((CircleCollider2D)right).radius = ((CircleCollider2D)main).radius;
            ((CircleCollider2D)left).radius = ((CircleCollider2D)main).radius;

        }
        else
        {
            ((BoxCollider2D)above).size = ((BoxCollider2D)main).size;
            ((BoxCollider2D)below).size = ((BoxCollider2D)main).size;
            ((BoxCollider2D)right).size = ((BoxCollider2D)main).size;
            ((BoxCollider2D)left).size = ((BoxCollider2D)main).size;
        }
        viewSize = ScreenWrap.GetSize();

        below.offset = new Vector2(0 + main.offset.x, viewSize.y + main.offset.y);
        above.offset = new Vector2(0 + main.offset.x, -viewSize.y + main.offset.y);
        right.offset = new Vector2(viewSize.x + main.offset.x, 0 + main.offset.y);
        left.offset = new Vector2(-viewSize.x + main.offset.x, 0 + main.offset.y);

        above.isTrigger = main.isTrigger;
        below.isTrigger = main.isTrigger;
        right.isTrigger = main.isTrigger;
        left.isTrigger = main.isTrigger;


    }

    void Update()
    {
        Vector2 pos = transform.position - Camera.main.transform.position;
        bool wrap = false;
        if (pos.x < -viewSize.x / 2){
            pos.Set(
                viewSize.x / 2 - Mathf.Abs(pos.x - viewSize.x / 2) % viewSize.x,
                pos.y);
            wrap = true;
        }else if(pos.x > viewSize.x/2){
            pos.Set(
                -viewSize.x / 2 + (pos.x + viewSize.x / 2) % viewSize.x,
                pos.y); 
            wrap = true;
        }

        if (pos.y < -viewSize.y / 2)
        {
            pos.Set(
                pos.x,
                viewSize.y / 2 - Mathf.Abs(pos.y - viewSize.y / 2) % viewSize.y);
            wrap = true;
        }
        else if (pos.y > viewSize.y / 2)
        {
            pos.Set(
                pos.x,
                -viewSize.y / 2 + (pos.y + viewSize.y / 2) % viewSize.y);
            wrap = true;
        }


        if (wrap)
        {
            if(GetComponent<Player>() != null)
                GetComponent<Player>().footSteps.Stop();

            transform.position = pos + (Vector2)Camera.main.transform.position;
            
            if(GetComponent<Player>() != null)
                GetComponent<Player>().footSteps.Play();
        }
    }
    
    
    public void SetColliderSize(Vector2 v){
        if (colliderType == ColliderType.Circle)
            ((CircleCollider2D)main).radius = v.x;        
        else
            ((BoxCollider2D)main).size = v;
    }
    
    public void SetColliderOffset(Vector2 v){
        main.offset = v;
    }
    
    
}

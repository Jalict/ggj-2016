using UnityEngine;
using System.Collections;

public class ScreenWrap : MonoBehaviour {

    public Camera above;
    public Camera below;
    public Camera right;
    public Camera left;

    // Use this for initialization
    void Awake () {
        Vector2 viewSize = GetSize();

        Camera main = Camera.main;

        above.orthographicSize = main.orthographicSize;
        below.orthographicSize = main.orthographicSize;
        right.orthographicSize = main.orthographicSize;
        left.orthographicSize = main.orthographicSize;

        below.transform.localPosition = new Vector2(0, viewSize.y);
        above.transform.localPosition = new Vector2(0, -viewSize.y);
        
        right.transform.localPosition = new Vector2(viewSize.x,0);        
        left.transform.localPosition = new Vector2(-viewSize.x,0);
    }
    
    public static Vector2 GetSize(){
        return new Vector2((Camera.main.orthographicSize * 2 * (Camera.main.aspect)), Camera.main.orthographicSize * 2);
    }
    
    public static void IgnoreCollisions(GameObject g1, GameObject g2){
        Collider2D[] c1 = g1.GetComponents<Collider2D>();
        Collider2D[] c2 = g2.GetComponents<Collider2D>();
        
        for (int i = 0; i < c1.Length; i++)
        {
            for (int j = 0; j < c2.Length; j++)
            {
                Physics2D.IgnoreCollision(c1[i], c2[j]);
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class ScreenWrap : MonoBehaviour {

    public Camera above;
    public Camera below;
    public Camera right;
    public Camera left;

    // Use this for initialization
    void Start () {
        Vector2 viewSize = GetSize();

        below.transform.localPosition = new Vector2(0, viewSize.y);
        above.transform.localPosition = new Vector2(0, -viewSize.y);
        
        right.transform.localPosition = new Vector2(viewSize.x,0);        
        left.transform.localPosition = new Vector2(-viewSize.x,0);
    }
    
    public static Vector2 GetSize(){
        return new Vector2((Camera.main.orthographicSize * 2 * (16f / 9)), Camera.main.orthographicSize * 2);
    }
}

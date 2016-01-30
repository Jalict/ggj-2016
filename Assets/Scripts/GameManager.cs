using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {
    
    private static GameManager instance;

    public Transform[] spawnPoints;
	public GameObject spiritPrefab;

    public Text timer;
    private float timeLeft = 300;

    public static bool isActive { 
		get { 
			return instance != null; 
		} 
	}

	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType(typeof(GameManager)) as GameManager;
 
				if (instance == null)
				{
					GameObject go = new GameObject("_gamemanager");
					DontDestroyOnLoad(go);
					instance = go.AddComponent<GameManager>();
				}
			}
			return instance;
		}
	}

	void Start() {
	}

	// Use this for initialization
	void Awake () {
	   if(spawnPoints.Length == 0)
            Debug.LogError("GameManager has no spawnpoints set!");
    }
	
	// Update is called once per frame
	void Update () {
		if(timeLeft > 0)
		timeLeft -= Time.deltaTime;
		timer.text = "Time remaining: " + (int)timeLeft;
	}

    public void RespawnPlayer(Player player, float t = 0){
        
		if(t == 0){
            //TODO: add logic to cleverly spawn the player
			          
            CameraShake.Instance.start(.1f, .2f);
        }else{
            StartCoroutine(delayRespawn(player, t));
        }
    }
    
    IEnumerator delayRespawn(Player player, float t){
        player.gameObject.SetActive(false);

		Vector3 spawnPosition = spawnPoints [Random.Range (0, spawnPoints.Length)].position;

		GameObject obj = Instantiate(spiritPrefab, player.transform.position+Vector3.back, Quaternion.identity) as GameObject;
		obj.GetComponent<MoveToSpawn> ().spawnPosition = spawnPosition;
		obj.GetComponent<MoveToSpawn> ().spawnTime = t;

        yield return new WaitForSeconds(t);

		player.transform.position = spawnPosition;

        RespawnPlayer(player);
        player.gameObject.SetActive(true);
       
    }
}

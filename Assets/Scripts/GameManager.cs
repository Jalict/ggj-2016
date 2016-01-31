using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {
    
    private static GameManager instance;

    public Transform[] spawnPoints;
	public GameObject spiritPrefab;
	public GameObject playerPrefab;

    public Text timer;
    private float timeLeft = 300;

    public bool[] isJoined;
    private bool gameStarted = false;

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
		DontDestroyOnLoad(gameObject);
	   if(spawnPoints.Length == 0)
            Debug.LogError("GameManager has no spawnpoints set!");
    }
	
	// Update is called once per frame
	void Update () {
		if(timeLeft > 0)
		timeLeft -= Time.deltaTime;
		timer.text = "Time remaining: " + (int)timeLeft;

		if(Time.realtimeSinceStartup > 10.0f && gameStarted == false) {
			Application.LoadLevel (1);			
			gameStarted = true;
			StartCoroutine(waitSpawning());
		}
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
    public void SpawnPlayer(int i){
    	Vector3 spawnPosition = spawnPoints [i].position;

    	GameObject obj = Instantiate(playerPrefab, spawnPosition, Quaternion.identity) as GameObject;
    	obj.GetComponent<Player> ().playerNum = i;
        
            //TODO: add logic to cleverly spawn the player
			          
            CameraShake.Instance.start(.1f, .2f);
    }

    IEnumerator waitSpawning() {
    	yield return new WaitForSeconds(0.5f);
    	for(int i = 0; i < 4; i++){
			if(isJoined[i]){
					SpawnPlayer(i);
			}
		}	
    }
}



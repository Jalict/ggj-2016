﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    
    private static GameManager instance;

    public Transform[] spawnPoints;

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

	// Use this for initialization
	void Awake () {
	   if(spawnPoints.Length == 0)
            Debug.LogError("GameManager has no spawnpoints set!");
    }
	
	// Update is called once per frame
	void Update () {
	}
    
    
    public void RespawnPlayer(Player player, float t = 0){
        if(t == 0){
            //TODO: add logic to cleverly spawn the player
            player.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        }else{
            StartCoroutine(delayRespawn(player, t));
        }
    }
    
    IEnumerator delayRespawn(Player player, float t){
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(t);

        RespawnPlayer(player);
        player.gameObject.SetActive(true);
       
    }
}
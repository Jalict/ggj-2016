using UnityEngine;
using System.Collections;

/*---------------------------------------
   Made by @KasperHdl - www.kasperhdl.dk
-----------------------------------------*/

public class CameraShake : MonoBehaviour {

	/* Instructions:
		Place this script on the camera

		NOTE: to be able to move the camera while shaking you must use the "public Vector3 position" variable or place the camera in another object

		CameraShake will shake the camera. The shaking dimensions enables you to control which dimension the shake will occur
		the standard shake amount is the go to variable if shake is starting with no extra arguments
		the same for the duration
	*/
//--- Debug ---//
	private bool debug = true;//if true then press space to test shaking

//--- Public ---//
	public Vector3 position;

	public Vector3 shakingDimensions = new Vector3(1f,1f,1f);
	public float standardShakeAmount = 1f;
	public float standardShakeDuration = 1f;


//--- Private ---//


	private bool isShaking;

	private float currentShakeEnd;
	private float currentShakeAmount;

	private float startTime = -1;

//--- Singleton ---//
    
    private static CameraShake instance;

    public static bool isActive { 
		get { 
			return instance != null; 
		} 
	}

	public static CameraShake Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType(typeof(CameraShake)) as CameraShake;
 
				if (instance == null)
				{
					GameObject go = new GameObject("_CameraShake");
					DontDestroyOnLoad(go);
					instance = go.AddComponent<CameraShake>();
				}
			}
			return instance;
		}
	}


//--- Unity Methods ---//
	void Start () {
		position = transform.position;
		currentShakeAmount = standardShakeAmount;
	}

	void Update () {
		if(debug){
			//if debug mode is on and space is pressed.. SHAKING!
			if(Input.GetKeyDown(KeyCode.Space)){
				start();
			}
		}

		//start a scheduled start
		if(startTime != -1 && startTime < Time.time && !isShaking){
			isShaking = true;
		}
		if(isShaking){
			if(currentShakeEnd > Time.time){
				//SHAKE IT!
				Vector3 randomVector = new Vector3(Random.Range(-1f,1f) * shakingDimensions.x,Random.Range(-1f,1f) * shakingDimensions.y,Random.Range(-1f,1f) * shakingDimensions.z);
				transform.position = randomVector * currentShakeAmount + position;
			}else{
				//stop shaking
				stop();
			}
		}
	}

//--- Public Methods ---//

	///<summary>Stop shaking</summary>
	public void stop(){
		isShaking = false;
		transform.position = position;
	}

	///<summary>Stops shaking after duration</summary>
	///<param>duration</param>
	public void stop(float dur){
		currentShakeEnd = Time.time + dur;
	}

	///<summary>Stops shaking in at Time</summary>
	public void stopAt(float end){
		currentShakeEnd = end;
	}

	///<summary>set the standard duration for a shake</summary>
	public void setStandardDuration(float duration){
		standardShakeDuration = duration;
		currentShakeEnd = duration + Time.time;
	}


	///<summary>Set the Standard amount of shake</summary>
	public void setStandardAmount(float amount){
		standardShakeAmount = amount;
		currentShakeAmount = amount;
	}

	///<summary>Start shaking</summary>
	public void start(){start(standardShakeDuration,standardShakeAmount);}
	///<summary>Start shaking</summary>
	///<param>duration of shake</param>
	public void start(float duration){start(duration,standardShakeAmount);}
	///<summary>Start shaking</summary>
	///<param>duration of shake</param>
	///<param>amount of shake</param>
	public void start(float duration, float amount){
		currentShakeEnd = Time.time + duration;
		currentShakeAmount = amount;

		isShaking = true;
	}

	///<summary>Start shaking at time with current values</summary>
	public void startAt(float start){
		startTime = start;
	}

	///<summary>Start shaking and stop at time</summary>
	///<param>end of shake</param>
	public void startThenStopAt(float end){startThenStopAt(end,standardShakeAmount);}
	///<summary>Start shaking and stop at time</summary>
	///<param>end of shake</param>
	///<param>amount of shake</param>
	public void startThenStopAt(float end,float amount){
		currentShakeEnd = end;
		currentShakeAmount = amount;
		isShaking = true;
	}
}

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	//private Vector3 ballDist;
	public GameObject ball;
	public float ang;
	public Vector3 look;
	private int distance;
	
	private bool topView;
	private bool canChange;
	
	// Use this for initialization
	void Start () {
		ang = 0;
		topView = false;
		distance = 10;
		canChange = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//Cambio del angulo de la camara
		if (Input.GetKeyDown (KeyCode.T))
			toggleCamera ();
		
		//Rotacion de la camara
		float vx = Input.GetAxis("Horizontal");
		ang += 2 * vx * Time.deltaTime;
		
		//Obtencion del vector look (apunta a la bola blanca)
		Vector3 v = new Vector3 (distance * Mathf.Cos (ang), 3, distance * Mathf.Sin (ang));
		look = -1f/distance*v + new Vector3 (0, 3f/distance, 0);
		
		//Posicion y rotacion de la camara
		if (topView) {
			transform.position = transform.position*0.85f + 0.15f*new Vector3(-28,70,0);
			transform.eulerAngles = transform.eulerAngles*0.85f + 0.15f*new Vector3(60,90,0);
		}
		else {
			transform.position = transform.position*0.85f + 0.15f*(ball.transform.position + v);
			transform.LookAt (ball.transform.position);
		}
		
	}

	public void fixTopView(){
		canChange = false;
		topView = true;
	}

	public void unfixCamera(){
		canChange = true;
		topView = false;
	}

	public void toggleCamera(){
		if(canChange)
			topView = !topView;
	}

}
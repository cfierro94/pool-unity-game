using UnityEngine;
using System.Collections;

public class White_ball : MonoBehaviour {
	
	public Rigidbody rb;
	public CameraController camera;
	public Controller controller;

	private float posY;
	private bool resting;
	private int firstTouch;
	
	//private Plane plane;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		posY = transform.position.y;
		resting = false;
		firstTouch = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (resting) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			return;
		}
		if (rb.velocity.sqrMagnitude < 0.4) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		} 
		else if (rb.velocity.sqrMagnitude < 10) {
			rb.velocity = 0.9f*rb.velocity;
			rb.angularVelocity = 0.9f*rb.angularVelocity;
		}
		if (transform.position.y > posY) {
			transform.position = new Vector3(transform.position.x,posY,transform.position.z);
		}
		else if (transform.position.y < posY-2) {
			Debug.Log("Se cayo la bola blanca");
			controller.newBallOut(0);
			//transform.position = new Vector3(-58.4f,40f,0);
			resting = true;
			camera.fixTopView();
		}
	}
	
	public void startMove(float mag){
		rb.AddForce ((10*mag+40)*camera.look);
	}

	public int getFirstTouch(){
		return firstTouch;
	}

	public void newTurn(){
		firstTouch = 0;
	}

	void OnCollisionEnter(Collision collision) {
		if (firstTouch == 0 && collision.gameObject.tag.Equals ("Balls")) {
			firstTouch = collision.gameObject.GetComponent<Balls>().num;
		}
	}

	public void wakeUp(){
		resting = false;
	}

}

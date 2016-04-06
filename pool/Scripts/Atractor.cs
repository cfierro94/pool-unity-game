using UnityEngine;
using System.Collections;

public class Atractor : MonoBehaviour {

	public int atractX;
	public int atractZ;

	void Start(){
		/*GameObject wb = GameObject.FindGameObjectWithTag ("MainBall");
		Physics.IgnoreCollision (GetComponent<Collider> (), wb.GetComponent<Collider> ());
		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Balls");
		foreach(GameObject ball in balls)
			Physics.IgnoreCollision (GetComponent<Collider> (), ball.GetComponent<Collider> ());*/
	}

	void OnTriggerStay(Collider collision) {
		if (collision.gameObject.tag.Equals ("MainBall") 
		    || collision.gameObject.tag.Equals ("Balls")) {
			float vel = collision.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude;
			vel = (vel>50)?vel/3:0;
			Vector3 atForce = new Vector3(atractX,-1,atractZ)*vel;
			collision.gameObject.GetComponent<Rigidbody>().AddForce(atForce);
			//other.GetComponent<Rigidbody>().AddForce(other.transform.position-transform.position);
		}
	}

}

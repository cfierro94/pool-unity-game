using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	public void ChangeToScene(int scene){
		Application.LoadLevel (scene);
	}
}

using UnityEngine;
using System.Collections;

public class Bar2D : MonoBehaviour {

	Texture2D[] ballIcons;

	public Controller controller;

	//Bola9
	private int actual;

	//Bola 8
	private bool[] show;

	// Use this for initialization
	void Start () {
		loadAllIcons ();
		if (controller.getGameMode () == 8)
			show = initAllTrue();
		else
			actual = 1;
	}

	bool[] initAllTrue(){
		bool[] outs = new bool[15];
		for(int k=0; k<15; k++){
			outs[k] = true;
		}
		return outs;
	}

	void loadAllIcons(){
		ballIcons = new Texture2D[15];
		for (int i=1; i<=15; i++) {
			string path = "iconball00"+((i<10)?"0":"")+i;
			ballIcons[i-1] = Resources.Load(path) as Texture2D;
		}
	}

	void OnGUI(){
		if (!controller.isPlaying ())
			return;
		if (controller.getGameMode () == 8) {
			int i = 0;
			int j = 0;
			for(int k=0; k<15; k++){
				if(show[k]){
					if(k<7){
						GUI.DrawTexture (new Rect (10+30*i, 10, 28, 28), ballIcons[k]);
						i++;
					}
					else if(k>7){
						GUI.DrawTexture (new Rect (Screen.width-102-30*j, 10, 28, 28), ballIcons[k]);
						j++;
					}
				}
			}
		}
		else if(controller.getGameMode() == 9 && actual<10)
			GUI.DrawTexture (new Rect (10, 10, 32, 32), ballIcons[actual-1]);
	}

	public void setActualBall(int a){
		actual = a;
	}

	public void ballOut(int n){
		show [n-1] = false;
	}

}

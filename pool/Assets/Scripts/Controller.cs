using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public White_ball mainBall;
	public Palo palo;
	public HandPlacer placer;
	public CameraController cam;
	public Bar2D bar;

	private GameObject[] balls;

	private float maxv;
	private bool inMov;
	private bool handling;
	private bool preventFault;
	private bool playing;

	private string finalmsg;

	public int totalPlayers = 2;
	private int actualPlayer = 0;

	private ArrayList caidas;
	private int turn;

	private GUIStyle style;

	public int GAME_MODE = 8;

	//Bola 8
	private int[] remBalls = {7,7};

	//Bola 9
	private int actual = 1;
	private int caidasPrev = 0;

	// Use this for initialization
	void Start () {
		balls = GameObject.FindGameObjectsWithTag ("Balls");
		caidas = new ArrayList ();
		//newTurn9 ();
		//startHandling ();
		inMov = false;
		handling = false;
		preventFault = false;
		playing = true;

		style = new GUIStyle ();

		style.normal.textColor = Color.black;
		style.fontSize = 16;

		if (GAME_MODE == 9)
			removeOtherBalls ();

	}
	
	// Update is called once per frame
	void Update () {
		if (!playing || handling) {
			return;
		}
		//Obtiene la velocidad maxima de todas las bolas
		maxv = mainBall.rb.velocity.sqrMagnitude;
		foreach (GameObject gobj in balls) {
			Balls bp = gobj.GetComponent<Balls>();
			if(maxv < bp.rb.velocity.sqrMagnitude)
				maxv = bp.rb.velocity.sqrMagnitude;
		}
		palo.canPlay = (maxv<0.01);

		//Detector de fin de turno
		if (inMov && palo.canPlay) {
			if(GAME_MODE == 8)
				newTurn8();
			else if(GAME_MODE == 9)
				newTurn9();
		}
		inMov = !palo.canPlay;
	}

	//Se llama cuando se cae una pelota
	public void newBallOut(int n){
		caidas.Add (n);
		if (GAME_MODE == 8 && n!=0) {
			bar.ballOut(n);
		}
		if (n > 0 && n < 8)
			remBalls [0] = remBalls [0] - 1;
		else if (n > 8)
			remBalls [1] = remBalls [1] - 1;
	}

	//Logica de turnos para el juego Bola8
	void newTurn8(){
		if (preventFault) {
			preventFault = false;
			return;
		}
		turn++;
		if(!checkType(mainBall.getFirstTouch(),actualPlayer) && !caidas.Contains(8)){
			Debug.Log("Fault");
			startHandling();
			actualPlayer = (actualPlayer+1)%totalPlayers;
		}
		else if (caidas.Count > 0) {
			//Si se bota la bola 8 se pierde o gana
			if(caidas.Contains(8)){
				if(playerFin(actualPlayer) && !caidas.Contains(0)){
					finalmsg = "Player "+(actualPlayer+1)+" won!";
					playing = false;
				}
				else{
					finalmsg = "Player "+(actualPlayer+1)+" lost!";
					playing = false;
				}
			}
			else if(caidas.Contains(0) || !checkType((int) caidas[0],actualPlayer)){
				Debug.Log("Fault");
				startHandling();
				actualPlayer = (actualPlayer+1)%totalPlayers;
			}
		}
		else {
			actualPlayer = (actualPlayer+1)%totalPlayers;
		}
		if (playing) {
			Debug.Log ("Turno " + turn + " para el jugador " + actualPlayer);
			caidas.Clear ();
			mainBall.newTurn ();
		}
		else {
			palo.canPlay = false;
		}
	}

	//Bola 8: Devuelve true si el jugador p ha botado todas las bolas de su tipo
	bool playerFin(int p){
		return remBalls [p] == 0;
	}

	//Bola 8: Devuelve true si la bola n es del tipo del jugador p
	bool checkType(int n, int p){
		return (playerFin(p) && n==8)||(p==0 && n>0 && n<8) || (p==1 && n>8);
	}

	private void startHandling(){
		placer.startHandling (getMainBallRegion());
		cam.fixTopView ();
		palo.canPlay = false;
		handling = true;
		preventFault = true;
	}

	public void onHandPlace(Vector3 pos){
		mainBall.transform.position = pos + new Vector3(0,-1,0);
		mainBall.wakeUp ();
		cam.unfixCamera ();
		palo.canPlay = true;
		handling = false;
	}

	//Logica de turnos para el juego Bola9
	void newTurn9(){
		if (preventFault) {
			if(mainBall.getFirstTouch()==0){
				preventFault = false;
				return;
			}
			else{
				Debug.Log ("Fault");
				startHandling ();
				actualPlayer = (actualPlayer + 1) % totalPlayers;
			}
		}
		turn++;
		if (caidas.Count == caidasPrev) {
			if (mainBall.getFirstTouch () == actual)
				actualPlayer = (actualPlayer + 1) % totalPlayers;
			else {
				Debug.Log ("Fault");
				startHandling ();
				actualPlayer = (actualPlayer + 1) % totalPlayers;
			}
		} 
		else if (actual < 9 && caidas.Contains (9)) {
			finalmsg = "Player "+(actualPlayer+1)+" lost!";
			playing = false;
		} 
		else if (caidas.Contains (actual)) {
			while (caidas.Contains(actual) && actual<10) {
				actual++;
				Debug.Log ("La bola " + actual + " es la actual");
				bar.setActualBall(actual);
			}
			if (actual == 10 && !caidas.Contains (0)) {
				finalmsg = "Player "+(actualPlayer+1)+" won!";
				playing = false;
			}
		} 
		else if (!caidas.Contains (0)) {
			Debug.Log ("Fault");
			startHandling ();
			actualPlayer = (actualPlayer + 1) % totalPlayers;
		}
		if(caidas.Contains(0)){
			Debug.Log("Fault");
			startHandling();
			actualPlayer = (actualPlayer + 1) % totalPlayers;
			caidas.Remove(0);
		}
		if (playing) {
			Debug.Log ("Turno " + turn + " para el jugador " + actualPlayer);
			mainBall.newTurn ();
			caidasPrev = caidas.Count;
		}
		else {
			palo.canPlay = false;
		}
	}

	int getMainBallRegion(){
		float z = mainBall.transform.position.z;
		if (z < -37 + 74 / 3)
			return 0;
		else if (z > 37 - 74 / 3)
			return 2;
		else
			return 1;
	}

	void removeOtherBalls(){
		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Balls");
		for(int i=0; i<balls.Length; i++){
			if( balls[i].GetComponent<Balls>().num > 9)
				balls[i].transform.position = new Vector3(0,0,-10);
		}
	}

	void OnGUI(){
		if (playing) {
			if (GUI.Button (new Rect (Screen.width - 60, 5, 58, 30), "Camera"))
				cam.toggleCamera ();
			if (GAME_MODE == 9)
				GUI.Label (new Rect (Screen.width / 2 - 60, 15, 120, 24), "Player " + (actualPlayer + 1), style);
			else if (actualPlayer == 0)
				GUI.Label (new Rect (Screen.width / 2 - 60, 15, 120, 24), "← Player 1", style);
			else
				GUI.Label (new Rect (Screen.width / 2 - 60, 15, 120, 24), "Player 2 →", style);
		}
		else {
			GUI.Label (new Rect (Screen.width / 2 - 100, 15, 200, 24), finalmsg, style);
			if (GUI.Button (new Rect (Screen.width - 100, 5, 88, 30), "Play Again"))
				Application.LoadLevel(0);
		}
	}

	public int getGameMode(){
		return GAME_MODE;
	}

	public bool isPlaying(){
		return playing;
	}

}

using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GUISkin perSkin;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

		GUI.skin = perSkin;


		if(GUI.Button(new Rect(Screen.width/2 - 86,Screen.height/2 - 70,180,30),"Player vs IA")){
			Application.LoadLevel("MainGame");
		}

		if(GUI.Button(new Rect(Screen.width/2 - 86,Screen.height/2 - 20,180,30),"Player vs Player")){
			Application.LoadLevel("MainGame");
		}

		if(GUI.Button(new Rect(Screen.width/2 - 86,Screen.height/2 + 30,180,30),"IA vs IA")){
			Application.LoadLevel("MainGame");
		}

		if(GUI.Button(new Rect(Screen.width/2 - 86,Screen.height/2 + 80,180,30),"Exit")){
			Application.Quit();
		}
			
	}
}

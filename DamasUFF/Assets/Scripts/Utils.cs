using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static Play max(Play play1, Play play2){
		if(play1 == null){
			return play2;
		}

		if(play1.getCount() > play2.getCount()){
			return play1;
		}else{
			return play2;
		}
	}

	public static Play min(Play play1, Play play2){
		if(play1 == null){
			return play2;
		}

		if(play1.getCount() < play2.getCount()){
			return play1;
		}else{
			return play2;
		}
	}
}

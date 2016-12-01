using UnityEngine;
using System.Collections;

public class AI : Player {

	public Object board;

	public AI(){

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	public void VerifyPlay (int[,] tab)
	{
		//true: WHITE
		//false: BLACK
		MinMax mm = new MinMax(tab, true, 9);
		Play p = mm.Search(); 
	}
}

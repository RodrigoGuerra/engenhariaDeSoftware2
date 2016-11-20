using UnityEngine;
using System.Collections;

public class House : MonoBehaviour {

	[ReadOnly]public int line;
	[ReadOnly]public int column;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void SetPosition(int line, int column){
		this.line = line;
		this.column = column;

	}


	///======================

}

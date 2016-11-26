using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {
	public ControlGame controlGame;

	public int id;
	public bool isQueen;

	public Color color;

	public int line;
	public int column;

	// Use this for initialization
	void Start () {
		controlGame = GameObject.Find("GameController").GetComponent<ControlGame>();
	}
	
	// Update is called once per frame
	void Update () {

		MouseClick();

	}

	private void MouseClick(){
		//Uses raycast to define which piece the mouse is clicking
		if(Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)){
				if(hit.collider.gameObject.GetComponent<Piece>() != null){
					//controlGame gets the reference of last piece clicked by mouse
					controlGame.selectedPiece = hit.collider.gameObject.GetComponent<Piece>();
				}else{
					controlGame.selectedPiece = null;
				}
			}
		}
	}
}

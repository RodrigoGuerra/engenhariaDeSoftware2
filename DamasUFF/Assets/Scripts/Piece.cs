using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour
{
	public ControlGame controlGame;


	public int id;
	public bool isQueen;

	public Color color;

	public int line;
	public int column;

	// Use this for initialization
	void Start ()
	{
		controlGame = GameObject.Find ("GameController").GetComponent<ControlGame> ();

	}
	
	// Update is called once per frame
	void Update ()
	{

		MouseClick ();

	}


	private void MouseClick ()
	{
		//Uses raycast to define which piece the mouse is clicking
		if (Input.GetMouseButtonDown (0)) {


			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.gameObject.GetComponent<Piece> () != null
				     && hit.collider.gameObject.GetComponent<Piece> () == this) {
					//controlGame gets the reference of last piece clicked by mouse


					controlGame.selectedPiece = this;
					Debug.Log (controlGame.selectedPiece);

					if ((controlGame.selectedPiece.CompareTag ("WhitePieceTag") && controlGame.currentPlayerTurn.color == Color.white)
					     || (controlGame.selectedPiece.CompareTag ("BlackPieceTag") && controlGame.currentPlayerTurn.color == Color.black)) {
					
						Verifier.VerifyPlayByPiece (this.line, this.column, controlGame.piecesArray,controlGame);

					//	Debug.Log ("Piece line: "+this.line+"  Piece Column: " + this.column);

					}
					

					//TESTING MOVEMENT
					//	controlGame.EfectuatePlay (controlGame.selectedPiece,new House() );      
				} else {
					//controlGame.selectedPiece = null;
				
				}
			}

		} 
	}
}

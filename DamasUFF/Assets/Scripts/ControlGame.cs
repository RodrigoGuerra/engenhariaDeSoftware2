using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class ControlGame : MonoBehaviour
{
	public Piece selectedPiece;

	public GameObject board;

	public List<House> houses;
	public House[,] housesArray;
	public float movementSpeed = 250f;

	public Queue<MovementAction> movementsToGo;

	private bool isMoving;
	private House HouseToGo;
	private Piece pieceToMove;


	// Use this for initialization
	void Start ()
	{
		houses = new List<House> ();
		houses = board.GetComponentsInChildren<House> ().ToList ();
		movementsToGo = new Queue<MovementAction> ();
		housesArray = new House[8, 8];

		int k = 0;
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				housesArray [i, j] = houses.ElementAt (k);
				k++;
			}
		}
			
		InstantiateBoard (GameTypeEnum.PlayerVsAI, DifficultyEnum.Normal);


	}
	
	// Update is called once per frame
	void Update ()
	{
		

		if (isMoving) {
			
			float step = (movementSpeed * 0.01f) * Time.deltaTime;

			Vector3 positionToGo = new Vector3 (HouseToGo.transform.position.x, 0.15f, HouseToGo.transform.position.z);
			pieceToMove.transform.position = Vector3.MoveTowards (pieceToMove.transform.position, positionToGo, step);
			if (pieceToMove.transform.position == positionToGo) {

				isMoving = false;		
				Debug.Log ("ismoving =false, chegou na posicao");
			
							
			} else {

				if (movementsToGo !=null && movementsToGo.Count != 0) {

					MovementAction m = movementsToGo.Dequeue ();
					EfectuatePlay (m.piece, m.houseToGo);
				}
					
			}
		
		}
	}


	public void EfectuatePlay (Piece piece, House houseToGo)
	{
		
		if (!isMoving) {		
	
			GameObject p = piece.transform.parent.gameObject;

			HouseToGo = houseToGo;
			pieceToMove = piece;


			//for TESTING
			//HouseToGo = housesArray [piece.line + 1, piece.column + 1];     

	

			isMoving = true;
			Debug.Log ("ismoving =true, iniciou movimento");
		}


	}

	public void EfectuateListOfPlays(Piece p, List<MovementAction> listOfPlays)
	{
		for (int i = 0; i < listOfPlays.Count; i++) {
			movementsToGo.Enqueue (listOfPlays.ElementAt(i));
		}

	}

	public void VerifyStatus ()
	{


	}

	public void LoadGame ()
	{		

					
	}

	public void SaveGame ()
	{
	}

	public void InstantiateBoard (GameTypeEnum gameType, DifficultyEnum difficulty)
	{
				
					
					
		//Set column and line of each House
		int count = 0;
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				House t = houses.ElementAt (count);
				t.column = j;
				t.line = i;

				count++;
			}
		}
		
		//Instantiate Objects to organize teams
		GameObject teams = new GameObject ("Teams");
		GameObject teamWhite = new GameObject ("Whites");
		teamWhite.transform.SetParent (teams.transform);
		GameObject teamBlack = new GameObject ("Blacks");
		teamBlack.transform.SetParent (teams.transform);

		//Instantiate each piece for each team
		int k = 0;
		bool alterna = false;
		for (int i = 0; i < 8; i++) {
			if (i < 3) {
				k = alterna ? 1 : 0;
				while (k < 8) {
				
					GameObject p = (GameObject)Instantiate (Resources.Load ("Prefabs/BlackMenChecker_T"));
					p.transform.SetParent (teamBlack.transform);
					p.transform.position = new Vector3 ((float)housesArray [i, k].transform.position.x, 0.15f, (float)housesArray [i, k].transform.position.z);
					Piece piece = p.GetComponent<Piece> ();
					piece.column = k;
					piece.line = i;

					k += 2;
				}
				alterna = alterna ? false : true;
			} else if (i >= 5) {

				k = alterna ? 1 : 0;
				while (k < 8) {

					GameObject p = (GameObject)Instantiate (Resources.Load ("Prefabs/WhiteMenChecker_T"));
					p.transform.SetParent (teamWhite.transform);
					p.transform.position = new Vector3 ((float)housesArray [i, k].transform.position.x, 0.15f, (float)housesArray [i, k].transform.position.z);
					Piece piece = p.GetComponent<Piece> ();
					piece.column = k;
					piece.line = i;
					k += 2;
				}
				alterna = alterna ? false : true;

			}
		}
	}

	private void InstantiateHousesAndPieces ()
	{
	}

}

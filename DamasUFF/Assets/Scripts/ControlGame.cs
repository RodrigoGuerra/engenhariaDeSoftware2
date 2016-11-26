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

	// Use this for initialization
	void Start ()
	{
		houses = new List<House> ();
		houses = board.GetComponentsInChildren<House> ().ToList ();
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
	
	}

	public void EfectuatePlay (Piece piece, MovementAction start, MovementAction end)
	{
		GameObject p = piece.transform.parent.gameObject;


		p.transform.Translate (new Vector3 ());


	}

	public void EfectuateListOfPlays (List<MovementAction> listOfPlays)
	{

		/*foreach (MovementAction play in listOfPlays) {
			EfectuatePlay (play.start, play.end);
		}*/
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

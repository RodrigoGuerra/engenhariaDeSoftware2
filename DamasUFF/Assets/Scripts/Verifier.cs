using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Verifier : MonoBehaviour
{
	[ReadOnly]private static int MAX = 8;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public static List<List<int[]>> VerifyPlayByPiece (int i, int j, int[,] m, ControlGame cg)
	{
		int temp = m [i, j];
		
		List<List<int[]>> plays = new List<List<int[]>> ();

		switch (m [i, j]) {
		case (int)PieceTypeEnum.White:
			plays = Verify (i, j, 2, m, -1, plays, false, (int)PieceTypeEnum.White);
			break;
		case (int)PieceTypeEnum.Black:
			plays = Verify (i, j, 2, m, 1, plays, false, (int)PieceTypeEnum.Black);
			break;
		case (int)PieceTypeEnum.KingWhite:
			plays = Verify (i, j, MAX, m, -1, plays, true, (int)PieceTypeEnum.KingWhite);
			break;
		case (int)PieceTypeEnum.KingBlack:			
			plays = Verify (i, j, MAX, m, 1, plays, true, (int)PieceTypeEnum.KingBlack);
			break;
		}

		if (cg != null) {
			
			foreach (List<int[]> l in plays) {
				foreach (int[] item in l) {
					int linha = item [0]; //linha
					int coluna = item [1]; //coluna

					Debug.Log ("{" + linha + "," + coluna + "} ");
					cg.housesArray[linha,coluna].TurnOnLEDHouse();

				}
				Debug.Log ("------------------------------------------------");
			}

			m [i, j] = temp;
		}

		return plays;
	}

	private static List<List<int[]>> Verify (int i, int j, int max, int[,] m, int d, List<List<int[]>> plays, bool isKing, int color)
	{
		int opponentColor = 2;
		if (d == 1) {
			opponentColor = 1;
		}

		m [i, j] = 0;

		//Jogada para trás
		//Direita
		PlayDirection (plays, m, max, opponentColor, i, j, d, isKing, isKing, 1, 1, "pd_BR", color);
		//Esquerda
		PlayDirection (plays, m, max, opponentColor, i, j, d, isKing, isKing, 1, -1, "pd_BL", color);

		//Jogada para frente
		//Direita
		PlayDirection (plays, m, max, opponentColor, i, j, d, isKing, isKing, -1, 1, "pd_FR", color);
		//Esquerda
		PlayDirection (plays, m, max, opponentColor, i, j, d, isKing, isKing, -1, -1, "pd_FL", color);

		return plays;
	}

	public static List<List<int[]>> EatPiece (int i, int j, int max, int[,] m, int d, List<int[]> play, bool ate, string name, int color)
	{		
		int opponentColor = 2;
		if (d == 1) {
			opponentColor = 1;
		}

		List<List<int[]>> plays = new List<List<int[]>> ();

		//Jogada para trás
		//Direita
		EatDirection (plays, play, max, m, opponentColor, i, j, d, 1, 1, name, color);
		//Esquerda
		EatDirection (plays, play, max, m, opponentColor, i, j, d, 1, -1, name, color);

		//Jogada para frente
		//Direita
		EatDirection (plays, play, max, m, opponentColor, i, j, d, -1, 1, name, color);
		//Esquerda
		EatDirection (plays, play, max, m, opponentColor, i, j, d, -1, -1, name, color);
			
		return plays;		
	}

	public static List<List<int[]>> Insert (int i, int j, int[,] m, List<List<int[]>> Plays)
	{
		int[] position = { i, j };
		if (m [i, j] != 0) {
			List<int[]> play = new List<int[]> ();
			play.Add (position);
			Plays.Add (play);
		}
		return Plays;
	}

	public static bool Compare (int i, int j, int[,] m, int num)
	{
		return (i < MAX && i >= 0 && j < MAX && j >= 0 && m [i, j] == num);
	}

	public static int[,] MatchTab (int[,] m)
	{
		int[,] resp = new int[m.GetLength (1), m.GetLength (1)];

		for (int i = 0; i < resp.GetLength (0); i++) {
			for (int j = 0; j < resp.GetLength (1); j++) {
				resp [i, j] = m [i, j];
			}
		}
		return resp;
	}

	public static void AddPlays (List<List<int[]>> plays, List<int[]> play, int startPoint)
	{		
		for (int i = startPoint; i < play.Count; i++) {
			List<int[]> jogadaTemp = new List<int[]> ();
			for (int j = 0; j <= i; j++) {
				jogadaTemp.Add (play [j]);
			}
			plays.Add (jogadaTemp);
		}
	}

	public static List<int[]> ConcatPlays (List<int[]> playA, List<int[]> playB)
	{

		for (int i = 0; i < playB.Count; i++) {
			playA.Add (playB [i]);
		}

		return playA;
	}

	public static List<int[]> ConcatPlays (List<int[]> playA, List<int[]> playB, int max)
	{
		List<int[]> tempPlay = new List<int[]> ();
		for (int i = 0; i < playA.Count; i++) {
			tempPlay.Add (playA [i]);
		}
		for (int i = 0; i <= max; i++) {
			tempPlay.Add (playB [i]);
		}
		return tempPlay;
	}

	public static List<List<int[]>> ConcatListOfPlays (List<int[]> lastPlay, List<List<int[]>> newPlays)
	{

		if (newPlays == null) {
			return null;
		}

		List<List<int[]>> jogadasTemp = new List<List<int[]>> ();
		for (int k = 0; k < newPlays.Count; k++) {
			for (int i = 0; i < newPlays [k].Count; i++) {
				jogadasTemp.Add (ConcatPlays (lastPlay, newPlays [k], i));
			}
		}

		return jogadasTemp;
	}

	public static void AddAtTheEnd (List<List<int[]>> plays1, List<List<int[]>> plays2)
	{
		if (plays2 == null) {
			return;
		}

		for (int i = 0; i < plays2.Count; i++) {
			plays1.Add (plays2 [i]);
		}
	}

	public static int MaxCount (List<List<int[]>> plays, int[] pos)
	{
		int maxCount = 0, actualCount = 0;
		if (plays == null) {
			return maxCount;
		}

		for (int i = 0; i < plays.Count; i++) {
			actualCount = Count (plays [i], pos);
			if (actualCount > maxCount) {
				maxCount = actualCount;
			}
		}
		return maxCount;
	}

	public static int Count (List<int[]> plays, int[] pos)
	{
		int count = 0;
		if (plays == null) {
			return count;
		}

		for (int j = 0; j < plays.Count; j++) {
			if (j == 0) {
				if (!IsAdjacent (pos, plays [j])) {
					count++;
				}
			} else if (!IsAdjacent (plays [j - 1], plays [j])) {
				count++;
			}
		}

		return count;
	}

	public static bool IsAdjacent (int[] pos1, int[] pos2)
	{
		if (Mathf.Abs (pos1 [0] - pos2 [0]) == 1) {
			return true;
		}

		if (Mathf.Abs (pos1 [1] - pos2 [1]) == 1) {
			return true;
		}

		return false;
	}

	public static int[,] UpdateTab (List<int[]> plays, int[,] m, int[] pos, int type)
	{
		int[,] resp = MatchTab (m);
		int l = 0;
		int c = 0;
		if (plays == null) {
			return resp;
		}

		resp [pos [0], pos [1]] = 0;
		for (int j = 0; j < plays.Count; j++) {
			l = 0;
			c = 0;
			if (j == 0) {
				if (!IsAdjacent (pos, plays [j])) {
					l = CenterPieces (pos [0], plays [j] [0]);
					c = CenterPieces (pos [1], plays [j] [1]);

					resp [l, c] = 0;
				}
			} else if (!IsAdjacent (plays [j - 1], plays [j])) {
				l = CenterPieces (plays [j - 1] [0], plays [j] [0]);
				c = CenterPieces (plays [j - 1] [1], plays [j] [1]);

				resp [l, c] = 0;
			}
		}
		for (int i = 0; i < MAX; i++) {
			if (resp [MAX - 1, i] == 1) {
				resp [MAX - 1, i] = (int)PieceTypeEnum.KingWhite;
			}
			if (resp [0, i] == 2) {
				resp [0, i] = (int)PieceTypeEnum.KingBlack;
			}
		}							

		return resp;
	}

	public static int CenterPieces (int pos1, int pos2)
	{
		if (pos1 > pos2) {
			return pos1 - 1;
		} else if (pos1 == pos2) {
			return pos1;
		}

		return pos1 + 1;
	}

	public static bool TypeFromPosition (int[] pos, int[,] tab)
	{
		return ((tab [pos [0], pos [1]] == (int)PieceTypeEnum.White)
		|| (tab [pos [0], pos [1]] == (int)PieceTypeEnum.KingWhite));
	}

	public static void PlayDirection (List<List<int[]>> plays, int[,] m, int max, int opponentColor, int i, int j, int d, bool isKing, bool isBackward, int signal_line, int signal_column, string name, int color)
	{
		List<int[]> play = new List<int[]> ();
		List<List<int[]>> tempPlays = new List<List<int[]>> ();
		int[,] tab = MatchTab (m);
		bool ate = false;
		for (int k = 1; k < max; k++) {			
			if (Compare (i + signal_line * k * d, j + signal_column * k, tab, opponentColor) || Compare (i + signal_line * k * d, j + signal_column * k, tab, opponentColor + 2)) {
				if (Compare (i + signal_line * 2 * k * d, j + signal_column * 2 * k, m, 0)) {
					tab [i + k * d, j + k] = 0;

					int[] position = { i + 2 * k * d, j + signal_column * 2 * k };

					play.Add (position);

					AddAtTheEnd (tempPlays, ConcatListOfPlays (play, EatPiece (i + signal_line * 2 * k * d, j + signal_column * 2 * k, max, tab, d, play, false, name, color)));

					ate = true;
					k++;
				} else {
					k = max;
				}
			} else if (Compare (i + signal_line * k * d, j + signal_column * k, tab, 0) && ((isKing && isBackward) || (!isBackward))) {
				tab [i + signal_line * k * d, j + signal_column * k] = color;

				int[] posicao = { i + signal_line * k * d, j + signal_column * k };

				play.Add (posicao);

				if (ate) {
					AddAtTheEnd (tempPlays, ConcatListOfPlays (play, EatPiece (i + signal_line * k * d, j + signal_column * k, max, tab, d, play, false, name, color)));
				}

			} else {
				k = max;
			}
		}
		AddPlays (plays, play, 0);
		AddAtTheEnd (plays, tempPlays);
	}

	public static void EatDirection (List<List<int[]>> plays, List<int[]> play, int max, int[,] m, int opponentColor, int i, int j, int d, int signal_line, int signal_column, string name, int color)
	{
		List<int[]> play1 = new List<int[]> ();
		List<List<int[]>> tempPlays = new List<List<int[]>> ();
		int[,] tab = MatchTab (m);
		bool ate = false;
		for (int k = 1; k < max; k++) {			
			if (Compare (i + signal_line * k * d, j + signal_column * k, tab, opponentColor) || Compare (i + k * d, j + k, tab, opponentColor + 2)) {
				if (Compare (i + signal_line * 2 * k * d, j + signal_column * 2 * k, tab, 0)) {
					tab [i + signal_line * k * d, j + signal_column * k] = 0;

					int[] posicao = { i + signal_line * 2 * k * d, j + signal_column * 2 * k };

					play1.Add (posicao);
					AddAtTheEnd (tempPlays, ConcatListOfPlays (play1, EatPiece (i + signal_line * 2 * k * d, j + signal_column * 2 * k, max, tab, d, play1, false, name, color)));
					ate = true;
					k++;
				} else {
					k = max;
				}
			} else if (Compare (i + signal_line * k * d, j + signal_column * k, tab, 0)) {
				tab [i + signal_line * k * d, j + signal_column * k] = color;

				int[] position = { i + signal_line * k * d, j + signal_column * k };

				play1.Add (position);

				if (ate) {
					AddAtTheEnd (tempPlays, ConcatListOfPlays (play, EatPiece (i + signal_line * k * d, j + signal_column * k, max, tab, d, play1, false, name, color)));
				}

			} else {
				k = max;
			}
		}
		if (ate) {
			AddPlays (plays, play1, 0);
			AddAtTheEnd (plays, tempPlays);
		}
	}

}

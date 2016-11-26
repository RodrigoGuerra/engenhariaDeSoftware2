using UnityEngine;
using System.Collections;

public class Enums : MonoBehaviour {





	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public enum GameTypeEnum{PlayerVsAI, AIvsAI, PlayerVsPlayer}
public enum DifficultyEnum{Easy, Normal, Hard}
public enum HouseTypeEnum{}
public enum PieceTypeEnum{White = 1, Black = 2, KingWhite = 3, KingBlack = 4}
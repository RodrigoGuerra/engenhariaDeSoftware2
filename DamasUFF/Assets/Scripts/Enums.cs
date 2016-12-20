using UnityEngine;
using System.Collections;

public class Enums
{






}

public enum GameTypeEnum
{
	PlayerVsAI,
	AIvsAI,
	PlayerVsPlayer
}

public enum DifficultyEnum
{
	Easy,
	Normal,
	Hard
}

public enum HouseTypeEnum
{
}

public enum PieceTypeEnum
{
	White = 1,
	Black = 2,
	KingWhite = 3,
	KingBlack = 4
}

public enum Status
{
	Continue,
	Draw,
	WinWhite,
	WinBlack
}
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UnitCaseTests {
	
	public GameObject drive = GameObject.Find("Drive"); 

	[Test]
	public void VerifyTestNull(){
		int[,] m = {{1, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 1},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 1, 0, 0, 0, 0},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 1, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(0, 1, m, null);

		Assert.That(listOfPlays.Count == 0);
	}
	/*********************************************************************
	 * PEÇAS PRETAS
	 * NORMAIS
	**********************************************************************/
	[Test]
	public void VerifyTestNormalWalkBlack(){
		int[,] m = {{1, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 1},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 1, 0, 0, 0, 0},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 1, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(0, 0, m, null);

		Assert.That(listOfPlays.Count == 1);
	}

	[Test]
	public void VerifyTestEatingMultipleBlack(){
		int[,] m = {{1, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 1},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 1, 0, 0, 0, 0},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(4, 0, m, null);

		Assert.That(listOfPlays.Count == 2);
	}

	[Test]
	public void VerifyTestEatingRightOneBlack(){
		int[,] m = {{1, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 1},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 0, 0, 0, 0, 0},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(4, 0, m, null);

		Assert.That(listOfPlays.Count == 1);
	}

	[Test]
	public void VerifyTestEatingLeftOneBlack(){
		int[,] m = {{1, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 2, 0},
			{0, 1, 0, 0, 0, 0, 0, 0},
			{2, 0, 2, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(4, 2, m, null);

		Assert.That(listOfPlays.Count == 2);
	}

	/*********************************************************************
	 * PEÇAS PRETAS
	 * DAMAS
	**********************************************************************/

	[Test]
	public void VerifyTestNormalWalkBlackQueen(){
		int[,] m = {{4, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 1},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 0, 0, 0, 0, 0},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(0, 0, m, null);

		Assert.That(listOfPlays.Count == 7);
	}

	[Test]
	public void VerifyTestEatingMultipleBlackQueen(){
		int[,] m = {{4, 0, 1, 0, 0, 0, 0, 0},
			{0, 1, 0, 1, 0, 0, 0, 1},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 4, 0, 0, 0, 0},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(0, 0, m, null);

		Assert.That(listOfPlays.Count == 2);
	}

	[Test]
	public void VerifyTestEatingRightOneBlackQueen(){
		int[,] m = {{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 0, 0, 0, 0, 1},
			{4, 0, 0, 0, 0, 0, 0, 0},
			{0, 4, 0, 0, 0, 0, 0, 0},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(2, 0, m, null);

		Assert.That(listOfPlays.Count == 1);
	}

	[Test]
	public void VerifyTestEatingLeftOneBlackQueen(){
		int[,] m = {{0, 0, 4, 0, 0, 0, 0, 0},
			{0, 1, 0, 4, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 2, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{2, 0, 2, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(0, 2, m, null);

		Assert.That(listOfPlays.Count == 1);
	}



	/*********************************************************************
	 * PEÇAS BRANCAS
	 * NORMAIS
	**********************************************************************/
	[Test]
	public void VerifyTestNormalWalkWhite(){
		int[,] m = {{1, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 1},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 1, 0, 0, 0, 0},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 1, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(5, 5, m, null);

		Assert.That(listOfPlays.Count == 2);
	}

	[Test]
	public void VerifyTestEatingMultipleWhite(){
		int[,] m = {{1, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 1},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 1, 0, 0, 0, 0},
			{2, 0, 0, 0, 2, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 2, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(3, 3, m, null);

		Assert.That(listOfPlays.Count == 3);
	}

	[Test]
	public void VerifyTestEatingRightOneWhite(){
		int[,] m = {{1, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 1},
			{2, 0, 0, 0, 0, 0, 2, 0},
			{0, 1, 0, 0, 0, 0, 0, 0},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(1, 7, m, null);

		Assert.That(listOfPlays.Count == 1);
	}

	[Test]
	public void VerifyTestEatingLeftOneWhite(){
		int[,] m = {{1, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 1, 0, 0},
			{0, 0, 0, 0, 0, 0, 2, 0},
			{0, 1, 0, 0, 0, 0, 0, 0},
			{2, 0, 2, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(1, 5, m, null);

		Assert.That(listOfPlays.Count == 2);
	}

	/*********************************************************************
	 * PEÇAS BRANCAS
	 * DAMAS
	**********************************************************************/

	[Test]
	public void VerifyTestNormalWalkWhiteQueen(){
		int[,] m = {{3, 0, 1, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 1},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 0, 0, 0, 0, 0},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(0, 0, m, null);

		Assert.That(listOfPlays.Count == 7);
	}

	[Test]
	public void VerifyTestEatingMultipleWhiteQueen(){
		int[,] m = {{3, 0, 1, 0, 0, 0, 0, 0},
			{0, 2, 0, 2, 0, 0, 0, 1},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 0, 3, 0, 0, 0, 0},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(0, 0, m, null);

		Assert.That(listOfPlays.Count == 2);
	}

	[Test]
	public void VerifyTestEatingRightOneWhiteQueen(){
		int[,] m = {{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 2, 0, 0, 0, 0, 0, 1},
			{3, 0, 0, 0, 0, 0, 0, 0},
			{0, 3, 0, 0, 0, 0, 0, 0},
			{2, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(2, 0, m, null);

		Assert.That(listOfPlays.Count == 1);
	}

	[Test]
	public void VerifyTestEatingLeftOneWhiteQueen(){
		int[,] m = {{0, 0, 3, 0, 0, 0, 0, 0},
			{0, 2, 0, 3, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 2, 0},
			{0, 3, 0, 0, 0, 0, 0, 0},
			{2, 0, 2, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 0, 0, 0, 0, 0, 0}};

		List<List<int[]>> listOfPlays;

		listOfPlays = Verifier.VerifyPlayByPiece(0, 2, m, null);

		Assert.That(listOfPlays.Count == 1);
	}




}

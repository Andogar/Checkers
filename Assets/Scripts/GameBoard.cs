using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public Piece[,] pieces = new Piece[8, 8];
    public GameObject whitePiece;
    public GameObject blackPiece;

    private void Start()
    {
        GenerateBoard();
    }


    private void GenerateBoard()
    {
        // Generate White Pieces
        for (int y = 0; y < 3; y++)
        {
            for(int x = 0; x < 8; x += 2)
            {
                //Generate Piece
                GeneratePiece(x, y);
            }
        }

        //Generate Black Pieces
        for (int y = 5; y < 8; y++)
        {
            for (int x = 0; x < 8; x += 2)
            {
                GeneratePiece(x, y);
            }
        }
    }

    private void GeneratePiece(int x, int y)
    {
        GameObject newPiece;
        if (y < 3)
        {
            newPiece = whitePiece;
        } else
        {
            newPiece = blackPiece;
        }
        GameObject go = Instantiate(newPiece) as GameObject;
        go.transform.SetParent(transform);
        Piece piece = go.GetComponent<Piece>();
        pieces[x,y] = piece;
    }

    private void MovePieces()
    {

    }
}

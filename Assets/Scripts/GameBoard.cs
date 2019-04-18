using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public Piece[,] piecesArray = new Piece[8, 8];
    public GameObject whitePiece;
    public GameObject blackPiece;

    // offsets for both the board and the pieces
    private Vector3 boardOffset = new Vector3(-4.0f, 0, -4.0f);
    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

    private Vector2 mouseOver;
    private Piece selectedPiece;

    private void Start()
    {
        GenerateBoard();
    }

    private void Update()
    {
        UpdateMouseOver();

        //If it is my turn
        {
            int x = (int)mouseOver.x;
            int y = (int)mouseOver.y;
            if (Input.GetMouseButtonDown(0) && x != -1 && y != -1)
            {
                if (piecesArray[x, y] != null)
                {
                    Debug.Log("Selected Piece is before selection: " + selectedPiece);
                    SelectPiece(x, y);
                    Debug.Log("Selected Piece is after selection: " + selectedPiece);
                }
                else if (piecesArray[x, y] == null && selectedPiece != null)
                    PlacePiece(selectedPiece, x, y);
            }
        }
    }

    private void UpdateMouseOver()
    {
         if(!Camera.main)
            Debug.Log("Unable to find main camera");

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
        } 
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }
    }

    private void SelectPiece(int x, int y)
    {
        if (x < 0 || x >= piecesArray.Length || y < 0 || y > piecesArray.Length)
            return;

        if (selectedPiece != null)
        {
            //Set the currently selected piece back to it's old location and select the new piece
            piecesArray[selectedPiece.x, selectedPiece.y] = selectedPiece;
        }
        Piece piece = piecesArray[x, y];
        selectedPiece = piece;
        piecesArray[x, y] = null;
    }

    private void PlacePiece (Piece piece, int x, int y)
    {
        //Already assumes the coordinates contain a null value
        piecesArray[x, y] = piece;
        piece.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
        selectedPiece = null;
    }

    private void GenerateBoard()
    {
        //Generate White Pieces
        for (int y = 0; y < 3; y++)
        {
            bool oddRow = (y % 2 == 0);
            for(int x = 0; x < 8; x += 2)
            {
                //Generate the piece object 
                if (oddRow)
                    GeneratePiece(x, y);
                else
                    GeneratePiece(x + 1, y);
            }
        }
        //Generate Black Pieces
        for (int y = 7; y > 4; y--)
        {
            bool oddRow = (y % 2 == 0);
            for (int x = 0; x < 8; x += 2)
            {
                //Generate the piece object 
               if (oddRow)
                    GeneratePiece(x, y);
                else
                    GeneratePiece(x + 1, y);
            }
        }
    }

    private void GeneratePiece(int x, int y)
    {
        GameObject newPiece;
        //Check where the Y coordinate is at to check if it should be a white or black piece
        if (y < 3)
        {
            newPiece = whitePiece;
        } else
        {
            newPiece = blackPiece;
        }
        //Create a new Game Object instance of whatever color the conditional shows
        GameObject go = Instantiate(newPiece) as GameObject;
        //sets the parent of the transform to the board's transform so that the pieces are children of the board itself
        go.transform.SetParent(transform);
        //Grab the game object and cast it to a piece (I think?)
        Piece piece = go.GetComponent<Piece>();
        //Places the piece in the proper spot in the array, then calls MovePiece to place it on the board
        piecesArray[x,y] = piece;

        MovePiece(piece, x, y);
    }

    private void MovePiece(Piece piece, int x, int y)
    {
        //Places the piece to the proper position based on it's position in the array
        piece.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
        piece.x = x;
        piece.y = y;
    }
}

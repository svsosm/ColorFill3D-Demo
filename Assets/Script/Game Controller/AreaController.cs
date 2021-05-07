using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaController : MonoBehaviour
{
    [SerializeField] private GameObject Gate;
    [SerializeField] private LevelSettingScriptableObject levelSetting;

    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private GameObject levelbar;

    public PieceController[,] pieces;

    private Vector3 _tempVector;

    private Dictionary<int, List<PieceController>> _areas;

    public bool isCreateArea = false;

    private int width;
    private int height;


    private void Start()
    {
        width = levelSetting.width;
        height = levelSetting.height;
        CreateArea(levelSetting.startPos, width, height);
    }

    //Create game area
    public void CreateArea(Vector3 startPos, int width, int height)
    {
        _tempVector.y = startPos.y;
        pieces = new PieceController[width, height];

        for(int i=0; i < width; i++)
        {
            for(int j=0; j < height; j++)
            {
                _tempVector.x = i + startPos.x;
                _tempVector.z = j + startPos.z;

                GameObject piece = Instantiate(piecePrefab, _tempVector, Quaternion.identity);
                piece.name = "Piece_" + i + "_" + j;
                PieceController pieceController = piece.GetComponent<PieceController>();
                pieceController.x = i;
                pieceController.z = j;
                piece.transform.SetParent(gameObject.transform);
                pieces[i, j] = piece.GetComponent<PieceController>();
                pieceController.ChangePieceState(PieceState.EMPTY);

            }
        }
        isCreateArea = true;
    }

    public PieceController GetPiece(int x, int z)
    {
        if (x < 0 || x >= pieces.GetLength(0))
        {
            return null;
        }

        if (z < 0 || z >= pieces.GetLength(1))
        {
            return null;
        }

        return pieces[x, z];
    }

    //Track convert filled.
    public void FillTrack()
    {
        foreach (PieceController piece in pieces)
        {
            if (piece.CurrentPieceState == PieceState.TRACK)
            {
                piece.ChangePieceState(PieceState.FILLED);
            }
        }
    }

    public void FillArea()
    {
        _areas = new Dictionary<int, List<PieceController>>();

        int currentAreaIndex = 0;

        foreach (PieceController piece in pieces)
        {
            _areas.Add(currentAreaIndex, new List<PieceController>()); //create piece list
            IndexNeighbourPieces(piece, currentAreaIndex); //Neighboring pieces are added to the list.
            currentAreaIndex++;
        }

        int smallestArea = int.MaxValue;
        int smallestAreaIndex = 0;
        int areasCount = 0;
        foreach (KeyValuePair<int, List<PieceController>> keyValuePair in _areas)
        {
            if (keyValuePair.Value.Count == 0)
            {
                continue;
            }

            areasCount++;

            if (smallestArea > keyValuePair.Value.Count)
            {
                smallestAreaIndex = keyValuePair.Key;
                smallestArea = keyValuePair.Value.Count;
            }
        }

        if (areasCount > 1)
        {
            foreach (PieceController piece in _areas[smallestAreaIndex])
            {
                piece.ChangePieceState(PieceState.FILLED);
            }
        }

        CheckLevelEnd();

        foreach (PieceController piece in pieces)
        {
            piece.indexed = false;
        }
    }

    private void IndexNeighbourPieces(PieceController piece, int index)
    {
        if (piece == null || piece.indexed || piece.CurrentPieceState != PieceState.EMPTY)
        {
            return;
        }

        piece.indexed = true;

        _areas[index].Add(piece); //add neighbour in piece list

        IndexNeighbourPieces(GetPiece(piece.x, piece.z + 1), index);
        IndexNeighbourPieces(GetPiece(piece.x + 1, piece.z), index);
        IndexNeighbourPieces(GetPiece(piece.x, piece.z - 1), index);
        IndexNeighbourPieces(GetPiece(piece.x - 1, piece.z), index);
    }

   

    private void CheckLevelEnd()
    {
        int filledCount = 0;

        foreach (PieceController piece in pieces)
        {
            if (piece.CurrentPieceState == PieceState.FILLED)
            {
                filledCount++;
            }
        }

        int percentage = (int)(((float)filledCount / pieces.Length) * 100);
        levelbar.GetComponent<Scrollbar>().size =(float) percentage / 100;

        if (percentage >= levelSetting.percentageOfWin)
        {
            foreach (PieceController piece in pieces)
            {
                if (piece.CurrentPieceState == PieceState.EMPTY) //If the percentage is greater than the winning percentage, fill all the pieces.
                {
                    piece.ChangePieceState(PieceState.FILLED);
                }
            }
            levelbar.GetComponent<Scrollbar>().size = 1;
            if (Gate != null) //If there is a gate, activate its animation.
            {
                Gate.GetComponent<Animator>().enabled = true;
            }
            GameplayController.Instance.WonLevel();
            
        }

    }


}

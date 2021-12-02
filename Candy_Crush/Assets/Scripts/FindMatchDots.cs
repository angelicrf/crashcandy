using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class FindMatchDots : MonoBehaviour
{
    public List<GameObject> allMatchesFound = new List<GameObject>();
    private MainBoard mainBoard;
    public List<GameObject> newColmDots = new List<GameObject>();
    public List<GameObject> newRowDots = new List<GameObject>();
    public List<GameObject> newAdjacentDots = new List<GameObject>();
    //public Dots otherDot;
    void Start()
    {
        mainBoard = FindObjectOfType<MainBoard>();
        // otherDot = mainBoard.currentDot.otherDot.GetComponent<Dots>();
        if (allMatchesFound != null || newRowDots != null || newColmDots != null)
        {
            if (allMatchesFound.Count > 0)
            {
                allMatchesFound.Clear();
            }
            if (newRowDots.Count > 0)
            {
                newRowDots.Clear();
            }
            if (newColmDots.Count > 0)
            {
                newColmDots.Clear();
            }
        }

    }

    void Update()
    {

    }
    public void FindColorMatch(string thisColor)
    {
        for (int i = 0; i < mainBoard.boardWidth; i++)
        {
            for (int j = 0; j < mainBoard.boardHeight; j++)
            {
                if (mainBoard.Alldots[i, j])
                {
                    if (mainBoard.Alldots[i, j].tag == thisColor)
                    {
                        mainBoard.Alldots[i, j].GetComponent<Dots>().isFound = true;
                    }
                }
            }
        }
    }
    public IEnumerator FoundMatchesDots()
    {
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < mainBoard.boardWidth; i++)
        {
            for (int j = 0; j < mainBoard.boardHeight; j++)
            {
                GameObject currentMatch = mainBoard.Alldots[i, j];
                if (i > 0 && i < mainBoard.boardWidth - 1)
                {
                    if (mainBoard.Alldots[i, j] != null || mainBoard.Alldots[i + 1, j] != null || mainBoard.Alldots[i - 1, j])
                    {
                        GameObject leftDot = mainBoard.Alldots[i - 1, j];
                        GameObject rightDot = mainBoard.Alldots[i + 1, j];

                        if (currentMatch != null && leftDot != null && rightDot != null)
                        {
                            if (currentMatch.tag == leftDot.tag && currentMatch.tag == rightDot.tag)
                            {
                                AddAllMAtches(currentMatch);
                                AddAllMAtches(leftDot);
                                AddAllMAtches(rightDot);
                                UnionColumnnsLists(currentMatch, leftDot, rightDot);
                                UnionRowsLists(currentMatch, leftDot, rightDot);
                                UnionAdjacentLists(currentMatch, leftDot, rightDot);
                                ChangeIsFoundValue();
                            }
                        }
                    }
                }
                if (j > 0 && j < mainBoard.boardHeight - 1)
                {
                    if (mainBoard.Alldots[i, j] != null || mainBoard.Alldots[i, j - 1] != null || mainBoard.Alldots[i, j + 1])
                    {
                        GameObject upDot = mainBoard.Alldots[i, j + 1];
                        GameObject downDot = mainBoard.Alldots[i, j - 1];

                        if (currentMatch != null && upDot != null && downDot != null)
                        {
                            if (currentMatch.tag == upDot.tag && currentMatch.tag == downDot.tag)
                            {
                                AddAllMAtches(currentMatch);
                                AddAllMAtches(upDot);
                                AddAllMAtches(downDot);
                                UnionRowsLists(currentMatch, upDot, downDot);
                                UnionColumnnsLists(currentMatch, upDot, downDot);
                                UnionAdjacentLists(currentMatch, upDot, downDot);
                                ChangeIsFoundValue();
                            }
                        }
                    }
                }
            }
        }
    }
    private bool ChangeIsFoundValue()
    {
        if (mainBoard.currentDot)
        {
            mainBoard.currentDot.isFound = true;
            return mainBoard.currentDot.isFound;
        }
        if (mainBoard.currentDot.otherDot)
        {
            mainBoard.currentDot.otherDot.GetComponent<Dots>().isFound = true;
            return mainBoard.currentDot.otherDot.GetComponent<Dots>().isFound;
        }
        return false;
    }
    private void UnionColumnnsLists(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        if (dot1.GetComponent<Dots>().isColumnBomb)
        {
            allMatchesFound.Union(GetAllColumnDots(dot1.GetComponent<Dots>().columnDot));
        }
        if (dot2.GetComponent<Dots>().isColumnBomb)
        {
            allMatchesFound.Union(GetAllColumnDots(dot2.GetComponent<Dots>().columnDot - 1));
        }
        if (dot3.GetComponent<Dots>().isColumnBomb)
        {
            allMatchesFound.Union(GetAllColumnDots(dot3.GetComponent<Dots>().columnDot + 1));
        }
        dot1.GetComponent<Dots>().isFound = true;
        dot2.GetComponent<Dots>().isFound = true;
        dot3.GetComponent<Dots>().isFound = true;
    }
    private void UnionAdjacentLists(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        if (dot1.GetComponent<Dots>().isAdjacent)
        {
            allMatchesFound.Union(GetAdjacentDots(dot1.GetComponent<Dots>().columnDot, dot1.GetComponent<Dots>().rowDot));
        }
        if (dot2.GetComponent<Dots>().isAdjacent)
        {
            allMatchesFound.Union(GetAdjacentDots(dot2.GetComponent<Dots>().columnDot, dot2.GetComponent<Dots>().rowDot));
        }
        if (dot3.GetComponent<Dots>().isAdjacent)
        {
            allMatchesFound.Union(GetAdjacentDots(dot3.GetComponent<Dots>().columnDot, dot3.GetComponent<Dots>().rowDot));
        }
        dot1.GetComponent<Dots>().isFound = true;
        dot2.GetComponent<Dots>().isFound = true;
        dot3.GetComponent<Dots>().isFound = true;
    }
    private void UnionRowsLists(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        if (dot1.GetComponent<Dots>().isRowBomb)
        {
            allMatchesFound.Union(GetAllRowDots(dot1.GetComponent<Dots>().rowDot));
        }
        if (dot2.GetComponent<Dots>().isRowBomb)
        {
            allMatchesFound.Union(GetAllRowDots(dot2.GetComponent<Dots>().rowDot + 1));
        }
        if (dot3.GetComponent<Dots>().isRowBomb)
        {
            allMatchesFound.Union(GetAllRowDots(dot3.GetComponent<Dots>().rowDot - 1));
        }
        dot1.GetComponent<Dots>().isFound = true;
        dot2.GetComponent<Dots>().isFound = true;
        dot3.GetComponent<Dots>().isFound = true;
    }
    private void AddAllMAtches(GameObject thisItem)
    {
        if (!allMatchesFound.Contains(thisItem))
        {
            allMatchesFound.Add(thisItem);
        }
    }
    List<GameObject> GetAdjacentDots(int column, int row)
    {
        for (int i = column - 1; i <= column + 1; i++)
        {
            for (int j = row - 1; j <= row + 1; j++)
            {
                if (i >= 0 && i < mainBoard.boardWidth && j >= 0 && j < mainBoard.boardHeight)
                {
                    mainBoard.Alldots[i, j].GetComponent<Dots>().isFound = true;
                    newAdjacentDots.Add(mainBoard.Alldots[i, j]);
                }
            }
        }

        return newAdjacentDots;
    }
    List<GameObject> GetAllColumnDots(int column)
    {
        for (int i = 0; i < mainBoard.boardHeight; i++)
        {
            if (mainBoard.Alldots[column, i])
            {
                mainBoard.Alldots[column, i].GetComponent<Dots>().isFound = true;
                newColmDots.Add(mainBoard.Alldots[column, i]);
            }
        }
        return newColmDots;
    }
    List<GameObject> GetAllRowDots(int row)
    {
        for (int i = 0; i < mainBoard.boardWidth; i++)
        {
            if (mainBoard.Alldots[i, row])
            {
                mainBoard.Alldots[i, row].GetComponent<Dots>().isFound = true;
                newRowDots.Add(mainBoard.Alldots[i, row]);
            }
        }
        return newRowDots;
    }
    public void CheckDotsBomb()
    {
        if (mainBoard.currentDot != null)
        {
            if (mainBoard.currentDot.isFound)
            {
                if ((mainBoard.currentDot.angletoChange <= 45 && mainBoard.currentDot.angletoChange > -45)
                  || (mainBoard.currentDot.angletoChange <= 135 || mainBoard.currentDot.angletoChange > -135))
                {
                    mainBoard.currentDot.MakeRowBombs();
                }
                else
                {
                    mainBoard.currentDot.MakeColumnBombs();
                }
                mainBoard.currentDot.isFound = false;
            }
            else if (mainBoard.currentDot.otherDot)
            {
                if (mainBoard.currentDot.otherDot.GetComponent<Dots>().isFound)
                {
                    if ((mainBoard.currentDot.angletoChange <= 45 && mainBoard.currentDot.angletoChange > -45)
                     || (mainBoard.currentDot.angletoChange <= 135 || mainBoard.currentDot.angletoChange > -135))
                    {
                        mainBoard.currentDot.otherDot.GetComponent<Dots>().MakeRowBombs();
                    }
                    else
                    {
                        mainBoard.currentDot.otherDot.GetComponent<Dots>().MakeColumnBombs();
                    }
                    mainBoard.currentDot.otherDot.GetComponent<Dots>().isFound = false;
                }
            }
        }
    }
}

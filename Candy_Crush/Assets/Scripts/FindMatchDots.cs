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

                                if (!allMatchesFound.Contains(currentMatch))
                                {
                                    allMatchesFound.Add(currentMatch);
                                }
                                if (!allMatchesFound.Contains(leftDot))
                                {
                                    allMatchesFound.Add(leftDot);
                                }
                                if (!allMatchesFound.Contains(rightDot))
                                {
                                    allMatchesFound.Add(rightDot);
                                }
                                if (currentMatch.GetComponent<Dots>().isRowBomb || leftDot.GetComponent<Dots>().isRowBomb || rightDot.GetComponent<Dots>().rowArrow)
                                {
                                    allMatchesFound.Union(GetAllRowDots(j));
                                }
                                if (currentMatch.GetComponent<Dots>().isColumnBomb)
                                {
                                    allMatchesFound.Union(GetAllColumnDots(i));
                                }
                                if (leftDot.GetComponent<Dots>().isColumnBomb)
                                {
                                    allMatchesFound.Union(GetAllColumnDots(i - 1));
                                }
                                if (rightDot.GetComponent<Dots>().isColumnBomb)
                                {
                                    allMatchesFound.Union(GetAllColumnDots(i + 1));
                                }
                                if (mainBoard.currentDot)
                                {
                                    mainBoard.currentDot.isFound = true;
                                }
                                if (mainBoard.currentDot.otherDot)
                                {
                                    mainBoard.currentDot.otherDot.GetComponent<Dots>().isFound = true;
                                }
                                currentMatch.GetComponent<Dots>().isFound = true;
                                leftDot.GetComponent<Dots>().isFound = true;
                                rightDot.GetComponent<Dots>().isFound = true;

                                //return currentMatch.GetComponent<Dots>().isFound;
                            }
                        }
                    }
                }//
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

                                if (!allMatchesFound.Contains(currentMatch))
                                {
                                    allMatchesFound.Add(currentMatch);
                                }
                                if (!allMatchesFound.Contains(upDot))
                                {
                                    allMatchesFound.Add(upDot);
                                }
                                if (!allMatchesFound.Contains(downDot))
                                {
                                    allMatchesFound.Add(downDot);
                                }
                                if (currentMatch.GetComponent<Dots>().isColumnBomb || upDot.GetComponent<Dots>().isColumnBomb || downDot.GetComponent<Dots>().isColumnBomb)
                                {
                                    //Debug.Log("columnBobCalled...");
                                    allMatchesFound.Union(GetAllColumnDots(i));
                                }
                                if (currentMatch.GetComponent<Dots>().isRowBomb)
                                {
                                    allMatchesFound.Union(GetAllRowDots(j));
                                }
                                if (upDot.GetComponent<Dots>().isRowBomb)
                                {
                                    allMatchesFound.Union(GetAllRowDots(j + 1));
                                }
                                if (downDot.GetComponent<Dots>().isRowBomb)
                                {
                                    allMatchesFound.Union(GetAllRowDots(j - 1));
                                }
                                if (mainBoard.currentDot)
                                {
                                    mainBoard.currentDot.isFound = true;
                                }
                                if (mainBoard.currentDot.otherDot)
                                {
                                    mainBoard.currentDot.otherDot.GetComponent<Dots>().isFound = true;
                                }
                                currentMatch.GetComponent<Dots>().isFound = true;
                                upDot.GetComponent<Dots>().isFound = true;
                                downDot.GetComponent<Dots>().isFound = true;

                                //return currentMatch.GetComponent<Dots>().isFound;
                            }
                        }
                    }
                }
            }
        }
        //return false;
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
                if (allMatchesFound.Count == 6)
                {
                    mainBoard.currentDot.MakeRowBombs();
                }
                else if (allMatchesFound.Count == 9)
                {
                    mainBoard.currentDot.MakeColumnBombs();
                }
                mainBoard.currentDot.isFound = false;
            }
            else if (mainBoard.currentDot.otherDot)
            {
                if (mainBoard.currentDot.otherDot.GetComponent<Dots>().isFound)
                {
                    if (allMatchesFound.Count == 6)
                    {
                        mainBoard.currentDot.otherDot.GetComponent<Dots>().MakeRowBombs();
                    }
                    else if (allMatchesFound.Count == 9)
                    {
                        mainBoard.currentDot.otherDot.GetComponent<Dots>().MakeColumnBombs();
                    }
                    mainBoard.currentDot.otherDot.GetComponent<Dots>().isFound = false;
                }
            }
        }

    }
}

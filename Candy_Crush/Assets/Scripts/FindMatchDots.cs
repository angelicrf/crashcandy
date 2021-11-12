using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatchDots : MonoBehaviour
{
    public List<GameObject> allMatchesFound = new List<GameObject>();
    private MainBoard mainBoard;

    void Start()
    {
        mainBoard = FindObjectOfType<MainBoard>();
        if (allMatchesFound != null)
        {
            if (allMatchesFound.Count > 0)
            {
                allMatchesFound.Clear();
            }
        }
    }

    void Update()
    {

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
                        GameObject leftDot = mainBoard.Alldots[i + 1, j];
                        GameObject rightDot = mainBoard.Alldots[i - 1, j];

                        if (currentMatch != null && leftDot != null && rightDot != null)
                        {
                            if (currentMatch.tag == leftDot.tag && currentMatch.tag == rightDot.tag)
                            {
                                if (!allMatchesFound.Contains(leftDot))
                                {
                                    allMatchesFound.Add(leftDot);
                                }
                                if (!allMatchesFound.Contains(rightDot))
                                {
                                    allMatchesFound.Add(rightDot);
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
                                if (!allMatchesFound.Contains(upDot))
                                {
                                    allMatchesFound.Add(upDot);
                                }
                                if (!allMatchesFound.Contains(downDot))
                                {
                                    allMatchesFound.Add(downDot);
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
}

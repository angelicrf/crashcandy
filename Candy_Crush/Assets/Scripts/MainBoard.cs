using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Unity.Collections;
using UnityEditor.EditorTools;
using UnityEngine;

public enum DotStatus { move, pause };
public class MainBoard : MonoBehaviour
{
    public int boardWidth;
    public int boardHeight;
    public GameObject mainBackgroundPref;
    private GameObject[,] allTiles;
    public GameObject[,] Alldots;
    public SpriteRenderer brdSprtRnd;
    public GameObject[] dots;
    public GameObject ballonEffect;
    private bool isDroped;
    private FindMatchDots findMatchDots;
    public Dots currentDot;
    public DotStatus currentDotStatus = DotStatus.move;

    void Start()
    {
        findMatchDots = FindObjectOfType<FindMatchDots>();
        brdSprtRnd = mainBackgroundPref.GetComponent<SpriteRenderer>();
        Alldots = new GameObject[boardWidth, boardHeight];
        allTiles = new GameObject[boardWidth, boardHeight];
        isDroped = false;
        StartCoroutine(BoardIntilSetUp());
    }

    void Update()
    {

    }
    private IEnumerator BoardIntilSetUp()
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                Vector2 tempPos = new Vector2(i, j);

                GameObject newTiles = Instantiate(mainBackgroundPref, tempPos, Quaternion.identity) as GameObject;
                newTiles.name = "(" + i + " , " + j + ")";

                int dotPos = Random.Range(0, dots.Length);
                GameObject newDot = Instantiate(dots[dotPos], tempPos, Quaternion.identity);
                newDot.name = "(" + i + " , " + j + ")";
                Alldots[i, j] = newDot;

                int maxRun = 0;

                while (DotMatches(i, j, dots[dotPos]) && maxRun < 100)
                {
                    //dotPos = Random.Range(0, dots.Length);
                    DestroyAfterMove();
                    maxRun++;
                }
                maxRun = 0;
            }
        }
        yield return new WaitForSeconds(0.5f);
    }
    private bool DotMatches(int col, int row, GameObject dotObject)
    {
        if (col > 1 && row > 1)
        {
            if (Alldots[col - 1, row].tag == dotObject.tag && Alldots[col - 2, row].tag == dotObject.tag)
            {

                return true;
            }
            if (Alldots[col, row - 1].tag == dotObject.tag && Alldots[col, row - 2].tag == dotObject.tag)
            {

                return true;
            }

        }
        else if (col <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (Alldots[col, row - 1].tag == dotObject.tag
                 && Alldots[col, row - 2].tag == dotObject.tag)
                {

                    return true;
                }
            }
            if (col > 1)
            {
                if (Alldots[col - 1, row].tag == dotObject.tag
                && Alldots[col - 2, row].tag == dotObject.tag)
                {

                    return true;
                }
            }
        }
        return false;
    }
    private IEnumerator DestroyDot(int colmn, int row)
    {

        if (Alldots[colmn, row].GetComponent<Dots>().isFound)
        {
            // clear matches lists
            GameObject allBubbles = Instantiate(ballonEffect, Alldots[colmn, row].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Destroy(allBubbles);
            Destroy(Alldots[colmn, row]);
            Alldots[colmn, row] = null;
            currentDotStatus = DotStatus.move;
        }
    }
    public void DestroyAfterMove()
    {

        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (Alldots[i, j])
                {
                    StartCoroutine(DestroyDot(i, j));
                }
            }
        }
        if (findMatchDots.allMatchesFound != null)
        {
            MakeVariousBumbs();
        }
        //clear all Lists
        StartCoroutine(RemoveEmptySpace());

    }
    private void ClearAllLists()
    {
        if (findMatchDots.allMatchesFound != null)
        {
            if (findMatchDots.allMatchesFound.Count > 0)
            {
                findMatchDots.allMatchesFound.Clear();
            }
        }
        if (findMatchDots.newColmDots != null)
        {
            if (findMatchDots.newColmDots.Count > 0)
            {
                findMatchDots.newColmDots.Clear();
            }
        }
        if (findMatchDots.newRowDots != null)
        {
            if (findMatchDots.newRowDots.Count > 0)
            {
                findMatchDots.newRowDots.Clear();
            }
        }
        if (findMatchDots.newAdjacentDots != null)
        {
            if (findMatchDots.newRowDots.Count > 0)
            {
                findMatchDots.newRowDots.Clear();
            }
        }
    }
    private bool DefineColumnRow()
    {
        int horizantal = 0;
        int vertical = 0;
        Dots dotCompare = findMatchDots.allMatchesFound[0].GetComponent<Dots>();
        if (dotCompare)
        {
            foreach (GameObject thisObj in findMatchDots.allMatchesFound)
            {
                Dots thisDot = thisObj.GetComponent<Dots>();
                if (dotCompare.rowDot == thisDot.rowDot)
                {
                    horizantal++;
                }
                if (dotCompare.columnDot == thisDot.columnDot)
                {
                    vertical++;
                }

            }
        }
        return (horizantal == 5 || vertical == 5);
    }
    private void MakeVariousBumbs()
    {
        if (findMatchDots.allMatchesFound.Count == 9 || findMatchDots.allMatchesFound.Count == 6)
        {
            findMatchDots.CheckDotsBomb();
            //findMatchDots.allMatchesFound.Remove(Alldots[colmn, row]);
        }
        if (findMatchDots.allMatchesFound.Count == 4 || findMatchDots.allMatchesFound.Count == 8)
        {
            Debug.Log("adjacent dots");
            if (DefineColumnRow())
            {
                if (currentDot)
                {
                    if (currentDot.isFound)
                    {
                        if (!currentDot.isRainBowBomb)
                        {
                            currentDot.isFound = false;
                            currentDot.MakeColorBomb();
                        }
                    }
                }
                else
                {
                    if (currentDot.otherDot)
                    {
                        Dots newOtherDot = currentDot.otherDot.GetComponent<Dots>();
                        if (newOtherDot.isFound)
                        {
                            if (!newOtherDot.isRainBowBomb)
                            {
                                newOtherDot.isFound = false;
                                newOtherDot.MakeColorBomb();
                            }

                        }
                    }
                }

            }
            else
            {
                if (currentDot)
                {
                    if (currentDot.isFound)
                    {
                        if (!currentDot.isAdjacent)
                        {
                            currentDot.isFound = false;
                            currentDot.MakeAdjecentBomb();
                        }
                    }
                }
                else
                {
                    if (currentDot.otherDot)
                    {
                        Dots newOtherDot = currentDot.otherDot.GetComponent<Dots>();
                        if (newOtherDot.isFound)
                        {
                            if (!newOtherDot.isAdjacent)
                            {
                                newOtherDot.isFound = false;
                                newOtherDot.MakeAdjecentBomb();
                            }

                        }
                    }
                }
            }
        }
    }
    private IEnumerator RemoveEmptySpace()
    {
        int countDown = 0;
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (!Alldots[i, j])
                {
                    countDown++;
                }
                else if (countDown > 0)
                {
                    Alldots[i, j].GetComponent<Dots>().rowDot -= countDown;
                    Alldots[i, j] = null;
                }

            }
            countDown = 0;
        }
        yield return new WaitForSeconds(0.5f);
        ClearAllLists();
        StartCoroutine(StepsToRefile());
    }
    delegate void RefilleDot(int dlCol, int dlRow, GameObject[,] thisDot, GameObject[] spDot);
    RefilleDot RefileThis = delegate (int dlCol, int dlRow, GameObject[,] thisDot, GameObject[] spDot)
    {
        for (int i = 0; i < dlCol; i++)
        {
            for (int j = 0; j < dlRow; j++)
            {
                if (!thisDot[i, j])
                {
                    Vector2 tempRefillPos = new Vector2(i, j);
                    int thisRefill = Random.Range(0, spDot.Length);
                    GameObject newRefillObj = Instantiate(spDot[thisRefill], tempRefillPos, Quaternion.identity);
                    thisDot[i, j] = newRefillObj;
                    newRefillObj.name = "(" + i + " , " + j + ")";
                }
            }
        }
    };

    private bool IsMatchedDots()
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (Alldots[i, j])
                {
                    if (Alldots[i, j].GetComponent<Dots>().isFound)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private IEnumerator RefilleNewDots()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (Alldots[i, j] == null)
                {
                    Vector2 tempRefillPos = new Vector2(i, j);
                    int thisRefill = Random.Range(0, dots.Length);
                    GameObject newRefillObj = Instantiate(dots[thisRefill], tempRefillPos, Quaternion.identity);
                    Alldots[i, j] = newRefillObj;
                    newRefillObj.GetComponent<Dots>().rowDot = j;
                    newRefillObj.GetComponent<Dots>().columnDot = i;
                    //newRefillObj.name = "(" + i + " , " + j + ")";
                }
            }
        }
    }
    private IEnumerator StepsToRefile()
    {
        StartCoroutine(RefilleNewDots());
        yield return new WaitForSeconds(0.7f);
        while (IsMatchedDots())
        {
            yield return new WaitForSeconds(0.5f);
            DestroyAfterMove();

        }
        // call destroys
        yield return new WaitForSeconds(0.5f);
        currentDotStatus = DotStatus.move;
    }
}

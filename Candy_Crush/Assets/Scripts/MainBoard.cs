using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEditor.EditorTools;
using UnityEngine;

public class MainBoard : MonoBehaviour
{
    public int boardWidth;
    public int boardHeight;
    public GameObject mainBackgroundPref;
    private GameObject[,] allTiles;
    public GameObject[,] Alldots;
    public SpriteRenderer brdSprtRnd;
    public GameObject[] dots;

    void Start()
    {
        brdSprtRnd = mainBackgroundPref.GetComponent<SpriteRenderer>();
        Alldots = new GameObject[boardWidth, boardHeight];
        allTiles = new GameObject[boardWidth, boardHeight];

        BoardIntilSetUp();

    }

    void Update()
    {

    }
    private void BoardIntilSetUp()
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                Vector2 tempPos = new Vector2(i, j);

                GameObject newTiles = Instantiate(mainBackgroundPref, tempPos, Quaternion.identity) as GameObject;
                newTiles.name = "(" + i + " , " + j + ")";
                int dotPos = Random.Range(0, dots.Length);
                int maxRun = 0;
                while (DotMatches(i, j, dots[dotPos]) && maxRun < 100)
                {
                    dotPos = Random.Range(0, dots.Length);
                    maxRun++;
                }
                maxRun = 0;
                GameObject newDot = Instantiate(dots[dotPos], tempPos, Quaternion.identity);
                newDot.name = "(" + i + " , " + j + ")";
                Alldots[i, j] = newDot;
            }
        }
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
    private void DeactiveSpriteRnd()
    {
        brdSprtRnd.enabled = false;
    }
    private void DestroyDot(int colmn, int row)
    {
        if (Alldots[colmn, row].GetComponent<Dots>().isFound)
        {
            Destroy(Alldots[colmn, row]);
            Alldots[colmn, row] = null;
        }
    }
    public void DestroyAfterMove()
    {
        int countDown = 0;
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (Alldots[i, j] != null)
                {
                    DestroyDot(i, j);
                }
            }
        }
        StartCoroutine(RemoveEmptySpace());
    }
    private IEnumerator RemoveEmptySpace()
    {
        int countDown = 0;
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (Alldots[i, j] == null)
                {
                    countDown++;
                }
                else if (countDown > 0)
                {
                    Alldots[i, j].GetComponent<Dots>().rowDot -= countDown;
                    Debug.Log("rowDropedTwo");
                }
            }
            countDown = 0;
        }
        yield return new WaitForSeconds(0.5f);
    }

}

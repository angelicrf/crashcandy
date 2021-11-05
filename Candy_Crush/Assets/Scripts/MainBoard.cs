using System.Collections;
using System.Collections.Generic;
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
        //DeactiveSpriteRnd();

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
                GameObject newDot = Instantiate(dots[dotPos], tempPos, Quaternion.identity);
                newDot.name = "(" + i + " , " + j + ")";
                Alldots[i, j] = newDot;
            }
        }
    }
    private void DeactiveSpriteRnd()
    {
        brdSprtRnd.enabled = false;
    }
}

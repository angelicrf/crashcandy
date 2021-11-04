using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBoard : MonoBehaviour
{
    public int boardWidth;
    public int boardHeight;
    public GameObject mainBackgroundPref;
    private MainBackground[,] allTiles;

    void Start()
    {
        allTiles = new MainBackground[boardWidth, boardHeight];
        BoardIntilSetUp();
    }

    // Update is called once per frame
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
                Instantiate(mainBackgroundPref, tempPos, Quaternion.identity);
            }
        }
    }
}

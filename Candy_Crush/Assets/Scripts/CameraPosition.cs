using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraPosition : MonoBehaviour
{
    public MainBoard mainBoard;
    public float boardAspectRatio;
    public float offSet;
    public float padding;

    void Start()
    {
        mainBoard = FindObjectOfType<MainBoard>();
        padding = CalcPadding();
        offSet = -10;

        boardAspectRatio = (float)mainBoard.boardWidth / (float)mainBoard.boardHeight;
        if (mainBoard)
        {
            CameraReposition(mainBoard.boardWidth - 1, mainBoard.boardHeight - 1, offSet);
        }
    }
    private float CalcPadding()
    {
        float defaultPadding = -0.5f;
        int maxBrdValue = 20;

        if (mainBoard.boardWidth <= 6 && mainBoard.boardWidth > 0
          && mainBoard.boardHeight <= 10 && mainBoard.boardHeight > 0)
        {
            padding = defaultPadding;
        }
        if (mainBoard.boardWidth > 6 && mainBoard.boardWidth > 0
        || mainBoard.boardHeight > 10 && mainBoard.boardHeight > 0)
        {
            if (mainBoard.boardWidth > 6 && mainBoard.boardHeight! > 10)
            {
                for (int i = 6; i < maxBrdValue; i++)
                {
                    padding = defaultPadding + 0.1f;
                }
            }
            if (mainBoard.boardHeight > 10 && mainBoard.boardWidth! > 6)
            {
                for (int i = 10; i < maxBrdValue; i++)
                {
                    padding = defaultPadding + 0.1f;
                }
            }
            if (mainBoard.boardHeight > 10 && mainBoard.boardWidth > 6)
            {
                if (mainBoard.boardHeight - mainBoard.boardWidth == 4)
                {
                    for (int i = 10; i < maxBrdValue; i++)
                    {
                        padding = defaultPadding + 0.1f;
                    }
                }
                else
                {
                    mainBoard.boardHeight = 6;
                    mainBoard.boardHeight = 10;
                    padding = defaultPadding;
                }
            }
        }
        return padding;
    }
    private void CameraReposition(float camX, float camY, float camZ)
    {
        Vector3 camTemPos = new Vector3(camX / 2, camY / 2, camZ);
        transform.position = camTemPos;

        if (camX >= camY)
        {
            Camera.main.orthographicSize = (camX / 2 + padding) / boardAspectRatio;

        }
        else
        {
            Camera.main.orthographicSize = (camY / 2 + padding) / boardAspectRatio;
        }

    }
}

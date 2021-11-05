using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dots : MonoBehaviour
{
    private Vector2 startDotPosition;
    private Vector2 lastDotPosition;
    public float angletoChange;
    private int columnDot;
    private int rowDot;
    private int finalX;
    private int finalY;
    private MainBoard mainBoard;
    private GameObject otherDot;
    private Vector2 tempTargetPos;
    void Start()
    {
        mainBoard = FindObjectOfType<MainBoard>();
        finalX = (int)transform.position.x;
        finalY = (int)transform.position.y;
        columnDot = finalX;
        rowDot = finalY;
    }

    void Update()
    {
        finalX = columnDot;
        finalY = rowDot;
        if (Mathf.Abs((finalX - transform.position.x)) > 0.1)
        {
            tempTargetPos = new Vector2(finalX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempTargetPos, Time.deltaTime);
        }
        else
        {
            tempTargetPos = new Vector2(finalX, transform.position.y);
            transform.position = tempTargetPos;
            mainBoard.Alldots[columnDot, rowDot] = this.gameObject;
        }
        if (Mathf.Abs((finalY - transform.position.y)) > 0.1)
        {
            tempTargetPos = new Vector2(transform.position.x, finalY);
            transform.position = Vector2.Lerp(transform.position, tempTargetPos, Time.deltaTime);
        }
        else
        {
            tempTargetPos = new Vector2(transform.position.x, finalY);
            transform.position = tempTargetPos;
            mainBoard.Alldots[columnDot, rowDot] = this.gameObject;
        }
    }
    void OnMouseDown()
    {
        startDotPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void OnMouseUp()
    {
        lastDotPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateDifAngle();
    }
    void CalculateDifAngle()
    {
        angletoChange = Mathf.Atan2(lastDotPosition.y - startDotPosition.y, lastDotPosition.x - startDotPosition.x) * 180 / Mathf.PI;
        //Debug.Log("angelsDot " + angletoChange);
        MoveDots();
    }
    void MoveDots()
    {
        if ((angletoChange > -45 && angletoChange <= 45 && columnDot < mainBoard.boardWidth))
        {
            otherDot = mainBoard.Alldots[columnDot + 1, rowDot];
            otherDot.GetComponent<Dots>().columnDot -= 1;
            columnDot += 1;
        }
        else if ((angletoChange > 45 && angletoChange <= 135) && rowDot < mainBoard.boardHeight)
        {
            otherDot = mainBoard.Alldots[columnDot, rowDot + 1];
            otherDot.GetComponent<Dots>().rowDot -= 1;
            rowDot += 1;
        }
        else if ((angletoChange < -45 && angletoChange >= -135) && rowDot > 0)
        {
            otherDot = mainBoard.Alldots[columnDot, rowDot - 1];
            otherDot.GetComponent<Dots>().rowDot += 1;
            rowDot -= 1;
        }
        else if ((angletoChange > 135 || angletoChange <= -135) && columnDot > 0)
        {
            otherDot = mainBoard.Alldots[columnDot - 1, rowDot];
            otherDot.GetComponent<Dots>().columnDot += 1;
            columnDot -= 1;
        }
    }
}
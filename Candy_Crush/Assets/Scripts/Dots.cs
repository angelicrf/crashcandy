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
    public int rowDot;
    private int originalDotX;
    private int originalDotY;
    private int finalX;
    private int finalY;
    private MainBoard mainBoard;
    private GameObject otherDot;
    private Vector2 tempTargetPos;
    public bool isFound = false;
    public float isOkSwipe = 1f;
    void Start()
    {
        mainBoard = FindObjectOfType<MainBoard>();
        finalX = (int)transform.position.x;
        finalY = (int)transform.position.y;
        columnDot = finalX;
        rowDot = finalY;
        originalDotY = rowDot;
        originalDotX = columnDot;
    }

    void Update()
    {
        finalX = columnDot;
        finalY = rowDot;
        FindMatchedDots();
        /*   if (isFound)
          {
              SpriteRenderer newObject = GetComponent<SpriteRenderer>();
              newObject.color = new Color(0f, 0f, 0f, 0.2f);
          } */
        if (Mathf.Abs((finalX - transform.position.x)) > 0.1)
        {
            tempTargetPos = new Vector2(finalX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempTargetPos, Time.deltaTime);
            if (mainBoard.Alldots[columnDot, rowDot] != this.gameObject)
            {
                mainBoard.Alldots[columnDot, rowDot] = this.gameObject;
            }
        }
        else
        {
            tempTargetPos = new Vector2(finalX, transform.position.y);
            transform.position = tempTargetPos;

        }
        if (Mathf.Abs((finalY - transform.position.y)) > 0.1)
        {
            tempTargetPos = new Vector2(transform.position.x, finalY);
            transform.position = Vector2.Lerp(transform.position, tempTargetPos, Time.deltaTime);
            if (mainBoard.Alldots[columnDot, rowDot] != this.gameObject)
            {
                mainBoard.Alldots[columnDot, rowDot] = this.gameObject;
            }
        }
        else
        {
            tempTargetPos = new Vector2(transform.position.x, finalY);
            transform.position = tempTargetPos;

        }
    }
    void OnMouseDown()
    {

        startDotPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void OnMouseUp()
    {
        if (Mathf.Abs(lastDotPosition.y - startDotPosition.y) > isOkSwipe || Mathf.Abs(lastDotPosition.x - startDotPosition.x) > isOkSwipe)
        {
            lastDotPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateDifAngle();
        }
    }
    void CalculateDifAngle()
    {
        angletoChange = Mathf.Atan2(lastDotPosition.y - startDotPosition.y, lastDotPosition.x - startDotPosition.x) * 180 / Mathf.PI;
        //Debug.Log("angelsDot " + angletoChange);
        MoveDots();
    }
    void MoveDots()
    {

        if ((angletoChange > -45 && angletoChange <= 45 && columnDot < mainBoard.boardWidth - 1))
        {
            otherDot = mainBoard.Alldots[columnDot + 1, rowDot];
            otherDot.GetComponent<Dots>().columnDot -= 1;
            columnDot += 1;
        }
        else if ((angletoChange > 45 && angletoChange <= 135) && rowDot < mainBoard.boardHeight - 1)
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
        StartCoroutine(MoveMatchedDots());
    }
    bool FindMatchedDots()
    {
        if (columnDot > 0 && columnDot < mainBoard.boardWidth - 1)
        {
            GameObject newDot_1 = mainBoard.Alldots[columnDot - 1, rowDot];
            GameObject newDot_2 = mainBoard.Alldots[columnDot + 1, rowDot];

            if (newDot_1 != null && newDot_2 != null)
            {
                if ((this.gameObject.tag == newDot_2.gameObject.tag &&
                    this.gameObject.tag == newDot_1.gameObject.tag))

                {
                    isFound = true;
                    newDot_1.GetComponent<Dots>().isFound = true;
                    newDot_2.GetComponent<Dots>().isFound = true;
                    return isFound;
                }
            }
            if (rowDot > 0 && rowDot < mainBoard.boardHeight - 1)
            {
                GameObject newDot_3 = mainBoard.Alldots[columnDot, rowDot + 1];
                GameObject newDot_4 = mainBoard.Alldots[columnDot, rowDot - 1];

                if (newDot_3 != null && newDot_4 != null)
                {
                    if ((this.gameObject.tag == newDot_3.gameObject.tag &&
                        this.gameObject.tag == newDot_4.gameObject.tag))

                    {
                        isFound = true;
                        newDot_1.GetComponent<Dots>().isFound = true;
                        newDot_2.GetComponent<Dots>().isFound = true;
                        return isFound;
                    }
                }
            }

        }
        return isFound;
    }
    IEnumerator MoveMatchedDots()
    {
        yield return new WaitForSeconds(.5f);
        if (otherDot != null)
        {
            if (!isFound && !otherDot.GetComponent<Dots>().isFound)
            {
                otherDot.GetComponent<Dots>().columnDot = columnDot;
                otherDot.GetComponent<Dots>().rowDot = rowDot;
                columnDot = originalDotX;
                rowDot = originalDotY;
            }
            else
            {
                mainBoard.DestroyAfterMove();
            }
        }
        otherDot = null;
    }

}
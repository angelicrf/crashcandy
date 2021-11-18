using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dots : MonoBehaviour
{
    private Vector2 startDotPosition;
    private Vector2 lastDotPosition;
    public float angletoChange;
    public int columnDot;
    public int rowDot;
    private int originalDotX;
    private int originalDotY;
    private int finalX;
    private int finalY;
    private MainBoard mainBoard;
    public GameObject otherDot;
    private Vector2 tempTargetPos;
    public bool isFound = false;
    public float isOkSwipe = 1f;
    private FindMatchDots findNewMethod;
    public bool isRowBomb;
    public bool isColumnBomb;
    public bool isRainBowBomb;
    public GameObject rowArrow;
    public GameObject columnArrow;
    public GameObject rainBowBomb;
    void Start()
    {
        isRowBomb = false;
        isColumnBomb = false;
        isRainBowBomb = false;
        findNewMethod = FindObjectOfType<FindMatchDots>();
        mainBoard = FindObjectOfType<MainBoard>();
        finalX = (int)transform.position.x;
        finalY = (int)transform.position.y;
        columnDot = finalX;
        rowDot = finalY;
        originalDotY = rowDot;
        originalDotX = columnDot;
        mainBoard.currentDot = this;
    }

    void Update()
    {
        finalX = columnDot;
        finalY = rowDot;

        StartCoroutine(findNewMethod.FoundMatchesDots());
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
    void OnMouseOver()
    {
        CallRainBowBomb();
    }
    bool CallRainBowBomb()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRainBowBomb = true;
            GameObject newRainBow = Instantiate(rainBowBomb, transform.position, Quaternion.identity);
            newRainBow.transform.parent = transform;
            return isRainBowBomb;
        }
        return false;
    }
    void OnMouseDown()
    {
        if (mainBoard.currentDotStatus == DotStatus.move)
        {
            startDotPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    void OnMouseUp()
    {
        if (mainBoard.currentDotStatus == DotStatus.move)
        {
            lastDotPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateDifAngle();
        }

    }
    void CalculateDifAngle()
    {
        if (Mathf.Abs(lastDotPosition.y - startDotPosition.y) > isOkSwipe || Mathf.Abs(lastDotPosition.x - startDotPosition.x) > isOkSwipe)
        {
            angletoChange = Mathf.Atan2(lastDotPosition.y - startDotPosition.y, lastDotPosition.x - startDotPosition.x) * 180 / Mathf.PI;
            MoveDots();
            mainBoard.currentDotStatus = DotStatus.pause;
            mainBoard.currentDot = this;
        }
        else
        {
            mainBoard.currentDotStatus = DotStatus.move;
        }
    }
    void MoveDots()
    {

        if ((angletoChange > -45 && angletoChange <= 45) && columnDot < mainBoard.boardWidth - 1)
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
    public bool FindMatchedDots()
    {
        if (columnDot > 0 && columnDot < mainBoard.boardWidth - 1)
        {
            if (mainBoard.Alldots[columnDot - 1, rowDot] || mainBoard.Alldots[columnDot + 1, rowDot])
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
            }
            if (rowDot > 0 && rowDot < mainBoard.boardHeight - 1)
            {
                if (mainBoard.Alldots[columnDot, rowDot + 1] || mainBoard.Alldots[columnDot, rowDot - 1])
                {
                    GameObject newDot_3 = mainBoard.Alldots[columnDot, rowDot + 1];
                    GameObject newDot_4 = mainBoard.Alldots[columnDot, rowDot - 1];

                    if (newDot_3 != null && newDot_4 != null)
                    {
                        if ((this.gameObject.tag == newDot_3.gameObject.tag &&
                            this.gameObject.tag == newDot_4.gameObject.tag))

                        {
                            isFound = true;
                            newDot_3.GetComponent<Dots>().isFound = true;
                            newDot_4.GetComponent<Dots>().isFound = true;
                            return isFound;
                        }
                    }
                }
            }

        }
        return isFound;
    }
    IEnumerator MoveMatchedDots()
    {
        if (isRainBowBomb)
        {
            findNewMethod.FindColorMatch(otherDot.tag);
            isFound = true;
        }
        else if (otherDot.GetComponent<Dots>().isRainBowBomb)
        {
            findNewMethod.FindColorMatch(this.gameObject.tag);
            otherDot.GetComponent<Dots>().isFound = true;
        }
        yield return new WaitForSeconds(.5f);

        if (otherDot != null)
        {
            if (!isFound || !otherDot.GetComponent<Dots>().isFound)
            {
                otherDot.GetComponent<Dots>().columnDot = columnDot;
                otherDot.GetComponent<Dots>().rowDot = rowDot;
                columnDot = originalDotX;
                rowDot = originalDotY;
                mainBoard.currentDot = null;
                yield return new WaitForSeconds(.5f);
                mainBoard.currentDotStatus = DotStatus.move;
            }
            else
            {
                mainBoard.DestroyAfterMove();
            }
            // otherDot = null;
        }

    }
    public void MakeRowBombs()
    {
        isRowBomb = true;
        GameObject newRowArrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
        newRowArrow.transform.parent = transform;

    }
    public void MakeColumnBombs()
    {
        isColumnBomb = true;
        GameObject newColumnArrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
        newColumnArrow.transform.parent = transform;

    }

}
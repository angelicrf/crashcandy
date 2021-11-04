using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dots : MonoBehaviour
{
    private Vector2 startDotPosition;
    private Vector2 lastDotPosition;
    public float angletoChange;
    void Start()
    {

    }

    void Update()
    {

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
        Debug.Log("angelsDot " + angletoChange);
    }
}
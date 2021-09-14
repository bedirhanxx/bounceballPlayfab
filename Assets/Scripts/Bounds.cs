using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    public Transform LeftSide;
    public Transform RightSide;
    public Transform BorderLeft;
    public Transform BorderRight;
    public float offsetX;

    void Update()
    {
        float OrthoWidth = Camera.main.orthographicSize * GetComponent<Camera>().aspect;
        float MovePosY_Left = LeftSide.transform.position.y;
        float MovePosY_Right = RightSide.transform.position.y;
        LeftSide.transform.position = new Vector3 (transform.localPosition.x - OrthoWidth + offsetX, MovePosY_Left, -5.0f);
        RightSide.transform.position = new Vector3 (transform.localPosition.x + OrthoWidth - offsetX, MovePosY_Right, -5.0f);
        BorderLeft.transform.position = new Vector3 (transform.localPosition.x - OrthoWidth, transform.localPosition.y, -4.0f);
        BorderRight.transform.position = new Vector3 (transform.localPosition.x + OrthoWidth, transform.localPosition.y, -4.0f);
    }

}

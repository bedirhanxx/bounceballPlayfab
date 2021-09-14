using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VelocityDisplayer : MonoBehaviour
{
    public Transform Object;
    public GameObject Ball;
    public Text View;
    private Vector3 _position;
    private float startPosY;
    private bool isBeinHeld;

    public Color Color_Red;
    public Color Color_Blue;
    public Color Color_Black;
    public float velocity;

    private void OnEnable()
    {
        _position = Object.transform.position;
    }

    private void Start()
    {
        Ball = GameObject.FindGameObjectWithTag("Ball");
    }

    private void Update()
    {
        var dt = Time.deltaTime;
        var current = Object.transform.position;
        var delta = Vector3.Distance(current, _position);
        velocity = delta / dt;
        View.text = (velocity).ToString("#,##0.000");
        _position = current;
        if (isBeinHeld)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, mousePos.y - startPosY, 5f);
        }
        var pos = transform.localPosition;
        pos.y = Mathf.Clamp(transform.localPosition.y, -3.0f, 1.5f);
        transform.localPosition = pos;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            if (velocity > 40f) {
                collision.gameObject.GetComponent<Ball>().force_pw = 15;
                collision.gameObject.GetComponent<Ball>().force_pw_side = 3;
                collision.gameObject.GetComponent<Ball>().maxFollow = 4.9f;
                GetComponent<SpriteRenderer>().color = Color_Red;
                Debug.Log("HitFast");
            } else if (velocity > 10f) {
                collision.gameObject.GetComponent<Ball>().force_pw = 11;
                collision.gameObject.GetComponent<Ball>().force_pw_side = 3;
                collision.gameObject.GetComponent<Ball>().maxFollow = 4.8f;
                GetComponent<SpriteRenderer>().color = Color_Blue;
                Debug.Log("HitNormal");
            } else if (velocity < 10f) {
                collision.gameObject.GetComponent<Ball>().force_pw = 10;
                collision.gameObject.GetComponent<Ball>().force_pw_side = 3;
                collision.gameObject.GetComponent<Ball>().maxFollow = 4.5f;
                GetComponent<SpriteRenderer>().color = Color_Black;
                Debug.Log("HitSlow");
            }

        }
    }

    void OnMouseDown()
    {
        Vector3 mousePos;
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        isBeinHeld = true;
        startPosY = mousePos.y - this.transform.localPosition.y;
    }

    void OnMouseUp()
    {
        isBeinHeld = false;
    }
}
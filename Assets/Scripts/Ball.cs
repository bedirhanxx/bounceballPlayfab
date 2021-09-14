using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    public int force_pw;
    public int force_pw_side;
    public float maxFollow = 6f;
    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    public int traveledDistance;
    public bool enabled;
    public bool SpawnCoin;
    public Vector2 velocity;
    public GameObject backGroundImage;
    public GameObject CoinHolder;
    public GameObject Level;

    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * 4f, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Coin")
        {
            PlayfabController.PFC.GetCoin();
            Destroy(collision.gameObject);
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        float RectWidth = this.GetComponent<Collider2D>().bounds.size.x;
        float RectHeight = this.GetComponent<Collider2D>().bounds.size.y;
        float circleRad = collider.bounds.size.x;

        Vector3 contactPoint = collision.contacts[0].point;
        Vector3 center = collider.bounds.center;


        if(collision.gameObject.tag == "DeadZone")
        {
            this.gameObject.GetComponent<Renderer>().enabled = false;
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.gameObject.transform.position = new Vector3(0, 4.19f, -1f);
            MenuController.MC.setHsCore();
            Time.timeScale = 0;
        }


        if (collision.gameObject.tag == "Wall")
        {
            if (force_pw >= 10)
            {
                force_pw -= 4;
            }
            else
            {
                force_pw -= 1;
            }
            if (force_pw <= 3)
            {
                force_pw = 3;
            }
            GetComponent<Rigidbody2D>().AddForce(collision.contacts[0].normal * force_pw, ForceMode2D.Impulse);
        }

        if (collision.gameObject.tag == "StickRight")
        {
            if (contactPoint.y > center.y &&
               (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2))
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-force_pw_side, force_pw);
            }
            else if (contactPoint.x < center.x &&
             (contactPoint.y < center.y + RectHeight / 2 && contactPoint.y > center.y - RectHeight / 2))
            {
                if (collision.gameObject.GetComponent<VelocityDisplayer>().velocity > 1)
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-force_pw_side, force_pw);
                else
                    GetComponent<Rigidbody2D>().AddForce(collision.contacts[0].normal * 4f, ForceMode2D.Impulse);
            }
        }
        if (collision.gameObject.tag == "StickLeft")
        {
            if (contactPoint.y > center.y &&
               (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2))
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(force_pw_side, force_pw);
            }
            else if (contactPoint.x > center.x &&
             (contactPoint.y < center.y + RectHeight / 2 && contactPoint.y > center.y - RectHeight / 2))
            {
                if (collision.gameObject.GetComponent<VelocityDisplayer>().velocity > 1)
                    GetComponent<Rigidbody2D>().velocity = new Vector2(force_pw_side, force_pw);
                else
                    GetComponent<Rigidbody2D>().AddForce(collision.contacts[0].normal * 4f, ForceMode2D.Impulse);
            }
        }
    }
    void Update()
    {
        if (transform.localPosition.y >= 4.5f) {
            enabled = true;
            var pos = transform.localPosition;
            pos.y = Mathf.Clamp(transform.localPosition.y, -4.0f, maxFollow);
            transform.localPosition = pos;
        }
        else
        {
            SpawnCoin = false;
            enabled = false;
        }
        if (enabled)
        {
            float offset = 1f;
            backGroundImage.transform.DOMove(backGroundImage.transform.position - backGroundImage.transform.up * offset, 0.5f);
            CoinHolder.transform.DOMove(CoinHolder.transform.position - CoinHolder.transform.up * offset, 0.5f);
            if (Time.time > nextActionTime)
            {
                nextActionTime += period;
                traveledDistance += 1;                
            }
            if (!SpawnCoin)
            {
                Level.GetComponent<RandomSpawner>().spawnObjects();
                SpawnCoin = true;
            }
        }
    }

    

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallProterties : MonoBehaviour
{

    public int sizeX;
    public int sizeY;
    public int rbMass;
    public int redC;
    public int blueC;
    public int greenC;

    private GameObject Ball;

    void Start()
    {
        Ball = GameObject.FindGameObjectWithTag("Ball");
        Time.timeScale = 0;
        InvokeRepeating("getDynamicValues", 5.0f, 5f);
    }



    public void setStatsForBall()
    {
        #region StatsForBalls
        if (this.gameObject.name == "Default Ball")
        {
            sizeX = 1;
            sizeY = 1;
            rbMass = 1;
            redC = 255;
            greenC = 255;
            blueC = 255;
        }
        if (this.gameObject.name == "Red Ball")
        {
            sizeX = 2;
            sizeY = 2;
            rbMass = 2;
            redC = 255;
            greenC = 0;
            blueC = 0;
        }
        if (this.gameObject.name == "Yellow Ball")
        {
            sizeX = 4;
            sizeY = 4;
            rbMass = 5;
            redC = 255;
            greenC = 255;
            blueC = 0;
        }
        if (this.gameObject.name == "Blue Ball")
        {
            sizeX = 2;
            sizeY = 2;
            rbMass = 2;
            redC = 0;
            greenC = 200;
            blueC = 255;
        }
        if (this.gameObject.name == "Green Ball")
        {
            sizeX = 1;
            sizeY = 1;
            rbMass = 1;
            redC = 0;
            greenC = 255;
            blueC = 0;
        }
        if (this.gameObject.name == "Orange Ball")
        {
            sizeX = 1;
            sizeY = 1;
            rbMass = 1;
            redC = 255;
            greenC = 100;
            blueC = 0;
        }
        #endregion
        setAndGetStat();
    }





    void setAndGetStat()
    {
        PlayfabController.PFC.sizeX = sizeX;
        PlayfabController.PFC.sizeY = sizeY;
        PlayfabController.PFC.rbMass = rbMass;
        PlayfabController.PFC.redC = redC;
        PlayfabController.PFC.blueC = blueC;
        PlayfabController.PFC.greenC = greenC;
        PlayfabController.PFC.SetStats();
        PlayfabController.PFC.GetStats();

        Ball.transform.localScale = new Vector3(PlayfabController.PFC.sizeX, PlayfabController.PFC.sizeY, -1f);
        Ball.gameObject.GetComponent<Rigidbody2D>().mass = PlayfabController.PFC.rbMass;
        Ball.GetComponent<Renderer>().material.color = new Color32((byte)PlayfabController.PFC.redC, (byte)PlayfabController.PFC.greenC, (byte)PlayfabController.PFC.blueC, 255);

    }

    void getDynamicValues()
    {
        
        PlayfabController.PFC.GetStats();
        PlayfabController.PFC.SetStats();



        Debug.Log(PlayfabController.PFC.hScore + "  H SCORE");

        Ball.transform.localScale = new Vector3(PlayfabController.PFC.sizeX, PlayfabController.PFC.sizeY, -1f);
        Ball.gameObject.GetComponent<Rigidbody2D>().mass = PlayfabController.PFC.rbMass;
        Ball.GetComponent<Renderer>().material.color = new Color32((byte)PlayfabController.PFC.redC, (byte)PlayfabController.PFC.greenC, (byte)PlayfabController.PFC.blueC, 255);

        
    }




}

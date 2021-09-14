using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController MC;

    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI HScoreText;
    public TextMeshProUGUI OldHcoreText;
    public TextMeshProUGUI yScoreText;
    public TextMeshProUGUI ErrorText;
    public GameObject GameOverPanel;
    public GameObject ShopPanel;
    public GameObject InventoryPanel;
    public GameObject Ball;
    private int highScore;
    private int oldScore;

    void OnEnable()
    {
        MC = this;
    }

    private void Start()
    {
        GameOverPanel.SetActive(false);
    }

    public void OpenGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }
    public void OpenShop()
    {
        ShopPanel.SetActive(true);
        Time.timeScale = 0;

    }
    public void CloseShop()
    {
        ShopPanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void OpenIventory()
    {
        InventoryPanel.SetActive(true);
        Time.timeScale = 0;

    }

    public void CloseInventory()
    {
        InventoryPanel.SetActive(false);
        Time.timeScale = 1;
    }

    

    public void setHsCore()
    {
        StartCoroutine(setSCores());
    }


    IEnumerator setSCores()
    {
        OpenGameOverPanel();
        PlayfabController.PFC.GetStats();
        oldScore = PlayfabController.PFC.hScore;

        if (Ball.GetComponent<Ball>().traveledDistance >= PlayfabController.PFC.hScore)
        {
            yield return new WaitForSeconds(0.5f);
            highScore = Ball.GetComponent<Ball>().traveledDistance;
            PlayfabController.PFC.hScore = highScore;
        }
        else
        {
            oldScore = PlayfabController.PFC.hScore;
        }
        PlayfabController.PFC.SetStats();

    }

    public void reloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        CoinText.text = "C :" + PlayfabController.PFC.coin.ToString();
        ScoreText.text = Ball.GetComponent<Ball>().traveledDistance.ToString() + "m";
        HScoreText.text = PlayfabController.PFC.hScore.ToString() +"m";
        OldHcoreText.text = oldScore.ToString() + "m";
        yScoreText.text = Ball.GetComponent<Ball>().traveledDistance.ToString() + "m";
    }



}
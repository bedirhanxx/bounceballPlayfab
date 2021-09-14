using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class PlayfabController : MonoBehaviour
{
    public static PlayfabController PFC;
    public GetPlayerCombinedInfoRequestParams info;
    public string android_id;
    private string myID;
    public int coin = 0;
    public int hScore;
    public int oldhScore;
    public int sizeX;
    public int sizeY;
    public int rbMass;
    public int redC;
    public int blueC;
    public int greenC;

    public Item[] Items;
    public GameObject[] EnableOnLogin;
    public GameObject ContentArea;
    public GameObject ButtonObj;
    public GameObject InventoryContent;
    public GameObject LoadScreen;
    public int loadStage;
    [SerializeField]private List<GameObject> shopObjects = new List<GameObject>();
    [SerializeField]private List<GameObject> inventoryOjs = new List<GameObject>();

    public void OnEnable()
    {
        if (PFC == null)
        {
            PFC = this;
        }
        /*else
        {
            if (PFC != null)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);*/
    }

    public void Start()
    {
        LoginWithDeviceID(true);

    }
    #region Login

    private void LoginWithDeviceID(bool createAccount)
    {
        #if UNITY_ANDROID && UNITY_EDITOR
                android_id = SystemInfo.deviceUniqueIdentifier;
                Debug.Log("LoginWithAndroidDeviceID, UNITY_EDITOR");
        #endif
        #if UNITY_ANDROID
        PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
                {
                    TitleId = PlayFabSettings.TitleId,
                    AndroidDevice = SystemInfo.deviceModel,
                    OS = SystemInfo.operatingSystem,
                    AndroidDeviceId = android_id,
                    CreateAccount = createAccount,

                }, OnLoginWithDeviceIDSuccess, OnPlayFabError);
        #endif
    }

    private void OnLoginWithDeviceIDSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        GetStats();
        myID = result.PlayFabId;
        loadStage++;

        getLoginResult();
        UpdateInventory();
        GetItemPrices();
        
        
    }

    void getLoginResult()
    {
        LoginWithAndroidDeviceIDRequest loginRequest = new LoginWithAndroidDeviceIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            AndroidDevice = SystemInfo.deviceModel,
            OS = SystemInfo.operatingSystem,
            AndroidDeviceId = android_id,
        };
        loginRequest.InfoRequestParameters = info;
        PlayFabClientAPI.LoginWithAndroidDeviceID(loginRequest, result => {
            coin = result.InfoResultPayload.UserVirtualCurrency["CD"];
        }, error => {
            Debug.Log(error.ErrorMessage);
        });
    }

    [Button("GetGold")]
    public void GetCoin()
    {
        PurchaseItemRequest request = new PurchaseItemRequest();
        request.CatalogVersion = "Items";
        request.ItemId = "Coin";
        request.VirtualCurrency = "CD";
        request.Price = 0;

        PlayFabClientAPI.PurchaseItem(request, result => {
            coin += 10;
        }, error => {
            Debug.Log(error.ErrorMessage);
        });
    }

    [Button("Get Item Prices")]
    public void GetItemPrices()
    {
        GetCatalogItemsRequest request = new GetCatalogItemsRequest();
        request.CatalogVersion = "Items";
        PlayFabClientAPI.GetCatalogItems(request, result => {
            List<CatalogItem> items = result.Catalog;


            foreach (CatalogItem i in items)
            {
                uint cost = i.VirtualCurrencyPrices["CD"];
                foreach (Item editorItems in Items)
                {
                    if (editorItems.Name == i.ItemId)
                    {
                        editorItems.Cost = (int)cost;
                    }
                }

                Debug.Log(cost);
            }
            foreach (Item i in Items)
            {
                GameObject o = Instantiate(ButtonObj, ContentArea.transform.position, Quaternion.identity);
                o.name = i.Name;


                o.transform.SetParent(ContentArea.transform);
                o.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = i.Name + "[" + i.Cost + "]";

                


                o.GetComponent<Button>().onClick.AddListener(delegate {
                    MakePuchase(i.Name, i.Cost,i.gameObject);
                    
                });
                shopObjects.Add(o);
                CheckPursched(o.gameObject);
            }


            loadStage++;
        }, error => {

        });
        
    }
    [Button("Get Invetory Items")]
    void UpdateInventory()
    {
        GetUserInventoryRequest request = new GetUserInventoryRequest();

        PlayFabClientAPI.GetUserInventory(request, result => {

            if (inventoryOjs != null)
            {
                foreach (GameObject obj in inventoryOjs)
                {
                    Destroy(obj);
                }
                inventoryOjs.Clear();
            }
            List<ItemInstance> ii = result.Inventory;
            foreach (ItemInstance i in ii)
            {
                foreach (Item editorI in Items)
                {
                    if (editorI.Name == i.ItemId)
                    {
                        GameObject o = Instantiate(ButtonObj, InventoryContent.transform.position, Quaternion.identity);
                        o.name = editorI.Name;
                        o.transform.SetParent(InventoryContent.transform);
                        o.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = i.ItemId;
                        for (int inv = 0; inv < inventoryOjs.Count; inv++)
                        {
                            if (o.name == inventoryOjs[inv].name)
                            {
                                Destroy(o);
                                Debug.Log("Dupe item found");
                                break;
                            }
                        }
                        
                        inventoryOjs.Add(o);
                        CheckPursched(o.gameObject);
                        o.AddComponent<BallProterties>();
                        o.GetComponent<Button>().onClick.AddListener(delegate { o.GetComponent<BallProterties>().setStatsForBall(); });
                    }
                }
            }
        }, error => { });
        
    }


    void CheckPursched(GameObject o)
    {
        if (inventoryOjs.Contains(o))
        {
            Debug.Log("found");
            for (int inv = 0; inv < shopObjects.Count; inv++)
            {
                if (o.name == shopObjects[inv].name)
                {
                    shopObjects[inv].gameObject.GetComponent<Button>().interactable = false;
                    Debug.Log("Dupe item found");
                    break;
                }
            }
        }
    }


    void MakePuchase(string name, int price , GameObject selected)
    {
        PurchaseItemRequest request = new PurchaseItemRequest();
        request.CatalogVersion = "Items";
        request.ItemId = name;
        request.VirtualCurrency = "CD";
        request.Price = price;
        PlayFabClientAPI.PurchaseItem(request, result => {
            UpdateInventory();
            coin -= price;
            selected.GetComponent<Button>().interactable = false;
        }, error => {
            StartCoroutine(showErrorText());
            MenuController.MC.ErrorText.GetComponent<TMPro.TextMeshProUGUI>().text = error.ErrorMessage;
            Debug.Log(error.ErrorMessage);
        });
    }

    IEnumerator showErrorText()
    {
        MenuController.MC.ErrorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        MenuController.MC.ErrorText.gameObject.SetActive(false);
    }

    private void OnPlayFabError(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    #endregion Login

    #region PlayerStats

    public void SetStats()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.

            Statistics = new List<StatisticUpdate> {
               new StatisticUpdate {
                  StatisticName = "hScore", Value = hScore
               },
               new StatisticUpdate {
                  StatisticName = "oldhScore", Value = oldhScore
               },
               new StatisticUpdate {
                  StatisticName = "sizeX", Value = sizeX
               },
               new StatisticUpdate {
                  StatisticName = "sizeY", Value = sizeY
               },
               new StatisticUpdate {
                  StatisticName = "rbMass", Value = rbMass
               },
               new StatisticUpdate {
                  StatisticName = "redC", Value = redC
               },
               new StatisticUpdate {
                  StatisticName = "blueC", Value = blueC
               },
               new StatisticUpdate {
                  StatisticName = "greenC", Value = greenC
               },
            }
        },
           result => {
               Debug.Log("User statistics updated");
           },
           error => {
               Debug.LogError(error.GenerateErrorReport());
           }); ;
    }

    public void GetStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
           new GetPlayerStatisticsRequest(),
           OnGetStats,
           error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStats(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        loadStage++;
        foreach (var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);

            switch (eachStat.StatisticName)
            {
                case "hScore":
                    hScore = eachStat.Value;
                    break;
                case "oldhScore":
                    oldhScore = eachStat.Value;
                    break;
                case "sizeX":
                    sizeX = eachStat.Value;
                    break;
                case "sizeY":
                    sizeY = eachStat.Value;
                    break;
                case "rbMass":
                    rbMass = eachStat.Value;
                    break;
                case "redC":
                    redC = eachStat.Value;
                    break;
                case "blueC":
                    blueC = eachStat.Value;
                    break;
                case "greenC":
                    greenC = eachStat.Value;
                    break;
            }
        }
    }

    #endregion PlayerStats
    public bool check;
    private void Update()
    {
        if (loadStage == 3)
        {
            if (!check)
            {
                LoadScreen.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}
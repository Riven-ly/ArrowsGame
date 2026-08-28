using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxPanel : UIBase
{
    public Transform root;
    public Text title;
    public Button hideBtn;
    public Button tipsBtn;
    public Button historyBtn;

    public Text goldText;
    public Text myB;
    public Text levelText;
    public Text gold_DuiHuanText;
    public Text realityGoldText;
    public Text ex1;
    public Text ex2;
    public Text ex3;
    
    public Button collectBtn;
    public Text collectBtnText;
    public CanvasGroup canvasGroup;
    public List<TxPanelCell> cells;

    private string wd;
    private string WD;
    private string Wd;
    private string unit;
    private float limitGold = 0.2f;
    [HideInInspector]public TxPanelCell selectCell;
    public List<PlayerApiClient.WDLRecord> wdlOrders = new List<PlayerApiClient.WDLRecord>();

    private void Awake()
    {
        RectTransform rect = root.GetComponent<RectTransform>();
        float topBlockHeight = Screen.height - Screen.safeArea.yMax;
        rect.offsetMax = new Vector2(0, -topBlockHeight);
  
    }
    void Start()
    {
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });
        tipsBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<TxTipsPanel>();
        });
        historyBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<TxHistoryPanel>();
        });

        collectBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            CollectClickCallback();
        });
    }

    private void CollectClickCallback()
    {
        if (GameManager.Instance.playerInfo.level < selectCell.limitLv)
        {
            UIManager.Instance.OpenUI<TxFailedPanel>(selectCell);
            return;
        }

        if (HasCheckingOrder())
        {
            UIManager.Instance.OpenUI<TxFailedPanel>(3005);
            return;
        }

        UIManager.Instance.OpenUI<TxAccountPanel>(selectCell);
    }

    private void InitStr()
    {
        if(string.IsNullOrEmpty(unit))
        {
            unit = LanguageManager.Instance.GetText_Encrypt("Special_Diamond_unit");
            wd = LanguageManager.Instance.GetText_Encrypt("wd");
            WD = LanguageManager.Instance.GetText_Encrypt("WD");
            Wd = LanguageManager.Instance.GetText_Encrypt("Wd");

            title.text = WD;
            myB.text = string.Format(LanguageManager.Instance.GetText("TxPanel_myB"), LanguageManager.Instance.GetText_Encrypt("Bl"));

            string ex1SF = LanguageManager.Instance.GetText("TxPanel_ex1");
            //string Pm = LanguageManager.Instance.GetText_Encrypt("Pm");
            string wh = LanguageManager.Instance.GetText_Encrypt("wh");
            ex1.text = string.Format(ex1SF, "", wh, $"{unit}{limitGold.ToString("F2")}");
            ex3.text = LanguageManager.Instance.GetText("TxPanel_ex3");
        }

        goldText.text = GameManager.Instance.playerInfo.Gold.ToString("F2");
        levelText.text = $"{LanguageManager.Instance.GetText("TxPanel_levelText")}:{GameManager.Instance.playerInfo.level}";
        gold_DuiHuanText.text = $"100≈{unit}2.50";
        float realityGoldF = TxManager.Instance.GetRealityGold(GameManager.Instance.playerInfo.level);
        realityGoldText.text = $"≈{unit}{realityGoldF.ToString("F2")}";
        collectBtnText.text = Wd;

        if(realityGoldF < limitGold)
        {
            string ex2SF = LanguageManager.Instance.GetText("TxPanel_ex2");
            float ex2V = MathF.Round(limitGold - realityGoldF, 2);
            ex2.text = string.Format(ex2SF, $"{ex2V}");
            ex2.gameObject.SetActive(true);
            collectBtn.interactable = false;
            canvasGroup.alpha = 0.5f;
        }
        else
        {
            ex2.gameObject.SetActive(false);
            collectBtn.interactable = true;
            canvasGroup.alpha = 1f;
        }
        
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        if (data is PlayerApiClient.WDLListData orderData)
        {
            SetWDLOrders(orderData.list);
        }
        GameManager.IsGamePause = true;
        InitStr();

        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].Init(TxManager.Instance.limitLevel[i], this);
        }
        foreach (var cell in cells)
        {
            if(cell.gameObject.activeSelf)
            {
                selectCell = cell;
                cell.SetSelectState(true);
                break;
            }
        }

        SyncOrderStatus();

        string str = PlayerPrefs.GetString("Guide4Panel");
        if (string.IsNullOrEmpty(str))
        {
            UIManager.Instance.OpenUI<Guide4Panel>();
        }
    }

    public override void Hide()
    {
        AddCallback(() =>
        {
            GameManager.IsGamePause = false;
        });
        base.Hide();
    }

    public void SetWDLOrders(PlayerApiClient.WDLRecord[] orders)
    {
        wdlOrders.Clear();
        if (orders != null)
        {
            wdlOrders.AddRange(orders);
        }
    }

    private void SyncOrderStatus()
    {
        foreach (TxPanelCell cell in cells)
        {
            if (cell.info == null || cell.info.type != TxPanelCell.TxPanelCellType.Checking ||
                string.IsNullOrEmpty(cell.info.orderNumber))
            {
                continue;
            }

            foreach (PlayerApiClient.WDLRecord order in wdlOrders)
            {
                if (order.orderNo == cell.info.orderNumber)
                {
                    cell.ApplyOrderStatus(order.status);
                    break;
                }
            }
        }

        InitStr();
    }

    public bool HasCheckingOrder()
    {
        foreach (PlayerApiClient.WDLRecord order in wdlOrders)
        {
            if (order.status == "REVIEWING" || order.status == "PAYING")
            {
                return true;
            }
        }

        return false;
    }
}

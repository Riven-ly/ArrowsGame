using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxPanelCell : MonoBehaviour
{
    public enum TxPanelCellType
    {
        Playing,
        Checking,
        Complete,
    }
    public class TxPanelCellInfo
    {
        public TxPanelCellType type;
        public string orderNumber;
        public int frozenGold;

        public TxPanelCellInfo()
        {
            orderNumber = "";
            type = TxPanelCellType.Playing;
            frozenGold = 0;
        }
    }

    public TxPanelCellInfo info;
    public Button btn;
    public Text levelText;
    public Text multipleText;
    public Text goldText;
    public Transform mask;
    public Transform selectImgTrans;
    [HideInInspector]public int limitLv;

    private TxPanel txPanel;
    private string SaveKey = "TxPanelCellInfo_";
    private string unit;
    private string lvStr;
    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            txPanel.selectCell.SetSelectState(false);
            txPanel.selectCell = this;
            SetSelectState(true);
        });
    }
    public void Init(int _limitLv, TxPanel _txPanel)
    {
        limitLv = _limitLv;
        txPanel = _txPanel;
        if (info == null)
        {
            info = LoadInfo();
        }
        InitStr();

        SetSelectState(false);
        Refresh();
    }
    public void Refresh()
    {
        gameObject.SetActive(info.type == TxPanelCellType.Playing);
        mask.gameObject.SetActive(GameManager.Instance.playerInfo.level < limitLv);
        levelText.text = $"{lvStr} {limitLv}";
        multipleText.text = $"{TxManager.Instance.GetMultiple(limitLv).ToString("F1")}X";
        goldText.text = $"{unit}{TxManager.Instance.GetRealityGold(limitLv).ToString("F2")}";
    }
    private void InitStr()
    {
        if (string.IsNullOrEmpty(unit))
        {
            unit = LanguageManager.Instance.GetText_Encrypt("Special_Diamond_unit");
            lvStr = LanguageManager.Instance.GetText("TxPanel_levelText");
        }
    }

    public void SetSelectState(bool _b)
    {
        selectImgTrans.gameObject.SetActive(_b);
    }

    public void ApplyOrderStatus(string status)
    {
        if (status == "REJECTED" || status == "FAILED")
        {
            GameManager.Instance.playerInfo.Add_gold(info.frozenGold);
            GameManager.Instance.SavePlayerInfo();
            info = new TxPanelCellInfo();
            UIManager.Instance.GetUI<PlayerInfoUI>().Refresh();
        }
        else if (status == "SUCCESS")
        {
            info.type = TxPanelCellType.Complete;
        }
        else if (status == "REVIEWING" || status == "PAYING")
        {
            info.type = TxPanelCellType.Checking;
        }
        else
        {
            return;
        }

        SaveInfo();
        Refresh();
    }
    public TxPanelCellInfo LoadInfo()
    {
        string jsonStr = PlayerPrefs.GetString($"{SaveKey}{limitLv}", "");

        if (string.IsNullOrEmpty(jsonStr))
        {
            TxPanelCellInfo _tinfo = new TxPanelCellInfo();
            return _tinfo;
        }
        return JsonConvert.DeserializeObject<TxPanelCellInfo>(jsonStr);
    }

    public void SaveInfo()
    {
        string jsonStr = JsonConvert.SerializeObject(info, Formatting.Indented);

        Debug.Log($"{SaveKey}{limitLv}:\n" + jsonStr);
        PlayerPrefs.SetString($"{SaveKey}{limitLv}", jsonStr);
        PlayerPrefs.Save();
    }
}

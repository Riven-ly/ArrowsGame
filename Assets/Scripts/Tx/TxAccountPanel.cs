using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxAccountPanel : UIBase
{
    public Button hideBtn;
    public Button clickBtn;
    public CanvasGroup canvasGroup;
    public Text title;
    public Text clickBtnText;

    public InputField accountInput;
    public InputField emailInput;
    public InputField nameInput;
    public InputField phoneInput;

    private string WH;
    private string Wh;
    private TxPanelCell selectCell;
    // Start is called before the first frame update
    void Start()
    {
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });

        clickBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            TxClickCallback();
        });
    }

    private void TxClickCallback()
    {
        string name = nameInput.text.Trim();
        string phone = phoneInput.text.Trim();
        string email = emailInput.text.Trim();
        string account = accountInput.text.Trim();

        if (selectCell == null)
        {
            Debug.LogError("TxAccountPanel: no selected cell.");
            return;
        }

        if (!ValidateAccount(name, phone, email, account, out string errorMessage))
        {
            //Debug.LogError($"TxAccountPanel validation failed: {errorMessage}");
            UIManager.Instance.OpenUI<GeneralTipsPanel>(errorMessage);
            return;
        }

        if (PlayerApiClient.Instance == null)
        {
            Debug.LogError("TxAccountPanel: PlayerApiClient is not available.");
            return;
        }

        UIManager.Instance.OpenUIMask();
        clickBtn.interactable = false;
        canvasGroup.alpha = 0.5f;
        PlayerApiClient.Instance.ApplyWDL(name, phone, email, "VENMO", account, data =>
        {
            UIManager.Instance.HideUIMask();
            Debug.Log($"TxAccountPanel apply WDL returned Succeed: (status:{data.status})");
            CompleteWDL(data.orderNo);
        }, code =>
        {
            UIManager.Instance.HideUIMask();
            Debug.Log($"TxAccountPanel apply WDL failed: (code:{code})");
            UIManager.Instance.OpenUI<TxFailedPanel>(code, () =>
            {
                clickBtn.interactable = true;
                canvasGroup.alpha = 1f;
            });
        }, error =>
        {
            Debug.LogError($"TxAccountPanel apply WDL failed: {error}");
            Debug.Log("Re-fetch the order once and search from the latest order.");
            PlayerApiClient.Instance.GetWDLList(1, 50, orders =>
            {
                TxPanel txPanel = UIManager.Instance.GetUI<TxPanel>();
                if (txPanel != null)
                {
                    txPanel.wdlOrders.Clear();
                    txPanel.wdlOrders.AddRange(orders.list);
                }

                foreach (PlayerApiClient.WDLRecord order in orders.list)
                {
                    if (order.status == "REVIEWING" || order.status == "PAYING")
                    {
                        UIManager.Instance.HideUIMask();
                        Debug.Log("Find the orders in the latest order list that have the status of REVIEWING/PAYING");
                        CompleteWDL(order.orderNo);
                        return;
                    }
                }

                ShowApplyFailed();
            }, listError =>
            {
                Debug.LogError($"TxAccountPanel confirm WDL list failed: {listError}");
                ShowApplyFailed();
            });
        });
    }

    private void CompleteWDL(string orderNo)
    {
        OtherSdkManager.Instance.CustomEvent("cashout_level", "completeBtn", selectCell.transform.GetSiblingIndex() + 1);

        selectCell.info.type = TxPanelCell.TxPanelCellType.Checking;
        selectCell.info.orderNumber = orderNo;
        selectCell.info.frozenGold = GameManager.Instance.playerInfo.FrozenGold();
        GameManager.Instance.SavePlayerInfo();
        selectCell.SaveInfo();
        selectCell.Refresh();
        UIManager.Instance.OpenUI<TxSucceedPanel>(null, () =>
        {
            UIManager.Instance.GetUI<PlayerInfoUI>().Refresh();
            UIManager.Instance.GetUI<TxPanel>().Refresh();
            Hide();
        });
    }

    private void ShowApplyFailed()
    {
        UIManager.Instance.HideUIMask();
        UIManager.Instance.OpenUI<TxFailedPanel>(null, () =>
        {
            clickBtn.interactable = true;
            canvasGroup.alpha = 1f;
        });
    }

    private bool ValidateAccount(string name, string phone, string email, string account, out string errorMessage)
    {
        if (LanguageManager.Instance.type == MultilingualType.Portuguese)
        {
            return BrazilValidator.ValidateAll(name, phone, email, account, out errorMessage);
        }

        if (LanguageManager.Instance.type == MultilingualType.Indonesian)
        {
            return IndonesiaValidator.ValidateAll(name, phone, email, account, out errorMessage);
        }

        return AmericaValidator.ValidateAll(name, phone, email, account, out errorMessage);
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        selectCell = data as TxPanelCell;
        clickBtn.interactable = true;
        canvasGroup.alpha = 1f;
        if (string.IsNullOrEmpty(WH))
        {
            WH = LanguageManager.Instance.GetText_Encrypt("WH");
            Wh = LanguageManager.Instance.GetText_Encrypt("Wh");
        }
        title.text = string.Format(LanguageManager.Instance.GetText("TxAccountPanel_title"), WH);
        clickBtnText.text = Wh;
    }

    public override void Hide()
    {
        AddCallback(() =>
        {
            accountInput.text = string.Empty;
            emailInput.text = string.Empty;
            nameInput.text = string.Empty;
            phoneInput.text = string.Empty;
        });
        base.Hide();
    }
}

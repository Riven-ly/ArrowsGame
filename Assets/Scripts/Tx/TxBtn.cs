using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxBtn : MonoBehaviour, IEventListener
{
    public Button btn;
    public CanvasGroup canvasGroup;
    public Text text;

    private string unit;
    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(GameEvent.GetGold, this);
    }
    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(GameEvent.GetGold, this);
    }
    public void OnEventTriggered(GameEvent eventType, object data = null)
    {
        RefreshUI();
    }

    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUIMask();
            btn.interactable = false;
            canvasGroup.alpha = 0.5f;

            if (!PlayerApiClient.Instance.IsRegistered)
            {
                PlayerApiClient.Instance.Register(data => RequestOrderList(), error => RestoreButton());
                return;
            }

            RequestOrderList();
        });

        RefreshUI();
    }

    private void RequestOrderList()
    {
        PlayerApiClient.Instance.GetWDLList(1, 50, data =>
        {
            RestoreButton();
            UIManager.Instance.OpenUI<TxPanel>(data);
        }, error =>
        {
            RestoreButton();
            Debug.LogError($"TxBtn request WDL list failed: {error}");
        });
    }

    private void RestoreButton()
    {
        btn.interactable = true;
        canvasGroup.alpha = 1f;
        UIManager.Instance.HideUIMask();
    }

    private void RefreshUI()
    {
        if(string.IsNullOrEmpty(unit))
        {
            unit = LanguageManager.Instance.GetText_Encrypt("Special_Diamond_unit");
        }
        float v = TxManager.Instance.GetRealityGold(1);
        text.text = $"≈{unit}{v}";
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxBtn : MonoBehaviour, IEventListener
{
    public Button btn;
    public Text text;

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

        });

        RefreshUI();
    }

    private void RefreshUI()
    {
        float totalNote = 10000f;
        float totalUsd = 2.5f;
        float currentNote = GameManager.Instance.playerInfo.Gold;

        float usdResult = currentNote * (totalUsd / totalNote);

        float rounded = MathF.Round(usdResult, 3);
        //Debug.Log($"原始值:{usdResult}");
        //Debug.Log($"保留两位小数:{rounded}");
        if(rounded < 0.001f)
        {
            rounded = 0.001f;
        }
        if(GameManager.Instance.playerInfo.Gold == 0f)
        {
            rounded = 0f;
        }

        text.text = $"≈{LanguageManager.Instance.GetText_Encrypt("Special_Diamond_unit")}{rounded}";
    }
}

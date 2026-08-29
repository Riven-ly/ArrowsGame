using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TxManager : MonoBehaviour
{
    public static TxManager Instance;

    private string baseURL = "http://129.227.153.67:3491";
    private string appKey = "31089997";

    public List<int> limitLevel = new List<int>() { 15, 50, 120, 250, 500, 1000 };
    private List<float> multiples = new List<float>() { 1f, 1.2f, 1.4f, 1.6f, 1.8f, 2.0f };
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    public float GetMultiple(int lv)
    {
        for (int i = limitLevel.Count - 1; i >= 0; i--)
        {
            int levelThreshold = limitLevel[i];
            float mul = multiples[i];

            if (lv >= levelThreshold)
            {
                return mul;
            }
        }
        return 1f;
    }
    public float GetRealityGold(int lv)
    {
        float multiple = GetMultiple(lv);

        float currentNote = GameManager.Instance.playerInfo.Gold;

        float usdResult = AdManager.Instance.ConvertGoldToLocalCurrency(currentNote) * multiple;

        float rounded = MathF.Round(usdResult, 2);
        //Debug.Log($"原始值:{usdResult}");
        //Debug.Log($"保留两位小数:{rounded}");
        if (rounded < 0.01f)
        {
            rounded = 0.01f;
        }
        if (GameManager.Instance.playerInfo.Gold == 0f)
        {
            rounded = 0f;
        }

        return rounded;
    }
}

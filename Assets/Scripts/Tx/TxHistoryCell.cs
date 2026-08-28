using System;
using UnityEngine;
using UnityEngine.UI;

public class TxHistoryCell : MonoBehaviour
{
    public Text name_c;
    public Text time;
    public Text gold;
    public Text state;

    private string unit;
    public void Init(PlayerApiClient.WDLRecord order)
    {
        if (string.IsNullOrEmpty(unit))
        {
            unit = LanguageManager.Instance.GetText_Encrypt("Special_Diamond_unit");
        }
        name_c.text = order.orderNo;
        DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(order.createdAt).ToLocalTime().DateTime;
        string month = LanguageManager.Instance.GetText($"Month_{date.Month}");
        time.text = $"{month} {date.Day:00}.{date.Year}";
        gold.text = $"{unit}{order.applyAmount:F2}";

        string stateText = GetStateText(order.status);
        state.text = $"<color={GetStateColor(order.status)}>{stateText}</color>";
    }

    private string GetStateText(string status)
    {
        string key = "WDLStatus_" + status;
        string text = LanguageManager.Instance.GetText(key);
        return string.IsNullOrEmpty(text) ? status : text;
    }

    private string GetStateColor(string status)
    {
        switch (status)
        {
            case "REVIEWING":
                return "#C97837";
            case "PAYING":
                return "#FF8929";
            case "SUCCESS":
                return "#2FFF00";
            case "REJECTED":
            case "FAILED":
                return "#FF3500";
            default:
                return "#666666";
        }
    }
}

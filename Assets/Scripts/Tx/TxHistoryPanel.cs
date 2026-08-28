using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxHistoryPanel : UIBase
{
    public Button hidebtn;
    public Transform cellRoot;
    public TxHistoryCell cellTemplate;

    private readonly List<TxHistoryCell> historyCells = new List<TxHistoryCell>();

    private void Start()
    {
        hidebtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        ClearCells();
        TxPanel txPanel = UIManager.Instance.GetUI<TxPanel>();
        if (txPanel == null || txPanel.wdlOrders == null || cellTemplate == null)
        {
            return;
        }

        cellTemplate.gameObject.SetActive(false);
        foreach (PlayerApiClient.WDLRecord order in txPanel.wdlOrders)
        {
            TxHistoryCell cell = Instantiate(cellTemplate, cellRoot);
            cell.gameObject.SetActive(true);
            cell.Init(order);
            historyCells.Add(cell);
        }
    }

    private void ClearCells()
    {
        for (int i = 0; i < historyCells.Count; i++)
        {
            Destroy(historyCells[i].gameObject);
        }
        historyCells.Clear();
    }

    public override void Hide()
    {
        AddCallback(() =>
        {
            ClearCells();
        });
        base.Hide();
    }
}

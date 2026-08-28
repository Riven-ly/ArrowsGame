using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide3Panel : UIBase
{
    public Transform shouzhi;
    public Button btn;
    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        GameObject obj = UIManager.Instance.GetUI<PlayerInfoUI>().txTrans.gameObject;
        TxBtn txBtn = obj.transform.GetChild(0).GetComponent<TxBtn>();
        if(txBtn == null)
        {
            Hide();
        }

        Vector3 targetV = txBtn.btn.transform.position;
        shouzhi.transform.position = targetV;
        btn.transform.position = targetV;

        UIManager.Instance.GetUI<PlayerInfoUI>().GoldCanvasTop();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            UIManager.Instance.GetUI<PlayerInfoUI>().GoldCanvasRecover();
            txBtn.btn.onClick.Invoke();
            PlayerPrefs.SetString("Guide3Panel", "yes");
            Hide();
        });
        GameManager.IsGamePause = true;
    }

    public override void Hide()
    {
        AddCallback(() =>
        {
            GameManager.IsGamePause = false;
        });
        base.Hide();
    }

   
}

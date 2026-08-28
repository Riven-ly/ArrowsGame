using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide4Panel : UIBase
{
    public Transform shouzhi;
    public Button btn;
    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        if (GameManager.appATTtype == 0)
        {
            Hide();
        }

        Vector3 targetV = UIManager.Instance.GetUI<TxPanel>().collectBtn.transform.position;
        shouzhi.transform.position = targetV;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("Guide4Panel", "yes");
            Hide();
        });
     
    }

    public override void Hide()
    {
        base.Hide();
    }

}

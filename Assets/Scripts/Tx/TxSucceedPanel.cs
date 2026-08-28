using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxSucceedPanel : UIBase
{
    public Button hideBtn;
    public Text ex;
    public Text title;

    private string wh;
    private string WH;
    // Start is called before the first frame update
    void Start()
    {
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        if (string.IsNullOrEmpty(wh))
        {
            wh = LanguageManager.Instance.GetText_Encrypt("wh");
            WH = LanguageManager.Instance.GetText_Encrypt("WH");
        }
        title.text = string.Format(LanguageManager.Instance.GetText("TxSucceedPanel_title"), WH);
        string str = LanguageManager.Instance.GetText("TxSucceedPanel_ex");
        ex.text = string.Format(str, wh);
    }

    public override void Hide()
    {
        base.Hide();
    }
}

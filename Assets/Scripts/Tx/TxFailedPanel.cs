using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxFailedPanel : UIBase
{
    public Button hideBtn;
    public Text ex;

    private string wh;
    private string Wh;

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
        TxPanelCell cell = data as TxPanelCell;
        int? code = data is int value ? value : (int?)null;

        if (string.IsNullOrEmpty(wh))
        {
            wh = LanguageManager.Instance.GetText_Encrypt("wh");
            Wh = LanguageManager.Instance.GetText_Encrypt("Wh");
        }
        if (cell != null)
        {
            int diffV = cell.limitLv - GameManager.Instance.playerInfo.level;
            string str = LanguageManager.Instance.GetText("TxFailedPanel_ex");
            ex.text = string.Format(str, wh, cell.limitLv, diffV);
        }
        else if (code.HasValue && IsKnownCode(code.Value))
        {
            ex.text = LanguageManager.Instance.GetText(code.Value.ToString());
        }
        else
        {
            string str = LanguageManager.Instance.GetText("TxFailedPanel_ex2");
            ex.text = string.Format(str, Wh);
        }

    }

    private bool IsKnownCode(int code)
    {
        return code == 500 || code == 1001 || code == 1002 || code == 2001 ||
            code == 3001 || code == 3002 || code == 3003 || code == 3004 || code == 3005;
    }

    public override void Hide()
    {
        base.Hide();
    }
}

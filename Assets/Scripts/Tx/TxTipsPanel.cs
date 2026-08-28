using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxTipsPanel : UIBase
{
    public Transform EnglishTrans;
    public Transform PortugueseTrans;
    public Transform IndonesianTrans;

    public Button hideBtn;
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

        EnglishTrans.gameObject.SetActive(LanguageManager.Instance.type != MultilingualType.Indonesian && LanguageManager.Instance.type != MultilingualType.Portuguese);
        PortugueseTrans.gameObject.SetActive(LanguageManager.Instance.type == MultilingualType.Portuguese);
        IndonesianTrans.gameObject.SetActive(LanguageManager.Instance.type == MultilingualType.Indonesian );

    }
    public override void Hide()
    {
        base.Hide();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluationGamePanel : UIBase
{
    public Button btn;
    public Button hidebtn;

    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;
    public Button btn5;

    public CanvasGroup c1;
    public CanvasGroup c2;
    public CanvasGroup c3;
    public CanvasGroup c4;
    public CanvasGroup c5;


    private int target;
    // Start is called before the first frame update
    void Start()
    {
        hidebtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });
        btn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            PlayerPrefs.SetInt("EvaluationGameStar", target);
            PingJiaTiaoZhuan();
            Hide();
        });

        btn1.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            SetBtn(1);
        });
        btn2.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            SetBtn(2);
        });
        btn3.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            SetBtn(3);
        });
        btn4.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            SetBtn(4);
        });
        btn5.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            SetBtn(5);
        });
    }

    public void SetBtn(int index)
    {
        c1.alpha = index >= 1 ? 1f : 0f;
        c2.alpha = index >= 2 ? 1f : 0f;
        c3.alpha = index >= 3 ? 1f : 0f;
        c4.alpha = index >= 4 ? 1f : 0f;
        c5.alpha = index >= 5 ? 1f : 0f;
        target = index;
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        GameManager.IsGamePause = true;
        SetBtn(4);
    }
    public override void Hide()
    {
        AddCallback(() =>
        {
            GameManager.IsGamePause = false;
            PlayerPrefs.SetString("EvaluationGame", "yes");
        });
        base.Hide();
    }

    private void PingJiaTiaoZhuan()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
#elif UNITY_IOS
        return;
        string reviewUrl = $"itms-apps://itunes.apple.com/app/id{"IOSAppId"}?action=write-review";
        Application.OpenURL(reviewUrl);
#endif
    }
}

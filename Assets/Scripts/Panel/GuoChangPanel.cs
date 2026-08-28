using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuoChangPanel : UIBase
{
    public Transform mask;
    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        DOTween.Sequence().AppendInterval(20f / 60f).OnComplete(() =>
        {
            Hide();
        });
    }
    public override void Hide()
    {
        if (panelAnim == null)
        {
            gameObject.SetActive(false);
            callback?.Invoke();
            callback = null;
        }
        else
        {
            UIManager.Instance.OpenUIMask();
            panelAnim.Play("GuoChangHide");
            callback?.Invoke();
            callback = null;
            DOTween.Sequence().AppendInterval(20f / 60f).OnComplete(() =>
            {
                UIManager.Instance.HideUIMask();
                gameObject.SetActive(false);
            });
        }
    }
}

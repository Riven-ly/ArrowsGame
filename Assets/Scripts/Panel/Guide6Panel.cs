using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide6Panel : UIBase
{
    public Transform shouzhi;
    public Button btn;
    public Transform extrans;
    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        GameObject obj = UIManager.Instance.GetUI<GameScenePanel>().snakeGameController.GetBlackHoleGameObject(0);
        if(obj == null)
        {
            Hide();
        }

        Vector3 targetV = obj.transform.position;
        shouzhi.transform.position = targetV;

        targetV = shouzhi.transform.localPosition;
        extrans.transform.localPosition = new Vector3(0f, targetV.y - 350f, 0f);

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("Guide6Panel", "yes");
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

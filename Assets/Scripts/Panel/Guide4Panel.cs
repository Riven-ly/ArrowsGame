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

        GameObject obj = UIManager.Instance.GetUI<GameScenePanel>().snakeGameController.GetSnakeGameObject(2);
        if(obj == null)
        {
            Hide();
        }

        Vector3 targetV = Vector3.zero;
        shouzhi.transform.position = targetV;
        btn.transform.position = targetV;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            SnakeHeadView head = obj.GetComponentInChildren<SnakeHeadView>();
            head.GetComponent<Button>().onClick.Invoke();
            PlayerPrefs.SetString("Guide4Panel", "yes");
            Hide();
        });
     
    }

    public override void Hide()
    {
        base.Hide();
    }

}

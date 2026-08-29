using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide1Panel : UIBase
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

        Vector3 targetV = GetSnakeCenter(obj);
        shouzhi.transform.position = targetV;
        btn.transform.position = targetV;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            SnakeHeadView head = obj.GetComponentInChildren<SnakeHeadView>();
            head.GetComponent<Button>().onClick.Invoke();
            PlayerPrefs.SetString("Guide1Panel", "yes");
            OtherSdkManager.Instance.CustomEvent("game_guide1");
            Hide();
        });
     
    }

    public override void Hide()
    {
        base.Hide();
    }

    private Vector3 GetSnakeCenter(GameObject snakeObject)
    {
        RectTransform[] parts = snakeObject.GetComponentsInChildren<RectTransform>();

        Bounds bounds = new Bounds();
        bool initialized = false;

        foreach (RectTransform part in parts)
        {
            if (part == snakeObject.transform)
            {
                continue;
            }

            Vector3[] corners = new Vector3[4];
            part.GetWorldCorners(corners);

            foreach (Vector3 corner in corners)
            {
                if (!initialized)
                {
                    bounds = new Bounds(corner, Vector3.zero);
                    initialized = true;
                }
                else
                {
                    bounds.Encapsulate(corner);
                }
            }
        }

        return bounds.center;
    }
}

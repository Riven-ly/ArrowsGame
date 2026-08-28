using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide2Panel : UIBase
{
    public Transform itemRoot;
    public Button collectBtn;

    private List<ItemData> itemDatas;
    private List<ItemBase> itemBase;
    private void Start()
    {
        collectBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            CollectClick();
        });
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        if (itemDatas == null)
        {
            itemDatas = new List<ItemData>();
            itemDatas.Add(new ItemData(ItemType.GoldDui, 40));
        }
        itemBase = GameManager.Instance.CreatItems(itemDatas, itemRoot);
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void CollectClick()
    {
        PlayerPrefs.SetString("Guide2Panel", "yes");
        AddCallback(() =>
        {
            UIManager.Instance.OpenUI<GeneralRewardPanel2>(itemDatas, () =>
            {
                UIManager.Instance.GetUI<GameScenePanel>().ResetGame();
            });
        });
        Hide();
    }
}

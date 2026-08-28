using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWinPanel : UIBase
{
    public Transform root;
    public Transform itemRoot;
    public RewardAdButton rewardAdButton;
    public Button collectBtn;
    public Transform collectBtnTrans;
    public Text lvText;

    public OtherRewardTaskView otherRewardTaskView;

    private List<ItemData> itemDatas;
    private List<ItemBase> itemBase;

    private string page_id = "GameWinPanel";
    private void Awake()
    {
        RectTransform rect = root.GetComponent<RectTransform>();
        float topBlockHeight = Screen.height - Screen.safeArea.yMax;
        rect.offsetMax = new Vector2(0, -topBlockHeight);
    }
    private void Start()
    {
        collectBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            CollectClick();
        });
    }
    private void OnEnable()
    {
        isOpen = true;
    }
    private void OnDisable()
    {
        isOpen = false;
        ResetPanel();
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        AudioManager.Instance.PlaySceneSingleMusic("LevelComplete");
        lvText.text = $"{LanguageManager.Instance.GetText("Level")} {GameManager.Instance.playerInfo.level}";
        if (itemDatas == null)
        {
            itemDatas = new List<ItemData>();
            itemDatas.Add(new ItemData(ItemType.GoldDui, Random.Range(200, 401)));
        }

        GameManager.IsGamePause = true;
        itemBase = GameManager.Instance.CreatItems(itemDatas, itemRoot);
        itemBase[0].cntText.gameObject.SetActive(false);
        bool _isContainGold = false;
        foreach (var itemdata in itemDatas)
        {
            if (itemdata.itemType == ItemType.Gold || itemdata.itemType == ItemType.Diamond)
            {
                _isContainGold = true;
                break;
            }
        }
        rewardAdButton.Init(AdsCallback, page_id, _isContainGold);
        GameManager.Instance.GeneralBtnAnim(collectBtnTrans);

        otherRewardTaskView.Refresh();
    }

    private void AdsCallback()
    {
        AddCallback(() =>
        {
            UIManager.Instance.OpenUI<GeneralRewardPanel>(itemDatas, () =>
            {
                PlayGuide();
            });
        });
        Hide();
    }

    private void CollectClick()
    {
        AddCallback(() =>
        {
            PlayGuide();
        });
        Hide();
    }

    private void PlayGuide()
    {
        GameManager.IsGamePause = false;
        string str = PlayerPrefs.GetString("Guide2Panel");
        if (string.IsNullOrEmpty(str))
        {
            UIManager.Instance.OpenUI<Guide2Panel>();
        }
        else
        {
            UIManager.Instance.GetUI<GameScenePanel>().ResetGame();
        }
    }

    public override void Hide()
    {
        AddCallback(() =>
        {
            GameManager.Instance.playerInfo.level++;
            GameManager.Instance.SavePlayerInfo();
        });
 
        base.Hide();
    }

    private void ResetPanel()
    {
        foreach (Transform item in itemRoot)
        {
            Destroy(item.gameObject);
        }
        itemBase = null;
    }
}

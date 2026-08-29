using Newtonsoft.Json;
using UnityEngine;

public class OtherRewardTask : MonoBehaviour, IEventListener
{
    public static OtherRewardTask Instance;

    private const string SaveKey = "OtherRewardTaskInfo";
    public const int BonusTaskMaxCount = 50;
    public const int RewardTaskMaxCount = 50;
    [SerializeField] private OtherRewardTaskInfo taskInfo;
    public System.Action changedEvent;

    public int BonusAdsCount => taskInfo.bonusAdsCount;
    public int RewardAdsCount => taskInfo.rewardAdsCount;

    public float AverageRewardedRevenue => taskInfo.rewardedRevenueCount > 0
        ? taskInfo.rewardedRevenueSum / taskInfo.rewardedRevenueCount
        : 0.005f;

    public void ResetRewardAdsCount()
    {
        taskInfo.rewardAdsCount = 0;
        taskInfo.rewardedRevenueSum = 0f;
        taskInfo.rewardedRevenueCount = 0;
        Save();
    }

    public void RecordRewardedAdRevenue(float revenue)
    {
        taskInfo.rewardedRevenueSum += revenue;
        taskInfo.rewardedRevenueCount++;
        Save();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.RegisterListener(GameEvent.PlayAds, this);
        }
    }

    private void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.UnregisterListener(GameEvent.PlayAds, this);
        }
    }

    private void Start()
    {
        Load();
    }

    public void OnEventTriggered(GameEvent eventType, object data = null)
    {
        if (eventType != GameEvent.PlayAds)
        {
            return;
        }

        float revenue = data is float value ? value : 0.005f;
        if (taskInfo.bonusAdsCount < BonusTaskMaxCount)
        {
            taskInfo.bonusAdsCount++;
        }
        taskInfo.rewardAdsCount++;
        RecordRewardedAdRevenue(revenue);
    }

    private void Load()
    {
        string json = PlayerPrefs.GetString(SaveKey, string.Empty);
        taskInfo = string.IsNullOrEmpty(json) ? new OtherRewardTaskInfo() : JsonConvert.DeserializeObject<OtherRewardTaskInfo>(json);
    }

    private void Save()
    {
        PlayerPrefs.SetString(SaveKey, JsonConvert.SerializeObject(taskInfo));
        PlayerPrefs.Save();
    }
}

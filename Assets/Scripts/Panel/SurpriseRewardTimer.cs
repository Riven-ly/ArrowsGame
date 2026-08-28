using UnityEngine;

/// <summary>惊喜奖励弹出计时器。</summary>
public class SurpriseRewardTimer : MonoBehaviour
{
    [SerializeField] private float firstRewardDelay = 45f;
    [SerializeField] private float nextRewardMinDelay = 15f;
    [SerializeField] private float nextRewardMaxDelay = 30f;
    [SerializeField] private float elapsedTime;
    private float nextRewardTime;
    private bool rewardPending;
    private bool enabledForLevel;

    private void Update()
    {
        if (!enabledForLevel || rewardPending)
        {
            return;
        }
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= nextRewardTime)
        {
            rewardPending = true;
        }
    }

    /// <summary>开始当前关卡的奖励计时。</summary>
    public void StartLevel(int level)
    {
        enabledForLevel = level > 2;
        elapsedTime = 0f;
        nextRewardTime = firstRewardDelay;
        rewardPending = false;
    }

    /// <summary>在蛇成功移动后检查是否需要弹出奖励。</summary>
    public void CheckAfterSnakeMove()
    {
        if (!enabledForLevel || !rewardPending)
        {
            return;
        }
       
        UIManager.Instance.OpenUI<SurpriseRewardPanel>(true, () =>
        {
            elapsedTime = 0f;
            rewardPending = false;
            nextRewardTime = Random.Range(nextRewardMinDelay, nextRewardMaxDelay);
        });
    }

    /// <summary>重置当前关卡的奖励计时。</summary>
    public void ResetLevel()
    {
        elapsedTime = 0f;
        nextRewardTime = firstRewardDelay;
        rewardPending = false;
    }
}

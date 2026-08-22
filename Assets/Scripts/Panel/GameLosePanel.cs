using UnityEngine;
using UnityEngine.UI;

public class GameLosePanel : UIBase
{
    public Button restartButton;
    public RewardAdButton reviveButton;

    private GameScenePanel gameScenePanel;
    private void Start()
    {
        restartButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            RestartGame();
        });
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        gameScenePanel = data as GameScenePanel;

        reviveButton.Init(ReviveGame, "GameLosePanel", false);
    }

    private void RestartGame()
    {
        AddCallback(() =>
        {
            gameScenePanel.ResetGame();
        });
        Hide();
    }

    private void ReviveGame()
    {
        AddCallback(() =>
        {
            gameScenePanel.ReviveGame();
        });
        Hide();
    }
}

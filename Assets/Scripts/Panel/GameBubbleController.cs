using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameBubbleController : MonoBehaviour
{
    [SerializeField] private RectTransform bubbleScope;
    [SerializeField] private RectTransform gameBubble;
    [SerializeField] private Button gameBubbleButton;
    [SerializeField] private CanvasGroup gameBubbleCanvasGroup;
    [SerializeField] private RectTransform point1;
    [SerializeField] private RectTransform point2;
    [SerializeField] private RectTransform point3;
    [SerializeField] private RectTransform point4;

    private Coroutine bubbleCoroutine;
    private Tween bubbleTween;

    private void Start()
    {
        gameBubbleButton.onClick.AddListener(() =>
        {
            OnBubbleClicked();
        });
        gameBubble.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopBubbleLoop();
    }

    public void StartBubbleLoop()
    {
        StopBubbleLoop();
        bubbleCoroutine = StartCoroutine(BubbleLoop(30f));
    }

    private void StopBubbleLoop()
    {
        if (bubbleCoroutine != null)
        {
            StopCoroutine(bubbleCoroutine);
            bubbleCoroutine = null;
        }
        HideBubble();
    }

    private IEnumerator BubbleLoop(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        while (true)
        {
            ShowBubble();
            yield return new WaitForSeconds(60f);
            HideBubble();
            yield return new WaitForSeconds(Random.Range(5f, 15f));
        }
    }

    private void ShowBubble()
    {
        bubbleTween?.Kill();
        gameBubble.localPosition = point1.localPosition;
        gameBubbleCanvasGroup.alpha = 0f;
        gameBubbleCanvasGroup.DOFade(1f, 0.2f);
        gameBubble.gameObject.SetActive(true);
        bubbleTween = DOTween.Sequence()
            .Append(gameBubble.DOLocalMove(point2.localPosition, 4f).SetEase(Ease.Linear))
            .Append(gameBubble.DOLocalMove(point3.localPosition, 5f).SetEase(Ease.Linear))
            .Append(gameBubble.DOLocalMove(point4.localPosition, 4f).SetEase(Ease.Linear))
            .Append(gameBubble.DOLocalMove(point1.localPosition, 5f).SetEase(Ease.Linear))
            .SetLoops(-1, LoopType.Restart)
            .SetTarget(gameBubble);
    }

    private void OnBubbleClicked()
    {
        if (bubbleCoroutine != null)
        {
            StopCoroutine(bubbleCoroutine);
        }
        HideBubble();

        UIManager.Instance.OpenUI<SurpriseRewardPanel>(null, () =>
        {
            bubbleCoroutine = StartCoroutine(BubbleLoop(Random.Range(5f, 15f)));
        });
    }

    private void HideBubble()
    {
        bubbleTween?.Kill();
        bubbleTween = null;
        if (gameBubble != null && gameBubble.gameObject.activeSelf)
        {
            gameBubbleCanvasGroup.DOFade(0f, 0.2f)
                .SetTarget(gameBubble)
                .OnComplete(() => gameBubble.gameObject.SetActive(false));
        }
    }
}

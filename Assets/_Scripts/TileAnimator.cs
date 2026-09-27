using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TileAnimator : MonoBehaviour
{
    //This class will: 
    /*
     * - Tween tiles
     */
    private GridController gridController; 
    [SerializeField] private float growScalar;
    [SerializeField] private float growTweenDuration, moveTweenDuration;
    [SerializeField] private float distance;
    public UnityEvent OnPushTweenComplete;
    private void Awake()
    {
        if(OnPushTweenComplete == null) OnPushTweenComplete = new UnityEvent();
    }

    public float DoPushGraphics(Vector2 direction, Transform t)
    {
        DoPushTween(direction, t);
        return (growTweenDuration + moveTweenDuration) * 2;
    }
    public float DoPushTween(Vector2 direction, Transform t)
    {
        
        Vector3 scale = t.localScale;
        Vector3 pos = t.localPosition;
        SpriteRenderer spriteRenderer = t.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
        Sequence PushSequence = DOTween.Sequence();
        PushSequence.Append(t.DOScale(scale * growScalar, growTweenDuration).SetEase(Ease.OutBounce));
        PushSequence.Append(t.DOLocalMove(pos + (Vector3) (distance * direction), moveTweenDuration).SetEase(Ease.InQuint));
        PushSequence.Append(t.DOLocalMove(pos, moveTweenDuration).SetEase(Ease.OutQuint));
        PushSequence.Append(t.DOScale(scale, growTweenDuration).SetEase(Ease.OutElastic)).OnComplete(
            () => {spriteRenderer.sortingOrder = 0; OnPushTweenComplete.Invoke();
            });

        PushSequence.Play();

        return 2 * (growTweenDuration + moveTweenDuration);
        // Debug.Log("Do it!");
    }

    public float DoPushMultipleTween(Transform pusher, Vector2Int[] directions)
    {
        Sequence AllPushes = DOTween.Sequence();
        for (int i = 0; i < directions.Length; i++)
        {
            AllPushes.Append(DoPushTween(pusher, directions[i]));
        }

        AllPushes.Play();
        return AllPushes.Duration();
    }
    
    public Sequence DoPushTween(Transform t, Vector2 dir)
    {
        
        Vector3 scale = t.localScale;
        Vector3 pos = t.localPosition;
        SpriteRenderer spriteRenderer = t.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
        Sequence PushSequence = DOTween.Sequence();
        PushSequence.Append(t.DOScale(scale * growScalar, growTweenDuration).SetEase(Ease.OutBounce));
        PushSequence.Append(t.DOLocalMove(pos + (Vector3) (distance * dir), moveTweenDuration).SetEase(Ease.InQuint));
        PushSequence.Append(t.DOLocalMove(pos, moveTweenDuration).SetEase(Ease.OutQuint));
        PushSequence.Append(t.DOScale(scale, growTweenDuration).SetEase(Ease.OutElastic)).OnComplete(
            () => {spriteRenderer.sortingOrder = 0; OnPushTweenComplete.Invoke();
            });

        return PushSequence;
        // Debug.Log("Do it!");
    }
}

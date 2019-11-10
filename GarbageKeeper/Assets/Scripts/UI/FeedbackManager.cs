using UnityEngine;
using System.Collections.Generic;

public class FeedbackManager : MonoBehaviour
{
    public GameObject feedbackPrefab;
    private List<Feedback> _feedbacks;

    public class FeedbackInfo
    {
        public Settings.Elements elementType;
        public int amount;
    }

    private float _timeSinceLastPushed = 0;
    private List<FeedbackInfo> _feedbacksQueue;
    private static FeedbackManager _instance = null;
    public static FeedbackManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
        _feedbacks = new List<Feedback>();
        _feedbacksQueue = new List<FeedbackInfo>();
    }

    private void Update()
    {
        if(_feedbacksQueue.Count != 0)
        {
            if(_timeSinceLastPushed >= 0.2f)
            {
                var nextFeedback = _feedbacksQueue[0];
                PushFeedback(nextFeedback.elementType, nextFeedback.amount);
                _feedbacksQueue.RemoveAt(0);
                _timeSinceLastPushed = 0f;
            }
            else
            {
                _timeSinceLastPushed += Time.deltaTime;
            }
        }
    }

    public void OrderPushFeedback(Settings.Elements pElementType, int pAmount)
    {
        _feedbacksQueue.Add(new FeedbackInfo()
        {
            elementType = pElementType,
            amount = pAmount
        });
    }

    private void PushFeedback(Settings.Elements elementType, int amount)
    {
        foreach (var feedback in _feedbacks)
        {
            feedback.OrderMoveUp();
        }
        var newFeedback = Instantiate<GameObject>(feedbackPrefab);
        newFeedback.transform.SetParent(transform);
        newFeedback.transform.localScale = Vector3.one;
        newFeedback.transform.localPosition = Vector3.zero;
        newFeedback.transform.localRotation = Quaternion.identity;

        var feedbackComponent = newFeedback.GetComponent<Feedback>();
        feedbackComponent.Load(elementType, amount);
        _feedbacks.Add(feedbackComponent);
    }

    public void DestroyFeedback(Feedback feedback)
    {
        _feedbacks.Remove(feedback);
        Destroy(feedback.gameObject);
    }

    public void Clear()
    {
        foreach (var feedback in _feedbacks)
        {
            feedback.OrderDestroy();
        }
    }
}

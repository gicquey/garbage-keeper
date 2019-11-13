using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Feedback : MonoBehaviour
{
    public const float TimeToLive = 5f;
    public Transform child;
    public Image icon;
    public Text text;
    public List<Sprite> spritesPerElementType;

    private float _timeSinceCreation;
    private int _moveUpOrdered;
    private bool _destroyOrdered;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _moveUpOrdered = 1;
        _timeSinceCreation = 0f;
        _destroyOrdered = false;
    }

    private void Update()
    {
        _timeSinceCreation += Time.deltaTime;
        if (_timeSinceCreation >= TimeToLive && !_destroyOrdered)
        {
            OrderDestroy();
        }
    }

    public void Load(Settings.Elements elementType, int amount)
    {
        icon.sprite = spritesPerElementType[(int)elementType];
        if(amount > 0)
        {
            text.text = "+" + amount.ToString();
            text.color = Color.green;
        }
        else
        {
            text.text = amount.ToString();
            text.color = Color.red;
        }
    }

    public void OrderMoveUp()
    {
        ++_moveUpOrdered;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Normal"))
        {
            StartMoveUp();
        }
    }

    public void OrderDestroy()
    {
        _destroyOrdered = true;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Normal"))
        {
            StartDestroy();
        }
    }

    private void OnEnterNormal()
    {
        if (_destroyOrdered)
        {
            StartDestroy();
        }
        else if (_moveUpOrdered > 0)
        {
            StartMoveUp();
        }
    }

    private void StartMoveUp()
    {
        --_moveUpOrdered;
        _animator.SetTrigger("MoveUp");
    }

    private void EndMoveUp()
    {
        this.transform.localPosition += child.transform.localPosition;
        child.transform.localPosition = Vector3.zero;
    }

    private void StartDestroy()
    {
        _destroyOrdered = false;
        _animator.SetTrigger("Destroy");
    }

    private void EndDestroy()
    {
        GetComponentInParent<FeedbackManager>().DestroyFeedback(this);
    }
}

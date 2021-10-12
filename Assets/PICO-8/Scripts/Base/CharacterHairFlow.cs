using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHairFlow : MonoBehaviour
{
    public Transform PlayerTrans;   // player
    private int _hairCount = 6;
    [SerializeField]
    private List<Vector3> _hairPositions = new List<Vector3>();    // 6
    [SerializeField]
    private List<SpriteRenderer> _hairRenderers = new List<SpriteRenderer>();    // 6

    private Color _normalRed = new Color(1f, 0f, 77 / 255f);
    private Color _dashBlue = new Color(41 / 255f, 173 / 255f, 1f);

    private void Start()
    {
        AddMovementEvent();
        for (int i = 0; i < _hairCount; i++)
        {
            _hairPositions.Add(PlayerTrans.localPosition);
        }
    }

    private void AddMovementEvent()
    {
        CharacterMovement.DashBeginEvent += DashBeginHandler;
        CharacterMovement.DashEndEvent += DashEndHandler;
        CharacterMovement.LandingEvent += LandingHandler;
    }

    public void ResetPlace()
    {
        _hairPositions.Clear();
        for (int i = 0; i < _hairCount; i++)
        {
            _hairPositions.Add(PlayerTrans.localPosition);
        }
        foreach (var item in _hairRenderers)
        {
            item.transform.localPosition = PlayerTrans.localPosition;
        }
    }

    public void UpdateHairFlow(CharacterMovement movement)
    {
        // 当前帧位置
        Vector2 currentPos = PlayerTrans.localPosition;
        // 更新历史位置列表(每帧更新1次，最多保留n帧之前的位置)
        _hairPositions.RemoveAt(0);
        _hairPositions.Add(currentPos);
        // 依次设置每个头发的位置
        // 每隔deltaIndex，将下一个历史位置设置给下一个头发
        float deltaIndex = (float)_hairCount / _hairRenderers.Count;
        for (int j = 0; j < _hairCount; j++)
        {
            // 1 ~ m_PositionList.Count
            int index = Mathf.CeilToInt((j + 1) * deltaIndex);
            SpriteRenderer renderer = _hairRenderers[_hairRenderers.Count - j - 1];
            // 头发的顺序为从大到小，位置的顺序为从远（旧）到近（新）
            renderer.transform.localScale = new Vector3(-movement.Facing, 1, 1);
            renderer.transform.localPosition = _hairPositions[index - 1];
        }
    }

    private void SetHairColor(Color color)
    {
        foreach (var item in _hairRenderers)
        {
            if (item.color != color) item.color = color;
        }
    }

    private void DashBeginHandler(CharacterMovement movement)
    {
        SetHairColor(_dashBlue);
    }

    private void DashEndHandler(CharacterMovement movement)
    {
        if (movement.OnGround) SetHairColor(_normalRed);
    }

    private void LandingHandler(CharacterMovement movement)
    {
        SetHairColor(_normalRed);
    }
}


using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHairFlow : MonoBehaviour
{
    public Transform TargetTrans;   // player

    private int _positionCount = 6;
    private List<Vector3> _positionList = new List<Vector3>();    // 6
    [SerializeField]
    private List<SpriteRenderer> _hairRenderers = new List<SpriteRenderer>();    // 6

    private Color _normalRed = new Color(1f, 0f, 77 / 255f);
    private Color _dashBlue = new Color(41 / 255f, 173 / 255f, 1f);

    private void Start()
    {
        AddMovementEvent();
        for (int i = 0; i < _positionCount; i++)
        {
            _positionList.Add(TargetTrans.localPosition);
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
        _positionList.Clear();
        for (int i = 0; i < _positionCount; i++)
        {
            _positionList.Add(TargetTrans.localPosition);
        }
        foreach (var item in _hairRenderers)
        {
            item.transform.localPosition = TargetTrans.localPosition;
        }
    }

    public void SetXScale(int x)
    {
        foreach (var renderer in _hairRenderers)
        {
            renderer.transform.localScale = new Vector3(x, 1, 1);
        }
    }

    public void UpdateHairFlow(CharacterMovement movement)
    {
        // 调整HairFlow的朝向
        SetXScale(movement.Facing);
        // 当前帧位置
        Vector2 currentPos = TargetTrans.localPosition;
        // 更新历史位置列表(每帧更新1次，最多保留n帧之前的位置)
        _positionList.RemoveAt(0);
        _positionList.Add(currentPos);
        // 依次设置每个头发的位置
        // 每隔deltaIndex，将下一个历史位置设置给下一个头发
        float deltaIndex = (float)_positionCount / _hairRenderers.Count;
        for (int j = 0; j < _hairRenderers.Count; j++)
        {
            // 0 ~ m_PositionList.Count
            int index = Mathf.CeilToInt((j + 1) * deltaIndex) - 1;
            Console.LogFormat("index {0}", index);
            // 头发的顺序为从大到小，位置的顺序为从远（旧）到近（新）
            _hairRenderers[_hairRenderers.Count - j - 1].transform.localPosition = _positionList[index];
        }
        if (movement.Speed == Vector2.zero)
        {
            ResetPlace();
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


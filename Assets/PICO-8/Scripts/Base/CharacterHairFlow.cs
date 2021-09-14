using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHairFlow : MonoBehaviour
{
    public Transform TargetTrans;
    public Transform[] HairTrans;
    private List<Vector3> _positionList;
    private SpriteRenderer[] _hairRenderers;

    private Color _normalRed = new Color(1f, 0f, 77 / 255f);
    private Color _dashBlue = new Color(41 / 255f, 173 / 255f, 1f);

    private void Start()
    {
        _positionList = new List<Vector3>();
        for (int i = 0; i < 5; i++)
            _positionList.Add(TargetTrans.localPosition);
        _hairRenderers = GetComponentsInChildren<SpriteRenderer>();   //包含父Renderer
        AddMovementEvent();
    }

    private void AddMovementEvent()
    {
        CharacterMovement.DashBeginEvent += DashBeginHandler;
        CharacterMovement.DashEndEvent += DashEndHandler;
        CharacterMovement.LandingEvent += LandingHandler;
    }

    public void UpdateHairFlow(CharacterMovement movement)
    {
        transform.localScale = new Vector3(movement.Facing, 1, 1);
        var currentPosition = TargetTrans.localPosition;
        // 更新历史位置列表(每帧更新1次，最多保留n帧之前的位置)
        _positionList.RemoveAt(0);
        _positionList.Add(currentPosition);
        var deltaDistance = (float)_positionList.Count / HairTrans.Length;
        for (int i = 0; i < HairTrans.Length; i++)
        {
            // 0 ~ m_PositionList.Count
            int index = Mathf.CeilToInt((i + 1) * deltaDistance) - 1;
            // 头发的顺序为从大到小，位置的顺序为从远（旧）到近（新）
            HairTrans[HairTrans.Length - i - 1].localPosition = _positionList[index];
        }
    }

    public void ResetPlace()
    {
        foreach (var item in HairTrans)
        {
            item.localPosition = TargetTrans.localPosition;
        }

        _positionList.Clear();
        for (int i = 0; i < 5; i++)
            _positionList.Add(transform.localPosition);
    }

    public void SetHairFlow(Vector2 speed)
    {

    }

    private void SetHairColor(Color color)
    {
        foreach (var item in _hairRenderers)
        {
            if (item.color != color) item.color = color;
        }
    }

    private void DashBeginHandler(CharacterMovement obj)
    {
        SetHairColor(_dashBlue);
    }

    private void DashEndHandler(CharacterMovement obj)
    {
        if (obj.OnGround) SetHairColor(_normalRed);
    }

    private void LandingHandler(CharacterMovement obj)
    {
        SetHairColor(_normalRed);
    }
}


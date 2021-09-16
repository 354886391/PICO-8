using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHairFlow : MonoBehaviour
{
    public Transform TargetTrans;   // player
    private int _positionCount = 6;
    private int _hairRendererCount = 6;
    private List<Vector3> _positionList;    // 6
    [SerializeField]
    private SpriteRenderer[] _hairRenderers;    // 7

    private Color _normalRed = new Color(1f, 0f, 77 / 255f);
    private Color _dashBlue = new Color(41 / 255f, 173 / 255f, 1f);

    private void Start()
    {
        _positionList = new List<Vector3>();
        for (int i = 0; i < _positionCount; i++)
            _positionList.Add(TargetTrans.localPosition);
        //_hairRenderers = GetComponentsInChildren<SpriteRenderer>();   //包含父Renderer
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
        _positionList.RemoveAt(0);
        var targetPosition = TargetTrans.localPosition;
        _positionList.Add(targetPosition);
        for (int i = 0; i < _hairRendererCount; i++)
        {
            _hairRenderers[i].transform.localScale = new Vector3(-movement.Facing, 1, 1);
            _hairRenderers[i].transform.localPosition = _positionList[_positionCount - 1 - i] + new Vector3(-movement.Facing * 0.5f, -0.125f, -0.1f);
        }
        if (movement.Speed == Vector2.zero)
        {
            ResetPlace();
        }
    }

    public void ResetPlace()
    {
        foreach (var item in _hairRenderers)
        {
            item.transform.localPosition = TargetTrans.localPosition;
        }
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


#define NO_ENABLE_DEBUG
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
        var facing = movement.Facing;
        Vector3 offset = new Vector3(facing * 0.25f, 0, 0);
        Vector2 currentPos = PlayerTrans.localPosition - offset;
        _hairPositions.RemoveAt(0);
        _hairPositions.Add(currentPos);
        for (int i = 0; i < _hairCount; i++)
        {
            var index = Mathf.CeilToInt((i + 1) * 0.75f);
            var renderer = _hairRenderers[_hairCount - i];
            renderer.transform.localScale = new Vector3(-facing, 1, 1);
            renderer.transform.localPosition = _hairPositions[index - 1];
        }
    }

    private void SetVisable(bool visable)
    {
        foreach (var item in _hairRenderers)
        {
            item.enabled = visable;
        }
    }

    public void AutoHideHairFlow(float time)
    {
        SetVisable(false);
        Utility.DelayCall(time, () => { SetVisable(true); ResetPlace(); });
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


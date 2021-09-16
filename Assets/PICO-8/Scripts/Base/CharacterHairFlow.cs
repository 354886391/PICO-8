using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHairFlow : MonoBehaviour
{
    public Transform TargetTrans;   // player
    private int _positionCount;
    private int _hairRendererCount;
    private List<Vector3> _positionQueue;    // 5
    private SpriteRenderer[] _hairRenderers;    // 7

    private Color _normalRed = new Color(1f, 0f, 77 / 255f);
    private Color _dashBlue = new Color(41 / 255f, 173 / 255f, 1f);

    private void Start()
    {
        _positionQueue = new List<Vector3>();
        for (int i = 0; i < 5; i++)
            _positionQueue.Add(TargetTrans.localPosition);
        _hairRenderers = GetComponentsInChildren<SpriteRenderer>();   //包含父Renderer
        _positionCount = _positionQueue.Count;
        _hairRendererCount = _hairRenderers.Length;
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
        _positionQueue.RemoveAt(0);
        var targetPosition = TargetTrans.localPosition;
        _positionQueue.Add(targetPosition);
        for (int i = 1; i < _hairRenderers.Length; i++)
        {
            _hairRenderers[i].transform.localPosition = _positionQueue[i];
        }     
    }

    public void ResetPlace()
    {
        foreach (var item in _hairRenderers)
        {
            item.transform.localPosition = TargetTrans.localPosition;
        }
        _positionQueue.Clear();
        for (int i = 0; i < 5; i++)
            _positionQueue.Add(transform.localPosition);
    }

    private void SetHairColor(Color color)
    {
        foreach (var item in _hairRenderers)
        {
            //if (item.color != color) item.color = color;
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


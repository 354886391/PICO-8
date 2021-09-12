using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterHairFlow : MonoBehaviour
{
    public Transform[] HairTrans;
    public Vector3[] HairMaxPositions;
    public Vector3[] HairOriginPosition;
    private SpriteRenderer[] _hairRenderers;


    private Color _normalRed = new Color(1f, 0f, 77 / 255f);
    private Color _dashBlue = new Color(41 / 255f, 173 / 255f, 1f);

    private void Start()
    {
        _hairRenderers = GetComponentsInChildren<SpriteRenderer>();   //包含父Renderer
        AddMovementEvent();
    }

    private void AddMovementEvent()
    {
        CharacterMovement.DashBeginEvent += DashBeginHandler;
        CharacterMovement.LandingEvent += LandingHandler;
    }

    public void ResetHairPlace()
    {
        for (int i = 0; i < HairTrans.Length; i++)
        {
            HairTrans[i].DOLocalMove(HairOriginPosition[i], 0.1f, true);
        }
    }

    public void SetHairFlow(int face, Vector2 speed)
    {
        for (int i = 0; i < HairTrans.Length; i++)
        {
            HairTrans[i].DOLocalMove(HairMaxPositions[i], 0.1f, true);
        }
    }

    private void SetHairColor(Color color)
    {
        foreach (var item in _hairRenderers)
        {
            item.color = color;
        }
    }

    private void LandingHandler(CharacterMovement obj)
    {
        SetHairColor(_normalRed);
    }

    private void DashBeginHandler(CharacterMovement obj)
    {
        SetHairColor(_dashBlue);
    }

    [ContextMenu("InitHairMaxPosition")]
    private void InitHairMaxPosition()
    {
        HairMaxPositions = new Vector3[HairTrans.Length];
        for (int i = 0; i < HairTrans.Length; i++)
        {
            HairMaxPositions[i] = HairTrans[i].localPosition;
        }
    }
}

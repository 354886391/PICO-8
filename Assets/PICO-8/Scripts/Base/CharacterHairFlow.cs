using DG.Tweening;
using UnityEngine;

public class CharacterHairFlow : MonoBehaviour
{
    public Transform[] HairTrans;
    public Vector3[] HairMaxPositions;
    public Vector3[] HairMaxYPositions;
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
        CharacterMovement.DashEndEvent += DashEndHandler;
        CharacterMovement.LandingEvent += LandingHandler;
    }

    public void UpdateHairFlow(CharacterMovement movement)
    {
        if (movement.Speed == Vector2.zero)
        {
            ResetHairPlace();
        }
        else
        {
            SetHairFlow(movement.Speed);
        }
    }

    public void ResetHairPlace()
    {

        for (int i = 0; i < HairTrans.Length; i++)
        {
            HairTrans[i].DOLocalMove(HairOriginPosition[i], 0.1f, false);
        }
    }

    public void SetHairFlow(Vector2 speed)
    {
        for (int i = 0; i < HairTrans.Length; i++)
        {
            Vector2 tempTarget = Vector2.one;
            if (speed.x != 0)
            {
                tempTarget = HairMaxPositions[i];
            }
            if (speed.y != 0)
            {
                tempTarget = HairMaxYPositions[i];
            }
            //HairTrans[i].DOLocalMove(tempTarget, 0.2f);
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

    [ContextMenu("InitHairMaxPosition")]
    private void InitHairMaxXPosition()
    {
        HairMaxPositions = new Vector3[HairTrans.Length];
        for (int i = 0; i < HairTrans.Length; i++)
        {
            HairMaxPositions[i] = HairTrans[i].localPosition;
        }
    }

    [ContextMenu("InitHairMaxYPosition")]
    private void InitHairMaxYPosition()
    {
        HairMaxYPositions = new Vector3[HairTrans.Length];
        for (int i = 0; i < HairTrans.Length; i++)
        {
            HairMaxYPositions[i] = HairTrans[i].localPosition;
        }
    }

    public class HairNode
    {
        public float MaxRange;
        public float MinRange;
        public Vector3 Position;
        public HairNode NextNode;

        public void SetRange(float max, float min)
        {
            MaxRange = max;
            MinRange = min;
        }

        private bool MoveNext(ref HairNode node)
        {
            node = NextNode;
            return NextNode != null;
        }
    }


}


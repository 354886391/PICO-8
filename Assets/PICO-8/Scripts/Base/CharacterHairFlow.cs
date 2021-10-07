using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHairFlow : MonoBehaviour
{
    public Transform TargetTrans;   // player
    private int _positionCount = 6;
    private int _hairRendererCount = 6;
    private List<Vector3> _positionList = new List<Vector3>();    // 6
    [SerializeField]
    private List<SpriteRenderer> _hairRenderers = new List<SpriteRenderer>();    // 6

    private Color _normalRed = new Color(1f, 0f, 77 / 255f);
    private Color _dashBlue = new Color(41 / 255f, 173 / 255f, 1f);

    private void Start()
    {
        for (int i = 0; i < _positionCount; i++)
            _positionList.Add(TargetTrans.localPosition);
        AddMovementEvent();
    }

    private void AddMovementEvent()
    {
        CharacterMovement.DashBeginEvent += DashBeginHandler;
        CharacterMovement.DashEndEvent += DashEndHandler;
        CharacterMovement.LandingEvent += LandingHandler;
    }

    public void ResetPlace()
    {

        foreach (var item in _hairRenderers)
            item.transform.localPosition = TargetTrans.localPosition;
    }

    public void UpdateHairFlow(CharacterMovement movement)
    {
        _positionList.RemoveAt(0);
        _positionList.Add(TargetTrans.localPosition);
        for (int i = 0; i < _hairRendererCount; i++)
        {
            _hairRenderers[i].transform.localScale = new Vector3(-movement.Facing, 1, 1);
            _hairRenderers[i].transform.localPosition = _positionList[_positionCount - 1 - i] + new Vector3(-movement.Facing * 0.5f, -0.125f, -0.1f);
        }
    }

    private int index = 0;
    public IEnumerator UpdateHairFlow2(CharacterMovement movement)
    {
        if (index == 6) index = 0;  // 更新到结尾时重置索引
        _positionList[5 - index] = TargetTrans.localPosition;
        _hairRenderers[index].transform.localScale = new Vector3(-movement.Facing, 1, 1);
        _hairRenderers[index].transform.localPosition = _positionList[index];
        ++index;
        yield return null;

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

public class player_hair
{
    private class node
    {
        public float x;
        public float y;
        public float size;
    }

    private node[] hair = new node[5];

    public player_hair(CharacterMovement obj)
    {
        for (var i = 0; i <= 4; i++)
            hair[i] = new node() { x = obj.Speed.x, y = obj.Speed.y, size = E.max(1, E.min(2, 3 - i)) };
    }

    public void draw_hair(CharacterMovement obj, int facing, int djump)
    {
        var c = (djump == 1 ? 8 : (djump == 2 ? (7 + E.flr((G.frames / 3) % 2) * 4) : 12));
        var last = new Vector2(obj.x + 4 - facing * 2, obj.y + (E.btn(G.k_down) ? 4 : 3));
        foreach (var h in hair)
        {
            h.x += (last.x - h.x) / 1.5f;
            h.y += (last.y + 0.5f - h.y) / 1.5f;
            E.circfill(h.x, h.y, h.size, c);
            last = new Vector2(h.x, h.y);
        }
    }
}

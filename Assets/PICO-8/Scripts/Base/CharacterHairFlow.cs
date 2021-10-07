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
    private HairFlow _hairFlow = new HairFlow();
    [SerializeField]
    private List<SpriteRenderer> _hairRenderers = new List<SpriteRenderer>();    // 6

    private Color _normalRed = new Color(1f, 0f, 77 / 255f);
    private Color _dashBlue = new Color(41 / 255f, 173 / 255f, 1f);

    private void Start()
    {
        AddMovementEvent();
        _hairFlow.Initialize(TargetTrans.localPosition);
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

    private int index = 0;
    public void UpdateHairFlow(CharacterMovement movement)
    {
        //_positionList.RemoveAt(0);
        //_positionList.Add(TargetTrans.localPosition);
        //for (int i = 0; i < _hairRendererCount; i++)
        //{
        //    _hairRenderers[i].transform.localScale = new Vector3(-movement.Facing, 1, 1);
        //    _hairRenderers[i].transform.localPosition = _positionList[_positionCount - 1 - i] + new Vector3(-movement.Facing * 0.5f, -0.125f, -0.1f);
        //}
        if (index == 6) index = 0;  // 更新到结尾时重置索引
        _hairRenderers[index].transform.localScale = new Vector3(-movement.Facing, 1, 1);
        _hairRenderers[index].transform.localPosition = _hairFlow.Update(TargetTrans.localPosition, () => ++index);

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

[System.Serializable]
public class HairFlow
{
    [System.Serializable]
    private class Node
    {
        public int id;
        public Node next;
        public Color color;
        public Vector2 position;
        public Renderer renderer;

        public Node(int id, Vector2 position)
        {
            this.id = id;
            this.position = position;
        }

        public Node(Vector2 position, Node next)
        {
            this.position = position;
            this.next = next;
        }

        public Node(Vector2 position, Renderer renderer, Color color, Node next)
        {
            this.position = position;
            this.renderer = renderer;
            this.color = color;
            this.next = next;
        }
    }

    [SerializeField]
    private Node _head, _tail;

    // 尾节点指向首节点
    // 遍历节点, 更新其Position值

    private void Create(int id, Vector2 position)
    {
        _head = _tail = new Node(id, position);
    }

    private void Add(int id, Vector2 position)
    {
        _tail = _tail.next = new Node(id, position);
    }

    private bool MoveNext(ref Node node)
    {
        node = node.next;
        return node != null;
    }

    public void SetPosition(Vector2 position)
    {
        _head.position = position;
    }
    public Vector2 GetPosition()
    {
        return _head.position;
    }

    // 创建一个环形链表
    public void Initialize(Vector2 position)
    {
        Create(0, position);
        for (int i = 1; i < 6; i++)
        {
            Add(i, position);
        }
        _tail.next = _head;
    }

    // 更新并返回位置
    public Vector2 Update(Vector2 position, System.Action callback)
    {
        SetPosition(position);
        if (MoveNext(ref _head))
            callback.Invoke();
        return _head.position;  // 头发飘动方向反了, 赋值应该由近及远(越迟更新的(坐标新)离角色最近)
    }
}

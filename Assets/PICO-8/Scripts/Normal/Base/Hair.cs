using UnityEngine;

[System.Serializable]
public class Hair
{
    /// <summary>
    /// 双向链表
    /// </summary>
    [System.Serializable]
    public class Node
    {
        public int id;
        public Color color;
        public Vector3 position;
        public Node prev;
        public Node next;

        public Node() { }

        public Node(int id)
        {
            this.id = id;
        }

        public Node(int id, Color color, Vector3 position)
        {
            this.id = id;
            this.color = color;
            this.position = position;
        }

        public Node(int id, Color color, Vector3 position, Node prev, Node next)
        {
            this.id = id;
            this.color = color;
            this.position = position;
            this.prev = prev;
            this.next = next;
        }
    }

    [SerializeField]
    private Node _head, _tail;
    [SerializeField]
    public Node _forward, _inversion; // 正向遍历和反向遍历

    private void Start()
    {
        Init(6, Color.white, Vector3.zero);
    }

    /// <summary>
    /// 创建并初始化链表
    /// </summary>
    /// <param name="count"></param>
    public void Init(int count, Color color, Vector3 position)
    {
        Create(new Node(0));
        for (int i = 1; i < count; i++)
        {
            AddNew(new Node(i, color, position));
        }
        _forward = _tail.next = _head;
        _inversion = _head.prev = _tail;
    }

    public void Create(Node node)
    {
        _head = _tail = node;
    }

    public void AddNew(Node node)
    {
        node.prev = _tail;
        _tail = _tail.next = node;
    }

    public void MoveNext()
    {
        _forward = _forward.next;
    }

    public void MovePrev()
    {
        _inversion = _inversion.prev;
    }

    public void ResetForward()
    {
        _forward = _head;
    }

    public void ResetInversion()
    {
        _inversion = _head;
    }

    public Vector3 GetPosition(Node node)
    {
        return node.position;
    }

    [ContextMenu("Test")]
    public void Test()
    {
        for (int i = 0; i < 20; i++)
        {
            Console.LogFormat("id {0}", _forward.id);
            MoveNext();
        }
        Console.Log("#######################");
        for (int i = 0; i < 20; i++)
        {
            Console.LogFormat("id {0}", _inversion.id);
            MovePrev();
        }
    }

}

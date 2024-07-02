using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTurnBasedCharacter : MonoBehaviour
{
    #region Path_Finding
    [SerializeField]
    protected GridMap m_gridMap;

    protected Point m_curPoint;
    protected Vector3 m_curPos;

    protected AttackComponent m_attackComponent;
    public Action<SkillData> OnSkillCompleteEvent;

    public Point CurPoint
    {
        get => m_curPoint;
    }
    #endregion

    #region Model
    
    #endregion

    #region Turn_Base


    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!m_gridMap.MapInitializeCompleted)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(MoveTo(target));

        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var stack = StartPathFinding(m_curPoint, m_gridMap.GetPointViaPosition(target));
            while (stack.Count > 0)
            {
                Debug.Log(stack.Pop());
            }
        }
    }
    public abstract void Initialize();


    #region Turn_Based_System

    public abstract void OnTurnStart();
    public abstract void OnTurnEnd();

    public virtual void EndTurn()
    {
        TurnManager.Instance.EndTurn();
    }
    #endregion

    #region Attack_System
    /// <summary>
    /// Called by AttackComponent of other objects, for applying skill effects
    /// </summary>
    public void ApplySkillEffect(SkillModel model)
    {
        ApplyPhysicalDamage(model.PhysicalDamage);
        ApplyMagicalDamage(model.MagicalDamage);
    }
    public abstract void SetSkillData(SkillData skillData);

    protected abstract void ApplyPhysicalDamage(float damage);
    protected abstract void ApplyMagicalDamage(float damage);


    #endregion

    #region Path_Finding
    public void SetGridMap(GridMap map)
    {
        m_gridMap = map;
    }
    public virtual IEnumerator MoveTo(Point targetPoint)
    {
        m_curPoint = m_gridMap.GetPointViaPosition(m_curPos);

        var stack = StartPathFinding(m_curPoint, targetPoint);
        yield return StartCoroutine(RapidMove(stack));
    }
    public virtual IEnumerator MoveTo(Vector3 targetPos)
    {
        m_curPoint = m_gridMap.GetPointViaPosition(m_curPos);
        //transform.position = m_gridMap.GetPositionViaPoint(m_curPoint);
        var stack = StartPathFinding(m_curPoint, m_gridMap.GetPointViaPosition(targetPos));
        yield return StartCoroutine(RapidMove(stack));

    }
    protected virtual IEnumerator RapidMove(Stack<Point> pathStack)
    {
        while(pathStack.Count>0)
        {
            yield return new WaitForSeconds(0.1f);
            Point nextTarget = pathStack.Pop();
            transform.position = m_gridMap.GetPositionViaPoint(nextTarget);
            m_curPoint = m_gridMap.GetPointViaPosition(transform.position);
            m_curPos = transform.position;
        }

    }
    protected virtual bool CheckGridUseSkillAvailable(Point target)
    {

        bool res = true;
        if (target.X < 0 || target.Y < 0 || target.X > m_gridMap.StepWidth - 1 || target.Y > m_gridMap.StepHeight - 1)
        {
            res &= false;
        }

        if (m_gridMap.IsObstacle(m_gridMap.GetPointState(target)))
        {
            res &= false;
        }

        return res;

    }
    protected virtual bool CheckGridWalkAvailable(Point target)
    {
        bool res = true; 
        if(target.X<0||target.Y<0||target.X>m_gridMap.StepWidth-1||target.Y>m_gridMap.StepHeight-1)
        {
            res &= false;
        }

        if(m_gridMap.IsObstacle(m_gridMap.GetPointState(target)))
        {
            res &= false;
        }

        return res;
    }

    HashSet<Point> traversedNodes;
    Queue<BFSTree> queue;
    protected Stack<Point> StartPathFinding(Point Start, Point Target)
    {
        queue = new Queue<BFSTree>();
        traversedNodes = new HashSet<Point>();

        BFSTree StartNode = getBFSTree(null, Start);
        queue.Enqueue(StartNode);
        traversedNodes.Add(Start);

        var res =  PathFinding(null, Target);
        CollectBFSTree(StartNode);
        return res;
    }
    private Stack<Point> PathFinding(BFSTree StartNode, Point target)
    {
        while(queue.Count>0)
        {
            BFSTree cur = queue.Dequeue();
            Point curPoint = cur.Value;

            if (!CheckGridWalkAvailable(cur.Value))
            {
                continue;
            }

            if (curPoint.Equals(target))
            {
                return CalculatePath(cur);
            }

            Point up = new Point(curPoint.X, curPoint.Y + 1);
            Point left = new Point(curPoint.X - 1, curPoint.Y);
            Point down = new Point(curPoint.X, curPoint.Y - 1);
            Point right = new Point(curPoint.X + 1, curPoint.Y);

            if (!traversedNodes.Contains(up))
            {
                traversedNodes.Add(up);
                BFSTree upNode = getBFSTree(cur, up);
                cur.Children.Add(upNode);
                queue.Enqueue(upNode);
            }

            if (!traversedNodes.Contains(left))
            {
                traversedNodes.Add(left);
                BFSTree leftNode = getBFSTree(cur, left);
                cur.Children.Add(leftNode);
                queue.Enqueue(leftNode);
            }

            if (!traversedNodes.Contains(down))
            {
                traversedNodes.Add(down);
                BFSTree downNode = getBFSTree(cur, down);
                cur.Children.Add(downNode);
                queue.Enqueue(downNode);
            }

            if (!traversedNodes.Contains(right))
            {
                traversedNodes.Add(right);
                BFSTree rightNode = getBFSTree(cur, right);
                cur.Children.Add(rightNode);
                queue.Enqueue(rightNode);
            }
        }
        Debug.Log($"Pathfinding failed, can't find point{target}");
        return null;


    }
    private Stack<Point> CalculatePath(BFSTree lastNode)
    {
        Stack<Point> res = new Stack<Point>();
        res.Push(lastNode.Value);
        while(lastNode.Parent!=null)
        {
            lastNode = lastNode.Parent;
            res.Push(lastNode.Value);
        }
        return res;
    }

    private Queue<BFSTree> objectPool;
    private BFSTree getBFSTree(BFSTree parent, Point value)
    {
        if(objectPool==null)
        {
            objectPool = new Queue<BFSTree>();
        }
        if(objectPool.Count>0)
        {
            BFSTree res = objectPool.Dequeue();
            res.Parent = parent;
            res.Value = value;
            return res;
        }
        else
        {
            BFSTree res = new BFSTree(parent, value);
            return res;
        }
    }
    private void CollectBFSNode(BFSTree node)
    {
        node.Reset();
        objectPool.Enqueue(node);
    }
    private void CollectBFSTree(BFSTree head)
    {
        foreach (var item in head.Children)
        {
            CollectBFSTree(item);
        }
        CollectBFSNode(head);
    }
}

public class BFSTree
{
    public Point Value;
    public BFSTree Parent;
    public List<BFSTree> Children;

    public BFSTree()
    {
        Children = new List<BFSTree>();
    }
    public void Reset()
    {
        Value = new Point(-1,-1);
        Parent = null;
        Children = new List<BFSTree>();
    }
    public BFSTree(BFSTree father)
    {
        Parent = father;
        Children = new List<BFSTree>();
    }
    public BFSTree (Point value)
    {
        Value = value;
        Children = new List<BFSTree>();
    }
    public BFSTree(BFSTree father, Point value)
    {
        Value = value;
        Parent = father;
        Children = new List<BFSTree>();
    }
    public BFSTree(BFSTree father, BFSTree[] children)
    {
        Parent = father;
        Children = new List<BFSTree>();
    }
}
#endregion
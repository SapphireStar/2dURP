using System.Collections;
using System.Collections.Generic;
using MyPackage;
using UnityEngine;

public class PlayerTurnBasedController : BaseTurnBasedCharacter
{
    public LineRenderer MouseHoverRouteRenderer;
    private List<GameObject> mouseHoverRouteList;
    private Point curMousePoint;

    private PlayerStatsModel playerStatsModel;

    private bool canMove;
    
    // Start is called before the first frame update
    public void Awake()
    {
        Initialize();
    }
    void Start()
    {
        
    }

    float timer = 0.05f;
    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0.05f;
            MouseHoverRoute();
        }
        ClickMove();
    }
    public override void Initialize()
    {
        mouseHoverRouteList = new List<GameObject>();
        m_curPoint = new Point(1, 1);
        m_curPos = m_gridMap.GetPositionViaPoint(m_curPoint);

        InitializeModels();
    }

    #region Player_Behavior
    private void ClickMove()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (playerStatsModel.IsMoving || !canMove)
            {
                return;
            }


            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (m_gridMap.GetPointViaPosition(mousePos).Equals(m_curPoint))
            {
                return;
            }
            if (!CheckGridAvailable(m_gridMap.GetPointViaPosition(mousePos)))
            {
                return;
            }

            int routeLength = GetRouteLength();
            if (routeLength > playerStatsModel.RemainSteps)
            {
                return;
            }
            playerStatsModel.RemainSteps -= routeLength;
            StartCoroutine(MoveTo(mousePos));
        }
    }

    #endregion


    #region Models
    private void InitializeModels()
    {
        playerStatsModel = ModelManager.Instance.GetModel<PlayerStatsModel>(typeof(PlayerStatsModel));
        playerStatsModel.PropertyValueChanged += OnPlayerStatsHandler;
    }

    private void OnPlayerStatsHandler(object sender, PropertyValueChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case  "RemainSteps":
                
                break;
            default:
                break;
        }
    }
    #endregion

    #region Turn_Based_System
    public override void OnTurnStart()
    {
        canMove = true;
        playerStatsModel.RemainSteps = playerStatsModel.MaxSteps;
        EventSystem.Instance.SendEvent<PlayerTurnStartEvent>(typeof(PlayerTurnStartEvent), new PlayerTurnStartEvent());
    }

    public override void OnTurnEnd()
    {
        canMove = false;
        playerStatsModel.RemainSteps = 0;
        EventSystem.Instance.SendEvent<PlayerTurnEndEvent>(typeof(PlayerTurnEndEvent), new PlayerTurnEndEvent());
    }


    #endregion

    #region Path_Finding
    protected override bool CheckGridAvailable(Point target)
    {
        bool res = base.CheckGridAvailable(target);

        return res;

    }
    protected override IEnumerator MoveTo(Vector3 targetPos)
    {
        playerStatsModel.IsMoving = true;

        m_curPoint = m_gridMap.GetPointViaPosition(m_curPos);
        //transform.position = m_gridMap.GetPositionViaPoint(m_curPoint);
        var stack = StartPathFinding(m_curPoint, m_gridMap.GetPointViaPosition(targetPos));
        yield return StartCoroutine(RapidMove(stack));

        playerStatsModel.IsMoving = false;
    }
    protected override IEnumerator RapidMove(Stack<Point> pathStack)
    {
        while (pathStack.Count > 0)
        {
            yield return new WaitForSeconds(0.1f);
            Point nextTarget = pathStack.Pop();
            transform.position = m_gridMap.GetPositionViaPoint(nextTarget);
            m_curPoint = m_gridMap.GetPointViaPosition(transform.position);
            m_curPos = transform.position;
        }
    }
    /// <summary>
    /// Calculate the length of the target route, to determine whether player can reach that point
    /// </summary>
    /// <returns></returns>
    private int GetRouteLength()
    {
        Stack<Point> stack = StartPathFinding(m_curPoint, curMousePoint);
        int routeLength = stack.Count-1;//subtract the Start Point
        return routeLength;
    }
    private void MouseHoverRoute()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Point newMousePoint = m_gridMap.GetPointViaPosition(mousePos);



        if(CheckGridAvailable(newMousePoint))
        {
            curMousePoint = newMousePoint;
        }
/*        if (newMousePoint.Equals(curMousePoint))
        {
            return;
        }*/
        
        Stack<Point> stack = StartPathFinding(m_curPoint, curMousePoint);
        int routeLength = stack.Count;
        MouseHoverRouteRenderer.positionCount = routeLength;
        for (int i = 0; i < routeLength; i++)
        {
            Point point = stack.Pop();
            Vector3 pos = m_gridMap.GetPositionViaPoint(point);
            MouseHoverRouteRenderer.SetPosition(i, new Vector3(
                pos.x,
                pos.y,
                -5
                ));
        }
    }
    #endregion


}
#region Events
public class PlayerTurnStartEvent : IEventHandler
{

}
public class PlayerTurnEndEvent : IEventHandler
{

}
#endregion
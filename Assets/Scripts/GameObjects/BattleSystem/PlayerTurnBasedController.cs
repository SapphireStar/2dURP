using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnBasedController : BaseTurnBasedCharacter
{
    public LineRenderer MouseHoverRouteRenderer;
    private List<GameObject> mouseHoverRouteList;
    private Point curMousePoint;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(MoveTo(mousePos));
        }
        
    }
    public void Initialize()
    {
        mouseHoverRouteList = new List<GameObject>();
        m_curPoint = new Point(1, 1);
        m_curPos = m_gridMap.GetPositionViaPoint(m_curPoint);
    }

    public void EndTurn()
    {

    }

    private void MouseHoverRoute()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Point newMousePoint = m_gridMap.GetPointViaPosition(mousePos);



        if(!CheckGridAvailable(newMousePoint))
        {
            return;
        }
/*        if (newMousePoint.Equals(curMousePoint))
        {
            return;
        }*/
        curMousePoint = newMousePoint;


        Stack<Point> stack = StartPathFinding(m_curPoint, newMousePoint);
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
}

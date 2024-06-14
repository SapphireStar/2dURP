using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGridCharacterController : MonoBehaviour
{
    public GridMap map;
    private Point curPos;
    // Start is called before the first frame update
    void Start()
    {
        curPos = new Point(10, 10);
        transform.position = map.GetPositionViaPoint(curPos);
    }

    // Update is called once per frame
    void Update()
    {
        if(map.StepHeight<=0)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.A)&&curPos.X>0)
        {
            curPos.X--;
            transform.position = map.GetPositionViaPoint(curPos);
        }
        if (Input.GetKeyDown(KeyCode.S) && curPos.Y > 0)
        {
            curPos.Y--;
            transform.position = map.GetPositionViaPoint(curPos);
        }
        if (Input.GetKeyDown(KeyCode.D) && curPos.X < map.StepWidth - 1 )
        {
            curPos.X++;
            transform.position = map.GetPositionViaPoint(curPos);
        }
        if (Input.GetKeyDown(KeyCode.W) && curPos.Y< map.StepHeight - 1)
        {
            curPos.Y++;
            transform.position = map.GetPositionViaPoint(curPos);
        }
    }
}

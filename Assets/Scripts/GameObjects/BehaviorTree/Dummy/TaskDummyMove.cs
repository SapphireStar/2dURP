using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class TaskDummyMove : Action
{
    private BaseTurnBasedCharacter character;
    private bool isMoveComplete;

    private bool moved;
    public override void OnAwake()
    {
        isMoveComplete = false;
        moved = false;
        character = GetComponent<BaseTurnBasedCharacter>();

    }

    public override void OnStart()
    {
            StartCoroutine(StartMove());
    }
    public override TaskStatus OnUpdate()
    {


        if (isMoveComplete)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
        }
    }
    IEnumerator StartMove()
    {
        isMoveComplete = false;
        
        Point target = TurnManager.Instance.Player.CurPoint;
        yield return StartCoroutine(character.MoveTo(getAvailablePointFromPlayer(target)));
        isMoveComplete = true;
    }

    private Point getAvailablePointFromPlayer(Point player)
    {
        Point p1 = new Point(player.X + 1, player.Y);
        Point p2 = new Point(player.X - 1, player.Y);
        Point p3 = new Point(player.X, player.Y + 1);
        Point p4 = new Point(player.X, player.Y - 1);
        Point min = p1;
        if(Point.Distance(min,character.CurPoint)> Point.Distance(p2, character.CurPoint))
        {
            min = p2;
        }
        if (Point.Distance(min, character.CurPoint) > Point.Distance(p3, character.CurPoint))
        {
            min = p3;
        }
        if (Point.Distance(min, character.CurPoint) > Point.Distance(p4, character.CurPoint))
        {
            min = p4;
        }
        return min;
    }
}

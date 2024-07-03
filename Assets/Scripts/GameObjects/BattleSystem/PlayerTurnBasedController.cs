using System;
using System.Collections;
using System.Collections.Generic;
using MyPackage;
using UnityEngine;

public enum PlayerStates
{
    Idle,
    Moving,
    Moved,
    Attacking,
    Attacked,
    Died,
}
public class PlayerTurnBasedController : BaseTurnBasedCharacter
{
    public GameObject MouseHoverRouteGreen;
    public GameObject MouseHoverRouteRed;
    private List<GameObject> mouseHoverRouteList;
    private Point curMousePoint;

    private PlayerStatsModel playerStatsModel;

    private bool canMove;
    private Animator anim;
    private bool isInitialized;

    #region StateMachine
    public PlayerStateMachine playerStateMachine;
    #endregion
    // Start is called before the first frame update
    public void Awake()
    {
        
    }
    void Start()
    {

    }

    
    // Update is called once per frame
    void Update()
    {
        //Behaviors
        Movement();
        ClickUseSkill();
        ClickCancelSkill();
        if (Input.GetKeyDown(KeyCode.H))
        {
            ApplyPhysicalDamage(10);
        }

        //Attack Component
        DisplayAttackPos();
    }
    public override void Initialize()
    {
        playerStateMachine = new PlayerStateMachine(this);

        m_curPos = transform.position;
        m_curPoint = m_gridMap.GetPointViaPosition(m_curPos);

        mouseHoverRouteList = new List<GameObject>();

        m_attackComponent = GetComponent<AttackComponent>();
        OnSkillCompleteEvent += OnSkillCompleteHandler;

        anim = GetComponent<Animator>();

        InitializeModels();

        #region Temp_Player_Stats_Initialization
        playerStatsModel.Health = 100;
        playerStatsModel.PlayerState = PlayerStates.Idle;
        playerStatsModel.MaxMainAction = 1;
        playerStatsModel.MaxBonusAction = 1;
        playerStatsModel.MaxSteps = 5;
        #endregion
    }
    void OnDestroy()
    {
        OnSkillCompleteEvent -= OnSkillCompleteHandler;
        playerStatsModel.PropertyValueChanged -= OnPlayerStatsHandler;
    }

    #region Player_Behavior

    [SerializeField]
    float BFSInterval = 0.01f;
    float timer = 0;

    private void Movement()
    {
        if (playerStatsModel.PlayerState != PlayerStates.Moving)
        {
            ClearMouseHoverRoute();
            return;
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            
            timer = BFSInterval;
            MouseHoverRoute();
        }
        ClickMove();
    }
    public void ClickMove()
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
            if (!CheckGridWalkAvailable(m_gridMap.GetPointViaPosition(mousePos)))
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
    private void ClickUseSkill()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (playerStatsModel.PlayerState != PlayerStates.Attacking || playerStatsModel.PlayerState == PlayerStates.Attacked)
            {
                return;
            }
            playerStatsModel.PlayerState = PlayerStates.Attacked;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Point skillTarget = m_gridMap.GetPointViaPosition(mousePos);
            if (!CheckGridUseSkillAvailable(m_gridMap.GetPointViaPosition(mousePos)))
            {
                Debug.Log("Skill Target is Not available");
                return;
            }
            //Use AttackComponent to use skill
            m_attackComponent.SetTarget(skillTarget);
        }
    }
    private void ClickCancelSkill()
    {
        if(playerStatsModel.PlayerState == PlayerStates.Died)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && !playerStatsModel.IsMoving)
        {
            if(playerStatsModel.PlayerState != PlayerStates.Idle)
            {
                playerStatsModel.PlayerState = PlayerStates.Idle;
            }
        }
    }
    public void OnClickMoveButton()
    {
        if (playerStatsModel.PlayerState != PlayerStates.Idle)
        {
            return;
        }
        playerStatsModel.PlayerState = PlayerStates.Moving;
        m_attackComponent.ResetSkill();
    }

    
    public void OnClickSkillButton(SkillData skillData)
    {
        if (playerStatsModel.PlayerState != PlayerStates.Idle)
        {
            return;
        }
        //can't attack during moving
        if (playerStatsModel.IsMoving )
        {
            return;
        }
        //Check remain actions
        if (skillData.MainAction && playerStatsModel.RemainMainAction <= 0)
        {
            return;
        }
        if(skillData.BonusAction && playerStatsModel.RemainBonusAction <= 0)
        {
            return;
        }

        playerStatsModel.PlayerState = PlayerStates.Attacking;
        SetSkillData(skillData);

    }




    #endregion


    #region Models_Events
    private void InitializeModels()
    {
        playerStatsModel = ModelManager.Instance.GetModel<PlayerStatsModel>(typeof(PlayerStatsModel));
        playerStatsModel.PropertyValueChanged += OnPlayerStatsHandler;
    }

    private void OnPlayerStatsHandler(object sender, PropertyValueChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "PlayerState":
                if((PlayerStates)e.Value == PlayerStates.Idle)
                {
                    m_attackComponent.ResetSkill();
                }
                break;
            case "IsMoving":
                if ((bool)e.Value)
                {
                    anim.SetBool("isRun", true);
                }
                else
                {
                    anim.SetBool("isRun", false);
                }
                break;
            default:
                break;
        }
    }


    #endregion

    #region Attack_System

    private float attackPosTimer;
    [Tooltip("The refresh interval of the position of AttackPos display prefab")]
    public float AttackPosInterval;
    private void DisplayAttackPos()
    {
        if (attackPosTimer <= 0)
        {
            attackPosTimer = AttackPosInterval;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_attackComponent.DisplayAttackPos(mousePos);
        }
        else
        {
            attackPosTimer -= Time.deltaTime;
        }
    }
    public override void SetSkillData(SkillData skillData)
    {
        //Click skill button means player want to attack
        if (m_attackComponent == null)
        {
            Debug.Log("Current Character has no AttackComponent, Set Skill Failed");
            return;
        }
        m_attackComponent.SetSkill(skillData);
    }
    protected override bool CheckGridUseSkillAvailable(Point target)
    {
        bool res = base.CheckGridUseSkillAvailable(target);

        return res;

    }
    protected override void ApplyPhysicalDamage(float damage)
    {
        if(playerStatsModel.PlayerState != PlayerStates.Idle)
        {
            return;
        }
        

        playerStatsModel.Health -= damage;
        anim.SetTrigger("hurt");
        if (playerStatsModel.Health <= 0)
        {
            playerStatsModel.PlayerState = PlayerStates.Died;
            anim.SetTrigger("die");
        }

    }

    protected override void ApplyMagicalDamage(float damage)
    {

    }

    private void OnSkillCompleteHandler(SkillData skillData)
    {
        playerStatsModel.PlayerState = PlayerStates.Idle;
        if (skillData.MainAction)
        {
            playerStatsModel.RemainMainAction--;
        }
        if (skillData.BonusAction)
        {
            playerStatsModel.RemainBonusAction--;
        }
    }
    #endregion


    #region Turn_Based_System
    public override void OnTurnStart()
    {
        canMove = true;
        playerStatsModel.RemainSteps = playerStatsModel.MaxSteps;
        playerStatsModel.RemainMainAction = playerStatsModel.MaxMainAction;
        playerStatsModel.RemainBonusAction = playerStatsModel.MaxBonusAction;

        EventSystem.Instance.SendEvent<PlayerTurnStartEvent>(typeof(PlayerTurnStartEvent), new PlayerTurnStartEvent());
    }

    public override void OnTurnEnd()
    {
        canMove = false;
        playerStatsModel.PlayerState = PlayerStates.Idle;
        playerStatsModel.RemainSteps = 0;
        playerStatsModel.RemainMainAction = 0;
        playerStatsModel.RemainBonusAction = 0;
        m_attackComponent.ResetSkill();

        EventSystem.Instance.SendEvent<PlayerTurnEndEvent>(typeof(PlayerTurnEndEvent), new PlayerTurnEndEvent());
    }


    #endregion

    #region Path_Finding
    protected override bool CheckGridWalkAvailable(Point target)
    {
        bool res = base.CheckGridWalkAvailable(target);

        //player can't walk on another character
        TurnManager.Instance.TryGetTurnObject(target, out BaseTurnBasedCharacter character);
        if (character != null && character!=this)
        {
            res &= false;
        }

        return res;

    }
    public override IEnumerator MoveTo(Vector3 targetPos)
    {
        playerStatsModel.IsMoving = true;

        m_curPoint = m_gridMap.GetPointViaPosition(m_curPos);
        //transform.position = m_gridMap.GetPositionViaPoint(m_curPoint);
        var stack = StartPathFinding(m_curPoint, m_gridMap.GetPointViaPosition(targetPos));
        yield return StartCoroutine(RapidMove(stack));

        playerStatsModel.IsMoving = false;

        //If player has no RemainSteps, auto go to Idle state
        if (playerStatsModel.RemainSteps <= 0)
        {
            playerStatsModel.PlayerState = PlayerStates.Idle;
        }
        
    }
    protected override IEnumerator RapidMove(Stack<Point> pathStack)
    {
        while (pathStack.Count > 0)
        {
            Point nextTarget = pathStack.Pop();
            Vector3 targetPos = m_gridMap.GetPositionViaPoint(nextTarget);
            
            while (Vector3.Distance(targetPos, transform.position) > 0.01f)
            {
                Vector3 moveDir = Vector3.Normalize(targetPos - transform.position);
                transform.Translate(moveDir * Time.deltaTime);
                if (moveDir.x > 0 && transform.localScale.x<0)
                {
                    transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
                }
                else if (moveDir.x < 0 && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                }
                yield return new WaitForEndOfFrame();
            }

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
    public void MouseHoverRoute()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Point newMousePoint = m_gridMap.GetPointViaPosition(mousePos);



        if(CheckGridWalkAvailable(newMousePoint)&& !playerStatsModel.IsMoving)
        {
            curMousePoint = newMousePoint;
        }
        /*        if (newMousePoint.Equals(curMousePoint))
                {
                    return;
                }*/


        ClearMouseHoverRoute();
        Stack<Point> stack = StartPathFinding(m_curPoint, curMousePoint);
        int routeLength = stack.Count;
        for (int i = 0; i < routeLength; i++)
        {
            Point point = stack.Pop();
            Vector3 pos = m_gridMap.GetPositionViaPoint(point);
            if (i <= playerStatsModel.RemainSteps|| playerStatsModel.IsMoving)//The route should be green during moving
            {
                mouseHoverRouteList.Add(Instantiate(MouseHoverRouteGreen, pos, Quaternion.identity));
            }
            else
            {
                mouseHoverRouteList.Add(Instantiate(MouseHoverRouteRed, pos, Quaternion.identity));
            }
            
        }
    }

    public void ClearMouseHoverRoute()
    {
        int length = mouseHoverRouteList.Count;
        for (int i = 0; i < length; i++)
        {
            DestroyImmediate(mouseHoverRouteList[i]);
        }
        mouseHoverRouteList.Clear();
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
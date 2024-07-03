using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackComponent : MonoBehaviour
{
    public GridMap gridMap;

    public GameObject RangeDisplayPrefab;
    private List<GameObject> rangeDisplayPrefabs;
    public GameObject AttackPosPrefab;
    private List<GameObject> attackPosPrefabs;

    private BaseTurnBasedCharacter character;
    private SkillData curSkillData;
    private SkillModel curSkillModel;

    private Animator anim;
    private Point curTarget;

    private bool isAttacking;
    private float attackPosTimer;
    [SerializeField]
    private float attackPosInterval = 0.01f;
    public Color colorAttackPosAvailable;
    public Color colorAttackPosUnavailable;
    public void Start()
    {
        character = GetComponent<BaseTurnBasedCharacter>();
        anim = GetComponent<Animator>();
        if(anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
        rangeDisplayPrefabs = new List<GameObject>();
        attackPosPrefabs = new List<GameObject>();
        
    }
    public void Update()
    {

        
    }
    public void SetSkill(SkillData data)
    {
        ResetSkill();

        curSkillData = data;
        curSkillModel = new SkillModel();

        CalculateValue();
        DisplayRange();

        isAttacking = true;
    }
    /// <summary>
    /// Calculate the final value of the skill according to character status
    /// </summary>
    public void CalculateValue()
    {
        if(curSkillData==null)
        {
            Debug.Log("Skill Data is not setted");
            return;
        }
        curSkillModel.PhysicalDamage = curSkillData.PhysicalDamage;
        curSkillModel.MagicalDamage = curSkillData.MagicalDamage;

    }

    public void DisplayRange()
    {
        Debug.Log("Display the attackable grids");
        int radius = curSkillData.RangeRadius;
        for (int i = 1; i <= radius; i++)
        {
            for (int j = 0; j < i; j++)
            {
                Point p1 = new Point(character.CurPoint.X - j, character.CurPoint.Y+(i - j));
                Point p2 = new Point(character.CurPoint.X + j, character.CurPoint.Y-(i - j));
                Point p3 = new Point(character.CurPoint.X + (i - j), character.CurPoint.Y+j);
                Point p4 = new Point(character.CurPoint.X - (i - j), character.CurPoint.Y-j);
                rangeDisplayPrefabs.Add(Instantiate(RangeDisplayPrefab, gridMap.GetPositionViaPoint(p1), Quaternion.identity));
                rangeDisplayPrefabs.Add(Instantiate(RangeDisplayPrefab, gridMap.GetPositionViaPoint(p2), Quaternion.identity));
                rangeDisplayPrefabs.Add(Instantiate(RangeDisplayPrefab, gridMap.GetPositionViaPoint(p3), Quaternion.identity));
                rangeDisplayPrefabs.Add(Instantiate(RangeDisplayPrefab, gridMap.GetPositionViaPoint(p4), Quaternion.identity));

            }
        }
    }

    /// <summary>
    /// Display the grids that can be effect by skill according to mouse position
    /// </summary>
    public void DisplayAttackPos(Vector3 pos)
    {
        if (!isAttacking)
        {
            return;
        }

        ClearAttackPosPrefabs();
        Point attackPoint = gridMap.GetPointViaPosition(pos);
        Vector3 exactPos = gridMap.GetPositionViaPoint(attackPoint);

        GameObject attackPos = Instantiate(AttackPosPrefab, exactPos, Quaternion.identity);
        if (!CheckTargetInRange(attackPoint))
        {
            attackPos.GetComponent<SpriteRenderer>().color = colorAttackPosUnavailable;
        }
        else
        {
            attackPos.GetComponent<SpriteRenderer>().color = colorAttackPosAvailable;
        }
        attackPosPrefabs.Add(attackPos);

    }

    private bool CheckTargetInRange(Point target)
    {
        if(!curSkillData.CanApplySelf&&target.Equals(character.CurPoint))
        {
            return false;
        }
        int distance = Mathf.Abs(target.X - character.CurPoint.X) + Mathf.Abs(target.Y - character.CurPoint.Y);
        if(distance > curSkillData.RangeRadius)
        {
            return false;
        }
        return true;
    }
    public void SetTarget(Point target)
    {
        if (!CheckTargetInRange(target))
        {
            Debug.Log("Target too far");
            return;
        }
        curSkillModel.Targets.Add(target);


        ChangeDirWhenAttack(target);
        ClearRangeDisplayPrefabs();
        ClearAttackPosPrefabs();
        PlaySkillAnim(target);
    }
    /// <summary>
    /// Change character direction according to attack point
    /// </summary>
    /// <param name="target"></param>
    protected void ChangeDirWhenAttack(Point target)
    {
        if (target.X - character.CurPoint.X > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (target.X - character.CurPoint.X < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
    }
    public void PlaySkillAnim(Point target)
    {
        Debug.Log($"Play the attack animation at {target}");
        anim.SetTrigger(curSkillData.AnimTrigger);
        curTarget = target;
    }
    public void OnAttackAnimationComplete()
    {
        ApplySkillEffect(curTarget);
    }
    public void ApplySkillEffect(Point point)
    {
        if (curSkillModel == null)
        {
            Debug.Log("SkillModel is empty");
            return;
        }

        BaseTurnBasedCharacter target;
        TurnManager.Instance.TryGetTurnObject(point, out target);
        if(target != null)
        {
            target.ApplySkillEffect(curSkillModel);
        }
        else
        {
            Debug.Log("Skill Missed");
        }

        Debug.Log($"Apply Skill at Point: {point}");
        character.OnSkillCompleteEvent?.Invoke(curSkillData);//Pass skill complete event

        ResetSkill();
    }

    public void ClearRangeDisplayPrefabs()
    {
        for (int i = 0; i < rangeDisplayPrefabs.Count; i++)
        {
            Destroy(rangeDisplayPrefabs[i]);
        }
        rangeDisplayPrefabs.Clear();
    }
    public void ClearAttackPosPrefabs()
    {
        for (int i = 0; i < attackPosPrefabs.Count; i++)
        {
            Destroy(attackPosPrefabs[i]);
        }
        attackPosPrefabs.Clear();
    }
    public void ResetSkill()
    {
        curSkillData = null;
        curSkillModel = null;

        ClearRangeDisplayPrefabs();
        ClearAttackPosPrefabs();

        isAttacking = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackComponent : MonoBehaviour
{
    public GridMap gridMap;
    public GameObject RangeDisplayPrefab;
    private List<GameObject> rangeDisplayPrefabs;
    private BaseTurnBasedCharacter character;
    private SkillData curSkillData;
    private SkillModel curSkillModel;
    public void Start()
    {
        character = GetComponent<BaseTurnBasedCharacter>();
        rangeDisplayPrefabs = new List<GameObject>();
    }

    public void SetSkill(SkillData data)
    {
        ResetSkill();

        curSkillData = data;
        curSkillModel = new SkillModel();

        CalculateValue();
        DisplayRange();
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
    private bool CheckTargetInRange(Point target)
    {
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

        PlaySkillAnim(target);
    }

    public void PlaySkillAnim(Point target)
    {
        Debug.Log($"Play the attack animation at {target}");
        ApplySkillEffect(target);
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

        ResetSkill();
    }

    public void ResetSkill()
    {
        curSkillData = null;
        curSkillModel = null;

        for (int i = 0; i < rangeDisplayPrefabs.Count; i++)
        {
            DestroyImmediate(rangeDisplayPrefabs[i]);
        }
        rangeDisplayPrefabs.Clear();
    }
}

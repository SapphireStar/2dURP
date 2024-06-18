using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTurnBasedController : BaseTurnBasedCharacter
{
    private float m_health;
    public float Health
    {
        get => m_health;
        set
        {
            m_health = value;
        }
    }

    public override void OnTurnEnd()
    {
        Debug.Log("Dummy Turn Ended");
    }

    public override void OnTurnStart()
    {
        Debug.Log("Dummy Turn Started");
        StartCoroutine(TestNextTurn());
    }
    IEnumerator TestNextTurn()
    {
        yield return new WaitForSeconds(2);
        EndTurn();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_curPos = transform.position;
        m_curPoint = m_gridMap.GetPointViaPosition(m_curPos);

        Health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void ApplyPhysicalDamage(float damage)
    {
        if (damage == 0)
        {
            return;
        }
        Health -= damage;
        Debug.Log($"Damage applied to dummy, cur health: {Health}");
    }

    protected override void ApplyMagicalDamage(float damage)
    {
        if (damage == 0)
        {
            return;
        }
        Health -= damage;
    }

    public override void SetSkillData(SkillData skillData)
    {
        Debug.Log("Dummy Set SkillData");
    }
}

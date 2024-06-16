using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTurnBasedController : BaseTurnBasedCharacter
{
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

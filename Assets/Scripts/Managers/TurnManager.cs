using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoSingleton<TurnManager>
{
    [SerializeField]
    private List<BaseTurnBasedCharacter> turnObjects;

    private TurnModel turnModel;
    // Start is called before the first frame update
    void Start()
    {
        turnModel = ModelManager.Instance.GetModel<TurnModel>(typeof(TurnModel));
        TurnStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TurnStart()
    {
        turnModel.Order = 0;
        turnObjects[turnModel.Order].OnTurnStart();
    }
    public void EndTurn()
    {
        int lastOrder = turnModel.Order % turnObjects.Count;
        turnObjects[lastOrder].OnTurnEnd();

        turnModel.Order++;
        int curOrder = turnModel.Order % turnObjects.Count;
        turnObjects[curOrder].OnTurnStart();
        
    }



}


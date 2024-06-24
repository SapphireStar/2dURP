using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoSingleton<TurnManager>
{
    [SerializeField]
    private List<BaseTurnBasedCharacter> turnObjects;
    [SerializeField]
    private GridMap gridMap;

    private Dictionary<Point, BaseTurnBasedCharacter> turnObjectsPointsDictionary;
    [SerializeField]
    private PlayerTurnBasedController player;
    public PlayerTurnBasedController Player
    {
        get => player;
    }

    private TurnModel turnModel;
    // Start is called before the first frame update
    void Start()
    {
        turnObjectsPointsDictionary = new Dictionary<Point, BaseTurnBasedCharacter>();
        turnModel = ModelManager.Instance.GetModel<TurnModel>(typeof(TurnModel));
        //TODO:Initialize Turn Objects at here
        foreach (var item in turnObjects)
        {
            item.Initialize();
        }

        UpdateTurnObjectsPos();//Update positions of all objects
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

        UpdateTurnObjectsPos();
    }
    /// <summary>
    /// Update all TurnObjects' point pos for using by attack system 
    /// </summary>
    private void UpdateTurnObjectsPos()
    {
        turnObjectsPointsDictionary.Clear();
        foreach (var turnObject in turnObjects)
        {
            turnObjectsPointsDictionary[turnObject.CurPoint] = turnObject;
        }
    }

    public void TryGetTurnObject(Point point, out BaseTurnBasedCharacter target)
    {
        turnObjectsPointsDictionary.TryGetValue(point, out target);
    }
}


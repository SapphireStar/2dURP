using System.Collections;
using System.Collections.Generic;
using Isekai.UI.Models;
using MyPackage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnBasedScreen : MonoBehaviour
{
    public Button ButtonEndTurn;
    public TextMeshProUGUI TextCurrentTurn;
    public TextMeshProUGUI TextRemainSteps;
    //CanvasGroup can set the Alpha and Interactable of all UI widgets in this Canvas at the same time
    public CanvasGroup GroupCanvas;

    private TurnModel turnModel;
    private PlayerStatsModel playerStatsModel;

    public void Awake()
    {
        DisableInteractiveUI();
        SubscribeEvents();
        ButtonEndTurn.onClick.AddListener(OnClickButtonEndTurn);
    }
    void Start()
    {
        turnModel = ModelManager.Instance.GetModel<TurnModel>(typeof(TurnModel));
        turnModel.PropertyValueChanged += OnTurnModelChangedHandler;

        playerStatsModel = ModelManager.Instance.GetModel<PlayerStatsModel>(typeof(PlayerStatsModel));
        playerStatsModel.PropertyValueChanged += OnPlayerStatsModelChangedHandler;
    }

    void Update()
    {
        
    }
    private void DisableInteractiveUI()
    {
        ButtonEndTurn.gameObject.SetActive(false);
        GroupCanvas.interactable = false;
        GroupCanvas.alpha = 0.5f;
        GroupCanvas.blocksRaycasts = false;
    }
    private void EnableInteractiveUI()
    {
        ButtonEndTurn.gameObject.SetActive(true);
        GroupCanvas.interactable = true;
        GroupCanvas.alpha = 1;
        GroupCanvas.blocksRaycasts = true;
    }

    private void OnTurnModelChangedHandler(object sender, PropertyValueChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "Order":
                TextCurrentTurn.text = $"Turn: {e.Value}";
                break;
            
        }

    }
    private void OnPlayerStatsModelChangedHandler(object sender, PropertyValueChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "RemainSteps":
                TextRemainSteps.text = $"Remain Steps: {e.Value}";
                break;
            /*            case "IsMoving":
                            if ((bool)e.Value)
                            {
                                DisableInteractiveUI();
                            }
                            else
                            {
                                EnableInteractiveUI();
                            }
                            break;*/
            case "PlayerState":
                if ((PlayerStates)e.Value == PlayerStates.Idle)
                {
                    EnableInteractiveUI();

                }
                else
                {
                    DisableInteractiveUI();
                }
                break;
        }
    }

    private void SubscribeEvents()
    {
        EventSystem.Instance.Subscribe<PlayerTurnStartEvent>(typeof(PlayerTurnStartEvent), OnPlayerTurnStart);

    }
    private void OnClickButtonEndTurn()
    {
        DisableInteractiveUI();
        TurnManager.Instance.EndTurn();

    }
    private void OnPlayerTurnStart(PlayerTurnStartEvent e)
    {
        EnableInteractiveUI();
    }

    
    public void OnDestroy()
    {
        EventSystem.Instance.Unsubscribe<PlayerTurnStartEvent>(typeof(PlayerTurnStartEvent), OnPlayerTurnStart);

        turnModel.PropertyValueChanged -= OnTurnModelChangedHandler;

        playerStatsModel.PropertyValueChanged -= OnPlayerStatsModelChangedHandler;
    }
}

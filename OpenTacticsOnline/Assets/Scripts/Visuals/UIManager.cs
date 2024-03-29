using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIManager
{
    private static GameObject battleUICanvas;
    private static Button[] actionButtons;
    
    public static void Init(GameObject battleCanvas)
    {
        battleUICanvas = battleCanvas;
        actionButtons = battleUICanvas.GetComponentsInChildren<Button>();
        DisableButtons();
    }

    public static void EnableButtons(List<TurnAction> actions)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actionButtons[i].gameObject.SetActive(true);
            actionButtons[i].GetComponentInChildren<TMP_Text>().text = actions[i].name;
            TurnAction action = actions[i]; 
            actionButtons[i].onClick.AddListener(() => StateManager.PushGameState(new HeroTargetSelectionState(action)));
        }
    }
    
    public static void DisableButtons()
    {
        foreach (Button button in actionButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
    }
}

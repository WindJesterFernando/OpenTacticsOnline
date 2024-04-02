using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIManager
{
    private static GameObject battleUICanvas;
    private static Button[] actionButtons;

    private static Image[] turnOrderImages;

    public static void Init(GameObject battleCanvas)
    {
        battleUICanvas = battleCanvas;

        Transform buttonsLayoutGroup = battleUICanvas.transform.Find("TurnActionBtnsVerticalLayout");
        actionButtons = buttonsLayoutGroup.GetComponentsInChildren<Button>();
        DisableButtons();

        Transform turnOrderHorizontalLayout = battleUICanvas.transform.Find("TurnOrderHorizontalLayout");
        turnOrderImages = turnOrderHorizontalLayout.GetComponentsInChildren<Image>();
        DisableTurnOrder();
    }

    public static void EnableButtons(List<TurnAction> actions)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actionButtons[i].gameObject.SetActive(true);
            actionButtons[i].GetComponentInChildren<TMP_Text>().text = actions[i].name;
            TurnAction action = actions[i];
            actionButtons[i].onClick
                .AddListener(() => StateManager.PushGameState(new HeroTargetSelectionState(action)));
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

    public static void EnableTurnOrder(Hero[] heroesTurnOrder)
    {
        for (int i = 0; i < heroesTurnOrder.Length; i++)
        {
            Hero hero = heroesTurnOrder[i];
            GameObject heroVisualRep = hero.visualRepresentation;
            SpriteRenderer heroSpriteRenderer = heroVisualRep.GetComponent<SpriteRenderer>();
            
            turnOrderImages[i].gameObject.SetActive(true);
            turnOrderImages[i].sprite = heroSpriteRenderer.sprite;
            turnOrderImages[i].color = heroSpriteRenderer.color;
        }
    }
    

    public static void DisableTurnOrder()
    {
        foreach (Image image in turnOrderImages)
        {
            image.gameObject.SetActive(false);
        }
    }
}

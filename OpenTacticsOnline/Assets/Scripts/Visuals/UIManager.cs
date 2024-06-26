using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIManager
{
    private static Button[] playButtons;
    private static Button[] actionButtons;
    private static GameObject popupText;
    private static Button menuButton;

    private static List<Image> turnOrderImages;
    private static List<Slider> heroHealthBars;
    private static readonly Vector3 ActiveHeroScale = new Vector3(1.25f, 1.25f, 1.25f);
    private const float KnockedOutHeroAlpha = 0.3f;

    public static void Init(GameObject menuCanvas, GameObject battleCanvas)
    {
        menuButton = menuCanvas.transform.Find("MenuButton").GetComponent<Button>();
        DisableMenuButton();

        Transform playButtonsLayoutGroup = menuCanvas.transform.Find("PlayModeButtonLayout");
        playButtons = playButtonsLayoutGroup.GetComponentsInChildren<Button>();
        DisablePlayButtons();

        popupText = menuCanvas.transform.Find("PopupText").gameObject;
        DisablePopupText();

        Transform actionButtonsLayoutGroup = battleCanvas.transform.Find("TurnActionBtnsVerticalLayout");
        actionButtons = actionButtonsLayoutGroup.GetComponentsInChildren<Button>();
        DisableActionButtons();

        turnOrderImages = new List<Image>();
        heroHealthBars = new List<Slider>();
        Transform turnOrderHorizontalLayout = battleCanvas.transform.Find("TurnOrderHorizontalLayout");
        foreach (Transform child in turnOrderHorizontalLayout.transform)
        {
            turnOrderImages.Add(child.GetComponent<Image>());
            heroHealthBars.Add(child.GetComponentInChildren<Slider>());
        }
        DisableTurnOrder();
    }


    public static void EnableActionButtons(List<TurnAction> actions)
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

    public static void DisableActionButtons()
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
            GameObject heroVisualRep = hero.GetVisualRepresentation();
            SpriteRenderer heroSpriteRenderer = heroVisualRep.GetComponent<SpriteRenderer>();
            
            turnOrderImages[i].gameObject.SetActive(true);
            turnOrderImages[i].sprite = heroSpriteRenderer.sprite;
            turnOrderImages[i].color = heroSpriteRenderer.color;
            heroHealthBars[i].maxValue = hero.maxHealth;
        }
        SetActiveHero(0);
        RefreshHeroHealthState(heroesTurnOrder);
    }

    public static void SetActiveHero(int index)
    {
        foreach (Image image in turnOrderImages)
        {
            image.transform.localScale = Vector3.one;
        }

        turnOrderImages[index].transform.localScale = ActiveHeroScale;
    }

    public static void RefreshHeroHealthState(Hero[] heroes)
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            Hero hero = heroes[i];
            Color color = turnOrderImages[i].color;
            color.a = hero.IsAlive() ? 1 : KnockedOutHeroAlpha;
            turnOrderImages[i].color = color;
            heroHealthBars[i].value = hero.currentHealth;
        }
    }
    
    public static void DisableTurnOrder()
    {
        foreach (Image image in turnOrderImages)
        {
            image.gameObject.SetActive(false);
        }
    }

    public static void EnablePlayButtons()
    {
        playButtons[0].gameObject.SetActive(true);
        playButtons[0].onClick
            .AddListener(() => StateManager.PushGameState(new LocalRoomState()));

        playButtons[1].gameObject.SetActive(true);
        playButtons[1].onClick
            .AddListener(() => StateManager.PushGameState(new GameRoomState()));
    }

    public static void DisablePlayButtons()
    {
        foreach (Button button in playButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
    }

    public static void EnablePopupText(string text)
    {
        popupText.GetComponent<TMP_Text>().text = text;
        popupText.SetActive(true);
    }

    public static void DisablePopupText()
    {
        popupText.gameObject.SetActive(false);
    }

    public static void EnableMenuButton()
    {
        menuButton.gameObject.SetActive(true);
        menuButton.onClick
            .AddListener(() => StateManager.PopGameStateUntilStateIs(typeof(TitleState)));
    }

    public static void DisableMenuButton()
    {
        menuButton.onClick.RemoveAllListeners();
        menuButton.gameObject.SetActive(false);
    }
}

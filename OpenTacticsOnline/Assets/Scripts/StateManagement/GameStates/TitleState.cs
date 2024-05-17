public class TitleState : AbstractGameState
{
    public override void OnStateEnter()
    {
        UIManager.EnablePlayButtons();
    }

    public override void OnStateContinue()
    {
        UIManager.EnablePlayButtons();
    }

    public override void OnStatePause()
    {
        UIManager.DisablePlayButtons();
    }
}
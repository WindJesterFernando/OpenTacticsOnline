using UnityEngine;

public class SelectSaveFileState : AbstractGameState
{
    public SelectSaveFileState()
    {

    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            StateManager.PopGameState();
            StateManager.PushGameState(new MainBattleState());
        }
    }
}
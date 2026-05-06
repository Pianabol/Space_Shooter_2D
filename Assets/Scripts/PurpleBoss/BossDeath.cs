using UnityEngine;

public class BossDeath : BossBaseState
{
    public override void RunState()
    {
        EndGameManager.endManager.StartResolveSequence();
        gameObject.SetActive(false);
    }
}

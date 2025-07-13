using UnityEngine;

public class PlayerObserver : MonoBehaviour
{
    PlayerFreezeController playerFreezeController;

    private void Awake()
    {
        playerFreezeController = FindAnyObjectByType<PlayerFreezeController>();
    }

    public void SetFreeze(bool freeze)
    {
        if (freeze)
        {
            playerFreezeController.SetFreeze(true);
        }
        else
        {
            playerFreezeController.SetFreeze(false);
        }
    }
}

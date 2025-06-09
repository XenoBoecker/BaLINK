using UnityEngine;
using GameEvents;

public class TEST_MovingObject : MonoBehaviour
{
    private void OnEnable()
    {
        InputEvent.onPlayerBlinked += PlayerBlinked;
    }

    private void OnDisable()
    {
        InputEvent.onPlayerBlinked -= PlayerBlinked;
    }

    private void PlayerBlinked()
    {
        transform.position = new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);
    }
}

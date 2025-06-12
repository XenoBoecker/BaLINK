using UnityEngine;
using GameEvents;

public class Hacks : MonoBehaviour
{
    [SerializeField] private bool _enableHacks = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!_enableHacks) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            InputEvents.PlayerBlinked();
        }
    }
}

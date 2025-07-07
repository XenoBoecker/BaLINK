using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuUI; // Reference to the pause menu UI

    PlayerFreezeController _playerFreezeController;
    TargetManager _targetManager;

    InputSystem_Actions _inputActions;

    private bool _isPaused = false;
    public bool IsPaused => _isPaused; // Expose the paused state to other scripts
    // Awake is called when the script instance is being loaded
    void Awake()
    {
        _playerFreezeController = FindAnyObjectByType<PlayerFreezeController>();
        _targetManager = FindAnyObjectByType<TargetManager>();
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Enable();

        _pauseMenuUI.SetActive(false); // Ensure the pause menu is initially hidden
    }

    // OnEnable is called when the object becomes enabled and active
    void OnEnable()
    {
        _inputActions.Player.Pause.performed += OnPausePerformed;
    }

    // OnDisable is called when the behaviour becomes disabled or inactive
    void OnDisable()
    {
        _inputActions.Player.Pause.performed -= OnPausePerformed;
    }

    // This method is called when the pause action is performed
    void OnPausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (PlayerInMinigame())
        {
            return;
        }

        if (context.performed)
        {
            PauseGame(!_isPaused);
        }
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            _pauseMenuUI.SetActive(true); // Show the pause menu UI

            _playerFreezeController.SetFreeze(true); // Freeze the player
            _targetManager.SetFreeze(true);
        }
        else
        {
            _pauseMenuUI.SetActive(false); // Hide the pause menu UI

            _playerFreezeController.SetFreeze(false); // Unfreeze the player
            _targetManager.SetFreeze(false);
        }

        _isPaused = pause;
    }

    private bool PlayerInMinigame()
    {
        return Camera.main == null;
    }

    // Update is called once per frame

}

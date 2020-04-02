using UnityEngine;
using UnityEngine.InputSystem;

public class ShowInnerGravityField : MonoBehaviour
{
    private MeshRenderer _renderInside;

    PlayerInputActions _inputAction;

    public InputAction mapAction;

    void Awake()
    {
        //InputActions
        _inputAction = new PlayerInputActions();

        mapAction = _inputAction.Player.Map;
    }

    private void Start()
    {
        _renderInside = gameObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        //InputAction replaces "Input.GetButton("Example") and holds a bool
        if (mapAction.triggered)
        {
            _renderInside.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider player)
    {
        if (!player.CompareTag("MainCamera")) return;
        _renderInside.enabled = true;
    }

    private void OnTriggerExit(Collider player)
    {
        if (!player.CompareTag("MainCamera")) return;
        _renderInside.enabled = false;
    }

    //InputActions
    //Activates all actions in Player action maps (action maps are Player and UI)
    private void OnEnable()
    {
        mapAction.Enable();
        _inputAction.Player.Enable();
    }

    //Disables all actions in Player action maps (action maps are Player and UI)
    private void OnDisable()
    {
        mapAction.Disable();
        _inputAction.Player.Disable();
    }
}

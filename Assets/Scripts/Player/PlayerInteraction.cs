using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private Vector3 interactionRayPoint;
    [SerializeField] private float interactionRayDistance;
    private Interactable currentInteractable;

    private Camera _mainCamera;

    #endregion

    #region UNITY EVENT FUNCTIONS

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleInteractionCheck();

        if (Input.GetKeyDown(interactionKey)) HandleInteractionInput();
    }

    #endregion

    #region METHOODS   

    private void HandleInteractionCheck()
    {
        float rayLength = currentInteractable ? (_mainCamera.ViewportPointToRay(interactionRayPoint).origin - currentInteractable.transform.position).magnitude : interactionRayDistance;
        Debug.DrawRay(_mainCamera.ViewportPointToRay(interactionRayPoint).origin, _mainCamera.ViewportPointToRay(interactionRayPoint).direction * rayLength, Color.red);
        if (Physics.Raycast(_mainCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hitInfo, interactionRayDistance))
        {
            if (((1 << hitInfo.transform.gameObject.layer) & interactionLayer) == 0)
            {
                SetAsNull();
                return;
            }

            if (!currentInteractable)
            {
                currentInteractable = hitInfo.collider.GetComponent<Interactable>();
                currentInteractable?.OnFocus();
            }
            else if (hitInfo.transform.GetInstanceID() != currentInteractable.transform.GetInstanceID())
                SetAsNull();

            /*if (currentInteractable == null && hitInfo.collider.TryGetComponent(out currentInteractable))
                if (currentInteractable) currentInteractable.OnFocus();
                else if (hitInfo.transform.GetInstanceID() != currentInteractable.transform.GetInstanceID())
                    SetAsNull();*/
        }
        else SetAsNull();
    }

    private void SetAsNull()
    {
        if (currentInteractable)
        {
            currentInteractable?.OnLoseFocus();
            currentInteractable = null;
        }
    }

    private void HandleInteractionInput()
    {
        if (currentInteractable != null && Physics.Raycast(_mainCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hitInfo, interactionRayDistance, interactionLayer))
        {
            currentInteractable?.OnInteract();
        }
    }

    #endregion
}

#region Backup

/*#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            private PlayerInput _playerInput;
    #endif*/

#region Is Current Device Mouse
/* {
     get
     {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
         return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                 return false;
#endif
     }
 }*/
#endregion

#endregion
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{   
    private IIteractable interactableInRange = null;
    public GameObject interactionIcon;

    void Start()
    {
        interactionIcon.SetActive(false);
    }

    void Update()
    {
        if (interactableInRange != null)
        {
            if (interactableInRange.CanInteract())
            {
                if (!interactionIcon.activeSelf)
                    interactionIcon.SetActive(true);
            }
            else
            {
                if (interactionIcon.activeSelf)
                    interactionIcon.SetActive(false);
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact();
            if (interactableInRange != null && !interactableInRange.CanInteract())
            {
                interactionIcon.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IIteractable interactable))
        {
            interactableInRange = interactable;

            if (interactable.CanInteract())
            {
                interactionIcon.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IIteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
            interactionIcon.SetActive(false);
        }
    }
}

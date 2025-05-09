using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandColliderToggle : MonoBehaviour
{
    public Collider[] collidersToDisable;

    private XRBaseInteractor interactor;

    void Awake()
    {
        interactor = GetComponent<XRBaseInteractor>();
        interactor.selectEntered.AddListener(OnGrab);
        interactor.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        foreach (var col in collidersToDisable)
            col.enabled = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        foreach (var col in collidersToDisable)
            col.enabled = true;
    }
}

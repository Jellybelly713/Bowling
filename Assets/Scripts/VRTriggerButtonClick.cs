using UnityEngine;
using UnityEngine.InputSystem;

public class VRTriggerButtonClick : MonoBehaviour
{
    // Set to XRI Left/Right Interaction -> Activate
    public InputActionReference triggerAction;

    [Header("Interaction Settings")]
    public float maxDistance = 0.3f;
    public float castRadius = 0.06f;   // interaction thickness
    public LayerMask hitMask = ~0;     // limit to button layer if desired

    private void OnEnable()
    {
        if (triggerAction != null)
            triggerAction.action.performed += OnTrigger;
    }

    private void OnDisable()
    {
        if (triggerAction != null)
            triggerAction.action.performed -= OnTrigger;
    }

    private void OnTrigger(InputAction.CallbackContext ctx)
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.SphereCast(ray, castRadius, out RaycastHit hit, maxDistance, hitMask))
        {
            // Spawn ball button
            var spawn = hit.collider.GetComponentInParent<SpawnButton>();
            if (spawn != null)
            {
                spawn.Press();
                return;
            }

            // Reset pins button
            var reset = hit.collider.GetComponentInParent<ResetPinsButton>();
            if (reset != null)
            {
                reset.Press();
                return;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * maxDistance;

        // draw ray
        Gizmos.DrawLine(start, end);

        // draw sphere at end
        Gizmos.DrawWireSphere(end, castRadius);
    }
#endif
}



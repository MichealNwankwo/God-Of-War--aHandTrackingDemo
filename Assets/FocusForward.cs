using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FocusForward : MonoBehaviour
{
    [Header("Options")]
    public float minDistance = 0.1f;
    public float maxDistance = 15f;
    public float focusSpeed = 0.15f;
    public bool useSphereCast = true;
    public float sphereCastRadius = 0.5f;

    private Volume _volume;
    private DepthOfField _depthOfField;
    private Coroutine _focusCoroutine;
    private float _targetDistance;

    private Volume Volume => GetPostProcessVolume();
    private DepthOfField DepthOfField => _depthOfField ??= GetDepthOfField();

    private Volume GetPostProcessVolume()
    {
        if (_volume != null) return _volume;

        var volumes = FindObjectsByType<Volume>(FindObjectsSortMode.None);
        if (volumes.Length == 0)
        {
            Debug.LogError(" No Post Process Volume found in the scene!");
            return null;
        }

        _volume = volumes[0];
        Debug.Log(" Post Process Volume found: " + _volume.name);
        return _volume;
    }

    private DepthOfField GetDepthOfField()
    {
        if (_volume == null || !_volume.profile.TryGet(out DepthOfField dof))
        {
            Debug.LogError(" Depth of Field effect not found in Volume Profile!");
            return null;
        }
        Debug.Log(" Depth of Field effect found!");
        return dof;
    }

    private void Update()
    {
        if (Volume == null || DepthOfField == null) return;

        CheckFocus();
    }

    private void CheckFocus()
    {
        var hitInfo = CalculateHit();
        if (!hitInfo.HasValue)
        {
            Debug.Log(" No object detected. Focusing at max distance.");
            SwitchFocus(maxDistance);
            return;
        }

        var focusDistance = CalculateFocusDistance(hitInfo.Value);
        Debug.Log($" Object detected at {focusDistance} meters. Adjusting focus.");
        SwitchFocus(focusDistance);
    }

    private void SwitchFocus(float newTargetDistance)
    {
        if (Mathf.Approximately(DepthOfField.focusDistance.value, newTargetDistance)) return;
        if (Mathf.Approximately(_targetDistance, newTargetDistance)) return;

        Debug.Log($" Changing focus distance to {newTargetDistance} meters.");
        _targetDistance = newTargetDistance;
        if (_focusCoroutine != null) return;
        _focusCoroutine = StartCoroutine(SetFocusDistance());
    }

    private IEnumerator SetFocusDistance()
    {
        var elapsed = 0f;
        var startValue = DepthOfField.focusDistance.value;
        while (elapsed < focusSpeed)
        {
            DepthOfField.focusDistance.value = Mathf.Lerp(startValue, _targetDistance, elapsed / focusSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        DepthOfField.focusDistance.value = _targetDistance;
        Debug.Log($" Focus set to {_targetDistance} meters.");
        _focusCoroutine = null;
    }

    private float CalculateFocusDistance(RaycastHit hitInfo)
    {
        var distance = Vector3.Distance(transform.position, hitInfo.point);
        return Mathf.Clamp(distance, minDistance, maxDistance);
    }

    private RaycastHit? CalculateHit()
    {
        var thisTransform = transform;
        var hit = useSphereCast
            ? Physics.SphereCast(thisTransform.position, sphereCastRadius, thisTransform.forward, out var hitInfo, maxDistance)
            : Physics.Raycast(thisTransform.position, thisTransform.forward, out hitInfo, maxDistance);

        if (hit)
        {
            Debug.Log($"Raycast hit: {hitInfo.collider.name} at {hitInfo.distance} meters.");
        }
        return hit ? hitInfo : null;
    }
}

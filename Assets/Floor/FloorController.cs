using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public PlayerInventory playerInventory;

    [Tooltip("Ordered list of positions. Weight 1 => index 0, Weight 2 => index 1, and so on.")]
    public List<Transform> positions = new List<Transform>();

    public float moveSpeed = 2f;

    private int currentTargetIndex = -1;
    private Transform currentTargetTransform;

    void Awake()
    {
        if (playerInventory == null)
            playerInventory = FindObjectOfType<PlayerInventory>();
    }

    void Update()
    {
        if (positions == null || positions.Count == 0)
            return;

        // this should not be here, too bad!
        UpdateTargetFromWeight(false);

        MoveToTarget();
    }

    private void UpdateTargetFromWeight(bool force)
    {
        if (playerInventory == null) return;

        int weightRounded = Mathf.RoundToInt(playerInventory.currentWeight);
        int desiredIndex = MapWeightToIndex(weightRounded);

        if (!force && desiredIndex == currentTargetIndex)
            return;

        currentTargetIndex = desiredIndex;
        currentTargetTransform = (currentTargetIndex >= 0 && currentTargetIndex < positions.Count)
            ? positions[currentTargetIndex]
            : null;
    }

    private int MapWeightToIndex(int weight)
    {
        if (positions == null || positions.Count == 0)
            return -1;

        int clampedWeight = Mathf.Max(1, weight); // weight 0 -> index 0 fallback
        int idx = clampedWeight - 1;
        idx = Mathf.Clamp(idx, 0, positions.Count - 1);
        return idx;
    }

    private void MoveToTarget()
    {
        if (currentTargetTransform == null)
            return;

        Vector3 targetWorldPos = currentTargetTransform.position;

        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
    }
}

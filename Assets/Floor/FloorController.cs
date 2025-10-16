using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerMovement playerMovement;

    [Tooltip("Ordered list of positions. Weight 1 => index 0, Weight 2 => index 1, and so on.")]
    public List<Transform> positions = new List<Transform>();

    public float moveSpeed = 2f;

    private int currentTargetIndex = 1;
    private Transform currentTargetTransform;

    private Coroutine moveCoroutine;

    /// <summary>
    /// Called externally from PlayerInventory whenever weight changes and we should re-evaluate the floor target.
    /// </summary>
    public void RequestMoveToTarget()
    {
        UpdateTargetFromWeight(true);

        // restart movement coroutine so it reacts immediately
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        moveCoroutine = StartCoroutine(MoveToTargetCoroutine());
    }

    private void UpdateTargetFromWeight(bool force)
    {
        if (playerInventory == null)
            playerInventory = FindObjectOfType<PlayerInventory>();

        if (playerInventory == null) return;

        int weightRounded = Mathf.RoundToInt(playerInventory.currentAvgWeight);
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

        return Mathf.Clamp(weight, 0, positions.Count);
    }

    private IEnumerator MoveToTargetCoroutine()
    {
        if (currentTargetTransform == null)
            yield break;

        // disable player input/movement but keep CharacterController enabled
        if (playerMovement != null)
            playerMovement.canMove = false;

        while (currentTargetTransform != null)
        {
            Vector3 targetWorldPos = currentTargetTransform.position;
            Vector3 currentPos = transform.position;

            float distance = Vector3.Distance(currentPos, targetWorldPos);
            if (distance <= 0.001f)
            {
                transform.position = targetWorldPos;
                break;
            }

            transform.position = Vector3.MoveTowards(currentPos, targetWorldPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // re-enable player input
        if (playerMovement != null)
            playerMovement.canMove = true;

        moveCoroutine = null;

        //when platform finishes moving, automatically save the current state
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveOverwrite();
            Debug.Log("FloorController Platform finished moving and has saved snapshot.");
        }
    }

    public FloorState GetFloorState()
    {
        return new FloorState
        {
            currentTargetIndex = currentTargetIndex,
            position = transform.position
        };
    }

    public void ApplyFloorState(FloorState state)
    {
        if (state == null)
        {
            Debug.LogWarning("FloorController null state.");
            return;
        }

        int savedIndex = state.currentTargetIndex;

        if (positions != null && positions.Count > 0)
        {
            if (savedIndex >= 0 && savedIndex < positions.Count)
            {
                currentTargetIndex = savedIndex;
                currentTargetTransform = positions[currentTargetIndex];
            }
            else
            {
                Debug.LogError("floor index error, out of range.");
            }
        }
        transform.position = state.position;
    }
}

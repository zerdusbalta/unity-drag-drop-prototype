using UnityEngine;

public class DropZone : MonoBehaviour
{
    public bool IsMoreThanHalfInside(Collider2D draggableCollider)
    {
        Collider2D zoneCollider = GetComponent<Collider2D>();

        Bounds dragBounds = draggableCollider.bounds;
        Bounds zoneBounds = zoneCollider.bounds;

        float overlapMinX = Mathf.Max(dragBounds.min.x, zoneBounds.min.x);
        float overlapMaxX = Mathf.Min(dragBounds.max.x, zoneBounds.max.x);
        float overlapMinY = Mathf.Max(dragBounds.min.y, zoneBounds.min.y);
        float overlapMaxY = Mathf.Min(dragBounds.max.y, zoneBounds.max.y);

        float overlapWidth = overlapMaxX - overlapMinX;
        float overlapHeight = overlapMaxY - overlapMinY;

        if (overlapWidth <= 0 || overlapHeight <= 0)
            return false;

        float overlapArea = overlapWidth * overlapHeight;
        float dragArea = dragBounds.size.x * dragBounds.size.y;

        return overlapArea >= dragArea * 0.2f;
    }

    public Vector3 GetSnapPosition()
    {
        return transform.position;
    }
}
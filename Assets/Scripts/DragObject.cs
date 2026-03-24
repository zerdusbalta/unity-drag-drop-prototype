using UnityEngine;
using TMPro;

public class DragObject : MonoBehaviour
{
    private bool isDragging;
    private bool isSnapping;
    private Vector3 offset;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    [SerializeField] private float snapSpeed = 10f;
    [SerializeField] private TextMeshProUGUI successText;

    private SpriteRenderer sr;
    private DropZone dropZone;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        dropZone = FindObjectOfType<DropZone>();
        startPosition = transform.position;

        if (successText != null)
        {
            successText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isSnapping)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, snapSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isSnapping = false;
            }
        }

        Vector3 mouseWorld = GetMouseWorldPosition();

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld);

            if (hit != null && hit.gameObject == gameObject)
            {
                isSnapping = false;
                isDragging = true;
                offset = transform.position - mouseWorld;
                sr.color = Color.green;

                if (successText != null)
                {
                    successText.gameObject.SetActive(false);
                }
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            transform.position = mouseWorld + offset;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            sr.color = Color.white;

            if (dropZone != null && dropZone.IsMoreThanHalfInside(GetComponent<Collider2D>()))
            {
                targetPosition = dropZone.GetSnapPosition();
                isSnapping = true;
                sr.color = Color.yellow;

                if (successText != null)
                {
                    successText.gameObject.SetActive(true);
                }
            }
            else
            {
                transform.position = startPosition;

                if (successText != null)
                {
                    successText.gameObject.SetActive(false);
                }
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
    }
}
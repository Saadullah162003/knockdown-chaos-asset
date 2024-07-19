using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DragAndShoot : MonoBehaviour
{
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;

    private Rigidbody rb;
    private bool isShoot;
    private Vector3 initialPosition;

    [SerializeField] private float forceMultiplier = 5.5f;
    [SerializeField] private float torqueMultiplier = 8.8f;
    [SerializeField] private float gravityScale = 6f;
    [SerializeField] private float angularDrag = 0.5f; // Set angular drag value

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.mass = 1f;
        rb.drag = 0.5f;
        rb.angularDrag = angularDrag; // Apply angular drag
        Physics.gravity = new Vector3(0, -9.81f * gravityScale, 0);

        initialPosition = transform.position;
    }

    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        mouseReleasePos = Input.mousePosition;
        Shoot(mouseReleasePos - mousePressDownPos);
    }

    void Shoot(Vector3 force)
    {
        if (isShoot)
            return;

        Vector3 forceDirection = new Vector3(force.x, force.y, force.y); // Corrected force direction
        rb.AddForce(forceDirection * forceMultiplier);
        rb.AddTorque(new Vector3(force.y, force.x, 0) * torqueMultiplier);

        isShoot = true;

        StartCoroutine(ResetBallPositionAfterDelay());
    }

    IEnumerator ResetBallPositionAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = initialPosition;

        isShoot = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 forceDirection = rb.velocity.normalized;
            rb.AddTorque(Vector3.Cross(forceDirection, Vector3.up) * torqueMultiplier);
        }
    }

    void FixedUpdate()
    {
        Vector3 customGravity = new Vector3(0, -9.81f * gravityScale, 0);
        rb.AddForce(customGravity - Physics.gravity, ForceMode.Acceleration); // Apply custom gravity correctly
    }
}

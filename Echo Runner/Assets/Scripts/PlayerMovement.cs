using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float laneDistance = 2.5f;
    private int currentLane = 1;

    public float jumpForce = 7f;
    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 0)
        {
            currentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 2)
        {
            currentLane++;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            StartCoroutine(Slide());
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, forwardSpeed);
        rb.angularVelocity = Vector3.zero;

        Vector3 targetPosition = new Vector3((currentLane - 1) * laneDistance, rb.position.y, rb.position.z);
        rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, Time.fixedDeltaTime * 10f));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit Obstacle!");
            SceneManager.LoadScene(0);
        }
    }

    private IEnumerator Slide()
    {
        Vector3 originalScale = transform.localScale;
        transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);

        yield return new WaitForSeconds(1f);

        transform.localScale = originalScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5.0f;
    Vector2 moveDirection = Vector2.zero;
    public PlayerInputActions PlayerControls;
    public Projectile laserPrefab;
    private InputAction move;
    private InputAction Fire;
    private bool LaserOn;

    public void Awake()
    {
        PlayerControls = new PlayerInputActions();
    }
    private void OnEnable()
    {
        move = PlayerControls.Player.Move;
        move.Enable();
        Fire = PlayerControls.Player.Fire;
        Fire.Enable();
        Fire.performed += LaserShoot;
    }
    private void OnDisable()
    {
        move.Disable();
        Fire.Disable();
    }
    private void LaserShoot(InputAction.CallbackContext context)
    {
        if (!LaserOn)
        {
            Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            projectile.Destroyed += Laserdestroyed;
            LaserOn = true;
        }
    }
    private void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }



    private void Laserdestroyed()
    {
        LaserOn = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==LayerMask.NameToLayer("Invader")|| collision.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            laserPrefab.setScore();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

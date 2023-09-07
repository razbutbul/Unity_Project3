using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Invaders : MonoBehaviour
{
    public Invader[] prefabs;
    public int rows = 5;
    public int columns = 11;
    public float speed = 1.0f;
    public int killed { get; private set; }
    public int totalInvaders => this.rows * this.columns;
    public Projectile missilePrefab;
    public Projectile starFallPrefab;
    public float MissileShootsRate = 1.0f;
    public float StarFallRate = 2.5f;
    public int Alive => this.totalInvaders - this.killed;
    [SerializeField] private Text Enemies;
    [SerializeField] private Text scroeText;


    private Vector3 _direction = Vector2.right;

    public void Awake()
    {
        for (int row = 0; row < this.rows; row++)
        {
            float width = 2.0f * (this.columns - 1);
            float height = 2.0f * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2); ;
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 2.0f), 0.0f);
            for (int col = 0; col < this.columns; col++)
            {
                Invader invader = Instantiate(this.prefabs[row], this.transform);
                invader.killed += DeadInvader;
                Vector3 position = rowPosition;
                position.x += col * 2.0f;
                invader.transform.localPosition = position;
            }
        }
    }
    private void Update()
    {
        this.transform.position += _direction * this.speed * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (_direction == Vector3.right && invader.position.x >= rightEdge.x - 1)
            {
                AdvanceRow();
            }
            else if (_direction == Vector3.left && invader.position.x <= leftEdge.x + 1)
            {
                AdvanceRow();
            }

        }
        updateScroe();

    }

    private void AdvanceRow()
    {
        _direction.x *= -1.0f;
        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }


    private void DeadInvader()
    {
        this.killed++;
        if (this.killed == this.totalInvaders)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        Enemies.text = "Enemies: " + Alive;
    }


    private void Start()
    {
        InvokeRepeating(nameof(MissileShoots), this.MissileShootsRate, this.MissileShootsRate);
        InvokeRepeating(nameof(StarFall), this.StarFallRate, this.StarFallRate);
    }
    private void MissileShoots()
    {
        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (Random.value < (1.0f / (float)this.Alive))
            {
                Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                break;

            }
        }
    }

    private void StarFall()
    {
        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (Random.value < (1.0f / (float)this.Alive))
            {
                Instantiate(this.starFallPrefab, invader.position, Quaternion.identity);
                break;

            }
        }
    }
    private void updateScroe()
    {
        scroeText.text = "Score: " + starFallPrefab.getScroe();
    }
}
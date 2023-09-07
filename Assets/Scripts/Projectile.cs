using UnityEngine;
using UnityEngine.UI;


public class Projectile : MonoBehaviour
{
    public static int score = 0;
    public Vector3 direction;
    public float speed;
    public System.Action Destroyed;



    private void Update()
    {
        this.transform.position += this.direction * this.speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.Destroyed != null)
        {
            this.Destroyed.Invoke();
        }
        if (this.gameObject.layer == LayerMask.NameToLayer("Star") && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            score += 100;
        }
        Destroy(this.gameObject);

    }
    public int getScroe()
    {
        return score;
    }
    public void setScore()
    {
        score = 0;
    }

}
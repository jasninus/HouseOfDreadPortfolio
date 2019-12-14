public class ExorcistHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;
    private int health;

    [SerializeField] private float invulnerabilityTime;
    private float lastDamageTime;

    [SerializeField] private Image healthBar;

    [HideInInspector] public bool immune;

    [SerializeField] private Animator anim;

    private AudioSource audio;

    [SerializeField] private AudioClip hitSFX;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (immune)
        {
            return;
        }

        if (Time.time > lastDamageTime + invulnerabilityTime)
        {
            ApplyDamage(damage);
            CheckForGameOver();
        }
    }

    private void ApplyDamage(int damage)
    {
        audio.clip = hitSFX;
        audio.Play();
        anim.SetBool("Damage", true);
        lastDamageTime = Time.time;
        health -= damage;

        healthBar.fillAmount = (float)health / maxHealth;
    }

    private void CheckForGameOver()
    {
        if (health <= 0)
        {
            GameOver();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "UnholyWater" && !immune)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene(4);
    }

    public void StopTakingDamage()
    {
        anim.SetBool("Damage", false);
    }
}
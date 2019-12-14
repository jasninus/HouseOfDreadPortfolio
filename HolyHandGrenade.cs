using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HolyHandGrenade : MonoBehaviour
{
    [SerializeField] private int damage;

    [SerializeField] private float fuseTime, range;
    private float startTime;

    private bool hasExploded;

    private ParticleSystem particle;

    private AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        particle = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > startTime + fuseTime && !hasExploded)
        {
            particle.Play(true);
            audio.Play();
            hasExploded = true;
            IDamageable[] hit = Physics2D.OverlapCircleAll(transform.position, range).Select(c => c.transform.GetComponent<IDamageable>()).ToArray();

            foreach (IDamageable damageable in hit)
            {
                damageable?.TakeDamage(damage);
            }

            Debug.Log("BOOM! Holyness overcomes you");
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(Destroy(particle.main.duration));
        }
    }

    private IEnumerator Destroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}

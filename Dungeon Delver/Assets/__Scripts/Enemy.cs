using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected static Vector3[] direction = new Vector3[] { Vector3.right, Vector3.up, Vector3.left, Vector3.down };

    [Header("Set in Inspector: Enemy")]
    public float maxHealth = 10;
    public float knockbackSpeed = 10;
    public float knockbackDuration = 0.25f;
    public float stunDuration = 3;
    public float invincibleDuration = 0.5f;
    public GameObject[] randomItemDrops;
    public GameObject guaranteedItemDrop = null;

    [Header("Set Dinamically: Enemy")]
    public float health;
    public bool invincible = false;
    public bool stun = false;
    public bool knockback = false;

    private float knockbackDone = 0;
    private float stunDone = 0;
    private float invincibleDone = 0;
    private Vector3 knockbackVel;

    protected Animator anim;
    protected Rigidbody rigid;
    protected SpriteRenderer sRend;

    protected virtual void Awake()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        sRend = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        //Проверить состояние неуязвимости и необходимость ее выполнения
        if (invincible && Time.time > invincibleDone) invincible = false;
        if (stun && Time.time > stunDone) stun = false;
        if (knockback)
        {
            rigid.velocity = knockbackVel;
            if (Time.time < knockbackDone) return;
            knockback = false;
            rigid.velocity = Vector3.zero;
        }
        if (stun)
        {
            sRend.color = Color.blue;
            return;
        }
        sRend.color = invincible ? Color.red : Color.white;
        anim.speed = 1;
    }

    private void OnTriggerEnter(Collider colld)
    {
        if (invincible) return;//Выйти, если враг неуязвим
        DamageEffect dEf = colld.gameObject.GetComponent<DamageEffect>();
        StunEffect sEf = colld.gameObject.GetComponent<StunEffect>();
        if (dEf == null)
        {
            if (sEf != null)
            {
                stunDone = Time.time + stunDuration;
                anim.speed = 0;
                rigid.velocity = Vector3.zero;
                stun = true;
                return;
            }
            return;
        }
        if (stun)
        {
            health -= dEf.damage * 2;
        } else
        {
            health -= dEf.damage;
        }
        if (health <= 0) Die();
        invincible = true;
        sRend.color = Color.red;
        invincibleDone = Time.time + invincibleDuration;

        if (dEf.knockback)//Выпонить отбрасывание
        {
            //Определить направление отскока
            Vector3 delta = transform.position - colld.transform.root.position;
            if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
            {
                delta.x = (delta.x > 0) ? 1 : -1;
                delta.y = 0;
            } else
            {
                delta.y = (delta.y > 0) ? 1 : -1;
                delta.x = 0;
            }

            //Применить скорость отбрасывания к обьекту
            knockbackVel = delta * knockbackSpeed;
            rigid.velocity = knockbackVel;

            //Установить режим knockback и время отбрасывания
            knockback = true;
            knockbackDone = Time.time + knockbackDuration;
            anim.speed = 0;
        }
    }

    void Die()
    {
        GameObject go;
        if (guaranteedItemDrop != null)
        {
            go = Instantiate(guaranteedItemDrop);
            go.transform.position = transform.position;
        } else if (randomItemDrops.Length > 0)
        {
            int n = Random.Range(0, randomItemDrops.Length);
            GameObject prefab = randomItemDrops[n];
            if (prefab != null)
            {
                go = Instantiate(prefab);
                go.transform.position = transform.position;
            }
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBullet : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 10f;

    [Header("Set Dinamically")]
    public int drayFacing = 1;

    private Rigidbody rigid;

    public DamageEffect dmg;

    private Vector3[] direction = new Vector3[] { Vector3.right, Vector3.up, Vector3.left, Vector3.down };

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        dmg = GetComponent<DamageEffect>();
    }

    private void Update()
    {
        rigid.velocity = direction[drayFacing] * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}

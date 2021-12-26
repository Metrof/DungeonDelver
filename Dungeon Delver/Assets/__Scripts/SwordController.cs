using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Sprite powerSword;
    public Sprite defoltSword;

    private GameObject sword;
    private Dray dray;
    private SpriteRenderer sRend;
    private DamageEffect dng;

    private void Start()
    {
        sword = transform.Find("Sword").gameObject;
        sRend = sword.GetComponent<SpriteRenderer>();
        dng = sword.GetComponent<DamageEffect>();
        dray = transform.parent.GetComponent<Dray>();
        //Деактивировать меч
        sword.SetActive(false);
    }
    private void Update()
    {
        dray.swordDamage = dng.damage;
        transform.rotation = Quaternion.Euler(0, 0, 90 * dray.facing);
        sword.SetActive(dray.mode == Dray.eMode.attack);
        if (dray.health == 10)
        {
            sRend.sprite = powerSword;
        } else
        {
            sRend.sprite = defoltSword;
        }
    }
}

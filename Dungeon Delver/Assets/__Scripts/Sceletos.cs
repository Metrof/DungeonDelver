using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sceletos : Enemy, IFacingMover
{
    [Header("Set in Inspector: Sceletos")]
    public int speed = 2;
    public float timeThinkMin = 1f;
    public float timeThinkMax = 3f;

    [Header("Set Dinamically: Sceletos")]
    public int facing = 0;
    public float timeNextDecision = 0;

    private InRoom inRm;

    public bool moving { get { return true; } }

    public float gridMult { get { return inRm.gridMult; } }

    public Vector2 roomPos { get { return inRm.roomPos; } set { inRm.roomPos = value; } }
    public Vector2 roomNum { get { return inRm.roomNum; } set { inRm.roomNum = value; } }

    protected override void Awake()
    {
        base.Awake();
        inRm = GetComponent<InRoom>();
    }

    protected override void Update()
    {
        base.Update();
        if (knockback || stun) return;
        if (Time.time >= timeNextDecision)
        {
            DecideDirection();
        }
        //ѕоле rigid унаследовано от класса Enemy и инициализируетс€ в Enemy.Awake()
        rigid.velocity = direction[facing] * speed;
    }
    void DecideDirection(int fIng = -1)
    {
        if (fIng == -1)
        {
            facing = Random.Range(0, 4);
        } else
        {
            facing = fIng;
        }
        timeNextDecision = Time.time + Random.Range(timeThinkMin, timeThinkMax);
    }

    public int GetFacing()
    {
        return facing;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public Vector2 GetRoomPosOnGrid(float mult = -1f)
    {
        return inRm.GetRoomPosOnGrid(mult);
    }
}

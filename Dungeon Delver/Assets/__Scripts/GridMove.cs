using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMove : MonoBehaviour
{
    private IFacingMover mover;

    private void Awake()
    {
        mover = GetComponent<IFacingMover>();
    }

    private void FixedUpdate()
    {
        if (!mover.moving) return;//Если обьект не перемещается-выйти
        int facing = mover.GetFacing();
        //Если обьект перемещается, применить выравнивание по сетке
        //Сначала получить координаты ближайшего узла сетки
        Vector2 rPos = mover.roomPos;
        Vector2 rPosGrid = mover.GetRoomPosOnGrid();
        //Этот код полагается на интерфейс IFacingMover (который использует InRoom) для определения шага сетки
        //затем подвинуть обьект в сторону линии сетки
        float delta = 0;
        if (facing == 0 || facing == 2)
        {
            //Движение по горизонтали, выравнивание по оси y
            delta = rPosGrid.y - rPos.y;
        } else
        {
            //Движение по вертикали, выравнивание по оси х
            delta = rPosGrid.x - rPos.x;
        }
        if (delta == 0) return;//Обьект уже выровнен по сетке
        float move = mover.GetSpeed() * Time.fixedDeltaTime;
        move = Mathf.Min(move, Mathf.Abs(delta));
        if (delta < 0) move = -move;
        if (facing == 0||facing == 2)
        {
            //Движение по горизонтали, выравнивание по оси y
            rPos.y += move;
        } else
        {
            //Движение по вертикали, выравнивание по оси х
            rPos.x += move;
        }
        mover.roomPos = rPos;
    }
}

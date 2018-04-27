using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataStructures
{
    public class currentInputs
    {
        private float forward;
        private float horizontal;
        private float mouse_x;
        private float mouse_y;
        private bool jumpBtn;

        void setFwd(float fwd)
        {
            this.forward = fwd;
        }
        void setHoriz(float hrz)
        {
            this.horizontal = hrz;
        }
        void setMouseX(float msX)
        {
            this.mouse_x = msX;
        }
        void setMouseY(float msY)
        {
            this.mouse_y = msY;
        }
        void setJump(bool jmp)
        {
            this.jumpBtn = jmp;
        }

    }
}

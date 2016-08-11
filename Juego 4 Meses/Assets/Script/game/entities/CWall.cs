using UnityEngine;
using System.Collections;

public class CWall : CSprite {
    
    private const int WALL_FULL = 0;
    private const int WALL_WINDOW = 1;
    private const int WALL_RIGHT = 2;
    private const int WALL_LEFT = 3;
    private int mWallType;

    public CWall(int aWallType)
    {
        mWallType = aWallType;
        if(mWallType == CWall.WALL_FULL)
        {
            setImage(Resources.Load<Sprite>("Sprites/Walls/Wall000"));
        }
        if (mWallType == CWall.WALL_WINDOW)
        {
            setImage(Resources.Load<Sprite>("Sprites/Walls/Wall001"));
        }
        if (mWallType == CWall.WALL_RIGHT)
        {
            setImage(Resources.Load<Sprite>("Sprites/Walls/Wall002"));
        }
        if (mWallType == CWall.WALL_LEFT)
        {
            setImage(Resources.Load<Sprite>("Sprites/Walls/Wall003"));
        }
    }

    override public void update()
    {
        base.update();
    }

    override public void render()
    {
        base.render();
    }

    override public void destroy()
    {
        base.destroy();
        
    }
}

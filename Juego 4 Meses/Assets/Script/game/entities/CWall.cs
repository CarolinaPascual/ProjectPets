using UnityEngine;
using System.Collections;

public class CWall : CSprite {
    
    private const int WALL_LEFT = 0;
    private const int WALL_RIGHT = 1;
    
    private int mWallType;

    public CWall(int aWallType)
    {
        mWallType = aWallType;
        if(mWallType == CWall.WALL_LEFT)
        {
            setImage(Resources.Load<Sprite>("Sprites/Background/background1"));
        }
        if (mWallType == CWall.WALL_RIGHT)
        {
            setImage(Resources.Load<Sprite>("Sprites/Background/background2"));
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

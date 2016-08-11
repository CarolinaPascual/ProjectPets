using UnityEngine;
using System.Collections;

public class CAnimatedBackground : CAnimatedSprite
{
    
    public CAnimatedBackground()
    {
        setFrames(Resources.LoadAll<Sprite>("Sprites/ui/animated_background"));
        setXY(0, 0);
        setSortingLayerName("Background");
        setName("background");
        setRegistration(CSprite.REG_TOP_LEFT);
        setWidth(CGameConstants.SCREEN_WIDTH);
        setHeight(CGameConstants.SCREEN_HEIGHT);
        render();

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

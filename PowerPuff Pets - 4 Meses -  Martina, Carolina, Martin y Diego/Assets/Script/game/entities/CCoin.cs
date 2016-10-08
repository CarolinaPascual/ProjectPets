using UnityEngine;
using System.Collections;

public class CCoin : CAnimatedSprite {

    private const int STATE_BOUNCING = 0;


    public CCoin()
    {
        setFrames(Resources.LoadAll<Sprite>("Sprites/coins"));
        setName("Coin");
        setSortingLayerName("Coin");
        setRegistration(CSprite.REG_TOP_LEFT);
        setWidth(100);
        setHeight(100);
        setRadius(50);
        setState(STATE_BOUNCING);
        render();
        CCoinManager.inst().add(this);
    }

    public override void update()
    {
        base.update();
        
        if(getZ() > 0)
        {
            setZ(0);
            setVelZ(getVelZ() * -1f * 0.8f);
        }
    }

    public override void render()
    {
        base.render();        
    }

    public override void destroy()
    {
        base.destroy();
    }


}

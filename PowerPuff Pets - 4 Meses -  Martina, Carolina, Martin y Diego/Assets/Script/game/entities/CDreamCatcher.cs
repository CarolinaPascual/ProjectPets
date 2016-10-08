using UnityEngine;
using System.Collections;

public class CDreamCatcher : CAnimatedSprite {

    public CDreamCatcher(float aX, float aY)
    {
        setFrames(Resources.LoadAll<Sprite>("Sprites/Objects/DreamCatcher/"));
        setSortingLayerName("Puzzles");
        setName("DreamCatcher");
        setXY(aX, aY);
        initAnimation(1, 4, 5, true);
        CDreamCatcherManager.inst().add(this);
    }

    public override void update()
    {
        base.update();
        
    }

    public override void render()
    {
        base.render();
    }

    public override void destroy()
    {
        base.destroy();
    }

    override public void hit()
    {
        setDead(true);
        base.hit();
    }
}

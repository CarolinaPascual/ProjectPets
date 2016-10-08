using UnityEngine;
using System.Collections;

public class CCloud : CSprite {
    //states, later will be changed just for size and adding a GOOD / BAD states separately to change the image depending on the levelstate
    //possible change to CAnimatedSprite for that
    static int SMALL_BAD = 0;
    static int MEDIUM_BAD = 1;
    static int LARGE_BAD = 2;
    static int SMALL_GOOD = 3;
    static int MEDIUM_GOOD = 4;
    static int LARGE_GOOD = 5;

    public CCloud(int aCloudSpeed, int aCloudType, int aX, int aY)
    {
        setImage(Resources.Load<Sprite>("Sprites/Clouds/cloud0" + aCloudType.ToString()));
        setSortingLayerName("Background");
        setName("cloud");
        setXY(aX,aY);
        setVelX(-aCloudSpeed);
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
}

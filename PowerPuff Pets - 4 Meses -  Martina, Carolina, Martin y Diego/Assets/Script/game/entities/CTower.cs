using UnityEngine;
using System.Collections;

public class CTower : CAnimatedSprite {

    public const int RANGE = 0;
    public const int MELEE = 1;
    public const int SIEGE = 2;


    public const int NOT_SELECTED = 0;
    public const int MOUSE_OVER = 1;
    public const int SELECTED = 2;

    public static int TOWER_WIDTH = 250;
    public static int TOWER_HEIGHT = 350;
    public int mLevel = 0;
    
    public CTower(int aType)
    {
        setType(aType);
        switch (getType())
        {
            case CTower.RANGE:
                setFrames(Resources.LoadAll<Sprite>("Sprites/towers/range"));
                setName("Range Tower");
                break;
            case CTower.MELEE:
                setFrames(Resources.LoadAll<Sprite>("Sprites/towers/melee"));
                setName("Melee Tower");
                break;
            case CTower.SIEGE:
                setFrames(Resources.LoadAll<Sprite>("Sprites/towers/siege"));
                setName("Siege Tower");
                break;
        }
        setState(CTower.NOT_SELECTED);
        setWidth(TOWER_WIDTH);
        setHeight(TOWER_HEIGHT);
        setRadius(125);
        setRegistration(CSprite.REG_DOWN_MIDDLE);
        setSortingLayerName("Player");
        setScale(0.5f);
        render();
    }

    public override void update()
    {
        if(getState() == CTower.NOT_SELECTED)
        {

        }
        else if (getState() == CTower.MOUSE_OVER)
        {

        }
        else if (getState() == CTower.SELECTED)
        {

        }
        base.update();
    }

    public override void setState(int aState)
    {
        base.setState(aState);
        setVisible(true);
        if (getState() == CTower.NOT_SELECTED)
        {
            Debug.Log("STATE NOT_SELECTED");
            initAnimation(1, 1, 3, false);
        }
        else if (getState() == CTower.MOUSE_OVER)
        {
            Debug.Log("STATE MOUSE_OVER");
            initAnimation(1, 1, 3, false);
        }
        else if (getState() == CTower.SELECTED)
        {
            Debug.Log("STATE SELECTED");
            initAnimation(1, 1, 3, false);
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

using UnityEngine;
using System.Collections;

public class CProyectile : CAnimatedSprite
{
    public const int RANGE = 0;
    public const int SIEGE = 1;

    public const int NONE = 0;
    public const int IMPACT = 1;

    public static int PROYECTILE_WIDTH = 250;
    public static int PROYECTILE_HEIGHT = 350;

    public CProyectile(int aType,CGameObject aTarget)
    {
        setType(aType);
        switch (getType())
        {
            case CProyectile.RANGE:
                setFrames(Resources.LoadAll<Sprite>("Sprites/units/proyectiles/arrows"));
                setName("Arrow");
                break;
            case CProyectile.SIEGE:
                setFrames(Resources.LoadAll<Sprite>("Sprites/units/proyectiles/boulders"));
                setName("Boulder");
                break;
        }
        setDamage(10);
        setSpeed(200);
        setRadius(25);
        setState(CTower.NOT_SELECTED);
        setWidth(PROYECTILE_WIDTH);
        setHeight(PROYECTILE_HEIGHT);
        setRegistration(CSprite.REG_TOP_LEFT);
        setSortingLayerName("Proyectiles");
        CPlayerManager.inst().add(this);
        #region proyectile
        /*CVector pSpeedXY = aTarget.getPos() - getPos();
        Debug.Log("X: " + pSpeedXY.x + "Y: " + pSpeedXY.y + "Z: " + pSpeedXY.z);
        pSpeedXY.setLenght(getSpeed());
        Debug.Log("X: " + pSpeedXY.x + "Y: " + pSpeedXY.y + "Z: " + pSpeedXY.z);
        //Arreglar para corregir con enemigos en movimiento
        setVelX(pSpeedXY.x);
        setVelY(pSpeedXY.y);
        float pTime = (aTarget.getX() - getX()) / getVelX();
        float pVelZ = CMath.getGreaterSquareRoot(2, -1350, (getZ() * 450 + (450 * 450 * pTime * pTime)));
        setVelZ(pVelZ);
        setAccelZ(900);
        */
        CVector pSpeed = aTarget.getPos() - CGameObject.GRAVITY;
        pSpeed.normalize();
        pSpeed = pSpeed * getSpeed();
        setVel(pSpeed);
        setAccelZ(900);
        #endregion
        render();
    }
    public override void setState(int aState)
    {
        base.setState(aState);
        if (getState() == NONE)
        {
            initAnimation(1, 1, 10, true);
        }
        else if (getState() == IMPACT)
        {
            stopMove();
            initAnimation(4,6,10,false);
        }
    }

    override public void update()
    {
        if(getState()== CProyectile.NONE)
        {
            CGameObject enemy = CEnemyManager.inst().collides(this);

            if (enemy != null)
            {
                setState(IMPACT);
                enemy.impact(this);
                return;
            }
        }
        if (getState() == IMPACT)
        {
            if (isEnded())
            {
                setDead(true);
            }
        }
        


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

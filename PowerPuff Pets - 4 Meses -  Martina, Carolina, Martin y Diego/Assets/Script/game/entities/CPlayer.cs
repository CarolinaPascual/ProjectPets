using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CPlayer : CSteeredVehicle
{
    public const int RANGE = 0;
    public const int MELEE = 1;
    public const int SIEGE = 2;

    // STATE MACHINE
    public const int STATE_STAND = 0;
    public const int STATE_WALKING = 1;
    public const int STATE_FREEZE = 2;
    public const int STATE_FALLING_DEAD = 3;
    public const int STATE_ATTACK = 4;
    public const int STATE_GAME_OVER = 5;
    public const int STATE_EXPLODING = 6;
    
    
    private const int TIME_FREEZE = 30;
    private const int MAX_BULLETS = 1;

    //private int Y_LIMIT = -CGameConstants.SCREEN_HEIGHT - CPlayer.PLAYER_HEIGHT;
    private const int SPEED = 240;
    private const int SPEED_LADDER = 240;
    private const int TURN_SPEED = 5;
    private const float ACCEL = 0.1f;
    private const float FRICTION = 0.99f;

	public static int PLAYER_WIDTH = 111;
	public static int PLAYER_HEIGHT = 132;
    public static int X_OFFSET_BOUNDING_BOX = 50;
    public static int Y_OFFSET_BOUNDING_BOX = 50;
    private int mBulletCount;
    private CTile tileTopLeft;
    private CTile tileTopRight;
    private CTile tileMiddleLeft;
    private CTile tileMiddleRight;
    private CTile tileDownLeft;
    private CTile tileDownRight;
    private CTile tileFloorLeft;
    private CTile tileFloorRight;
    private int mIgnoreRowFloor = 1;
    private float mOldX;
    private float mOldY;
    private float mOldVelY = 0f;
    private float mRange;
    
    public CPlayer(float aX, float aY, int aType)
	{
        setType(aType);
        setXY(aX, aY);
		setFrames (Resources.LoadAll<Sprite> ("Sprites/player"));
        //setOldXYPosition();
        setMaxSpeed(CTileMap.TILE_HEIGHT);
        setName("Player");
        setSortingLayerName ("Player");
        setRegistration(CSprite.REG_DOWN_MIDDLE);
        setWidth(PLAYER_WIDTH);
        setHeight(PLAYER_HEIGHT);
        mOldX = getX();
        mOldY = getY();
        setState (STATE_STAND);
        render ();
        //ver como generar IDs unicos y a su ves utiles para broadcast y a la vez diferenciamiento de multiples unidades
        setID("PLAYER");
        setMass(1);
        setMaxSpeed(400);
        setMaxForce(1000);
        setBehavior(CSteeredVehicle.SEEK);
        setRange(CTileMap.MAP_WIDTH * 3);
        setInSightDist(CTileMap.MAP_WIDTH * 4);
        CPlayerManager.inst().add(this);
        
    }

    public void restartPlayer()
    {
        
               
        
        
    }

    override public void update()
    {
        /*CGameObject enemy = CEnemyManager.inst().collides(this);

        if (enemy != null)
        {
            //enemy.hit();//TODO: crear la funcion hit en el enemigo
            if (!isDead())
            {
                setDead(true);
            }
                
        }*/
        if (getTarget() == null)
        {

            
        }
        if (getState() == STATE_STAND)
        {
            //Behavior FSM
            if (getBehavior() == CSteeredVehicle.SEEK)
            {
                setTarget(findClosestEnemy());
                findPath();
                seekNext();
            }
            else if (getBehavior() == CSteeredVehicle.FLEE)
            {
                CVector enemyPos = new CVector();
                flee(enemyPos);
            }
            else if (getBehavior() == CSteeredVehicle.ARRIVE)
            {
                arriveNext();
            }
            else if (getBehavior() == CSteeredVehicle.PURSUE)
            {
                pursue(getTarget());
            }
            else if (getBehavior() == CSteeredVehicle.EVADE)
            {
                evade(getTarget());
            }
            else if (getBehavior() == CSteeredVehicle.WANDER)
            {
                wander();
            }
        }
        
        if (getState() == STATE_WALKING)
        {
            //Behavior FSM
            if (getBehavior() == CSteeredVehicle.SEEK)
            {
                //Checks for enemies in sight
                CGameObject enemy = CEnemyManager.inst().inSight(this);
                if (enemy != null)
                {
                    Debug.Log("enemy not null");
                    seek(enemy.getPos());
                    if (enemy.getPos().dist(getPos()) <= getRange())
                    {
                        Debug.Log("attacking enemy");
                        setTarget(enemy);
                        setState(CPlayer.STATE_ATTACK);
                        return;
                    }
                }
                
                
                  
                //multiplying distance by sqrt of 2 to make sure diagonals are covered, substitute sqrt(2) by 1.5 for performance reasons
                if (getPos().dist(getDestination()) <= CTileMap.TILE_WIDTH * 1.5)
                {
                    Debug.Log("intermediate point reached");
                    seekNext();
                }
            }
            else if(getBehavior() == CSteeredVehicle.FLEE)
            {
                //code to move away from a certain enemy X distance and change state once you get that far
            }
            else if (getBehavior() == CSteeredVehicle.ARRIVE)
            {
                if (getPos().dist(getDestination()) <= CTileMap.TILE_WIDTH *1.5)
                {
                    Debug.Log("intermediate point reached");
                    arriveNext();
                }
            }
            else if (getBehavior() == CSteeredVehicle.EVADE)
            {
                evade(getTarget());
            }
            else if (getBehavior() == CSteeredVehicle.WANDER)
            {
                wander();
            }
        }
        if (getState() == STATE_ATTACK)
        {
            if(getTimeState() > getFireRate())
            {
                setTimeState(0.0f);
                fire();
            }
            if (getTarget().isDead())
            {
                setState(STATE_STAND);
                setBehavior(SEEK);
                return;
            }
        }
            /*        
            else if (getState() == CPlayer.STATE_DYING)
            {
                if (isEnded())
                {
                    setDead(true);

                }
            }
            else if (getState() == CPlayer.STATE_STAND_AFTER_DEAD)
            {
                Debug.Log(getTimeState()*10);
                if (getTimeState() > 3)
                {
                    setState(CPlayer.STATE_STAND);
                    return;
                }

            }

        */

            //checkBorders();
            base.update();
        //setOldXYPosition();



    }

    public CGameObject findClosestEnemy()
    {
        CGameObject pClosest = null;
        for (int i = 0; i < CEnemyManager.inst().getArray().Count; i++)
        {
            CGameObject pEnemy = CEnemyManager.inst().getArray()[i];
            if(pClosest == null)
            {
                pClosest = pEnemy;
            }else
            {
                if(CMath.dist(this,pEnemy) < CMath.dist(this, pClosest))
                {
                    pClosest = pEnemy;
                }
            }
            
                        
        }
        return pClosest;
    }

    public void fire()
    {
        CProyectile aProy = new CProyectile(getType(),getTarget());
        aProy.setXYZ(getX(), getY(), -getHeight());
        
        //Calculo target
        // Zf = 1/2 * a * t2 + V0* t + Z0
        //float pVelZ = CMath.getGreaterSquareRoot(450, aProy.getVelZ(), aProy.getZ());
        //float pXf = aProy.getX() + aProy.getVelX() * pTime;
        //float pYf = aProy.getY() + aProy.getVelY() * pTime;
        //CCoin nuCoin2 = new CCoin();
        //nuCoin2.setXYZ(pXf, pYf, 0);
    }

    public float getRange()
    {
        return mRange;
    }

    public void setRange(float aRange)
    {
        mRange = aRange;
    }

    override public void OnMessage(CTelegram aMessage)
    {
        //no base call since all the code will be handled on each specific object

    }

    override public void render()
	{
        if (getState() == CPlayer.STATE_GAME_OVER)
        {
            return;
        }

        else if (getState() == CPlayer.STATE_FREEZE)
        {
            if ((int)(getTimeState()*10) % 2 == 0)
            {
                setVisible(true);
            }
            else
            {
                setVisible(false);
            }
        }
        
        base.render ();
		
		
	}
	
	override public void destroy()
	{
        base.destroy();
	}

    
    
    private void checkBorders()
    {
        if (getX() < 0)
        {
            setX(0);
        }
        if (getX() + getWidth() > CGameConstants.WORLD_WIDTH)
        {
            setX(CGameConstants.WORLD_WIDTH - getWidth());
        }
        if(getY() > CGameConstants.SCREEN_HEIGHT)
        {
            setDead(true);
        }
    }
    private void controlMoveHorizontal()
    {
        if (!CKeyboard.pressed(CKeyboard.LEFT) && !CKeyboard.pressed(CKeyboard.RIGHT))
        {
            setVelX(0);
        }
        else
        {
            if (isWallLeft(getX(), mOldY))
            {
                setX((((int)((getX() + CPlayer.X_OFFSET_BOUNDING_BOX) / CTileMap.TILE_WIDTH) + 1) * CTileMap.TILE_WIDTH) - CPlayer.X_OFFSET_BOUNDING_BOX);
                setVelX(0);
            }
            else if (isWallRight(getX(), mOldY))
            {
                setX(((((int)((getX() - CPlayer.X_OFFSET_BOUNDING_BOX + getWidth()) / CTileMap.TILE_WIDTH)) * CTileMap.TILE_WIDTH) - getWidth()) + CPlayer.X_OFFSET_BOUNDING_BOX);
                setVelX(0);
            } else if (CKeyboard.pressed(CKeyboard.LEFT))
            {
                if (!isWallLeft(getX() - CPlayer.SPEED * Time.deltaTime,mOldY))
                {
                    setVelX(-CPlayer.SPEED);
                    setFlipH(true);
                }
            }
            else if(CKeyboard.pressed(CKeyboard.RIGHT))
            {
                if (!isWallRight(getX() + CPlayer.SPEED * Time.deltaTime, mOldY))
                {
                    setVelX(CPlayer.SPEED);
                    setFlipH(false);
                }
            }
        }
    }

 
    //TODO: REDO THE STATEMENTS SINCE ITS ALL COPY PASTE
    override public void setState(int aState)
	{
        if (getState() != aState)
        {
            base.setState(aState);
        setVisible (true);
        if (getState () == STATE_STAND) 
		{
            Debug.Log("STATE STAND");
            initAnimation (1, 1, 3, false);
			stopMove();
            
		}
        else if (getState () == STATE_WALKING) 
		{
            Debug.Log("STATE WALKING");
            initAnimation (1, 6, 5, true);
		}
        else if (getState() == STATE_FREEZE)
        {
            Debug.Log("STATE FREEZE");
            //initAnimation(2, 9, 10, true);
        }
        else if (getState() == STATE_FALLING_DEAD)
        {
            Debug.Log("STATE FALLING DEAD");
            //initAnimation(2, 9, 10, true);
        }
        else if (getState() == STATE_ATTACK)
        {
            Debug.Log("STATE_ATTACK");
            stopMove();
            setBehavior(NONE);
            initAnimation(7, 13, 8, true);
        }
        else if (getState() == STATE_GAME_OVER)
        {
            Debug.Log("STATE GAME OVER");
            //initAnimation(2, 9, 10, true);
        }
        
        }
        
       
    }
    

    public void checkPoints(float aX, float aY){
		int leftX = (int)((aX + CPlayer.X_OFFSET_BOUNDING_BOX) / CTileMap.TILE_WIDTH);
		int upY = (int)((aY + CPlayer.Y_OFFSET_BOUNDING_BOX) / CTileMap.TILE_HEIGHT);
		int rightX = (int)((aX + getWidth () - 1 - CPlayer.X_OFFSET_BOUNDING_BOX) / CTileMap.TILE_WIDTH);
		int downY = (int)((aY + getHeight () - 1 - CPlayer.Y_OFFSET_BOUNDING_BOX) / CTileMap.TILE_HEIGHT);
        int floorY = (int)((aY + getHeight() - 1) / CTileMap.TILE_HEIGHT);
        tileTopLeft = CTileMap.inst().getTile(leftX, upY);
		tileTopRight = CTileMap.inst().getTile(rightX, upY);
		tileDownLeft = CTileMap.inst().getTile(leftX, downY);
		tileDownRight = CTileMap.inst().getTile(rightX, downY);
        tileFloorLeft = CTileMap.inst().getTile(leftX, floorY);
        tileFloorRight = CTileMap.inst().getTile(rightX,floorY);
        //Debug.Log("tileTopLeft " + tileTopLeft.isWalkable());
        //Debug.Log("tileTopRight " + tileTopRight.isWalkable());
        //Debug.Log("tileDownLeft " + tileDownLeft.isWalkable());
        //Debug.Log("tileDownRight" + tileDownRight.isWalkable());
        //Debug.Log("tileFloorLeft" + tileFloorLeft.isWalkable());
        //Debug.Log("tileFloorRight" + tileFloorRight.isWalkable());
        
    }

    public bool isWallLeft(float aX, float aY)
    {
        checkPoints(aX, aY);
        //Debug.Log("isWallLeft " + (!tileTopLeft.isWalkable() || !tileDownLeft.isWalkable()).ToString());
        return !tileTopLeft.isWalkable() || !tileDownLeft.isWalkable() || !tileFloorLeft.isWalkable();
    }

    public bool isWallRight(float aX, float aY)
    {
        checkPoints(aX, aY);
        //Debug.Log("isWallRight " +(!tileTopRight.isWalkable() || !tileDownRight.isWalkable() || !tileFloorRight.isWalkable()));
        return !tileTopRight.isWalkable() || !tileDownRight.isWalkable() || !tileFloorRight.isWalkable();
    }
    
    
    public bool isWallAbove(float aX, float aY)
    {
        checkPoints(aX, aY);
        //Debug.Log("isWallAbove " +(!tileTopLeft.isWalkable() || !tileTopRight.isWalkable().ToString());
        return !tileTopLeft.isWalkable() || !tileTopRight.isWalkable();
    }

    public bool isWallBelow(float aX, float aY)
    {
        checkPoints(aX, aY);
        //Debug.Log("isWallBelow " +(!tileFloorLeft.isWalkable() || !tileFloorRight.isWalkable()).ToString());
        return !tileFloorLeft.isWalkable() || !tileFloorRight.isWalkable();
    }

    

    
    public void bulletDestroyed()

    {
        mBulletCount -= 1;
    }

    public bool isGameOver()
    {
        return getState() == CPlayer.STATE_GAME_OVER;
    }

    public bool isStateNormal()
    {
        return getState() == CPlayer.STATE_STAND || getState() == CPlayer.STATE_WALKING;
    }

    private void setOldXYPosition()
    {
        mOldX = getX();
        mOldY = getY();
        mOldVelY = getVelY();
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CPlayer : CAnimatedSprite
{
    // STATE MACHINE
    private const int STATE_STAND = 0;
	private const int STATE_WALKING = 1;
    private const int STATE_FREEZE = 2;
    private const int STATE_FALLING_DEAD = 3;
    private const int STATE_STAND_AFTER_DEAD = 4;
    private const int STATE_GAME_OVER = 5;
    private const int STATE_EXPLODING = 6;
    private const int STATE_JUMP = 7;
    private const int STATE_FALL = 8;
    private const int STATE_START_CLIMB_DOWN = 9;
    private const int STATE_CLIMB = 10;
    private const int STATE_END_CLIMB = 11;
    //From above to climb down to the platform, as seen from the side
    private const int STATE_START_HANG = 12;
    //Idle while hanging from the side of a platform
    private const int STATE_HANG = 13;
    //From below to climb up to the platform, as seen from the side
    private const int STATE_END_HANG = 14;
    private const int STATE_START_CRAWL = 15;
    private const int STATE_CRAWL = 16;
    private const int STATE_END_CRAWL = 17;
    private const int STATE_DYING = 18;
    private const int STATE_WINNING = 19;
    
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
    List<CNode> mPath;
    //temp variable to test path finding
    public int pathPos = 0;
    


    public CPlayer()
	{
		setFrames (Resources.LoadAll<Sprite> ("Sprites/player"));
        //setOldXYPosition();
        setMaxSpeed(CTileMap.TILE_HEIGHT);
        setName("Player");
        setSortingLayerName ("Player");
        setRegistration(CSprite.REG_DOWN_LEFT);
        setWidth(PLAYER_WIDTH);
        setHeight(PLAYER_HEIGHT);
        mOldX = getX();
        mOldY = getY();
        setState (STATE_STAND);
        render ();
        mPath = findPath();
        //ver como generar IDs unicos y a su ves utiles para broadcast y a la vez diferenciamiento de multiples unidades
        setID("PLAYER");
        CPlayerManager.inst().add(this);
        
	}

    public void restartPlayer()
    {
        
        setXY(0, CGameConstants.SCREEN_HEIGHT - getHeight() - CTileMap.TILE_HEIGHT / 2.5f);
        mOldX = getX();
        mOldY = getY();
        if (isDead())
        {
            setState(CPlayer.STATE_STAND_AFTER_DEAD);
            setDead(false);
            render();
            return;
        }
        else if(isGameOver())
        {
            setState(CPlayer.STATE_STAND);
            render();
            return;
        }
        
        
        
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

        if (getState() == STATE_STAND)
        {
            if (CKeyboard.pressed(CKeyboard.LEFT) || CKeyboard.pressed(CKeyboard.RIGHT))
            {
                setState(CPlayer.STATE_WALKING);
                return;
                
            }
        }
        
        if (getState() == STATE_WALKING)
        {
            if ((int)(getTimeState() * 10) % 1 == 0)
            {
                
                if (pathPos < mPath.Count)
                {
                    setXY(mPath[pathPos].getX() * CTileMap.TILE_WIDTH, mPath[pathPos].getY() * CTileMap.TILE_HEIGHT);
                    pathPos += 1;
                }else
                {
                    setState(CPlayer.STATE_STAND);
                    return;
                }
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

        else if (getState() == CPlayer.STATE_FREEZE || getState() == CPlayer.STATE_STAND_AFTER_DEAD)
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

    private List<CNode> findPath()
    {
        AStar mAStar = new AStar();
        if (mAStar.findPath(CTileMap.inst().getGrid()))
        {
            return mAStar.getPath();
        }
        return null;
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
        base.setState (aState);
        
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
        else if (getState() == STATE_STAND_AFTER_DEAD)
        {
            Debug.Log("STATE STAND AFTER DEAD");
            stopMove();
            initAnimation(1, 6, 3, true);
        }
        else if (getState() == STATE_GAME_OVER)
        {
            Debug.Log("STATE GAME OVER");
            //initAnimation(2, 9, 10, true);
        }
        else if (getState() == STATE_DYING)
        {
            Debug.Log("STATE DYING");
            stopMove();
            initAnimation(44, 45, 10, false);
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
        return getState() == CPlayer.STATE_STAND || getState() == CPlayer.STATE_WALKING || getState() == CPlayer.STATE_STAND_AFTER_DEAD || getState() == CPlayer.STATE_JUMP || getState() == CPlayer.STATE_FALL;
    }

    private void setOldXYPosition()
    {
        mOldX = getX();
        mOldY = getY();
        mOldVelY = getVelY();
    }

}

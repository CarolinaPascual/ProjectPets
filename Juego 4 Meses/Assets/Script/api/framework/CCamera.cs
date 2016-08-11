using UnityEngine;
using System.Collections;

public class CCamera : CGameObject {

    public const int WIDTH = CGameConstants.SCREEN_WIDTH;
    public const int HEIGHT = CGameConstants.SCREEN_HEIGHT;
    private CGameObject mGameObjectToFollow;
    public CCamera()
        {
        
        }

    public new int getWidth()
    {
        return CCamera.WIDTH;
    }

    public new int getHeight()
    {
        return CCamera.HEIGHT;
    }

    public void setGameObjectToFollow(CGameObject aGameObjectToFollow)
    {
        mGameObjectToFollow = aGameObjectToFollow;
    }

    public CGameObject getGameObjectToFollow()
    {
        return mGameObjectToFollow;
    }

    public override void update()
    {
        base.update();
        checkBorders();
    }

    private void checkBorders()
    {
        float xPlayer = mGameObjectToFollow.getX();
        CGame.inst().getCamera().setX(xPlayer - CGame.inst().getCamera().getWidth() / 2 + mGameObjectToFollow.getWidth() / 2);
        if (CGame.inst().getCamera().getX() < 0)
        {
            CGame.inst().getCamera().setX(0);
        }
        else if (CGame.inst().getCamera().getX() + CGame.inst().getCamera().getWidth() > CGameConstants.WORLD_WIDTH)
        {
            CGame.inst().getCamera().setX(CGameConstants.WORLD_WIDTH - CGame.inst().getCamera().getWidth());
        }


        float yPlayer = mGameObjectToFollow.getY();
        CGame.inst().getCamera().setY(yPlayer - CGame.inst().getCamera().getHeight() / 2 + mGameObjectToFollow.getHeight() / 2);
        if (CGame.inst().getCamera().getY() < -CGameConstants.WORLD_HEIGHT)
        {
            CGame.inst().getCamera().setY(-CGameConstants.WORLD_HEIGHT);
        }
        else if (CGame.inst().getCamera().getY() + CGame.inst().getCamera().getHeight() > CGameConstants.SCREEN_HEIGHT)
        {
            CGame.inst().getCamera().setY(CGameConstants.SCREEN_HEIGHT - CGame.inst().getCamera().getHeight());
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

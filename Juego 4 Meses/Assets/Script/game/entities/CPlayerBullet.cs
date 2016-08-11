using UnityEngine;
using System.Collections;

public class CPlayerBullet : CGenericBullet
{
	private CSprite mBulletSprite;
	
	public const float SPEED = -1800;
	
	public CPlayerBullet()
	{
		mBulletSprite = new CSprite ();
		mBulletSprite.setImage (Resources.Load<Sprite>("Sprites/bullet"));

		mBulletSprite.setName ("PlayerBullet");
		mBulletSprite.setSortingLayerName ("Bullets");

		setRadius (5);

		render ();
	}
	
	override public void update()
	{
		base.update ();
		mBulletSprite.update ();

		if (getY () < -getRadius ()) 
		{
			setDead (true);
		}

		CGenericEnemy enemy = CEnemyManager.inst ().collides (this) as CGenericEnemy;
		if (enemy != null) 
		{
			setDead (true);
			enemy.setDead(true);

			// TODO: ASTEROIDE SE ROMPE.
		}
	}
	
	override public void render()
	{
		base.render ();
		
		mBulletSprite.setX (getX ());
		mBulletSprite.setY (getY ());
		
		mBulletSprite.render ();
	}
	
	override public void destroy()
	{
		base.destroy ();	
		mBulletSprite.destroy ();
		mBulletSprite = null;
	}
}

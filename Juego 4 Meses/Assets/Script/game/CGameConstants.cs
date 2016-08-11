using UnityEngine;
using System.Collections;

public class CGameConstants
{
	public const int SCREEN_WIDTH = 1920;
	public const int SCREEN_HEIGHT = 1080;
    public const int WORLD_WIDTH = 12800;
    public const int WORLD_HEIGHT = 12800;
    public const int BACKGROUND_WIDTH = 1000;
    public const int BACKGROUND_HEIGHT = 1000;
    public const int WALL_WIDTH = 800;
    public const int WALL_HEIGHT = 800;
    public static readonly int[][] WALL_CONFIGURATIONS = {
                new int[] {3,2,1,0},
                new int[] {3,2,0,1},
                new int[] {0,3,2,0},
                new int[] {0,3,2,1},
                new int[] {1,3,2,1},
                new int[] {1,3,2,0},
                new int[] {0,0,0,0},
                new int[] {0,0,3,2},
                new int[] {1,1,3,2},
                new int[] {0,1,3,2},
                new int[] {1,0,3,2},
                new int[] {0,1,0,1},
                new int[] {1,0,1,0},
                new int[] {1,0,0,1},
                };

}

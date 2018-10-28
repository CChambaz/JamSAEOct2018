using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum floortype
{
    FLOWER = 20,
    ROCK = 40,
    NORMAL = 60,
}



public class BoardCreator : MonoBehaviour
{
    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        WALL = 0,
        FLOOR = 1
    }
    private int columns, rows;                             // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange(15, 20);         // The range of the number of rooms there can be.
    public IntRange roomWidth = new IntRange(3, 10);         // The range of widths rooms can have.
    public IntRange roomHeight = new IntRange(3, 10);        // The range of heights rooms can have.
    public IntRange corridorLength = new IntRange(6, 10);    // The range of lengths corridors between rooms can have.
    public GameObject[] floorTiles;
    public GameObject[] floor;// An array of floor tile prefabs.
    public GameObject[] wallTiles;                            // An array of wall tile prefabs.
    public GameObject[] outerWallTilesV;
    public GameObject[] outerWallTilesH;   // An array of outer wall tile prefabs.
    public GameObject player;
    public GameObject key;
    public GameObject door;

    public Sprite WallTopLeft;
    public Sprite WallTopMiddle;
    public Sprite WallTopRight;

    public Sprite WallBotLeft;
    public Sprite WallBotRight;

    public Sprite WallMiddleLeft;
    public Sprite WallMiddleRight;


    public Sprite barrierHMiddle;
    public Sprite barrierHLeft;
    public Sprite barrierHRight;

    public Sprite barrierVMiddle;

    private int[,] tiles;
    private GameObject[,] map_gameobject;
    // A jagged array of tile types representing the board, like a grid.
    private Room[] rooms;                                     // All the rooms that are created for this board.
    private Corridor[] corridors;                             // All the corridors that connect the rooms.
    private GameObject boardHolder;                          // GameObject that acts as a container for all other tiles.

    public Room[] GetRooms()
    {
        return rooms;
    }

    public int[,] Tiles
    {
        get
        {
            return tiles;
        }

        set
        {
            tiles = value;
        }
    }

    public void Init(int _columns, int _rows)
    {
        //_map = new GameObject[_columns, _rows];
        this.columns = _columns;
        this.rows = _rows;
        // Create the board holder.
        boardHolder = new GameObject("BoardHolder");

        SetupTilesArray();


        CreateRoomsAndCorridors();

        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        //SetupMap();

        GameObject[,] map_gameobject = new GameObject[rows, columns];
        int[,] Sides = new int[rows, columns];
        int[,] direction_out = new int[rows, columns];

        
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {

                    if (Tiles[x, y] == 0)
                    {
                    Vector3 pos = new Vector3(x, y, 0);
                    map_gameobject[x,y] = InstantiateFromArray(floor, pos.x, pos.y);
                    for (int j = -1; j <= +1; j++)
                    {
                        for (int k = -1; k <= +1; k++)
                        {
                            if (j != k)
                            {
                                if (IsInMapRange(x + j, y + k))
                                {
                                    if (Tiles[x + j, y + k] == 0)
                                    {
                                        direction_out[x + j, y + k] += 1;

                                        if (j == -1 && k == 0)
                                        {
                                            Sides[x + j, y + k] += 1;
                                        }
                                        if (j == 0 && k == 1)
                                        {
                                            Sides[x + j, y + k] += 2;
                                        }

                                        if (j == 0 && k == -1)
                                        {
                                            Sides[x + j, y + k] += 3;
                                        }

                                        if (j == 1 && k == 0)
                                        {
                                            Sides[x + j, y + k] += 4;
                                        }
                                        //W1 - S2 - N3 - E4
                                        //Sides[x + j, y + k] += i;

                                    }
                                }
                            }
                        }

                    }
                }
                    else
                    {
                        InstantiateFromArray(floorTiles, x, y);
                    }
            }
        }

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {

                if (Tiles[x, y] == 0)
                {
                    Vector3 pos = new Vector3(x, y, 0);

                    map_gameobject[x, y] = Instantiate(wallTiles[0], pos, Quaternion.identity);
                    //print(Sides[x, y]);
                    switch (Sides[x, y])
                    {
                        //Left
                        case 0:
                            //map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = Walls[0];
                            //map_gameobject[x, y].transform.localScale = new Vector3(1.6f, 2.1f, 1f);
                            map_gameobject[x, y].GetComponent<SpriteRenderer>().color = Color.blue;
                            break;
                        //top
                        case 1:
                            //map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = Sides_Left_Right[1];
                            //map_gameobject[x, y].transform.localScale = new Vector3(1.6f, 2.1f, 1f);
                            map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = barrierHLeft;
                            if (direction_out[x, y] == 0)
                            {


                            }
                            break;
                        case 2:
                            //map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = Sides_Top_Bottom[0];
                            //map_gameobject[x, y].transform.localScale = new Vector3(1.6f, 2.1f, 1f);
                            //map_gameobject[x, y].GetComponent<SpriteRenderer>().color = Color.grey;
                            break;
                        case 3:
                           
                             map_gameobject[x, y].GetComponent<SpriteRenderer>().color = Color.yellow;
                            if (direction_out[x, y] == 1)
                            {
                                //map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = Sides_Top_Bottom[1];
                              //  map_gameobject[x, y].GetComponent<SpriteRenderer>().color = Color.red;
                            }
                            if (direction_out[x, y] == 2)
                            {
                                //map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = Sides_Top_Bottom[1];
                                //map_gameobject[x, y].GetComponent<SpriteRenderer>().color = Color.magenta;
                            }
                            if (direction_out[x, y] == 3)
                            {
                                //map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = Sides_Top_LeftRight[1];
                                map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = WallTopLeft;
                                //Border_ Bottom_ Left
                            }

                            break;
                        case 4:
                            if (direction_out[x, y] == 1)
                            {
                                //map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = Sides_Left_Right[1];
                                map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = barrierHRight;
                            }
                            if (direction_out[x, y] == 2)
                            {
                             
                                if (Tiles[x, y + 1] == 0)
                                {
                                    //map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = Wall_Side_TopRight;
                                    map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = WallBotLeft;
                                }
                                else
                                {

                                    //map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = Sides_Left_Right[1];
                                    map_gameobject[x, y].GetComponent<SpriteRenderer>().color = Color.black;
                                }
                            }

                            if (direction_out[x, y] == 3)
                            {
                                //map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = Wall_Side_TopRight;
                                map_gameobject[x, y].GetComponent<SpriteRenderer>().color = Color.black;

                            }
                            if (direction_out[x, y] == 4)
                            {
                               // map_gameobject[x, y].GetComponent<SpriteRenderer>().color = Color.blue;
                            }
                            //print(direction_out[x, y]);
                            break;
                        case 5:
                            map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = barrierHMiddle;
                          
                            if (direction_out[x, y] == 2)
                            {
                              //  map_gameobject[x, y].GetComponent<SpriteRenderer>().color = Color.magenta;
                                if (x == 0 || x == rows-1)
                                {
                                    map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = barrierVMiddle;
                                }
                            }

                            if (direction_out[x, y] == 3)
                            {
                                map_gameobject[x, y].GetComponent<SpriteRenderer>().color = Color.yellow;
                            }

                            break;
                        case 6:
                            map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = WallMiddleLeft;
                            if (direction_out[x, y] == 2)
                            {
                                map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = WallTopRight;


                            }

                            if (direction_out[x, y] == 3)
                            {
                                map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = WallTopRight;
                            }


                          
                            //map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = Wall_Side_BottomLeft;
                            break;
                        case 7:
                            map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = WallBotRight;


                            if (direction_out[x, y] == 4)
                            {
                                map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = WallTopMiddle;
                            }

                            if (direction_out[x, y] == 5)
                            {
                                map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = WallTopMiddle;
                            }

                            //map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = Wall_Side_TopRight;
                            break;
                        case 9:
                            map_gameobject[x, y].GetComponent<SpriteRenderer>().sprite = WallMiddleRight;
                            //   map_gameobject[x, y].GetComponent<SpriteRenderer>().color = Color.yellow;
                            break;
                    }
         
                }
            }
        }

        //InstantiateTiles();
        InstantiateOuterWalls();


    }

    void SetupTilesArray()
    {
        // Set the tiles jagged array to the correct width.
        Tiles = new int[columns, rows];
    }


    void CreateRoomsAndCorridors()
    {
        // Create the rooms array with a random size.
        rooms = new Room[numRooms.Random];

        // There should be one less corridor than there is rooms.
        corridors = new Corridor[rooms.Length];

        // Create the first room and corridor.
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        // Setup the first room, there is no previous corridor so we do not use one.
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        // Setup the first corridor using the first room.
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

        for (int i = 1; i < rooms.Length; i++)
        {
            // Create a room.
            rooms[i] = new Room();

            // Setup the room based on the previous corridor.
            rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);

            // If we haven't reached the end of the corridors array...
            if (i < corridors.Length)
            {
                // ... create a corridor.
                corridors[i] = new Corridor();

                // Setup the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }
        }
        if (GameObject.Find("Player(Clone)") == null)
        {
            Vector3 playerPos = new Vector3(corridors[corridors.Length - 1].EndPositionX, corridors[corridors.Length - 1].EndPositionY, 0);
            Instantiate(player, playerPos, Quaternion.identity);
            int itroom_rdm = Random.Range(0, rooms.Length - 1);
            int width_rdm = Random.Range(0, rooms[itroom_rdm].roomWidth);
            int height_rdm = Random.Range(0, rooms[itroom_rdm].roomHeight);
            Vector3 keypos = new Vector3(rooms[itroom_rdm].xPos + width_rdm, rooms[itroom_rdm].yPos + height_rdm, 0);
            //Instantiate(key, keypos, Quaternion.identity);
        }

    }


    void SetTilesValuesForRooms()
    {
        // Go through all the rooms...
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;

                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    Tiles[xCoord, yCoord] = (int)TileType.FLOOR;
                }
            }
        }
    }


    void SetTilesValuesForCorridors()
    {
        // Go through every corridor...
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];

            // and go through it's length.
            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                // Start the coordinates at the start of the corridor.
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                // Depending on the direction, add or subtract from the appropriate
                // coordinate based on how far through the length the loop is.
                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }

                // Set the tile at these coordinates to Floor.
                if(IsInMapRange(xCoord, yCoord))
                Tiles[xCoord, yCoord] = (int)TileType.FLOOR;
            }
        }
    }


    void InstantiateTiles()
    {
        // Go through all the tiles in the jagged array...
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                // If the tile type is Wall...
                if (Tiles[i, j] == (int)TileType.WALL)
                {
                    // ... instantiate a wall over the top.
                    InstantiateFromArray(wallTiles, i, j);
                }
                else
                {
                    if (Tiles[i, j] == (int)TileType.FLOOR)
                    {
                        InstantiateFromArray(floor, i, j);
                    }
                }
            }
        }
    }


    void InstantiateOuterWalls()
    {
        // The outer walls are one unit left, right, up and down from the board.
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        // Instantiate both vertical walls (one on each side).
        InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        // Instantiate both horizontal walls, these are one in left and right from the outer walls.
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }


    void InstantiateVerticalOuterWall(float xCoord, float startingY, float endingY)
    {
        // Start the loop at the starting value for Y.
        float currentY = startingY;

        // While the value for Y is less than the end value...
        while (currentY <= endingY)
        {
            int i;
            if (xCoord == 0)
            {
                i = 1;
            }
            else
            {
                i = 0;
            }

            // The position to be instantiated at is based on the coordinates.
            Vector3 position = new Vector3(xCoord, currentY, 0f);

            // Create an instance of the prefab from the random index of the array.
            GameObject tileInstance = Instantiate(outerWallTilesV[i], position, Quaternion.identity) as GameObject;

            //_map[, (int)yCoord] = tileInstance;
            // Set the tile's parent to the board holder.
            tileInstance.transform.parent = boardHolder.transform;

            currentY++;
        }
    }


    void InstantiateHorizontalOuterWall(float startingX, float endingX, float yCoord)
    {
        // Start the loop at the starting value for X.
        float currentX = startingX;

        // While the value for X is less than the end value...
        while (currentX <= endingX)
        {
            int i;
            if (yCoord == 0)
            {
                i = 0;
            }
            else
            {
                i = 1;
            }

            // The position to be instantiated at is based on the coordinates.
            Vector3 position = new Vector3(currentX, yCoord, 0f);

            GameObject tileInstance = Instantiate(outerWallTilesH[i], position, Quaternion.identity) as GameObject;

            //_map[, (int)yCoord] = tileInstance;
            // Set the tile's parent to the board holder.
            tileInstance.transform.parent = boardHolder.transform;

            currentX++;
        }
    }


    GameObject InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;
        //_map[, (int)yCoord] = tileInstance;
        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;

        return tileInstance;
    }

    public void AddSprite()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;
                    // The coordinates in the jagged array are based on the room's position and it's width and height.

                }
            }
        }
    }

    public void createDoor()
    {
        int itroom_rdm = Random.Range(0, rooms.Length - 1);
        int width_rdm = Random.Range(0, rooms[itroom_rdm].roomWidth);
        int height_rdm = Random.Range(0, rooms[itroom_rdm].roomHeight);
        Vector3 doorpos = new Vector3(rooms[itroom_rdm].xPos + width_rdm, rooms[itroom_rdm].yPos + height_rdm, 0);
        //Instantiate(door, doorpos, Quaternion.identity);
    }

    void SetupMap()
    {
        List<List<Vector2>> regions = new List<List<Vector2>>();
        int[,] mapFlags = new int[rows, columns];

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                // If the tile type is Wall...
                if (Tiles[i, j] == (int)TileType.WALL)
                {
                    List<Vector2> newRegion = GetRegionTiles(i, j);
                    regions.Add(newRegion);
                }
            }
        }

        foreach (var region in regions)
        {
            if (region.Count < 20)
            {
                foreach (var tile in region)
                {
                    if (tiles[(int)tile.x, (int)tile.y] == (int)TileType.WALL)
                        tiles[(int)tile.x, (int)tile.y] = 1;
                }
            }
        }
    }

    List<Vector2> GetRegionTiles(int startX, int startY)
    {
        List<Vector2> tmpTiles = new List<Vector2>();
        int[,] mapFlags = new int[rows, columns];
        int tileType = tiles[startX, startY];

        Queue<Vector2> queue = new Queue<Vector2>();
        queue.Enqueue(new Vector2(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0)
        {
            Vector2 tile = queue.Dequeue();
            tmpTiles.Add(tile);

            for (int x = startX - 1; x <= startY + 1; x++)
            {
                for (int y = startX - 1; y <= startY + 1; y++)
                {
                    if (IsInMapRange(x, y) && (y == tile.y || x == tile.x))
                    {
                        if (mapFlags[x, y] == 1 && tiles[x, y] == tileType)
                        {
                            mapFlags[x, y] = 0;
                            queue.Enqueue(new Vector2(x, y));
                        }
                    }
                }
            }
        }

        return tmpTiles;
    }


    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < rows && y >= 0 && y < columns;
    }

}
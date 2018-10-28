using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager: MonoBehaviour {
    [SerializeField] private GameObject[] enemyPrefab;
    Room[] rooms;
    
    Vector2[] positionSpawns;

    private int counter_enemy = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnEnemies(BoardCreator _board_creator)
    {
      
        rooms = new Room[_board_creator.GetRooms().Length];
        positionSpawns = new Vector2[_board_creator.GetRooms().Length];
        rooms = _board_creator.GetRooms();
        int number_room = 0;
        //while (counter_enemy < GameObject.Find("Scoring").GetComponent<ScoringManger>().GetSlimes())
        //{
        //    number_room = 0;
        //    foreach (Room room in rooms)
        //    {
        //        if (counter_enemy < GameObject.Find("Scoring").GetComponent<ScoringManger>().GetSlimes())
        //        {
        //            Position_Spawns[number_room] = new Vector2(room.xPos + (float)room.roomWidth / 2, room.yPos + (float)room.roomHeight / 2);

        //            Vector2 size_spawn = new Vector2(((int)(room.roomWidth / 2.0f) - 0.5f), ((room.roomHeight / 2)));
        //            Vector3[] pos_patrol = new Vector3[4];
        //            pos_patrol[0] = new Vector3(Position_Spawns[number_room].x - (size_spawn.x), Position_Spawns[number_room].y + (size_spawn.y / 2), 0);
        //            pos_patrol[1] = new Vector3(Position_Spawns[number_room].x - (size_spawn.x), Position_Spawns[number_room].y - (size_spawn.y), 0);
        //            pos_patrol[2] = new Vector3(Position_Spawns[number_room].x + (size_spawn.x) - 1, Position_Spawns[number_room].y - (size_spawn.y), 0);
        //            pos_patrol[3] = new Vector3(Position_Spawns[number_room].x + (size_spawn.x) - 1, Position_Spawns[number_room].y + (size_spawn.y / 2), 0);
        //            GameObject tmp = Instantiate(_enemy_prefab[Random.Range(0, _enemy_prefab.Length-1)], new     Vector2(room.xPos + (float)Random.Range(0, room.roomWidth - 1), room.yPos + (float)Random.Range(0, room.roomHeight - 1)) - new Vector2(0.5f, 0.5f), Quaternion.identity);
        //            //  tmp.GetComponent<Slime>().SetPositionsPatroller(pos_patrol);
        //            counter_enemy++;
        //        }
        //        number_room++;
        //    }
        //    number_room--;
        //}
       
    }

    private void OnDrawGizmos()
    {
        int number_room = 0;
        if (rooms != null)
        {
            foreach (Room room in rooms)
            {
                Vector3 deltaGizmos = new Vector3(0.5f, 0.5f, 0);
                Vector3 pos_room = new Vector3(room.xPos, room.yPos, 0);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(pos_room - deltaGizmos, new Vector3(pos_room.x, pos_room.y + room.roomHeight, pos_room.z) - deltaGizmos);
                Gizmos.DrawLine(pos_room - deltaGizmos, new Vector3(pos_room.x + room.roomWidth, pos_room.y, pos_room.z) - deltaGizmos);
                Gizmos.DrawLine(new Vector3(pos_room.x + room.roomWidth, pos_room.y + room.roomHeight, pos_room.z) - deltaGizmos, new Vector3(pos_room.x + room.roomWidth, pos_room.y, pos_room.z) - deltaGizmos);
                Gizmos.DrawLine(new Vector3(pos_room.x + room.roomWidth, pos_room.y + room.roomHeight, pos_room.z) - deltaGizmos, new Vector3(pos_room.x, pos_room.y + room.roomHeight, pos_room.z) - deltaGizmos);

                Gizmos.color = Color.cyan;
                Vector2 size_spawn = new Vector2(((int)(room.roomWidth / 2.0f) - 0.5f), ((room.roomHeight / 2)));
                Gizmos.DrawLine(new Vector3(positionSpawns[number_room].x - (size_spawn.x), positionSpawns[number_room].y + (size_spawn.y / 2), 0), new Vector3(positionSpawns[number_room].x + (size_spawn.x) - 1, positionSpawns[number_room].y + (size_spawn.y / 2), 0));
                Gizmos.DrawLine(new Vector3(positionSpawns[number_room].x - (size_spawn.x), positionSpawns[number_room].y + (size_spawn.y / 2), 0), new Vector3(positionSpawns[number_room].x - (size_spawn.x), positionSpawns[number_room].y - (size_spawn.y), 0));
                Gizmos.DrawLine(new Vector3(positionSpawns[number_room].x - (size_spawn.x), positionSpawns[number_room].y - (size_spawn.y), 0), new Vector3(positionSpawns[number_room].x + (size_spawn.x) - 1, positionSpawns[number_room].y - (size_spawn.y), 0));
                Gizmos.DrawLine(new Vector3(positionSpawns[number_room].x + (size_spawn.x) - 1, positionSpawns[number_room].y - (size_spawn.y), 0), new Vector3(positionSpawns[number_room].x + (size_spawn.x) - 1, positionSpawns[number_room].y + (size_spawn.y / 2), 0));

                Gizmos.color = Color.red;
                Gizmos.DrawLine(positionSpawns[number_room], new Vector3(positionSpawns[number_room].x, positionSpawns[number_room].y - 1, 0));
                Gizmos.DrawLine(positionSpawns[number_room], new Vector3(positionSpawns[number_room].x - 1, positionSpawns[number_room].y, 0));
                Gizmos.DrawLine(new Vector3(positionSpawns[number_room].x - 1, positionSpawns[number_room].y - 1, 0), new Vector3(positionSpawns[number_room].x - 1, positionSpawns[number_room].y, 0));
                Gizmos.DrawLine(new Vector3(positionSpawns[number_room].x - 1, positionSpawns[number_room].y - 1, 0), new Vector3(positionSpawns[number_room].x, positionSpawns[number_room].y - 1, 0));
                number_room++;
            }
        }
    }

}

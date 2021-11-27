using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonGeneration : MonoBehaviour
{
	[SerializeField] private Vector2 worldSize = new Vector2(4, 4);
	List<Vector2> takenPositions = new List<Vector2>();
	int gridSizeX, gridSizeY;
	[SerializeField] private int numberOfRooms = 20;
	[SerializeField] private float randomCompare = 0.2f;
	[SerializeField] private float randomCompareStart = 0.2f;
	[SerializeField] private float randomCompareEnd = 0.01f;
	// Map Fields
	[SerializeField] private List<Sprite> mapIcons;
	[SerializeField] private MiniMap miniMap;
	MapRoomBtn[,] mapRooms;
	// WorldField
	private GameManager gameManager;
	Room[,] rooms;
	[SerializeField] private Corridor CorridorPrefab;
	[SerializeField] private Room RoomPrefab; 
	[SerializeField] private Transform worldRoot;
	void Start()
	{
		gameManager = GameManager.Instance;

		if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
		{ // make sure we dont try to make more rooms than can fit in our grid
			numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
		}
		gridSizeX = Mathf.RoundToInt(worldSize.x); //note: these are half-extents
		gridSizeY = Mathf.RoundToInt(worldSize.y);
		CreateRooms(); //lays out the actual map
		SetRoomDoors(); //assigns the doors where rooms would connect
		gameManager.EnterRoom(rooms[gridSizeX, gridSizeY], rooms[gridSizeX, gridSizeY].StartPoint);
	}
	void CreateRooms()
	{
		//setup
		miniMap.SetWorldSize(gridSizeX, gridSizeY);
		rooms = new Room[gridSizeX * 2, gridSizeY * 2];
		mapRooms = new MapRoomBtn[gridSizeX * 2, gridSizeY * 2];
		// FirstRoom
		rooms[gridSizeX, gridSizeY] = Instantiate(RoomPrefab, worldRoot);
		takenPositions.Insert(0, Vector2.zero);
		Vector2 checkPos = Vector2.zero;
		mapRooms[gridSizeX, gridSizeY] = miniMap.AddRoom(Vector2.zero);
		mapRooms[gridSizeX, gridSizeY].SetupIcone(mapIcons[0]);
		//add rooms
		for (int i = 0; i < numberOfRooms - 1; i++)
		{
			float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
			randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
			//grab new position
			checkPos = NewPosition();
			//test new position
			if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
			{
				int iterations = 0;
				do
				{
					checkPos = SelectiveNewPosition();
					iterations++;
				} while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
				if (iterations >= 50)
					print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
			}
			//finalize position
			rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = Instantiate(RoomPrefab, worldRoot);
			rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY].gameObject.SetActive(false);
			RoomType type = (RoomType)Random.Range(2, RoomType.GetValues(typeof(RoomType)).Length);
			if (i == numberOfRooms - 2)
				type = RoomType.Boss;
			mapRooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = miniMap.AddRoom(checkPos);
			mapRooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY].SetupIcone(mapIcons[(int)type]);
			Debug.Log(type);
			takenPositions.Insert(0, checkPos);
		}
	}
	Vector2 NewPosition()
	{
		int x = 0, y = 0;
		Vector2 checkingPos = Vector2.zero;
		do
		{
			int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // pick a random room
			x = (int)takenPositions[index].x;//capture its x, y position
			y = (int)takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);//randomly pick wether to look on hor or vert axis
			bool positive = (Random.value < 0.5f);//pick whether to be positive or negative on that axis
			if (UpDown)
			{ //find the position bnased on the above bools
				if (positive)
				{
					y += 1;
				}
				else
				{
					y -= 1;
				}
			}
			else
			{
				if (positive)
				{
					x += 1;
				}
				else
				{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x, y);
		} while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY); //make sure the position is valid
		return checkingPos;
	}
	Vector2 SelectiveNewPosition()
	{ // method differs from the above in the two commented ways
		int index = 0, inc = 0;
		int x = 0, y = 0;
		Vector2 checkingPos = Vector2.zero;
		do
		{
			inc = 0;
			do
			{
				//instead of getting a room to find an adject empty space, we start with one that only 
				//as one neighbor. This will make it more likely that it returns a room that branches out
				index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
				inc++;
			} while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
			x = (int)takenPositions[index].x;
			y = (int)takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);
			bool positive = (Random.value < 0.5f);
			if (UpDown)
			{
				if (positive)
				{
					y += 1;
				}
				else
				{
					y -= 1;
				}
			}
			else
			{
				if (positive)
				{
					x += 1;
				}
				else
				{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x, y);
		} while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
		if (inc >= 100)
		{ // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
			print("Error: could not find position with only one neighbor");
		}
		return checkingPos;
	}
	int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
	{
		int ret = 0; // start at zero, add 1 for each side there is already a room
		if (usedPositions.Contains(checkingPos + Vector2.right))
		{ //using Vector.[direction] as short hands, for simplicity
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.left))
		{
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.up))
		{
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.down))
		{
			ret++;
		}
		return ret;
	}
	void SetRoomDoors()
	{
		for (int x = 0; x < ((gridSizeX * 2)); x++)
		{
			for (int y = 0; y < ((gridSizeY * 2)); y++)
			{
				if (rooms[x, y] == null)
				{
					continue;
				}
				rooms[x, y].Btn = mapRooms[x, y];
				Vector2 gridPosition = new Vector2(x, y);
				rooms[x, y].GridPos = gridPosition;
				if (y - 1 >= 0 && rooms[x, y - 1] != null)
				{
					// Create corridor
					Corridor newCorridor = Instantiate(CorridorPrefab, worldRoot);
					newCorridor.GridPos = new Vector2(x, y - 0.5f);
					newCorridor.SetRooms(rooms[x, y - 1], rooms[x, y]);
					rooms[x, y].C_Down = newCorridor;
					rooms[x, y - 1].C_Up = newCorridor;
					newCorridor.gameObject.SetActive(false);
					mapRooms[x, y].Corridors.Add(miniMap.AddCorridor(gridPosition + new Vector2(0f ,- 0.5f), 90, mapRooms[x, y], mapRooms[x, y - 1]));
				}
				if (y + 1 < gridSizeY * 2 && rooms[x, y + 1] != null)
				{
					Corridor newCorridor = Instantiate(CorridorPrefab, worldRoot);
					newCorridor.GridPos = new Vector2(x, y + 0.5f);
					newCorridor.SetRooms(rooms[x, y], rooms[x, y + 1]);
					rooms[x, y + 1].C_Down = newCorridor;
					rooms[x, y ].C_Up = newCorridor;
					newCorridor.gameObject.SetActive(false);
					mapRooms[x, y].Corridors.Add(miniMap.AddCorridor(gridPosition + new Vector2(0f, 0.5f), 90, mapRooms[x, y], mapRooms[x, y + 1]));
				}
				if (x - 1 >= 0 && rooms[x - 1, y] != null)
				{
					Corridor newCorridor = Instantiate(CorridorPrefab, worldRoot);
					newCorridor.GridPos = new Vector2(x - 0.5f, y);
					newCorridor.SetRooms(rooms[x - 1, y], rooms[x, y]);
					rooms[x - 1, y].C_Right = newCorridor;
					rooms[x, y].C_Left = newCorridor;
					newCorridor.gameObject.SetActive(false);
					mapRooms[x, y].Corridors.Add(miniMap.AddCorridor(gridPosition + new Vector2(-0.5f, 0f), 0, mapRooms[x, y], mapRooms[x - 1, y]));
				}
				if (x + 1 < gridSizeX * 2 && rooms[x + 1, y] != null)
				{
					Corridor newCorridor = Instantiate(CorridorPrefab, worldRoot);
					newCorridor.GridPos = new Vector2(x + 0.5f, y);
					newCorridor.SetRooms(rooms[x, y], rooms[x + 1, y]);
					rooms[x, y].C_Right = newCorridor;
					rooms[x + 1, y].C_Left = newCorridor;
					newCorridor.gameObject.SetActive(false);
					mapRooms[x, y].Corridors.Add(miniMap.AddCorridor(gridPosition + new Vector2(0.5f, 0f), 0, mapRooms[x, y], mapRooms[x + 1, y]));
				}
			}
		}
	}
}
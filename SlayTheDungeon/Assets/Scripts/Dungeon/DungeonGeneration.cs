using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonGeneration : MonoBehaviour
{
	private DungeonParameters dungeonSettings;
	[SerializeField] private Vector2 worldSize = new Vector2(4, 4);
	List<Vector2> takenPositions = new List<Vector2>();
	int gridSizeX, gridSizeY;
	private int numberOfRooms;
	[SerializeField] private float randomCompare = 0.2f;
	[SerializeField] private float randomCompareStart = 0.2f;
	[SerializeField] private float randomCompareEnd = 0.01f;
	// Map Fields
	[SerializeField] private List<Sprite> mapIcons;
	private MiniMap miniMap;
	MapRoomBtn[,] mapRooms;
	// WorldField
	private Stack<RoomType> types = new Stack<RoomType>();
	private GameManager gameManager;
	Room[,] rooms;
	[SerializeField] private Corridor CorridorPrefab;
	[SerializeField] private Room RoomPrefab; 
	[SerializeField] private Transform worldRoot;
	private int chestFrequency, fireFrequency, monsterFrequency;

    protected void Awake()
    {
        gameManager = GameManager.Instance;
		miniMap = GameUI.Instance.MiniMap;
    }

    void Start()
	{
		if (DungeonParameters.Instance) dungeonSettings = DungeonParameters.Instance;
		numberOfRooms = dungeonSettings ? dungeonSettings.NumberOfRoom : 12;
		Debug.Log("Number of Rooms " + numberOfRooms.ToString());
		chestFrequency = dungeonSettings ? dungeonSettings.ChestFrequency : 5;
		monsterFrequency = dungeonSettings ? dungeonSettings.MonsterFrequency : 5;
		fireFrequency = dungeonSettings ? dungeonSettings.FireCampFrequency : 5;
		int size = (int)Mathf.Ceil(Mathf.Sqrt(numberOfRooms));
		worldSize = new Vector2(size, size);
		gridSizeX = Mathf.RoundToInt(worldSize.x); //note: these are half-extents
		gridSizeY = Mathf.RoundToInt(worldSize.y);
		CreateTypes();
		CreateRooms(); //lays out the actual map
		SetRoomCorridors(); //assigns the doors where rooms would connect
		FindBossRoom();
		gameManager.EnterRoom(rooms[gridSizeX, gridSizeY], rooms[gridSizeX, gridSizeY].StartPoint);
	}

    private void CreateTypes()
    {
		while (types.Count < numberOfRooms - 1)
		{
			int percent = Random.Range(0, 100);
			if (fireFrequency >= 5)
            {
				types.Push(RoomType.Firecamp);
				if (percent < (fireFrequency - 5) * 20)
                {
					types.Push(RoomType.Firecamp);
				}
			}
			else
            {
				if (percent < fireFrequency * 20)
                {
					types.Push(RoomType.Firecamp);
				}
				else
                {
					types.Push(RoomType.None);
				}
            }
			if (chestFrequency >= 5)
			{
				types.Push(RoomType.Chest);
				if (percent < (chestFrequency - 5) * 20)
				{
					types.Push(RoomType.Chest);
				}
			}
			else
			{
				if (percent < chestFrequency * 20)
				{
					types.Push(RoomType.Chest);
				}
				else
				{
					types.Push(RoomType.None);
				}
			}
			if (monsterFrequency >= 5)
			{
				types.Push(RoomType.Monster);
				if (percent < (monsterFrequency - 5) * 20)
				{
					types.Push(RoomType.Monster);
				}
			}
			else
			{
				if (percent < monsterFrequency * 20)
				{
					types.Push(RoomType.Monster);
				}
				else
				{
					types.Push(RoomType.None);
				}
			}
		}
	}

    void CreateRooms()
	{
		//setup
		miniMap.SetWorldSize(gridSizeX, gridSizeY);
		rooms = new Room[gridSizeX * 2, gridSizeY * 2];
		mapRooms = new MapRoomBtn[gridSizeX * 2, gridSizeY * 2];
		// FirstRoom
		CreateOneRoom(Vector2.zero, true);
		Vector2 checkPos;
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
			CreateOneRoom(checkPos);
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
	void SetRoomCorridors()
	{
		for (int x = 0; x < ((gridSizeX * 2)); x++)
		{
			for (int y = 0; y < ((gridSizeY * 2)); y++)
			{
				if (rooms[x, y] == null)
				{
					continue;
				}
				Vector2 gridPosition = new Vector2(x, y);
				rooms[x, y].Btn = mapRooms[x, y];
				rooms[x, y].GridPos = gridPosition;
				if (y - 1 >= 0 && rooms[x, y - 1] != null)
				{
					CreateOneCorridor(new Vector2Int(x,y), new Vector2Int(x, y - 1));
				}
				if (x - 1 >= 0 && rooms[x - 1, y] != null)
				{
					CreateOneCorridor(new Vector2Int(x, y), new Vector2Int(x - 1, y));
				}
			}
		}
	}

	private void CreateOneCorridor(Vector2Int roomPos, Vector2Int neighborPos)
    {
		bool horizontal = roomPos.x != neighborPos.x;
		Vector2Int smallVector = Vector2Int.Min(roomPos, neighborPos);
		Vector2Int bigVector = Vector2Int.Max(roomPos, neighborPos);
		Vector2 between = new Vector2(((float)roomPos.x + (float)neighborPos.x) / 2, ((float)roomPos.y + (float)neighborPos.y) / 2);
		// Create corridor
		Corridor newCorridor = Instantiate(CorridorPrefab, worldRoot);
		newCorridor.GridPos = between;
		newCorridor.Level = Mathf.Max(rooms[bigVector.x, bigVector.y].Level, rooms[smallVector.x, smallVector.y].Level);
		newCorridor.SetRooms(rooms[smallVector.x, smallVector.y], rooms[bigVector.x, bigVector.y]);
		if (!horizontal)
        {
			rooms[bigVector.x, bigVector.y].C_Down = newCorridor;
			rooms[smallVector.x, smallVector.y].C_Up = newCorridor;
		}
		else
        {
			rooms[bigVector.x, bigVector.y].C_Left = newCorridor;
			rooms[smallVector.x, smallVector.y].C_Right = newCorridor;
		}

		newCorridor.gameObject.SetActive(false);
		float angle = horizontal ? 0 : 90;
		MapCorridor newMapCorridor = miniMap.AddCorridor(between, angle, mapRooms[bigVector.x, bigVector.y], mapRooms[smallVector.x, smallVector.y]);
		mapRooms[bigVector.x, bigVector.y].Corridors.Add(newMapCorridor);
		mapRooms[smallVector.x, smallVector.y].Corridors.Add(newMapCorridor);
		newCorridor.LinkedMapElement = newMapCorridor;
	}
	private void CreateOneRoom(Vector2 checkPos, bool firstRoom = false)
    {
		rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = Instantiate(RoomPrefab, worldRoot);
		rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY].gameObject.SetActive(false);
		RoomType type;
		if (firstRoom)
        {
			rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY].Level = 0;
			type = RoomType.None;
		}
		else
        {
			type = types.Pop();
			rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY].Level = FindLevelOfRoom(checkPos, takenPositions);
		}
		MapRoomBtn newMapRoom = miniMap.AddRoom(checkPos);
		rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY].SetupRoom(type);
		rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY].LinkedDungeonElement = newMapRoom;
		mapRooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = newMapRoom;
		mapRooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY].SetupIcone(mapIcons[(int)type]);
		takenPositions.Insert(0, checkPos);
	}
	private void FindBossRoom()
    {
		List<Room> minNeighborRooms = new List<Room>();
		int minNeighborsNumber = 5;
		for (int x = 0; x < ((gridSizeX * 2)); x++)
		{
			for (int y = 0; y < ((gridSizeY * 2)); y++)
			{
				if (rooms[x, y] == null)
				{
					continue;
				}
				Vector2 gridPosition = new Vector2(x, y);
				if (NumberOfNeighbors(gridPosition - new Vector2(gridSizeX, gridSizeY), takenPositions) < minNeighborsNumber)
                {
					minNeighborRooms.Clear();
					minNeighborRooms.Add(rooms[x, y]);
					minNeighborsNumber = NumberOfNeighbors(gridPosition - new Vector2(gridSizeX, gridSizeY), takenPositions);
				}
				else if (NumberOfNeighbors(gridPosition - new Vector2(gridSizeX, gridSizeY), takenPositions) == minNeighborsNumber)
                {
					minNeighborRooms.Add(rooms[x, y]);
				}
			}
		}
		List<Room> potentialBossRooms = new List<Room>();
		int maxDistance = 0;
		foreach(Room minNeighborRoom in minNeighborRooms)
        {
			if (minNeighborRoom.Level > maxDistance)
            {
				potentialBossRooms.Clear();
				potentialBossRooms.Add(minNeighborRoom);
				maxDistance = minNeighborRoom.Level;
            }
			else if (minNeighborRoom.Level == maxDistance)
            {
				potentialBossRooms.Add(minNeighborRoom);
            }
        }
		Room bossRoom = potentialBossRooms[Random.Range(0, potentialBossRooms.Count - 1)];
		bossRoom.SetupRoom(RoomType.Boss);
		bossRoom.Btn.SetupIcone(mapIcons[(int)RoomType.Boss]);
	}
	private int FindLevelOfRoom(Vector2 checkingPos, List<Vector2> usedPositions)
    {
		int level = gridSizeX + gridSizeY + 1;
		if (usedPositions.Contains(checkingPos + Vector2.right))
		{ //using Vector.[direction] as short hands, for simplicity
			level = Mathf.Min(rooms[(int)checkingPos.x + 1 + gridSizeX, (int)checkingPos.y + gridSizeY].Level + 1, level);
		}
		if (usedPositions.Contains(checkingPos + Vector2.left))
		{
			level = Mathf.Min(rooms[(int)checkingPos.x - 1 + gridSizeX, (int)checkingPos.y + gridSizeY].Level + 1, level);
		}
		if (usedPositions.Contains(checkingPos + Vector2.up))
		{
			level = Mathf.Min(rooms[(int)checkingPos.x + gridSizeX, (int)checkingPos.y + 1 + gridSizeY].Level + 1, level);
		}
		if (usedPositions.Contains(checkingPos + Vector2.down))
		{
			level = Mathf.Min(rooms[(int)checkingPos.x + gridSizeX, (int)checkingPos.y - 1 + gridSizeY].Level + 1, level);
		}
		return level;
	}
}

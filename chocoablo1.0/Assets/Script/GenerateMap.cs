﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour {

	public int width;
	public int height;


	public string seed;
	public bool useRandomSeed;

	[Range(0,100)]
	public int randomfillpercent;


	int[,] map;
	// Use this for initialization
	void Start () {
		generateMap ();


	}

	void Update (){
		if (Input.GetMouseButtonDown (0)) {
			generateMap ();
		}

	}


	void generateMap(){
		map= new int[width,height];
		RandomFillMap ();

		for (int i = 1; i < 5; i++) {
			SmoothMap();
		}

		ProcessMap ();

		int borderSize = 1;
		int[,] borderedMap= new int[width + 2*borderSize,height + 2*borderSize];

		for (int x = 0; x <borderedMap.GetLength(0); x++) {
			for (int y = 0; y < borderedMap.GetLength(1); y++) {
				if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize) {
					borderedMap [x, y] = map [x - borderSize, y - borderSize];
				}
				else {
					borderedMap [x, y] = 1;
				}
			}
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator>();
		meshGen.GenerateMesh (borderedMap, 1);
	}

	void ProcessMap(){
		List<List<Coord>> wallRegions = getRegions(1);

		int wallThresholdSize=50;

		foreach (List<Coord> wallRegion in wallRegions) {
			if(wallRegion.Count < wallThresholdSize){
				foreach (Coord tile in wallRegion) {
					map [tile.tilesX, tile.tilesY] = 0;
				}
			}

		}

		List<List<Coord>> roomRegions = getRegions(0);
		int roomThresholdSize=50;
		List<Room> survivingRooms = new List<Room> ();

		foreach (List<Coord> roomRegion in roomRegions) {
			if (roomRegion.Count < roomThresholdSize) {
				foreach (Coord tile in roomRegion) {
					map [tile.tilesX, tile.tilesY] = 1;
				}
			} 
			else {
				survivingRooms.Add(new Room(roomRegion, map));
			}
		}
		survivingRooms.Sort ();
		survivingRooms [0].isMainRoom = true;
		survivingRooms [0].isAccessibleFromMainRoom = true;
		connectClosestRoom (survivingRooms);
	}

	void connectClosestRoom(List<Room> allRooms, bool forceAccessibilityFromMain =false){

		List<Room> roomListA = new List<Room> ();
		List<Room> roomListB = new List<Room> ();

		if (forceAccessibilityFromMain) {
			foreach (Room r in allRooms) {
				if (r.isAccessibleFromMainRoom) {
					roomListB.Add (r);
				} else {
					roomListA.Add (r);
				}
			}
		}
		else {
			roomListA = allRooms;
			roomListB = allRooms;
		}

		int bestDistance = 0;
		Coord bestTileA = new Coord ();
		Coord bestTileB = new Coord ();
		Room bestRoomA = new Room ();
		Room bestRoomB = new Room ();
		bool possibleConnectedFound = false;

		foreach (Room roomA in roomListA) {
			if (!forceAccessibilityFromMain) {
				possibleConnectedFound = false;
				if (roomA.connectedRooms.Count > 0) {
					continue;
				}
			}

			foreach (Room roomB in roomListB) {
				if (roomA == roomB || roomA.isConnected(roomB)) {
					continue;
				}
			
				for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++) {
					for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++) {
						Coord tileA = roomA.edgeTiles [tileIndexA];
						Coord tileB = roomB.edgeTiles [tileIndexB];
						int distanceBetweenRooms = (tileA.tilesX - tileB.tilesX)*(tileA.tilesX - tileB.tilesX) + (tileA.tilesY - tileB.tilesY)*(tileA.tilesY - tileB.tilesY);
						if (distanceBetweenRooms < bestDistance || !possibleConnectedFound) {
							bestDistance = distanceBetweenRooms;
							possibleConnectedFound = true;
							bestTileA = tileA;
							bestTileB = tileB;
							bestRoomA = roomA;
							bestRoomB = roomB;
						}
					}
				}
			}

			if (possibleConnectedFound && !forceAccessibilityFromMain) {
				createPassage (bestRoomA, bestRoomB, bestTileA, bestTileB);	
			}
		}
		if (possibleConnectedFound && forceAccessibilityFromMain) {
			createPassage (bestRoomA, bestRoomB, bestTileA, bestTileB);
			connectClosestRoom (allRooms, true);
		}
		if (!forceAccessibilityFromMain) {
			connectClosestRoom (allRooms, true);
		}
	}

	void createPassage(Room roomA, Room roomB, Coord tileA, Coord tileB){
		Room.ConnectRoom (roomA, roomB);
		//Debug.DrawLine(coordToWorldPoint(tileA), coordToWorldPoint(tileB), Color.green , 200,false);

		List<Coord> line = getLine (tileA, tileB);
		foreach (Coord c in line) {
			DrawCircle (c, 1);
		}

	}

	void DrawCircle(Coord c, int r){
		for(int x = -r ; x<= r ;x++){
			for(int y = -r ; y<= r ;y++){
				if (x * x + y * y <= r * r) {
					int drawX = c.tilesX + x;
					int drawY = c.tilesY + y;
					if(IsInMapRange(drawX, drawY)){
						map[drawX, drawY] = 0;
					}
				}
			}
		}
	}

	List<Coord> getLine(Coord from, Coord to){
		List<Coord> line = new List<Coord> ();
		int x = from.tilesX;
		int y = from.tilesY;
		bool inverted = false;

		int dx = to.tilesX - x;
		int dy = to.tilesY - y;

		int step = Math.Sign (dx);
		int gradientStep = Math.Sign (dy);

		int longest = Mathf.Abs (dx);
		int shortest = Mathf.Abs (dy);

		if (longest < shortest) {
			inverted = true;
			longest = Mathf.Abs (dy);
			shortest = Mathf.Abs (dx);

			step = Math.Sign (dy);
			gradientStep = Math.Sign (dx);
		}

		int gradientAccumulation = longest / 2;
		for (int i = 0; i < longest; i++) {
			line.Add (new Coord (x, y));
			if (inverted) {
				y += step;
			} else {
				x += step;
			}	

			gradientAccumulation += shortest;
			if (gradientAccumulation >= longest) {
				if (inverted) {
					x += gradientStep;
				} else {
					y += gradientStep;
				}
				gradientAccumulation -= longest;
			}
		}
	
		return line;
	}



	Vector3 coordToWorldPoint (Coord Tile){
		return new Vector3(-width /2+ .5f + Tile.tilesX, 2, -height /2 + .5f + Tile.tilesY);
	}

	List<List<Coord>> getRegions(int tileType){
		List<List<Coord>> region = new List<List<Coord>> ();
		int[,] mapFlags = new int[width, height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (mapFlags [x, y] == 0 && map [x, y] == tileType) {
					List<Coord> newRegion = getRegionTiles (x, y);
					region.Add(newRegion);

					foreach (Coord tile in newRegion) {
						mapFlags [tile.tilesX, tile.tilesY] = 1;
					}

				}
			}
		}

		return region;
	}


	List<Coord> getRegionTiles(int startX, int startY){
		List<Coord> tiles = new List<Coord> ();
		int[,] mapFlags = new int[width, height];
		int tileType = map [startX, startY];

		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (new Coord (startX, startY));
		mapFlags [startX, startY] = 1;

		while (queue.Count > 0) {

			Coord tile = queue.Dequeue ();
			tiles.Add (tile);

			for (int x = tile.tilesX - 1; x <= tile.tilesX + 1; x++) {
				for (int y = tile.tilesY - 1; y <= tile.tilesY + 1; y++) {
					if( IsInMapRange(x,y)&& (y == tile.tilesY || x == tile.tilesX)){
						if(mapFlags[x,y] == 0 & map[x,y] == tileType){
							mapFlags[x,y] =1;
							queue.Enqueue(new Coord(x,y));
						}
					}
				}
			}
		}
		return tiles;

	}

	bool IsInMapRange(int x, int y){
		return (x >= 0 && x < width && y >= 0 && y < height);
	}
	


	void RandomFillMap(){
		if (useRandomSeed) {
			seed = System.DateTime.Now.Ticks.ToString();
		}
	
		System.Random prng = new System.Random (seed.GetHashCode());

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if(x==0|| x==width -1 || y==0 || y==height -1){
					map[x,y]=1;
				}
				else{
					map [x, y] = (prng.Next (0, 100) < randomfillpercent) ? 1 : 0;
				}
			}
		}
	}

	void SmoothMap(){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int neighbourWallTiles = GetSurroundingWallCount (x, y);

				if (neighbourWallTiles > 4)
					map [x, y] = 1;
				else if (neighbourWallTiles < 4)
					map [x, y] = 0;
			}
		}
	
	}

	int GetSurroundingWallCount(int gridX, int gridY){
		int wallCount = 0;
			for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++){
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
				if(IsInMapRange(neighbourX, neighbourY)){
				//if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map [neighbourX, neighbourY];
					}
				} else {
					wallCount++;
				}

			}
		}
		return wallCount;
	}

	struct Coord {
		public int tilesX;
		public int tilesY;

		public Coord(int x, int y){
			tilesX = x;
			tilesY = y;

		}


	}

	class Room : IComparable<Room>{
		public List<Coord> tiles;
		public List<Coord> edgeTiles;
		public List<Room> connectedRooms;
		public int roomSize;
		public bool isAccessibleFromMainRoom;
		public bool isMainRoom;

		public Room(){
		}

		public Room(List<Coord> roomTiles, int[,] map){
			tiles = roomTiles;
			roomSize = tiles.Count;
			connectedRooms = new List<Room>();
			edgeTiles = new List<Coord>();

			foreach( Coord tile in tiles){
				for(int x=tile.tilesX-1; x<=tile.tilesX +1;x++){
					for(int y=tile.tilesY-1; y<=tile.tilesY +1;y++){
						if(x==tile.tilesX ||y == tile.tilesY){
							if(map[x,y] == 1){
								edgeTiles.Add(tile);
							}
						}
					}
				}

			}
		
		}

		public void setAccessibleFromMainRoom(){
			if (!isAccessibleFromMainRoom) {
				isAccessibleFromMainRoom = true;
				foreach (Room r in connectedRooms) {
					r.setAccessibleFromMainRoom ();
				}
			}


		}

		public static void ConnectRoom(Room roomA, Room roomB){
			if (roomA.isAccessibleFromMainRoom) {
				roomB.setAccessibleFromMainRoom ();
			} else if (roomB.isAccessibleFromMainRoom) {
				roomA.setAccessibleFromMainRoom ();
			}

			roomA.connectedRooms.Add (roomB);
			roomB.connectedRooms.Add (roomA);
		}

		public bool isConnected(Room otherRoom){
			return connectedRooms.Contains (otherRoom);
		}

		public int CompareTo(Room otherRoom){
			return otherRoom.roomSize.CompareTo(roomSize);
		}


	}


}

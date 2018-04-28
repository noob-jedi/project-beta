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
	/*
	List<Coord> RegionTiles(int startX, int startY){
		List<Coord> tiles = new List<Coord> ();
		int[,] mapFlags = new int[width, height];
		int tileType = map [startX, startY];

		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (new Coord (startX, startY));
		mapFlags [startX, startY] = 1;

		while (queue.Count > 1) {
			Coord tile = queue.Dequeue ();
			tiles.Add (tile);
			for (int x = tile.tilesX - 1; x <= tile.tilesX + 1; x++) {
				for (int y = tile.tilesY - 1; y <= tile.tilesY + 1; x++) {
				}
			}
		}

	}*/


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
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
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

	void OnDrawGizmos(){
		/*if (map != null) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					Gizmos.color = (map [x, y] == 1) ? Color.black : Color.white;
					Vector3 pos = new Vector3 (-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
					Gizmos.DrawCube (pos, Vector3.one);
				}
			}
		}*/
	}


}

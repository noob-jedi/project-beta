﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {
	
	public SquareGrid squaregrid;
	public MeshFilter walls;
	public MeshFilter cave;

	public bool is2D;

	List<Vector3> vertices;
	List<int> triangles;

	Dictionary <int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>> ();
	List<List<int>> outlines = new List<List<int>> ();
	HashSet<int> checkedVertices = new HashSet<int> ();

	public void GenerateMesh(int[,] map, float Squaresize){
		triangleDictionary.Clear ();
		outlines.Clear ();
		checkedVertices.Clear ();

		squaregrid = new SquareGrid (map, Squaresize);
		vertices = new List<Vector3> ();
		triangles = new List<int> ();

		for (int x = 0; x < squaregrid.squares.GetLength (0); x++) {
			for (int y = 0; y < squaregrid.squares.GetLength (1); y++) {
				TriangulateSquare (squaregrid.squares [x, y]);
			}
		}

		Mesh mesh = new Mesh ();
		cave.mesh = mesh;

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray ();
		mesh.RecalculateNormals ();

		if (!is2D) {
			createWallMesh ();
		}
	}

	void createWallMesh(){
		
		CalculateMeshOutlines ();

		List<Vector3> wallVertices = new List<Vector3> ();
		List<int> wallTriangles = new List<int> ();
		Mesh wallMesh = new Mesh ();
		float wallHeight = 5;

		foreach (List<int> outline in outlines) {
			for (int i = 0; i < outline.Count - 1; i++) {
				int startIndex = wallVertices.Count;
				wallVertices.Add (vertices [outline [i]]); // left
				wallVertices.Add (vertices [outline [i+1]]); // Right
				wallVertices.Add (vertices [outline [i]] - Vector3.up*wallHeight); // bottom left
				wallVertices.Add (vertices [outline [i+1]] - Vector3.up*wallHeight); // bottom right

				wallTriangles.Add (startIndex + 0);
				wallTriangles.Add (startIndex + 2);
				wallTriangles.Add (startIndex + 3);

				wallTriangles.Add (startIndex + 3);
				wallTriangles.Add (startIndex + 1);
				wallTriangles.Add (startIndex + 0);


			}
		}
		wallMesh.vertices = wallVertices.ToArray ();
		wallMesh.triangles = wallTriangles.ToArray ();
		walls.mesh = wallMesh;

		MeshCollider wallCollider = walls.gameObject.GetComponent<MeshCollider> ();

		wallCollider.sharedMesh = wallMesh;
	}


	void TriangulateSquare(Square square){
		switch (square.configuration) {
		case 0:
			break;
		//1 point
		case 1:
			MeshFromPoints (square.centerLeft,  square.centerBottom, square.bottomLeft);
			break;
		case 2:
			MeshFromPoints (square.bottomRight, square.centerBottom,  square.centerRight);
			break;
		case 4:
			MeshFromPoints (square.topRight, square.centerRight, square.centerTop); 
			break;
		case 8:
			MeshFromPoints ( square.topLeft, square.centerTop,square.centerLeft);
			break;

		//2 points
		case 3:
			MeshFromPoints (square.centerRight, square.bottomRight, square.bottomLeft, square.centerLeft);
			break;
		case 5:
			MeshFromPoints (square.centerTop, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft,square.centerLeft );
			break;			
		case 9:
			MeshFromPoints (square.topLeft, square.centerTop, square.centerBottom, square.bottomLeft);
			break;
		case 6:
			MeshFromPoints (square.centerTop, square.topRight, square.bottomRight, square.centerBottom);
			break;
		case 10:
			MeshFromPoints (square.topLeft, square.centerTop, square.centerRight, square.bottomRight , square.centerBottom, square.centerLeft);
			break;
		case 12:
			MeshFromPoints ( square.topLeft, square.topRight, square.centerRight, square.centerLeft);
			break;	
			

		//3 points
		case 7:
			MeshFromPoints (square.centerTop, square.topRight, square.bottomRight, square.bottomLeft, square.centerLeft);
			break;
		case 11:
			MeshFromPoints (square.topLeft, square.centerTop, square.centerRight, square.bottomRight,  square.bottomLeft);
			break;
		case 13:
			MeshFromPoints (square.topLeft, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft);
			break;
		case 14:
			MeshFromPoints (square.topLeft, square.topRight, square.bottomRight, square.centerBottom, square.centerLeft);
			break;
		
		//4 points
		case 15:
			MeshFromPoints (square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
			checkedVertices.Add (square.topLeft.vertexIndex);
			checkedVertices.Add (square.topRight.vertexIndex);
			checkedVertices.Add (square.bottomLeft.vertexIndex);
			checkedVertices.Add (square.bottomRight.vertexIndex);
			break;

		}


	}

	void MeshFromPoints(params Node[] points){
		AssignVertices (points);

		if (points.Length >= 3)
			CreateTriangle (points [0], points [1], points [2]);
		if(points.Length>=4)
			CreateTriangle (points [0], points [2], points [3]);
		if (points.Length >= 5)
			CreateTriangle (points [0], points [3], points [4]);
		if (points.Length >= 6)
			CreateTriangle (points [0], points [4], points [5]);
	}

	void AssignVertices(Node[] points){
		for(int i = 0;i < points.Length;i++){
			if (points [i].vertexIndex == -1) {
				points [i].vertexIndex = vertices.Count;
				vertices.Add(points[i].position);
			}
		}
	}

	void CreateTriangle(Node a, Node b, Node c){
	 	triangles.Add(a.vertexIndex);
		triangles.Add(b.vertexIndex);
		triangles.Add(c.vertexIndex);

		Triangle triangle = new Triangle (a.vertexIndex, b.vertexIndex, c.vertexIndex);
		AddTriangleDictionary (triangle.vertexIndexA, triangle);
		AddTriangleDictionary (triangle.vertexIndexB, triangle);
		AddTriangleDictionary (triangle.vertexIndexC, triangle);
	}

	void AddTriangleDictionary(int vertexIndexKey, Triangle triangle){
		if (triangleDictionary.ContainsKey(vertexIndexKey)){
			triangleDictionary[vertexIndexKey].Add(triangle);
		}
		else{
			List<Triangle> triangleList = new List<Triangle>();
			triangleList.Add (triangle);
			triangleDictionary.Add (vertexIndexKey, triangleList);

		}
			
	}

	void CalculateMeshOutlines(){
		for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++) {
			if (!checkedVertices.Contains (vertexIndex)) {
				int newOutLineVertex = GetConnectedOutlineVertex (vertexIndex);
				if (newOutLineVertex != -1) {
					checkedVertices.Add (vertexIndex);

					List<int> newOutline = new List<int> ();
					newOutline.Add (vertexIndex);
					outlines.Add (newOutline);
					followOutline (newOutLineVertex, outlines.Count - 1);
					outlines[outlines.Count - 1].Add(vertexIndex);
				}
			}
		}
	}

	void followOutline(int vertexIndex, int outlineIndex){
		outlines [outlineIndex].Add(vertexIndex);
		checkedVertices.Add (vertexIndex);
		int nextVertexIndex = GetConnectedOutlineVertex (vertexIndex);
		if (nextVertexIndex != -1) {
			followOutline (nextVertexIndex, outlineIndex);
		}

	}

	int GetConnectedOutlineVertex(int vertexIndex){
		List<Triangle> trianglesContainingVertex = triangleDictionary [vertexIndex];
		for (int i = 0; i < trianglesContainingVertex.Count; i++) {
			Triangle triangle = trianglesContainingVertex [i];

			for (int j = 0; j < 3; j++) {
				int vertexB = triangle [j];
				if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB)) {
					if (IsOutlineEdge (vertexIndex, vertexB)) {
						return vertexB;
					}
				}
			}

		}
		return -1;
				
	}

	bool IsOutlineEdge(int vertexA, int vertexB){
		List<Triangle> trianglesContainingVertexA = triangleDictionary [vertexA];
		int sharedTriangleCount = 0;

		for (int i = 0; i < trianglesContainingVertexA.Count; i++) {
			if (trianglesContainingVertexA [i].Contains (vertexB)) {
				sharedTriangleCount++;
				if (sharedTriangleCount > 1) {
					break;
				}

			}
			
		}
		return sharedTriangleCount==1;
	}

	struct Triangle{
		public int vertexIndexA;
		public int vertexIndexB;
		public int vertexIndexC;
		int[] vertices;

		public Triangle(int a, int b, int c){
			vertexIndexA=a;
			vertexIndexB=b;
			vertexIndexC=c;

			vertices = new int[3];
			vertices[0]=a;
			vertices[1]=b;
			vertices[2]=c;
		}

		public int this[int i]{
			get{
				return vertices [i];
			}
		}

		public bool Contains(int vertexIndex){
			return (vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC);
		}


	}



	public class SquareGrid{
		public Square [,] squares;

		public SquareGrid(int[,] map, float squareSize){
			int nodeCountX = map.GetLength(0);
			int nodeCountY = map.GetLength(1);
			float mapWidth = nodeCountX *squareSize;
			float mapHeight = nodeCountY *squareSize;

			ControlNode[,] controlNodes = new ControlNode[nodeCountX,nodeCountY];

			for(int x=0;x<nodeCountX; x++){
				for (int y=0;y<nodeCountY;y++){
					Vector3 pos = new Vector3(-mapWidth/2 + x*squareSize + squareSize/2 , 0, -mapHeight/2 + y*squareSize + squareSize/2);
					controlNodes[x,y] = new ControlNode(pos, map[x,y] ==1, squareSize);
				}

			}

			squares = new Square[nodeCountX-1, nodeCountY -1];
			for(int x=0;x<nodeCountX-1 ; x++){
				for(int y=0; y<nodeCountY-1;y++){
					squares[x,y]= new Square(controlNodes[x,y+1], controlNodes[x+1,y+1], controlNodes[x,y] , controlNodes[x+1,y]);
				}

			}
		}
		
	}


	public class Square{
		public ControlNode topLeft, topRight, bottomLeft, bottomRight;
		public Node centerLeft, centerTop, centerRight, centerBottom;
		public int configuration;

		public Square ( ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomLeft, ControlNode _bottomRight){
			topLeft = _topLeft;
			topRight= _topRight;
			bottomRight = _bottomRight;
			bottomLeft = _bottomLeft;

			centerLeft = bottomLeft.above;
			centerTop = topLeft.right;
			centerRight = bottomRight.above;
			centerBottom = bottomLeft.right;
		
			if(topLeft.active)
				configuration+=8;
			if(topRight.active)
				configuration+=4;
			if(bottomRight.active)
				configuration+=2;
			if(bottomLeft.active)
				configuration+=1;
			
			
		}

	}

	public class Node{
		public Vector3 position;
		public int vertexIndex = -1;

		public Node(Vector3 pos){
			position = pos;
		}
	}

	public class ControlNode : Node{
		public bool active;
		public Node above, right;

		public ControlNode(Vector3 pos, bool _active, float squareSize) : base(pos){
			active = _active;
			above = new Node(position + Vector3.forward * squareSize/2f);
			right = new Node(position + Vector3.right * squareSize /2f);
		}
	
	}

}	


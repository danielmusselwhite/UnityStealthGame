using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    private int width;
    private int height;
    private float cellSize; // size of each cell

    private Vector3 originPosition;
    
    private int[,] grid; // 2D array to represent the grid

    

    public GridMap(int width, int height, float cellSize, Vector3 originPosition){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        grid = new int[width, height];

        // initialize the grid with all nodes as walkable (0)
        for (int x = 0; x < width; x++){
            for (int z = 0; z < height; z++){
                grid[x, z] = 0;
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int z){
        // return the world position of the cell at the given x and z by multiplying the x and z by the cell size
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }
    public void GetXZ(Vector3 worldPosition, out int x, out int z){
        // return the x and z of the cell based on the world position, works by dividing the world position by the cell size then rounding down
        Debug.Log("worldPosition: "+worldPosition);
        x = Mathf.FloorToInt((worldPosition.x - originPosition.x )/ cellSize);
        z = Mathf.FloorToInt((worldPosition.z - originPosition.z )/ cellSize);
        Debug.Log("x val: "+x+", z val: "+z);
    }

    public int GetValue(int x, int z){
        // return the value of the cell at the given x and z
        if(x < 0 || x >= width || z < 0 || z >= height){
            // Debug.LogError("x: "+x+", z: "+z+" is out of bounds");
            return -1;
        }
        return grid[x, z];
    }
    public int GetValue(Vector3 worldPosition){
        // return the value of the cell at the given world position
        int x, z;
        GetXZ(worldPosition, out x, out z); // get the xz
        return GetValue(x, z); // return the value of the cell at the given xz
    }
    public void SetValue(int x, int z, int value){
        // if the x and z are within the grid, set the value at the given x and z
        if( x>= 0 && x < width && z >= 0 && z < height){ 
            grid[x, z] = value;
        }
    }
    public void SetValue(Vector3 worldPosition, int value){
        int x, z;
        GetXZ(worldPosition, out x, out z); // get the xz
        SetValue(x, z, value); // set the value at the given xz
    }

    public int GetWidth(){
        return width;
    }
    public int GetHeight(){
        return height;
    }
    public float GetCellSize(){
        return cellSize;
    }

    public int[,] GetGrid(){
        // return the grid as an array
        return grid;
    }
}

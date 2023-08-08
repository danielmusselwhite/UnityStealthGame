using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridMapTest : MonoBehaviour
{
    GridMap grid;
    public int gridHeight = 10;
    public int gridWidth = 10;
    public float gridCellWidth = 2f;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        grid = new GridMap(gridHeight, gridWidth, gridCellWidth, transform.position);   
        grid.SetValue(2,2,10);

        // for each cell in grid, check if there is a Wall in the cell, if so, set the value of the cell to 1
        for (int x = 0; x < gridWidth; x++){
            for (int z = 0; z < gridHeight; z++){
                Vector3 worldPosition = grid.GetWorldPosition(x, z);
                Collider[] colliders = Physics.OverlapBox(worldPosition, new Vector3(gridCellWidth/2, 0.5f, gridCellWidth/2));
                foreach(Collider collider in colliders){
                    if(collider.gameObject.tag == "Wall"){
                        grid.SetValue(x, z, 1);
                    }
                }
            }
        }        

        
        
    }

    void Update() {
        // if (Input.GetMouseButtonDown(0)){
            
        //     grid.SetValue(transform.position, grid.GetValue(transform.position) + 1);
        // }
    }


    void OnDrawGizmos()
    {
        if(!Application.isPlaying)
            grid = new GridMap(gridHeight, gridWidth, gridCellWidth, transform.position);   
        
        float cellSize = grid.GetCellSize();
        int width = grid.GetWidth();
        int height = grid.GetHeight();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x, y + 1), Color.black);
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x + 1, y), Color.black);
                Handles.Label(grid.GetWorldPosition(x, y) + new Vector3(cellSize/2, 1, cellSize/2), grid.GetValue(x, y).ToString());
            
                // Colour code wall vs empty cells
                if(grid.GetValue(x, y) == 1){
                    Gizmos.color = new Color(1, 0, 0, 0.25f);
                    Gizmos.DrawCube(grid.GetWorldPosition(x, y) + new Vector3(cellSize/2, 0, cellSize/2), new Vector3(cellSize, 0.5f, cellSize));
                }
                else{
                    Gizmos.color = new Color(0, 1, 0, 0.25f);
                    Gizmos.DrawCube(grid.GetWorldPosition(x, y) + new Vector3(cellSize/2, 0, cellSize/2), new Vector3(cellSize, 0.5f, cellSize));
                }
            }
        }
        Debug.DrawLine(grid.GetWorldPosition(width, 0), grid.GetWorldPosition(width, height), Color.black);
        Debug.DrawLine(grid.GetWorldPosition(0, height), grid.GetWorldPosition(width, height), Color.black);
    }
}

using Kociemba;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.InspectorCurveEditor;

public class Face: MonoBehaviour
{
    public string Name;
    public Color FaceColor;
    public List<string> AdjacentFaces;
    public Vector3 SideNormal;
    public string SideString;
    public Transform CurrentTransform;


    private CubeState cubeState;
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        AssignName();
    }

    void AssignName()
    {
        if (transform.parent != null)
        {
            // Set the name field to be parent's name + this GameObject's name
            Name = transform.parent.name + "-" + gameObject.name;
        }
        else
        {
            // If there's no parent, just use this GameObject's name
            Name = gameObject.name;
        }
    }


    public List<string> FourAdjacentSides()
    {
        var stateDict = cubeState.GetStateDictionary();
        var positionInfo = FindFacePosition(Name, stateDict);
        string side = positionInfo.Item1;
        int row = positionInfo.Item2;
        int col = positionInfo.Item3;
        string[,] matrix = stateDict[side];

        List<string> adjacentFaces = new List<string>();

        if (row == 1 && col == 1) // Center position
        {
            adjacentFaces.AddRange(new string[]
            {
                matrix[0, 1], // Above
                matrix[1, 0], // Left
                matrix[1, 2], // Right
                matrix[2, 1]  // Below
            });
        }
        else if (row == 1 || col == 1) // Edge but not corner
        {
            if (row == 0 && col == 1)
            {
                adjacentFaces.Add(matrix[0, 0]);
                adjacentFaces.Add(matrix[0, 2]);
                adjacentFaces.Add(matrix[1,1]);
            }
            else if (row == 1 && col == 0)
            {
                adjacentFaces.Add(matrix[0, 0]);
                adjacentFaces.Add(matrix[2, 0]);
                adjacentFaces.Add(matrix[1, 1]);
            }
            else if (row == 1 && col == 2)
            {
                adjacentFaces.Add(matrix[0, 2]);
                adjacentFaces.Add(matrix[2, 2]);
                adjacentFaces.Add(matrix[1, 1]);
            }
            else if (row == 2 && col == 1)
            {
                adjacentFaces.Add(matrix[2, 0]);
                adjacentFaces.Add(matrix[2, 2]);
                adjacentFaces.Add(matrix[1, 1]);
            }
        }
        else // Corner
        {
            if(row == 0 && col == 0)
            {
                adjacentFaces.Add(matrix[0, 1]);
                adjacentFaces.Add(matrix[1, 0]);
            }
            else if(row == 0 && col == 2)
            {
                adjacentFaces.Add(matrix[0, 1]);
                adjacentFaces.Add(matrix[1, 2]);
            }
            else if (row == 2 && col == 0)
            {
                adjacentFaces.Add(matrix[2, 1]);
                adjacentFaces.Add(matrix[1, 0]);
            }
            else if (row == 2 && col == 2)
            {
                adjacentFaces.Add(matrix[2, 1]);
                adjacentFaces.Add(matrix[1, 2]);
            }
            // Add the two adjacent faces on the same side

        }

        // Add the faces from the GameObject's parent's other enabled children
        GameObject currentGameObject = this.gameObject;
        if (currentGameObject != null && currentGameObject.transform.parent.gameObject != null)
        {
            Debug.Log(currentGameObject.transform.parent);
            foreach (Transform sibling in currentGameObject.transform.parent)
            {
                if (sibling.gameObject != currentGameObject && sibling.gameObject.activeSelf)
                {
                    // Assuming the name of the GameObject is the same as the face name in the state dictionary
                    string siblingFaceName = sibling.GetComponent<Face>().Name;
                    adjacentFaces.Add(siblingFaceName);
                }
            }
        }

        return adjacentFaces;
    }

    public (string, int, int) FindFacePosition(string faceName, Dictionary<string, string[,]> cubeState)
    {
        foreach (var side in cubeState)
        {
            string sideName = side.Key;
            string[,] matrix = side.Value;

            for (int i = 0; i < matrix.GetLength(0); i++) // Rows
            {
                for (int j = 0; j < matrix.GetLength(1); j++) // Columns
                {
                    if (matrix[i, j] == faceName)
                    {
                        return (sideName, i, j);
                    }
                }
            }
        }

        return ("Not Found", -1, -1); // Return this if the face name is not found
    }

    public GameObject FindfaceByname(string faceName)
    {
        GameObject face = GameObject.Find(faceName);
        return face;
    }

}

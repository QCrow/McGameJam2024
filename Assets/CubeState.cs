using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.InspectorCurveEditor;

public class CubeState : MonoBehaviour
{
    // sides
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();

    public static bool autoRotating = false;
    public static bool started = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp(List<GameObject> cubeSide)
    {
        Debug.Log("picking up");
        foreach (GameObject face in cubeSide)
        {
            // Attach the parent of each face (the little cube)
            // to the parent of the 4th index (the little cube in the middle) 
            // Unless it is already the 4th index
            if (face != cubeSide[4])
            {
                face.transform.parent.transform.parent = cubeSide[4].transform.parent;
            }
        }
    }    

    public void PutDown(List<GameObject> littleCubes, Transform pivot)
    {
        Debug.Log("putting down");
        foreach (GameObject littleCube in littleCubes)
        {
            if (littleCube != littleCubes[4])
            {
                littleCube.transform.parent.transform.parent = pivot;
            }
        }
    }

    string[,] GetSideString(List<GameObject> side)
    {
        string[,] sideArray = new string[3, 3];
        for (int i = 0; i < side.Count; i++)
        {
            int row = i / 3; // Integer division to get the row
            int col = i % 3; // Modulo to get the column
            sideArray[row, col] = side[i].GetComponent<Face>().Name;
        }
        return sideArray;
    }

    public void LogStateString()
    {
        var stateDictionary = GetStateDictionary();
        string fullStateString = "";

        foreach (KeyValuePair<string, string[,]> entry in stateDictionary)
        {
            string key = entry.Key;
            string[,] side = entry.Value;
            string sideString = key + ":\n";

            for (int i = 0; i < side.GetLength(0); i++) // Rows
            {
                for (int j = 0; j < side.GetLength(1); j++) // Columns
                {
                    sideString += side[i, j] + "  ";
                }
                sideString += "\n"; // New line at the end of each row
            }

            fullStateString += sideString + "\n"; // Append each side's string to the full state string
        }

        Debug.Log(fullStateString); // Log the entire state in one message

    }



    public Dictionary<string, string[,]> GetStateDictionary()
    {
        var stateDictionary = new Dictionary<string, string[,]>
        {
            { "Up", GetSideString(up) },
            { "Right", GetSideString(right) },
            { "Front", GetSideString(front) },
            { "Down", GetSideString(down) },
            { "Left", GetSideString(left) },
            { "Back", GetSideString(back) }
        };
        return stateDictionary;
    }

    public void AssignFaceAdjacent()
    {
        foreach(var x in up)
        {
            var Face = x.GetComponent<Face>();
            Face.AdjacentFaces = Face.FourAdjacentSides();
        }
        foreach (var x in down)
        {
            var Face = x.GetComponent<Face>();
            Face.AdjacentFaces = Face.FourAdjacentSides();
        }
        foreach (var x in left)
        {
            var Face = x.GetComponent<Face>();
            Face.AdjacentFaces = Face.FourAdjacentSides();
        }
        foreach (var x in right)
        {
            var Face = x.GetComponent<Face>();
            Face.AdjacentFaces = Face.FourAdjacentSides();
        }
        foreach (var x in front)
        {
            var Face = x.GetComponent<Face>();
            Face.AdjacentFaces = Face.FourAdjacentSides();
        }
        foreach (var x in back)
        {
            var Face = x.GetComponent<Face>();
            Face.AdjacentFaces = Face.FourAdjacentSides();
        }
    }


}

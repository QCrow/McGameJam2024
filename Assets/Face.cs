using Kociemba;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Mathematics;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
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
            //Debug.Log(currentGameObject.transform.parent);
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

    public (int finalx, int finaly) CubeTuple((int x, int y) Coordinate)
    {
        if (Coordinate.x == 0)
        {
            //(0,y) goes up or down 
            if (Coordinate.y > 0) return (-1, 0);
            else return (1, 0);
        }else if(Coordinate.y == 0) {
            if (Coordinate.x > 0) return (0, 1);
            else return (0, -1);
        }
        else
        {
            return (0, 0);
        }
    }

    public List<List<(string finalside, int finalx, int finaly)>> GetAllPossiblepositions(string chesstype, int range, (String side, int curx, int cury) current_position)
    {
        string currentSide = current_position.side;
        int currentX = current_position.curx;
        int currentY = current_position.cury;

        List<List<(string finalside, int finalx, int finaly)>> moveinfo = new List<List<(string finalside, int finalx, int finaly)>>();

        if (chesstype == "car")
        {
            List<(int, int)> moves = new List<(int, int)>();
            for (int i = 0; i < range; i++)
            {
                moves.Add((1, 0));
            }
            moveinfo.Add(ResolveUnitStep(current_position, moves));

            List<(int, int)> moves_2 = new List<(int, int)>();
            for (int i = 0; i < range; i++)
            {
                moves_2.Add((0, 1));
            }
            moveinfo.Add(ResolveUnitStep(current_position, moves_2));

            List<(int, int)> moves_3 = new List<(int, int)>();
            for (int i = 0; i < range; i++)
            {
                moves_3.Add((-1, 0));
            }
            moveinfo.Add(ResolveUnitStep(current_position, moves_3));

            List<(int, int)> moves_4 = new List<(int, int)>();

            for (int i = 0; i < range; i++)
            {
                moves_4.Add((0, -1));
            }
            moveinfo.Add(ResolveUnitStep(current_position, moves_4));


        }

        if (chesstype == "ele")
        {
            List<(int, int)> moves = new List<(int, int)>();
            var moves1 = new List<(int, int)>() { (0,1), (1, 0) };
            var moves2 = new List<(int, int)>() { (0, -1), (1, 0)};
            var moves3 = new List<(int, int)>() { (0, 1), (-1, 0)};
            var moves4 = new List<(int, int)>() { (0, -1), (-1, 0)};
            moveinfo.Add(ResolveUnitStep(current_position, moves1));
            moveinfo.Add(ResolveUnitStep(current_position, moves2));
            moveinfo.Add(ResolveUnitStep(current_position, moves3));
            moveinfo.Add(ResolveUnitStep(current_position, moves4));
        }

        if (chesstype == "horse")
        {
            List<(int, int)> moves = new List<(int, int)>
         {
             (0,1),
             (1, 0),
             (1,0),
         };

            moveinfo.Add(ResolveUnitStep(current_position, moves));

            List<(int, int)> moves_2 = new List<(int, int)>
         {
             (0,-1),
             (1, 0),
             (1,0),
         };

            moveinfo.Add(ResolveUnitStep(current_position, moves_2));

            List<(int, int)> moves_3 = new List<(int, int)>
         {
             (0,1),
             (-1, 0),
             (-1,0),
         };

            moveinfo.Add(ResolveUnitStep(current_position, moves_3));

            List<(int, int)> moves_4 = new List<(int, int)>
         {
             (0,-1),
             (-1, 0),
             (-1,0),
         };

            moveinfo.Add(ResolveUnitStep(current_position, moves_4));

            List<(int, int)> moves_5 = new List<(int, int)>
         {
             (1,0),
             (0, 1),
             (0,1),
         };

            moveinfo.Add(ResolveUnitStep(current_position, moves_5));

            List<(int, int)> moves_6 = new List<(int, int)>
         {
             (-1,0),
             (0, 1),
             (0,1),
         };

            moveinfo.Add(ResolveUnitStep(current_position, moves_6));

            List<(int, int)> moves_7 = new List<(int, int)>
         {
             (1,0),
             (0, -1),
             (0,-1),
         };

            var list = ResolveUnitStep(current_position, moves_7);

            moveinfo.Add(ResolveUnitStep(current_position, moves_7));

            List<(int, int)> moves_8 = new List<(int, int)>
         {
             (-1,0),
             (0, -1),
             (0,-1),
         };

            moveinfo.Add(ResolveUnitStep(current_position, moves_8));
        }
        return moveinfo;
    }


    public List<(string finalside, int finalx, int finaly)> ResolveUnitStep((string initside, int initx, int inity) previousPosition, List<(int moveX, int moveY)> unitMovementVectors)
    {
        //for(int q= 0; q < unitMovementVectors.Count; q++)
        //{
            //unitMovementVectors[q] = CubeTuple(unitMovementVectors[q]);
           // Debug.Log(unitMovementVectors[q]);
        //}
        List<(string finalSide, int finalX, int finalY)> movementSteps = new List<(string, int, int)>();
        int currentPositionX = previousPosition.initx;
        int currentPositionY = previousPosition.inity;
        var currentSide = previousPosition.initside;
        for (int i=0; i< unitMovementVectors.Count; i++)
        {
            int expectedx = currentPositionX;
            int expectedy = currentPositionY;
            //var modifiedTuple = CubeTuple((unitMovementVectors[i].moveX, unitMovementVectors[i].moveY));
            expectedx += unitMovementVectors[i].moveX;
            expectedy += unitMovementVectors[i].moveY;
            //Debug.Log("ExpectedX"+expectedx);
            //Debug.Log("ExpectedY"+expectedy);
            
            var direction = GetDirection(unitMovementVectors[i].moveX, unitMovementVectors[i].moveY);
            //Debug.Log(InCurrentSide(expectedx, expectedy, direction));
            if (InCurrentSide(expectedx, expectedy, direction))
            {
                var modifiedTuple = CubeTuple((unitMovementVectors[i].moveX, unitMovementVectors[i].moveY));
                movementSteps.Add((currentSide, currentPositionX+modifiedTuple.finalx, currentPositionY+modifiedTuple.finaly));
                currentPositionX += modifiedTuple.finalx;
                currentPositionY += modifiedTuple.finaly;
            }
            else
            {
                var targetSide = GetAdjacentSide(previousPosition.initside, direction);
                //map new side index, add to movementSteps, modify current position after add in list movements
                var positionOnNewSide=GetNewSideFirstPosition((currentPositionX, currentPositionY), direction, previousPosition.initside);
                movementSteps.Add((targetSide, positionOnNewSide.finalx, positionOnNewSide.finaly));
                currentPositionX = positionOnNewSide.finalx;
                currentPositionY = positionOnNewSide.finaly;
                currentSide = targetSide;
                for (int j = i+1; j < unitMovementVectors.Count; j++)
                {
                    unitMovementVectors[j] = ModifyTuple(unitMovementVectors[j], previousPosition.initside, targetSide);
                    Debug.Log(unitMovementVectors[j].moveX+" "+unitMovementVectors[j].moveY);
                }
            }
        }

        return movementSteps;
    }

    public (int finalx, int finaly) GetNewSideFirstPosition((int currentPositionX, int currentPositionY) currentPosition, string direction, string previousSide)
    {
        switch (direction)
        {
            case "Up":
                switch (currentPosition)
                {
                    case (0, 0): return (2, 0);
                    case (0, 1): return (2, 1);
                    case (0, 2): return (2, 2);
                    default: return (-1, -1);
                }
            case "Down":
                switch (currentPosition)
                {
                    case (2, 0): return (0, 0);
                    case (2, 1): return (0, 1);
                    case (2, 2): return (0, 2);
                    default: return (-1, -1);
                }
            case "Left":
                switch (currentPosition)
                {
                    case (0, 0): return (0, 2);
                    case (1, 0): return (1, 2);
                    case (2, 0): return (2, 2);
                    default: return (-1, -1);
                }
            case "Right":
                switch (currentPosition)
                {
                    case (0, 2): return (0, 0);
                    case (1, 2): return (1, 0);
                    case (2, 2): return (2, 0);
                    default: return (-1, -1);
                }

            default:
                return (-1, -1);
        }
    }

    public (int x, int y) ModifyTuple((int initx, int inity) initTuple, string initSide, string targetSide)
    {
        switch (initSide)
        {
            case "Front":
                return initTuple;
            case "Right":
                switch (targetSide)
                {
                    case "Up": return (-1,0);
                    case "Down": return (-1,0);
                    case "Front": return initTuple;
                    case "Back": return initTuple;
                    default: return (0,0);
                }
            case "Left":
                switch (targetSide)
                {
                    case "Up": return (1, 0);
                    case "Down": return (1, 0);
                    case "Front": return initTuple;
                    case "Back": return initTuple;
                    default: return (0, 0);
                }
            case "Back":
                switch (targetSide)
                {
                    case "Up": return (0,-1);
                    case "Down": return (0,1);
                    case "Left": return initTuple;
                    case "Right": return initTuple;
                    default: return (0, 0);
                }
            case "Down":
                switch (targetSide)
                {
                    case "Front": return initTuple;
                    case "Back": return (0, 1);
                    case "Left": return (0, 1);
                    case "Right": return (0, 1);
                    default: return (0, 0);
                }
            case "Up":
                switch (targetSide)
                {
                    case "Front": return initTuple;
                    case "Back": return (0, -1);
                    case "Left": return (0, -1);
                    case "Right": return (0, -1);
                    default: return (0, 0);
                }
            default:
                return (0,0);
        }
    }

    public string GetDirection(int x, int y)
    {
        if (x != 0) // The vector is horizontal
        {
            return x > 0 ? "Right" : "Left";
        }
        else if (y != 0) // The vector is vertical
        {
            return y > 0 ? "Up" : "Down";
        }
        else // The vector is (0, 0)
        {
            return "None"; // Or throw an exception, based on how you want to handle this case
        }
    }


    public bool InCurrentSide(int x, int y, string direction)
    {
        if((x==0)&& direction == "Up")
        {
            return false;
        }
        else if((y==0)&& direction == "Left")
        {
            return false;
        }
        else if ((x == 2) && direction == "Down")
        {
            return false;
        }
        else if ((y == 2) && direction == "Right")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public string GetAdjacentSide(string currentSide, string direction)
    {
        switch (currentSide)
        {
            case "Front":
                switch (direction)
                {
                    case "Up": return "Up";
                    case "Down": return "Down";
                    case "Left": return "Left";
                    case "Right": return "Right";
                    default: return "Invalid direction";
                }

            case "Back":
                switch (direction)
                {
                    case "Up": return "Up";
                    case "Down": return "Down";
                    case "Left": return "Right";
                    case "Right": return "Left";
                    default: return "Invalid direction";
                }

            case "Up":
                switch (direction)
                {
                    case "Up": return "Back";
                    case "Down": return "Front";
                    case "Left": return "Left";
                    case "Right": return "Right";
                    default: return "Invalid direction";
                }

            case "Down":
                switch (direction)
                {
                    case "Up": return "Front";
                    case "Down": return "Back";
                    case "Left": return "Left";
                    case "Right": return "Right";
                    default: return "Invalid direction";
                }

            case "Left":
                switch (direction)
                {
                    case "Up": return "Up";
                    case "Down": return "Down";
                    case "Left": return "Back";
                    case "Right": return "Front";
                    default: return "Invalid direction";
                }

            case "Right":
                switch (direction)
                {
                    case "Up": return "Up";
                    case "Down": return "Down";
                    case "Left": return "Front";
                    case "Right": return "Back";
                    default: return "Invalid direction";
                }

            default:
                return "Invalid side";
        }
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

using UnityEngine;

public class Face: MonoBehaviour
{
    public string Name;
    public Color FaceColor;
    public Face[] AdjacentFaces;
    public Vector3 SideNormal;
    public string SideString;
    public Transform CurrentTransform;

    void Start()
    {
        AssignName();
    }

    void AssignName()
    {
        if (transform.parent != null)
        {
            // Set the name field to be parent's name + this GameObject's name
            Name = transform.parent.name + gameObject.name;
        }
        else
        {
            // If there's no parent, just use this GameObject's name
            Name = gameObject.name;
        }
    }
}

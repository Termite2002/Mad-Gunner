using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    public Color startColor, endColor, shopColor;

    public int distanceToEnd;
    public bool includeShop;
    public int minDistanceToShop, maxDistanceToShop;

    public Transform generatorPoint;

    public enum Direction
    {
        up, right, down, left
    };
    public Direction selectedDirection;

    public float xOffset = 18f, yOffset = 10f;

    public LayerMask whatIsRoom;

    public GameObject endRoom, shopRoom;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    public RoomPrefabs rooms;

    private List<GameObject> generateOutlines = new List<GameObject>();

    public RoomCenter centerStart, centerEnd, centerShop;
    public RoomCenter[] potentialCenters;
    void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;

        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            layoutRoomObjects.Add(newRoom);

            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);

                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }
        }

        if (includeShop)
        {
            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop + 1);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }

        // Create room outlines
        CreateRoomOutline(Vector3.zero);
        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);

        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }

        foreach(GameObject outline in generateOutlines)
        {
            bool generateCenter = true;

            if (outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                generateCenter = false;
            }

            if (outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                generateCenter = false;
            }

            if (includeShop)
            {
                if (outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                    generateCenter = false;
                }
            }

            if (generateCenter)
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);

                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveGenerationPoint()
    {
        switch(selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;

            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;

            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;

            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }
    public void CreateRoomOutline(Vector3 roomPostition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPostition + new Vector3(0f, yOffset, 0f), .2f, whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPostition + new Vector3(0f, -yOffset, 0f), .2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPostition + new Vector3(-xOffset, 0f, 0f), .2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPostition + new Vector3(xOffset, 0f, 0f), .2f, whatIsRoom);

        int directionCount = 0;
        if (roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }

        switch(directionCount)
        {
            case 0:
                Debug.LogError("Found no room exists!");
                break;
            case 1:

                if (roomAbove)
                {
                    generateOutlines.Add(Instantiate(rooms.singleUp, roomPostition, transform.rotation));
                }

                if (roomBelow)
                {
                    generateOutlines.Add(Instantiate(rooms.singleDown, roomPostition, transform.rotation));
                }

                if (roomLeft)
                {
                    generateOutlines.Add(Instantiate(rooms.singleLeft, roomPostition, transform.rotation));
                }

                if (roomRight)
                {
                    generateOutlines.Add(Instantiate(rooms.singleRight, roomPostition, transform.rotation));
                }

                break;
            case 2:

                if (roomLeft && roomRight)
                {
                    generateOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPostition, transform.rotation));
                }

                if (roomAbove && roomBelow)
                {
                    generateOutlines.Add(Instantiate(rooms.doubleUpDown, roomPostition, transform.rotation));
                }

                if (roomAbove && roomRight)
                {
                    generateOutlines.Add(Instantiate(rooms.doubleUpRight, roomPostition, transform.rotation));
                }

                if (roomBelow && roomRight)
                {
                    generateOutlines.Add(Instantiate(rooms.doubleDownRight, roomPostition, transform.rotation));
                }

                if (roomBelow && roomLeft)
                {
                    generateOutlines.Add(Instantiate(rooms.doubleLeftDown, roomPostition, transform.rotation));
                }

                if (roomAbove && roomLeft)
                {
                    generateOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPostition, transform.rotation));
                }

                break;
            case 3:

                if (roomAbove && roomBelow && roomLeft)
                {
                    generateOutlines.Add(Instantiate(rooms.tripleUpDownLeft, roomPostition, transform.rotation));
                }

                if (roomRight && roomBelow && roomLeft)
                {
                    generateOutlines.Add(Instantiate(rooms.tripleLeftDownRight, roomPostition, transform.rotation));
                }

                if (roomRight && roomBelow && roomAbove)
                {
                    generateOutlines.Add(Instantiate(rooms.tripleUpDownRight, roomPostition, transform.rotation));
                }

                if (roomRight && roomLeft && roomAbove)
                {
                    generateOutlines.Add(Instantiate(rooms.tripleLeftUpRight, roomPostition, transform.rotation));
                }

                break;
            case 4:

                if (roomLeft && roomRight && roomBelow && roomAbove)
                {
                    generateOutlines.Add(Instantiate(rooms.fourway, roomPostition, transform.rotation));
                }

                break;
        }
    }
}
[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleLeft, singleRight,
        doubleLeftRight, doubleUpDown, doubleUpRight, doubleDownRight, doubleLeftDown, doubleLeftUp,
        tripleUpDownLeft, tripleLeftDownRight, tripleUpDownRight, tripleLeftUpRight,
        fourway;
}

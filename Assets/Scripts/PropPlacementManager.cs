using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PropPlacementManager : MonoBehaviour
{
    DungeonData dungeonData;

    [SerializeField]
    private TilemapVisualiser tilemapVisualiser;

    [SerializeField]
    private List<PropSets> propSets;

    [SerializeField]
    private List<Prop> propsToPlace;

    [SerializeField, Range(0, 1)]
    private float cornerPropPlacementChance = 0.7f;

    [SerializeField]
    private GameObject propPrefab;

    public UnityEvent OnFinished;

    private void Awake()
    {
        dungeonData = FindObjectOfType<DungeonData>();

    }

    // Function finds the currently selected dungeon style and populates the list of props to use
    public void SelectPropStyles()
    {
        propsToPlace.Clear();

        if (tilemapVisualiser == null)
        {
            return;
        }

        PropSets propsToUse = propSets[tilemapVisualiser.selectedStyle];

        foreach (var prop in propsToUse.propList)
        {
            propsToPlace.Add(prop);
        }
    }

    public void ProcessRooms()
    {
        if (dungeonData == null)
            return;

        SelectPropStyles();

        foreach (Room room in dungeonData.Rooms)
        {

            if (room.RoomDistanceRanking == dungeonData.Rooms.Count - 1)
            {
                //Place dungeon door
                List<Prop> door = propsToPlace.Where(x => x.SpecialTrait == "Portal").ToList();
                PlaceProps(room, door, room.InnerTiles, PlacementOriginCorner.BottomLeft);

            }

            //Place props in the corners
            List<Prop> cornerProps = propsToPlace.Where(x => x.Corner).ToList();
            PlaceCornerProps(room, cornerProps);

            //Place props near LEFT wall
            List<Prop> leftWallProps = propsToPlace
            .Where(x => x.NearWallLeft)
            .OrderByDescending(x => (x.PropSize.x / 0.16f) * (x.PropSize.y / 0.16f))
            .ToList();

            PlaceProps(room, leftWallProps, room.NearWallTilesLeft, PlacementOriginCorner.BottomLeft);

            //Place props near RIGHT wall
            List<Prop> rightWallProps = propsToPlace
            .Where(x => x.NearWallRight)
            .OrderByDescending(x => (x.PropSize.x / 0.16f) * (x.PropSize.y / 0.16f))
            .ToList();

            PlaceProps(room, rightWallProps, room.NearWallTilesRight, PlacementOriginCorner.TopRight);

            //Place props near UP wall
            List<Prop> topWallProps = propsToPlace
            .Where(x => x.NearWallUP)
            .OrderByDescending(x => (x.PropSize.x / 0.16f) * (x.PropSize.y / 0.16f))
            .ToList();

            PlaceProps(room, topWallProps, room.NearWallTilesUp, PlacementOriginCorner.TopLeft);

            //Place props near DOWN wall
            List<Prop> downWallProps = propsToPlace
            .Where(x => x.NearWallDown)
            .OrderByDescending(x => (x.PropSize.x / 0.16f) * (x.PropSize.y / 0.16f))
            .ToList();

            PlaceProps(room, downWallProps, room.NearWallTilesDown, PlacementOriginCorner.BottomLeft);

            //Place inner props
            List<Prop> innerProps = propsToPlace
                .Where(x => x.Inner)
                .OrderByDescending(x => (x.PropSize.x / 0.16f) * (x.PropSize.y / 0.16f))
                .ToList();
            PlaceProps(room, innerProps, room.InnerTiles, PlacementOriginCorner.BottomLeft);

        }

        // OnFinished?.Invoke();
        Invoke("RunEvent", 1);

    }

    public void RunEvent()
    {
        OnFinished?.Invoke();
    }

    private IEnumerator TutorialCoroutine(Action code)
    {
        yield return new WaitForSeconds(3);
        code();
    }

    /// <summary>
    /// Places props near walls. We need to specify the props and the placement start point
    /// </summary>
    /// <param name="room"></param>
    /// <param name="wallProps">Props that we should try to place</param>
    /// <param name="availableTiles">Tiles that are near the specific wall</param>
    /// <param name="placement">How to place bigger props. Ex near top wall we want to start placemt from the Top corner and find if there are free spaces below</param>
    private void PlaceProps(
        Room room, List<Prop> wallProps, HashSet<Vector2> availableTiles, PlacementOriginCorner placement)
    {
        //Remove path positions from the initial nearWallTiles to ensure the clear path to traverse dungeon
        HashSet<Vector2> tempPositons = new HashSet<Vector2>(availableTiles);
        tempPositons.ExceptWith(dungeonData.Path);

        //We will try to place all the props
        foreach (Prop propToPlace in wallProps)
        {

            //We want to place only certain quantity of each prop
            int quantity
                = UnityEngine.Random.Range(propToPlace.PlacementQuantityMin, propToPlace.PlacementQuantityMax + 1);

            for (int i = 0; i < quantity; i++)
            {
                //remove taken positions
                tempPositons.ExceptWith(room.PropPositions);
                //shuffle the positions
                List<Vector2> availablePositions = tempPositons.OrderBy(x => Guid.NewGuid()).ToList();
                //If placement has failed there is no point in trying to place the same prop again
                PropGroup propGroup = propToPlace as PropGroup;
                if (propGroup)
                {
                    if (TryPlacingMultiplePropBruteForce(room, propGroup, availablePositions, placement) == false)
                        break;

                }
                else
                {
                    if (TryPlacingPropBruteForce(room, propToPlace, availablePositions, placement) == false)
                        break;
                }
            }


        }
    }


    private bool TryPlacingMultiplePropBruteForce(Room room, PropGroup propGroupToPlace, List<Vector2> availablePositions, PlacementOriginCorner placement)
    {
        for (int i = 0; i < availablePositions.Count; i++)
        {
            //select the specified position (but it can be already taken after placing the corner props as a group)
            Vector2 position = availablePositions[i];
            if (room.PropPositions.Contains(position))
                continue;

            //check if there is enough space around to fit the prop
            List<Vector2> freePositionsAround
                = TryToFitProp(propGroupToPlace, availablePositions, position, placement);

            //If we have enough spaces place the prop
            if (freePositionsAround.Count == (propGroupToPlace.PropSize.x / 0.16f) * (propGroupToPlace.PropSize.y / 0.16f))
            {
                foreach (Prop propToPlace in propGroupToPlace.propsInGroup)
                {
                    //Place the gameobject
                    PlacePropGameObjectAt(room, position, propToPlace);
                }

                //Lock all the positions recquired by the prop (based on its size)
                foreach (Vector2 pos in freePositionsAround)
                {
                    //Hashest will ignore duplicate positions
                    room.PropPositions.Add(pos);
                }

                //Deal with groups
                if (propGroupToPlace.PlaceAsGroup)
                {
                    PlaceGroupObject(room, position, propGroupToPlace, 0.16f);
                }
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// Tries to place the Prop using brute force (trying each available tile position)
    /// </summary>
    /// <param name="room"></param>
    /// <param name="propToPlace"></param>
    /// <param name="availablePositions"></param>
    /// <param name="placement"></param>
    /// <returns>False if there is no space. True if placement was successful</returns>
    private bool TryPlacingPropBruteForce(
        Room room, Prop propToPlace, List<Vector2> availablePositions, PlacementOriginCorner placement)
    {
        //try placing the objects starting from the corner specified by the placement parameter
        for (int i = 0; i < availablePositions.Count; i++)
        {
            //select the specified position (but it can be already taken after placing the corner props as a group)
            Vector2 position = availablePositions[i];
            if (room.PropPositions.Contains(position))
                continue;

            //check if there is enough space around to fit the prop
            List<Vector2> freePositionsAround
                = TryToFitProp(propToPlace, availablePositions, position, placement);

            //If we have enough spaces place the prop
            if (freePositionsAround.Count == (propToPlace.PropSize.x / 0.16f) * (propToPlace.PropSize.y / 0.16f))
            {
                //Place the gameobject
                PlacePropGameObjectAt(room, position, propToPlace);
                //Lock all the positions recquired by the prop (based on its size)
                foreach (Vector2 pos in freePositionsAround)
                {
                    //Hashest will ignore duplicate positions
                    room.PropPositions.Add(pos);
                }

                //Deal with groups
                if (propToPlace.PlaceAsGroup)
                {
                    PlaceGroupObject(room, position, propToPlace, 0.16f);
                }
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the prop will fit (accordig to it size)
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="availablePositions"></param>
    /// <param name="originPosition"></param>
    /// <param name="placement"></param>
    /// <returns></returns>
    private List<Vector2> TryToFitProp(
        Prop prop,
        List<Vector2> availablePositions,
        Vector2 originPosition,
        PlacementOriginCorner placement)
    {
        List<Vector2> freePositions = new();

        //Perform the specific loop depending on the PlacementOriginCorner
        if (placement == PlacementOriginCorner.BottomLeft)
        {
            for (float xOffset = 0; xOffset < prop.PropSize.x; xOffset = xOffset + 0.16f)
            {

                for (float yOffset = 0; yOffset < prop.PropSize.y; yOffset = yOffset + 0.16f)
                {
                    // Vector2 tempPos = originPosition + new Vector2(xOffset, yOffset);
                    Vector2 tempPos = new Vector2(((xOffset * 100) + (originPosition.x * 100)) / 100, ((yOffset * 100) + (originPosition.y * 100)) / 100);

                    if (availablePositions.Contains(tempPos))
                        freePositions.Add(tempPos);
                }
            }
        }
        else if (placement == PlacementOriginCorner.BottomRight)
        {
            for (float xOffset = -prop.PropSize.x + 0.16f; xOffset <= 0; xOffset = xOffset + 0.16f)
            {
                for (float yOffset = 0; yOffset < prop.PropSize.y; yOffset = yOffset + 0.16f)
                {
                    // Vector2 tempPos = originPosition + new Vector2(xOffset, yOffset);
                    Vector2 tempPos = new Vector2(((xOffset * 100) + (originPosition.x * 100)) / 100, ((yOffset * 100) + (originPosition.y * 100)) / 100);

                    if (availablePositions.Contains(tempPos))
                        freePositions.Add(tempPos);
                }
            }
        }
        else if (placement == PlacementOriginCorner.TopLeft)
        {
            for (float xOffset = 0; xOffset < prop.PropSize.x; xOffset = xOffset + 0.16f)
            {
                for (float yOffset = -prop.PropSize.y + 0.16f; yOffset <= 0; yOffset = yOffset + 0.16f)
                {
                    // Vector2 tempPos = originPosition + new Vector2(xOffset, yOffset);
                    Vector2 tempPos = new Vector2(((xOffset * 100) + (originPosition.x * 100)) / 100, ((yOffset * 100) + (originPosition.y * 100)) / 100);

                    if (availablePositions.Contains(tempPos))
                        freePositions.Add(tempPos);
                }
            }
        }
        else
        {
            for (float xOffset = -prop.PropSize.x + 0.16f; xOffset <= 0; xOffset = xOffset + 0.16f)
            {
                for (float yOffset = -prop.PropSize.y + 0.16f; yOffset <= 0; yOffset = yOffset + 0.16f)
                {
                    // Vector2 tempPos = originPosition + new Vector2(xOffset, yOffset);
                    Vector2 tempPos = new Vector2(((xOffset * 100) + (originPosition.x * 100)) / 100, ((yOffset * 100) + (originPosition.y * 100)) / 100);

                    if (availablePositions.Contains(tempPos))
                        freePositions.Add(tempPos);
                }
            }
        }

        return freePositions;
    }

    /// <summary>
    /// Places props in the corners of the room
    /// </summary>
    /// <param name="room"></param>
    /// <param name="cornerProps"></param>
    private void PlaceCornerProps(Room room, List<Prop> cornerProps)
    {
        float tempChance = cornerPropPlacementChance;
        float originalChance = cornerPropPlacementChance;

        foreach (Vector2 cornerTile in room.CornerTiles)
        {
            if (UnityEngine.Random.value < tempChance)
            {
                tempChance = originalChance; // Reset the chance

                Prop propToPlace
                    = cornerProps[UnityEngine.Random.Range(0, cornerProps.Count)];
                PropGroup propGroup = propToPlace as PropGroup; //will return null if not of type PropGroup
                if (propGroup)
                {
                    foreach (Prop prop in propGroup.propsInGroup)
                    {
                        PlacePropGameObjectAt(room, cornerTile + new Vector2(prop.relativeCoordX, prop.relativeCoordY), prop);
                    }
                }
                else
                {
                    PlacePropGameObjectAt(room, cornerTile, propToPlace);
                }
                if (propToPlace.PlaceAsGroup)
                {
                    PlaceGroupObject(room, cornerTile, propToPlace, 0.32f);
                }
            }
            else
            {
                tempChance = Mathf.Clamp01(tempChance + 0.1f);
            }
        }
    }

    /// <summary>
    /// Helps to find free spaces around the groupOriginPosition to place a prop as a group
    /// </summary>
    /// <param name="room"></param>
    /// <param name="groupOriginPosition"></param>
    /// <param name="propToPlace"></param>
    /// <param name="searchOffset">The search offset ex 1 = we will check all tiles withing the distance of 1 unity away from origin position</param>
    private void PlaceGroupObject(
        Room room, Vector2 groupOriginPosition, Prop propToPlace, float searchOffset)
    {
        //*Can work poorely when placing bigger props as groups

        //calculate how many elements are in the group -1 that we have placed in the center
        int count = UnityEngine.Random.Range(propToPlace.GroupMinCount, propToPlace.GroupMaxCount) - 1;
        count = Mathf.Clamp(count, 0, 8);

        // find the available spaces around the center point.
        // searchOffset  limits the distance between available spaces and center point
        List<Vector2> availableSpaces = new List<Vector2>();
        for (float xOffset = -searchOffset; xOffset <= searchOffset; xOffset = xOffset + 0.16f)
        {
            for (float yOffset = -searchOffset; yOffset <= searchOffset; yOffset = yOffset + 0.16f)
            {
                Vector2 tempPos = groupOriginPosition + new Vector2(xOffset, yOffset);
                if (room.FloorTiles.Contains(tempPos) &&
                    !dungeonData.Path.Contains(tempPos) &&
                    !room.PropPositions.Contains(tempPos))
                {
                    availableSpaces.Add(tempPos);
                }
            }
        }

        //shuffle the list
        availableSpaces.OrderBy(x => Guid.NewGuid());

        //place the props (as many as we want or if there is less space fill all the available spaces)
        int tempCount = count < availableSpaces.Count ? count : availableSpaces.Count;
        for (int i = 0; i < tempCount; i++)
        {
            PlacePropGameObjectAt(room, availableSpaces[i], propToPlace);
        }

    }

    /// <summary>
    /// Place a prop as a new GameObject at a specified position
    /// </summary>
    /// <param name="room"></param>
    /// <param name="placementPostion"></param>
    /// <param name="propToPlace"></param>
    /// <returns></returns>
    private GameObject PlacePropGameObjectAt(Room room, Vector2 placementPostion, Prop propToPlace)
    {
        //Instantiate the prop at this positon
        GameObject prop = Instantiate(propPrefab);
        SpriteRenderer propSpriteRenderer = prop.GetComponentInChildren<SpriteRenderer>();

        //set the sprite
        propSpriteRenderer.sprite = propToPlace.PropSprite;

        //Add a collider
        if (propToPlace.Collidable)
        {
            CapsuleCollider2D collider
                = propSpriteRenderer.gameObject.AddComponent<CapsuleCollider2D>();
            collider.offset = Vector2.zero;

            // Add to blocking layer
            propSpriteRenderer.gameObject.layer = LayerMask.NameToLayer("Blocking");

            if (propToPlace.PropSize.x > propToPlace.PropSize.y)
            {
                collider.direction = CapsuleDirection2D.Horizontal;
            }

            Vector2 size
                = new Vector2(propToPlace.PropSize.x * 0.8f, propToPlace.PropSize.y * 0.8f);
            collider.size = size;

        }

        if (propToPlace.SpecialTrait == "Portal")
        {
            prop.name = "LadderPortal"; // Set GameObject name

            // Creating portal collider
            BoxCollider2D collider = prop.AddComponent<BoxCollider2D>();

            Vector2 size = new Vector2(propToPlace.PropSize.x * 0.8f, propToPlace.PropSize.y * 0.8f);
            collider.size = size;

            collider.offset = new Vector2(0.08f, 0.08f);

            Portal portal = prop.AddComponent<Portal>();

            // Setting destination of portal
            portal.sceneNames = new String[1];
            portal.sceneNames[0] = "Main";
        }
        else if (propToPlace.SpecialTrait == "Destructible")
        {
            Destructible destructible = prop.transform.GetChild(0).gameObject.AddComponent<Destructible>();
            //AddComponent<Destructable>();
            destructible.hitPoints = new System.Random().Next(1, 5);
            destructible.animator = prop.transform.GetChild(0).gameObject.AddComponent<Animator>();
            prop.transform.GetChild(0).tag = "Fighter";

            destructible.animator.runtimeAnimatorController = Resources.Load("Animations/" + propToPlace.nameOfAnimator) as RuntimeAnimatorController;

        }

        prop.transform.localPosition = (Vector2)placementPostion;

        //adjust the position to the sprite
        propSpriteRenderer.transform.localPosition = (Vector2)propToPlace.PropSize * 0.5f;
        propSpriteRenderer.sortingLayerName = propToPlace.sortingLayer;

        //Save the prop in the room data (dungeon data)
        room.PropPositions.Add(placementPostion);
        room.PropObjectReferences.Add(prop);
        return prop;
    }
}

/// <summary>
/// Where to start placing the prop ex. start at BottomLeft corner and search 
/// if there are free space to the Right and Up in case of placing a biggest prop
/// </summary>
public enum PlacementOriginCorner
{
    BottomLeft,
    BottomRight,
    TopLeft,
    TopRight
}

[System.Serializable]
public class PropSets
{
    public List<Prop> propList;
}

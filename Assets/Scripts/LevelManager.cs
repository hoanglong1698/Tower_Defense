using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameObject[] tilePrefabs;

    [SerializeField] private CameraMovement cameraMovement;

    [SerializeField] private Transform map;

    public Dictionary<Point, TileScript> Tiles { get; set; }

    private Point blueSpawn, redSpawn;

    [SerializeField] private GameObject bluePortalPrefab;

    [SerializeField] private GameObject redPortalPrefab;

    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();

        //load map from a text document
        string[] mapData = ReadLevelText();

        //calculate the x map size
        int mapX = mapData[0].ToCharArray().Length;
        //calculate the y map size
        int mapY = mapData.Length;

        Vector3 maxTile = Vector3.zero;

        //calculate the world start point, this is the top left corner of the screen
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapY; y++) //y position
        {
            char[] newTile = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++) //x position
            {
                //place tile in the world 
                PlaceTile(newTile[x].ToString(), x, y, worldStart);
            }
        }

        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;

        //set the camera limits to the max tile position
        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));

        SpawnPortals();
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);

        //create a new tile and make reference to that tile in the newTile variable
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //uses the new tile variable to change the position of the tile
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + TileSize * x, worldStart.y - TileSize * y, 0), map);

    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    private void SpawnPortals()
    {
        blueSpawn = new Point(0, 0);

        Instantiate(bluePortalPrefab, Tiles[blueSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);

        redSpawn = new Point(11, 6);

        Instantiate(redPortalPrefab, Tiles[redSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
    }
}

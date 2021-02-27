using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class HexGrid : MonoBehaviour
{
    public Hex[,] hexgrid;

    public int width;
    public int height;

    public float hex_size;

    public GameObject base_hex_object;

    void Start()
    {
        SetupGrid();
    }

    public void SetupGrid() //spawns in a grid with the desired dimensions
    {
        hexgrid = new Hex[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject new_object = Instantiate(base_hex_object,
                    oddq_offset_to_pixel(new Vector2Int(i,j)), Quaternion.identity, this.transform);

                Hex new_hex = new_object.GetComponent<Hex>();
                hexgrid[i, j] = new_hex;
                new_hex.setCoords(new Vector2Int(i,j));
                new_hex.setTerrain((TileType)Random.Range(0,2));
            }
        }
    }

    public List<Vector2Int> adjacent_coords(Vector2Int coords)
    {
        //come back later
        List<Vector2Int> results = new List<Vector2Int>();

        results.Add(new Vector2Int(coords.x, coords.y - 1));
        results.Add(new Vector2Int(coords.x, coords.y + 1));
        results.Add(new Vector2Int(coords.x - 1, coords.y));
        results.Add(new Vector2Int(coords.x + 1, coords.y));

        if (coords.y % 2 == 0)
        {
            results.Add(new Vector2Int(coords.x - 1, coords.y + 1));
            results.Add(new Vector2Int(coords.x + 1, coords.y + 1));
        }
        else
        {
            results.Add(new Vector2Int(coords.x - 1, coords.y - 1));
            results.Add(new Vector2Int(coords.x + 1, coords.y - 1));
        }

        for (int i = 0; i < 6; i++)
        {
            Vector2Int v = results[i];
            if (out_of_bounds(v))
            {
                results.RemoveAt(i);
                i--;
            }
        }

        return results;
    }

    public static bool is_adjacent(Vector2Int a, Vector2Int b)
    {
        return distance(a, b) == 1;
    }

    public static int distance(Vector2Int a, Vector2Int b)
    {
        return distance(oddq_to_cube(a), oddq_to_cube(b));
    }

    public static int distance(Vector3Int a, Vector3Int b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 2;
    }

    public List<Vector2Int> hexes_within_distance(Vector2Int center, int distance)
    {
        // returns hexes within the given distance that exist on the board
        List<Vector2Int> results = new List<Vector2Int>();
        for (int x = -distance; x <= distance; x++)
        {
            for (int y = Mathf.Max(-distance, -x - distance); y <= Mathf.Min(distance, distance - x);y++)
            {
                var z = -x - y;
                Vector2Int tentative = cube_to_oddq(oddq_to_cube(center) + new Vector3Int(x, y, z));
                if (!out_of_bounds(tentative))
                {
                    if (hexgrid[tentative.x, tentative.y] != null)
                        results.Add(tentative);
                }
            }
        }

        return results;
    }

    public bool out_of_bounds(Vector2Int v)
    {
        return (v.x < 0 || v.x >= width || v.y < 0 || v.y >= height);
    }

    public static Vector2Int cube_to_oddq(Vector3Int cube)
    {
        var col = cube.x;
        var row = cube.z + (cube.x - (cube.x & 1)) / 2;
        return new Vector2Int(col, row);
    }

    public static Vector3Int oddq_to_cube(Vector2Int hex)
    {
        var x = hex.x;
        var z = hex.y - (hex.x - (hex.x & 1)) / 2;
        var y = -x - z;
        return new Vector3Int(x, y, z);
    }

    public Vector2 oddq_offset_to_pixel(Vector2Int hex)
    {
        float x = hex_size * 3 / 2 * hex.x;

        float y = hex_size * Mathf.Sqrt(3) * (hex.y + 0.5f * (hex.x & 1));
        return new Vector2(x, y);
    }

    public void generate_map()
    {
        HashSet<Vector2Int> existing_nodes = new HashSet<Vector2Int>();
        System.Random random = new System.Random();
        while(existing_nodes.Count < hex_size)
        {
            int num1 = random.Next(0, existing_nodes.Count);
            Vector2Int existing_element = existing_nodes.ElementAt(num1);
            List<Vector2Int> adj_coords = adjacent_coords(existing_element);
            int num2 = random.Next(0, adj_coords.Count);
            Vector2Int new_element = adj_coords[num2];
            existing_nodes.Add(new_element);
            hexgrid[new_element.x, new_element.y] = new Hex();
            
        }

    }

    public void print_map()
    {
        for(int i = 0; i < width; i++)
            for(int j = 0; i < height; j++)
                

    }

}

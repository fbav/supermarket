using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class Spawn
{
    public int Product;
    public Vector3 Position;
    public Quaternion Rotation;
}

[Serializable]
public class ExperimentConfiguration
{
    public List<Spawn> Spawnable = new List<Spawn>();
    public List<int> Order = new List<int>();
}

[Serializable]
public struct Purchase
{
    public int Code;
    public float Time;
}

//This can be added to the controller....
public class ProductInventory : MonoBehaviour
{
    //Drag the config file here (This can be found in the Example directory)
    public TextAsset ConfigAsset;

    //Drag food prefabs here
    //The number of products for size is currently 55
    public GameObject[] Inventory;

    public int[] ItemsToCollect { get; private set; }
    public List<Purchase> ItemsCollected { get; private set; }

    //The list of all products in the inventory
    public Dictionary<int, GameObject> ProductList { get; private set; }

    private List<int> itemsOutstanding;

    public void Start()
    {
        Load(new MemoryStream(ConfigAsset.bytes));
    }

    public void Load(Stream stream)
    {
        var serialiser = new XmlSerializer(typeof(ExperimentConfiguration));
        var configuration = serialiser.Deserialize(stream) as ExperimentConfiguration;

        ProductList = new Dictionary<int, GameObject>();
        foreach (var item in Inventory)
        {
            ProductList.Add(item.GetComponent<ProductCode>().Code, item);
        }

        ItemsToCollect = configuration.Order.ToArray();
        ItemsCollected = new List<Purchase>();

        itemsOutstanding = new List<int>(ItemsToCollect);
    }

    //Check if the chosen item is on the shopping list by passing in the id from the selected item
    //Will probably have to write something to get the id from the selected item....
    public bool IsShoppingListItem(int code)
    {
        if (itemsOutstanding.Contains(code))
        {
            itemsOutstanding.Remove(code);
            Debug.Log("Correct!");
            return true;
        }
        else
        {
            Debug.Log("Incorrect!");
            return false;
        }
       
    }

    //Check if we have collected all the items
    public bool IsShoppingListComplete()
    {
        return itemsOutstanding.Count == 0;
    }


}


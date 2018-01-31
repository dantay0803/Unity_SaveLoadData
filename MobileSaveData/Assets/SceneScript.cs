using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class SceneScript : MonoBehaviour
{
    //https://www.raywenderlich.com/160815/save-load-game-unity

    [SerializeField]
    Transform parent;

    [SerializeField]
    GameObject prefabObject;

    float prefabObjectSpacing = 40;

    // Use this for initialization
    void Start()
    {
        SaveScore("HP", "1");
        SaveScore("RD", "2");
        Invoke("UpdateDisplay", 2f);
    }



    public void SaveScore(string name, string isbn)
    {
        var previousScores = LoadPreviousData();

        var newEntry = new WishListEntry();
        newEntry.title = name;
        newEntry.isbn = isbn;
       

        var bin = new BinaryFormatter();
        var filePath = Application.persistentDataPath + "/WishList.dat";

        using (var file = File.Open(filePath, FileMode.OpenOrCreate))
        {
            previousScores.Add(newEntry);
            bin.Serialize(file, previousScores);
        }
    }



    void UpdateDisplay()
    {
        var entries = LoadPreviousData();
        Debug.Log(entries.Count);

        float yPos = 0;

        for (int i = 0; i < entries.Count; i++)
        {
            GameObject go = Instantiate(prefabObject);
            go.transform.SetParent(parent);
            go.transform.localPosition = new Vector3(0, yPos, 0);
            go.transform.Find("Text").GetComponent<Text>().text = entries[i].title;
            yPos -= prefabObjectSpacing;
        }
    }



    public List<WishListEntry> LoadPreviousData()
    {
        // 1
        if (File.Exists(Application.persistentDataPath + "/WishList.dat"))
        {
            Debug.Log("Game Loaded");
            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/WishList.dat", FileMode.Open);
            var entries = (List<WishListEntry>)bf.Deserialize(file);
            file.Close();
            return entries;
        }
        else
        {
            Debug.Log("No game saved!");
            return new List<WishListEntry>();
            
        }
    }













}
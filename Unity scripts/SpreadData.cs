using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine;

public class SpreadData : MonoBehaviour
{

    
    public TextAsset csvFile; // Reference of CSV file
    public Sprite[] Sprites = new Sprite[1000];
    public GameObject[] images = new GameObject[1000];

    private char lineSeperater = '\n'; // It defines line seperate character
    private char fieldSeperator = ','; // It defines field seperate chracter
    public GameObject imagePrefab;
    public float yCoord = 8f;

    public float scale = 10000; 
    
    Color tempColor;

    
    void ReadFileAndLoacate()
    {
        float curX, curZ;
        Sprite curSprite;
        string path, template; 
        
        string[] records = csvFile.text.Split (lineSeperater);
        
        for (int i = 0; i < records.Length; ++i)
        {
            template = "/Images/{0}.png";
            path = string.Format(template, i.ToString());
            Sprites[i] = Resources.Load(path) as Sprite;
        }
        
        for (int i = 0; i < records.Length; ++i)
        {
            // get the coordinates
            string[] fields = records[i].Split(fieldSeperator);
            curX = float.Parse(fields[0]) * scale;
            curZ = float.Parse(fields[1]) * scale;
            
            // get the current image as sprite
            template = "Images/{0}";
            path = string.Format(template, (i + 1).ToString());
            curSprite  =Resources.Load<Sprite>(path);
            
            // create a prefab for each image locate it as x and z are the coordinates
            // (and all have the same y value- half of the image height)     
            GameObject curImage = Instantiate(imagePrefab, new Vector3(curX, yCoord, curZ), Quaternion.identity);
            images[i] = curImage;
            
            // add the image to the prefab
            curImage.transform.GetChild(0).GetComponent<Image>().sprite = curSprite;
            
        }
        

    }
    // Start is called before the first frame update
    void Start()
    {
        // load the data from the csv
        // create images prefabs 
        // locate them at the cordinated
        ReadFileAndLoacate();
    }


    private void OnTriggerEnter(Collider other)
    {
        tempColor = other.gameObject.transform.GetChild(0).GetComponent<Image>().color;
        tempColor.a = 0.42f;
        other.gameObject.transform.GetChild(0).GetComponent<Image>().color = tempColor;
        
    }
    private void OnTriggerExit(Collider other)
    {
        tempColor = other.gameObject.transform.GetChild(0).GetComponent<Image>().color;
        tempColor.a = 1f;
        other.gameObject.transform.GetChild(0).GetComponent<Image>().color = tempColor;
        
    }
}

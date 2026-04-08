using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameObject[] startingCrops;
    public List<GameObject> livingCrops = new List<GameObject>();
    public List<GameObject> livingPests = new List<GameObject>();
    public int pestsKilled = 0;
    public float startTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void AddPestToCount()
    {
        pestsKilled++;
        GameUIManager.Instance.UpdatePestsAndHealth();
    }

    public void ResetGame()
    {
        ClearVariables();
        SetupVariables();
    }

    private void ClearVariables()
    {
        pestsKilled = 0;
        livingCrops.Clear();
        livingPests.Clear();
    }

    private void SetupVariables()
    {
        startingCrops = GameObject.FindGameObjectsWithTag("Crop");
        foreach (GameObject crop in startingCrops)
        {
            if (crop.GetComponent<Crop>().isAlive)
            {
                livingCrops.Add(crop);
            }
        }
    }
}

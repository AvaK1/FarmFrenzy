using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    //the crop will have four states: growing, grown, harvestable, and dead. (state machine)
    [SerializeField] private List<Sprite> sprites;
    public bool isAlive = true;
    public bool isOccupied = false;
    //the statechangetime will be subtracted from current time to see how long it's been since the last state change
    private float stateChangeTime = 0;
    private float timeSinceLastState = 0;
    private SpriteRenderer spriteRenderer;

    public enum CropState
    {
        Growing,
        Grown,
        Harvestable,
        Dead
    }

    public CropState currentCropState = CropState.Growing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!GameManager.Instance.livingCrops.Contains(gameObject))
        {
            GameManager.Instance.livingCrops.Add(gameObject);
        }
        stateChangeTime = Time.time;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastState = Time.time - stateChangeTime;
        switch (currentCropState) //each one is going to check to see what the counter is at, then it's going to change state if it's been in that state for long enough. also, each state will display its own sprite
        {
            case CropState.Growing:
                if (timeSinceLastState >= 10)
                {
                    SetState(CropState.Grown);
                    spriteRenderer.sprite = sprites[1];
                }
                break;
            case CropState.Grown:
                if (timeSinceLastState >= 20)
                {
                    SetState(CropState.Harvestable);
                    spriteRenderer.sprite = sprites[2];
                }
                break;
            case CropState.Harvestable:
                
                break;
            case CropState.Dead:
                if (timeSinceLastState >= 30)
                {
                    spriteRenderer.sprite = sprites[0];
                    GameManager.Instance.livingCrops.Add(gameObject);
                    SetState(CropState.Growing);
                }
                break;
        }
    }

    public void Harvest()
    {
        //give player xp + chance for an item
        SetState(CropState.Grown);
        spriteRenderer.sprite = sprites[1];

        if (GameManager.Instance.cropHarvestCount == 3)
        {
            GameManager.Instance.SpawnWeaponBox(gameObject.transform.position);
            GameManager.Instance.cropHarvestCount = 0;
        }
        else
        {
            GameManager.Instance.cropHarvestCount++;
        }
    }

    public void Die()
    {
        isAlive = false;
        SetState(CropState.Dead);
        spriteRenderer.sprite = sprites[3];
        GameManager.Instance.livingCrops.Remove(gameObject);
    }

    //sets the currentstate to the new state and saves the time the state was changed
    private void SetState(CropState newState)
    {
        currentCropState = newState;
        stateChangeTime = Time.time;
    }
}

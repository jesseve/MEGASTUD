using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameController : MonoBehaviour {

	public delegate void Respawn();
	public static event Respawn RespawnEvent;

    public float spawnTimer;

	[SerializeField] private float checkTime = 5f;
	[SerializeField] private float moneyAmount;
	[SerializeField] private float energyAmount;
	[SerializeField] private Text moneyText = null;
	[SerializeField] private Text energyText = null;
	[SerializeField] private BuildingBase playerHeadquarters = null;
	[SerializeField] private Image resourceBar = null;
	public Text nextWawe;

    private List<NormalEnemy> enemies;
    private GameEnd gameEnd;
    private bool gameEnded;

    private float checkTimer;
	private float resourceTimer;
	private List<BuildingBase> activeBuildings;
	private List<BuildingBase> buildingsInQueue;
	private bool checkingBuildings;
	private AudioSource _audio;

	// Use this for initialization
	void Awake () {
		UpdateResourceTexts();
		checkTimer = checkTime;
		resourceTimer = checkTime;
		activeBuildings = new List<BuildingBase>();
		buildingsInQueue = new List<BuildingBase>();
		activeBuildings.Add(playerHeadquarters);
		_audio = GetComponent<AudioSource>();

        gameEnd = FindObjectOfType<GameEnd>();
        enemies = new List<NormalEnemy>();
        foreach(NormalEnemy n in FindObjectsOfType<NormalEnemy>()) {
            enemies.Add(n);
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (gameEnded == true) return;

		if(RespawnEvent != null)
			RespawnEvent();
		
		checkTimer -= Time.deltaTime;
		if(checkTimer <= 0f)
		{
			if(!checkingBuildings)
				StartCoroutine(HandleActiveBuildings());
			else
			{
				resourceTimer -= Time.deltaTime;
				if(resourceTimer <= 0f)
				{
					resourceTimer = checkTime;
					GetResources();
				}
			}
		} else resourceTimer = checkTime;

		resourceBar.fillAmount = checkTimer / checkTime;
		if(spawnTimer > 0)
		{
			spawnTimer -= Time.deltaTime;
			if(spawnTimer < 0)
			{
				spawnTimer = 0;
				_audio.clip = SoundManager.GetSoundClip(SoundClip.Alarm);
				_audio.Play();
			}

			int min = Mathf.RoundToInt(spawnTimer / 60);
			int sec = Mathf.RoundToInt(spawnTimer % 60);
			nextWawe.text = "Next Wave in " + min + ":"+sec;
		}

	}

	public void AddResources(ResourceType resource, float amount)
	{
		switch(resource)
		{
		case ResourceType.Money:
			moneyAmount += amount;
			break;

		case ResourceType.Energy:
			energyAmount += amount;
			break;
		}

		UpdateResourceTexts();
	}

	public bool CheckResourceAvailability(float money, float energy)
	{
		if(money > moneyAmount || energy > energyAmount)
			return false;

		return true;
	}

	public void UpdateResources(float money, float energy)
	{
		moneyAmount -= money;
		energyAmount -= energy;
		UpdateResourceTexts();
	}

	private void UpdateResourceTexts()
	{
		moneyText.text = moneyAmount.ToString();
		energyText.text = energyAmount.ToString();
	}

	public void AddNewBuilding(BuildingBase building)
	{
		buildingsInQueue.Add(building);
	}

	public void DeactivateBuilding(BuildingBase building)
	{
		if(activeBuildings.Contains(building))
			activeBuildings.Remove(building);
	}

	public void ReactivateBuilding(BuildingBase building)
	{
		if(buildingsInQueue.Contains(building))
			return;
		
		buildingsInQueue.Add(building);
		building.gameObject.SetActive(true);
	}

	private void GetResources()
	{
		foreach(BuildingBase building in activeBuildings)
		{
			if(building.buildingType == BuildingType.ResourceBuilding)
			{
				ResourceBuilding resourceBuilding = (ResourceBuilding)building;
				AddResources(resourceBuilding.resourceType, resourceBuilding.amountProduced);
			}
		}

		for(int i = buildingsInQueue.Count - 1; i >= 0; i--)
		{
			if(buildingsInQueue[i].buildingType == BuildingType.ResourceBuilding)
			{
				activeBuildings.Add(buildingsInQueue[i]);
				buildingsInQueue.RemoveAt(i);
			}
		}
	}

	private IEnumerator HandleActiveBuildings()
	{
		checkingBuildings = true;
		List<BuildingBase> remainingBuildings = new List<BuildingBase>();
		for(int i = activeBuildings.Count - 1; i >= 0;i--)
			remainingBuildings.Add(activeBuildings[i]);

		while(remainingBuildings.Count > 0)
		{
			for(int i = remainingBuildings.Count - 1; i >= 0;i--)
			{
				BuildingBase building = remainingBuildings[i];

				switch(building.buildingType)
				{
				case BuildingType.ResourceBuilding:
					ResourceBuilding resourceBuilding = (ResourceBuilding)building;
					AddResources(resourceBuilding.resourceType, resourceBuilding.amountProduced);
					remainingBuildings.RemoveAt(i);
					break;
				case BuildingType.OffensiveSpawner: case BuildingType.DefensiveSpawner:
					PlayerSpawner spawner = (PlayerSpawner)building;
					spawner.SpawnPlayerUnit();
					remainingBuildings.RemoveAt(i);
					break;
				default: 
					remainingBuildings.RemoveAt(i);
					break;
				}
			}
			yield return null;
		}
		checkTimer = checkTime;
		foreach(BuildingBase building in buildingsInQueue)
			activeBuildings.Add(building);

		buildingsInQueue.Clear();
		checkingBuildings = false;
	}

    public void HQDown(BuildingBase b) {
        if (gameEnded == true) return;
        if (b.CompareTag("PlayerHQ"))
            LoseGame();
        else if (b is NormalEnemy) {
            enemies.Remove(b as NormalEnemy);
            if (enemies.Count <= 0) {
                WinGame();                
            }
        }
    }

    private void WinGame() {
        StopObjects();
        gameEnded = true;
        gameEnd.EndGame(true);           
    }
    private void LoseGame() {
        StopObjects();
        gameEnded = true;
        gameEnd.EndGame(false);
    }

    private void StopObjects() {
        foreach (BuildingBase bb in activeBuildings)
        {
            if (bb is SpawningBuilding) {
                foreach (Unit u in (bb as SpawningBuilding).ActiveTroops) {
                    u.Stop();
                }
            }
            bb.Stop();
        }
    }
}

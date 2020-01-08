using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    private static LevelManager _instance;


    //Singleton awake
    private void Awake()
    {
        if(_instance != null && this != _instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    //singleton instance getter
    public static LevelManager Instance { get { return _instance; } }

    //enemy subtypes
    public GameObject bigsteroid;
    public GameObject smallsteroid;
    public GameObject star;
    public GameObject flare;
    public GameObject explosion;

    //enemy object pools, sizes, and vars
    ObjectPool bigPool;
    public bool bigCanGrow;
    public int bigCount;
    ObjectPool smallPool;
    public bool smallCanGrow;
    public int smallCount; //note, public so visible, but will always be set to bigCount*3
    ObjectPool starPool;
    public bool starCanGrow;
    public int starCount;
    ObjectPool flarePool;
    public bool flareCanGrow;
    public int flareCount;
    ObjectPool explosionPool;
    public bool explosionCanGrow;
    public int explosionCount;

    //player values
    public GameObject playerRig; //the player
    public GameObject playerExplosion; //the explosion for death

    //powerup values
    public GameObject powerup; //powerup prefab
    public GameObject extraLife; //extra life prefab
    public GameObject extraPoints; //extra points to spawn sometimes
    public float powerupChance; //chance to spawn a powerup
    public int lifeRound; //round to give an extra life on
    public float extraPointChance; //chance to spawn extra points
    public int extraPointsValue; //extra points point value

    //gameplay values
    public Vector3 spawnOrigin; //origin of spawn
    int points; //player's points
    int bigPoints; //number of bigsteroids destroyed
    int smallPoints; //number of smallsteroids destroyed
    int starPoints; //number of stars destroyed
    public int lives; //player's lives
    public float spawnTime; //time to spawn the player
    public float fadeTime; //time to fade out after last death
    int round; //the current round
    bool paused; //whether the game is paused or not

    //spawn values
    int enemiesInPlay; //how many enemies are in play, used to spawn new rounds
    public float closenessThreshold; //minimum distance an object must be from the player upon spawning in
    public float timeBetweenRounds; //time to wait between rounds after all enemies have died

    //UI values
    public Text score;
    public List<Image> lifeImages;
    public Image fadeOverlay;
    public Image pawsedOverlay;

    // Use this for initialization
    void Start () {
        //set points and save to value
        bigPoints = 0;
        PlayerPrefs.SetInt("bigPoints", 0);
        smallPoints = 0;
        PlayerPrefs.SetInt("smallPoints", 0);
        starPoints = 0;
        PlayerPrefs.SetInt("starPoints", 0);
        points = 0;
        PlayerPrefs.SetInt("points", 0);
        //game has start, make cursor invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //set paused to false
        paused = false;
        //set small count to always ensure 3 asteroids are availble for spawning after big one dies
        smallCount = bigCount * 3;
        //set up pools
        bigPool = new ObjectPool(bigsteroid, bigCanGrow, bigCount);
        smallPool = new ObjectPool(smallsteroid, smallCanGrow, smallCount);
        starPool = new ObjectPool(star, starCanGrow, starCount);
        flarePool = new ObjectPool(flare, flareCanGrow, flareCount);
        explosionPool = new ObjectPool(explosion, explosionCanGrow, explosionCount);

        //Start the next round
        enemiesInPlay = 0;
        round = PlayerPrefs.GetInt("StartRound", 0);
        StartRound();
    }

    /*****************************GAME FACILITATION METHODS*****************************/

    //Listens for escape to pause the game
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Messenger.Broadcast(GameEvents.PAUSE_EVENT);
                paused = true;
                pawsedOverlay.enabled = true;
            }
            else
            {
                Messenger.Broadcast(GameEvents.RESUME_EVENT);
                paused = false;
                pawsedOverlay.enabled = false;
            }
        }
    }

    //Starts a new round
    public void StartRound()
    {
        //increment round number
        round++;

        //loop through rounds, adding new enemies for each due to the following formula
        //
        // Add 1 bigsteroid per round
        // Add 1 star per 5 rounds
        //
        for(int i = 0; i < round; i++)
        {
            //spawn a bigsteroid, increment enemies in play by 4 to account for 3 smallsteroids
            SpawnObject(bigPool.GetObject());
            enemiesInPlay += 4;

            //if round multiple of 5, spawn a star
            if ( i % 5 == 0 && i != 0)
            {
                SpawnObject(starPool.GetObject());
                enemiesInPlay++;
            }
        }
    }

    //Places the given object in space where the player is not and activates it
    void SpawnObject(GameObject spaceJunk)
    {
        //get random spawn position within play bounds
        Vector3 spawnPos = new Vector3(Random.Range(-220f, 220f), Random.Range(-220f, 220f), Random.Range(-220f, 220f));
        //while the object is too close to the player, get a new spawn position
        while(Vector3.Distance(spawnPos, playerRig.transform.position) < closenessThreshold)
        {
            spawnPos = new Vector3(Random.Range(-220f, 220f), Random.Range(-220f, 220f), Random.Range(-220f, 220f));
        }

        //give a random rotation
        Quaternion spawnRot = Random.rotation;

        //apply and activate
        spaceJunk.transform.position = spawnPos;
        spaceJunk.transform.rotation = spawnRot;
        spaceJunk.SetActive(true);
    }

    //Gives a life to the player if < 3 lives
    public void GiveLife()
    {
        if (lives < 3)
        {
            lifeImages[lives].enabled = true;
            lives++;
        }
    }

    /*****************************ENEMY CALLED METHODS*****************************/

    //On bigsteroid death, spawns in smallsteroids
    public void BigsteroidDied(Vector3 position)
    {
        //spawn 3 smallsteroids
        for(int i = 0; i < 3; i++)
        {
            //NOTE: guaranteed to have one due to setting small count to 3 times big count
            GameObject sa = smallPool.GetObject();
            sa.transform.position = position;
            sa.SetActive(true);
        }
    }

    //Gives a flare to a star
    public GameObject GetFlare()
    {
        if (flarePool == null) return null;
        return flarePool.GetObject();
    }

    //Called to alert the manager on death of an enemy
    public void ObjectDied(Vector3 position, int points, int type)
    {
        enemiesInPlay--;
        GivePoints(points, type);

        //spawn explosion
        GameObject exp = explosionPool.GetObject();
        //if we got one, set it up for play
        if (exp)
        {
            exp.transform.position = position;
            exp.SetActive(true);
        }

        //Get random number, see if powerup spawns
        float chance = Random.Range(0f, 1f);
        if(chance < powerupChance)
        {
            Instantiate(powerup, position, Quaternion.identity);
        }

        //get another random number, see if the points spawn
        chance = Random.Range(0f, 1f);
        if(chance < extraPointChance)
        {
            Instantiate(extraPoints, position, Quaternion.identity);
        }

        //if no enemies left, check to spawn life, invoke next round
        if (enemiesInPlay == 0)
        {
            //if a life round, spawn an extra life
            if(round % lifeRound == 0)
            {
                Instantiate(extraLife, position, Quaternion.identity);
            }
            Invoke("StartRound", timeBetweenRounds);
        }
    }

    //Gives player points, type = 0 than big, 1 = small, 2 = star
    public void GivePoints(int points, int type)
    {
        //award points and record source
        this.points += points;
        switch (type)
        {
            case 0:
                bigPoints++;
                break;
            case 1:
                smallPoints++;
                break;
            case 2:
                starPoints++;
                break;
            default:
                break;
        }
        //update UI
        score.text = string.Format("SCORE: {0:00000000}", this.points);
    }

    /*****************************PLAYER DEATH METHODS*****************************/

    //Respawns the player
    public void PlayerDied()
    {
        //despawn player, play explosion effect
        playerRig.SetActive(false);
        playerExplosion.transform.position = playerRig.transform.position;
        playerExplosion.SetActive(true);

        //if lives left, respawn. Else trigger end game
        if (lives > 0)
        {
            lives--;
            //respawn player
            Invoke("SpawnPlayer", spawnTime);
            //update UI
            lifeImages[lives].enabled = false;
        }
        else
        {
            StartCoroutine(Fade());
            Invoke("GameOver", fadeTime);
        }
    }

    //Respawns player
    void SpawnPlayer()
    {
        //reset transform
        playerRig.transform.position = spawnOrigin;
        playerRig.transform.rotation = Quaternion.identity;
        Rigidbody rb = playerRig.GetComponent<Rigidbody>();
        //reset velocity
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //enable
        playerRig.SetActive(true);
    }

    //fades overlay after final death
    //TODO: fade music by inverse on death
    IEnumerator Fade()
    {
        float fadeTotal = 0.0f;
        float totalWaitTime = 0.0f;
        AudioSource music = GetComponent<AudioSource>();

        //while not 100% faded
        while(fadeTotal < 100)
        {
            yield return new WaitForSeconds(.1f);
            totalWaitTime += .1f;
            fadeTotal = totalWaitTime / fadeTime;
            music.pitch = 1 - fadeTotal;
            Color overlayColor = fadeOverlay.color;
            overlayColor.a = fadeTotal;
            fadeOverlay.color = overlayColor;
        }
    }
    
    //Calls game over logic
    void GameOver()
    {
        //enable cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //set points
        PlayerPrefs.SetInt("bigPoints", bigPoints);
        PlayerPrefs.SetInt("smallPoints", smallPoints);
        PlayerPrefs.SetInt("starPoints", starPoints);
        PlayerPrefs.SetInt("points", points);
        //load end screen
        SceneManager.LoadScene("GameOverScene");
    }
}

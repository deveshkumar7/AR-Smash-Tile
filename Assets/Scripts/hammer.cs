using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexTileManager : MonoBehaviour
{
    public List<GameObject> tiles;
    public Material whiteMaterial;
    public Material blackMaterial;
    public Material redMaterial;
    public Material greenMaterial;

    public GameObject gameOverPanel;
    public GameObject panel2;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text finalscoreText;
    public TMP_Text finalscoreText2;

    public AudioSource audioSource;
    public AudioClip correctHitSound;
    public AudioClip wrongHitSound;

    private GameObject currentBlackTile;
    private GameObject lastHitTile;
    public float interval = 0.5f;
    public float greenDuration = 0.2f; // Duration the tile stays green
    private int score = 0;
    private bool gameRunning = true;
    private float timer = 60f;

    void Start()
    {
        if (tiles == null || tiles.Count != 7)
        {
            Debug.LogError("Please assign exactly 7 tiles to the tiles list.");
            return;
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        foreach (GameObject tile in tiles)
        {
            tile.tag = "Tile";
            SetTileMaterial(tile, whiteMaterial);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (panel2 != null)
        {
            panel2.SetActive(false);
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(timer).ToString();
        }

        StartCoroutine(ChangeTileColorRoutine());
        StartCoroutine(TimerRoutine());
    }

    IEnumerator ChangeTileColorRoutine()
    {
        while (gameRunning)
        {
            ChangeRandomTileToBlack();
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator ResetGreenTile(GameObject tile)
    {
        yield return new WaitForSeconds(greenDuration);
        if (tile != currentBlackTile && gameRunning) // Only reset if it's not currently the black tile
        {
            SetTileMaterial(tile, whiteMaterial);
        }
    }

    IEnumerator TimerRoutine()
    {
        while (timer > 0 && gameRunning)
        {
            timer -= Time.deltaTime;

            if (timerText != null)
            {
                timerText.text = "Time: " + Mathf.CeilToInt(timer).ToString();
            }

            yield return null;
        }

        if (gameRunning)
        {
            EndGameDueToTime();
        }
    }

    void ChangeRandomTileToBlack()
    {
        if (currentBlackTile != null)
        {
            SetTileMaterial(currentBlackTile, whiteMaterial);
        }

        int randomIndex = Random.Range(0, tiles.Count);
        currentBlackTile = tiles[randomIndex];
        SetTileMaterial(currentBlackTile, blackMaterial);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameRunning) return;

        GameObject hitObject = other.gameObject;

        if (hitObject.CompareTag("Tile"))
        {
            HandleTileHit(hitObject);
        }
    }

    void HandleTileHit(GameObject hitTile)
    {
        if (hitTile == currentBlackTile)
        {
            if (hitTile != lastHitTile)
            {
                // Play correct hit sound
                if (audioSource != null && correctHitSound != null)
                {
                    audioSource.PlayOneShot(correctHitSound);
                }

                // Set the new hit tile to green
                SetTileMaterial(hitTile, greenMaterial);
                lastHitTile = hitTile;
                score++;

                if (scoreText != null)
                {
                    scoreText.text = "Score: " + score;
                }

                // Start coroutine to reset the green tile
                StartCoroutine(ResetGreenTile(hitTile));
            }
        }
        else
        {
            // Play wrong hit sound
            if (audioSource != null && wrongHitSound != null)
            {
                audioSource.PlayOneShot(wrongHitSound);
            }

            SetTileMaterial(hitTile, redMaterial);
            EndGame();
        }
    }

    void EndGame()
    {
        gameRunning = false;
        StopAllCoroutines();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            finalscoreText2.text = "Score: " + score;
        }
    }

    void EndGameDueToTime()
    {
        gameRunning = false;
        StopAllCoroutines();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (panel2 != null)
        {
            panel2.SetActive(true);
        }

        if (finalscoreText != null)
        {
            finalscoreText.text = "Score: " + score;
        }
    }

    void SetTileMaterial(GameObject tile, Material material)
    {
        Renderer renderer = tile.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
        else
        {
            Debug.LogError($"Tile {tile.name} does not have a Renderer component.");
        }
    }
}
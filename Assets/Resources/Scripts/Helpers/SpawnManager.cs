using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
    bool respawning = false;
    int botCount = 1;
    public Vector3 leftSpawnPoint = new Vector3(-80, 0, 0);
    public Vector3 rightSpawnPoint = new Vector3(80, 0, 0);


    void OnEnable()
    {
        botCount = 1;
    }

    public void RespawnBot(bool isPlayer = false)
    {
        if (respawning) return;
        respawning = true;
        botCount++;
        StartCoroutine(BotRespawnRoutine());
    }

    private IEnumerator BotRespawnRoutine()
    {
        yield return new WaitForSeconds(4.5f);
        if (GameManager.Instance.player.transform.position.x > 0)
        {
            SpawnPlayer(Constants.BOT, true);
        }
        else
        {
            SpawnPlayer(Constants.BOT, false);
        }
        respawning = false;
    }

    public void InstantiatePlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.GetComponent<CharacterManager>().Instantiate(player.name);
        }
    }
    public void SpawnPlayer(string characterName, bool isHost)
    {
        Vector3 spawnPoint = isHost ? leftSpawnPoint : rightSpawnPoint;
        GameObject character = Instantiate(Resources.Load(Constants.CHARACTER_PREFAB_PATH) as GameObject, spawnPoint, Quaternion.identity);
        character.name = characterName;
        if (characterName == Constants.BOT && GameManager.Instance.player != null)
        {
            GameManager.Instance.player.transform.parent.transform.parent.GetComponent<Health>().SetBotHealth(1, character);
            character.GetComponent<CharacterManager>().Instantiate(characterName);
            character.GetComponent<Buffs>().BuffByLevel(botCount);
            print(botCount);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = GetComponent<GameManager>();
    }

    public PlayerUnit Player;
    public Transform Orb;

    public List<SkillSO> SkillsList;
    private int thisWave;

    double gold;
    public double Gold => gold;

    private bool IsPlayerDead;

    private void Update()
    {
        if (Player.Hp > 0 && !IsPlayerDead) return;
        if (IsPlayerDead) return;
        if (EnemyManager.Instance.GetIsRaid())
        {
            EventManager.Instance.TriggerEvent(EventManager.EventFlags.RaidFailed);
        }
        else
        {
            EventManager.Instance.TriggerEvent(EventManager.EventFlags.GameOver);
        }
        IsPlayerDead = true;

    }

    public void SetMoney(double value)
    {
        gold = value;
    }

    public void AddMoney(int value)
    {
        gold += value;
        Player.AddGold(value);
    }

    public int GetWave()
    {
        return thisWave;
    }

    public void SetWave(int value)
    {
        thisWave = value;
        UIManager.Instance.SetWaveText(thisWave);
    }

    public void NextWave()
    {
        thisWave++;
        Player.SetWaveStat(thisWave);
        UIManager.Instance.SetWaveText(thisWave);
    }

    public void RestartThisWave()
    {
        Player.SetWaveStat(thisWave);
        UIManager.Instance.SetWaveText(thisWave);
        Player.ResetHP();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void RestartPreWave()
    {
        if(thisWave > 0)
            Player.SetWaveStat(thisWave - 1);
        thisWave--;
        UIManager.Instance.SetWaveText(thisWave);
        Player.ResetHP();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        DataManager.Instance.JsonSave();
    }
}

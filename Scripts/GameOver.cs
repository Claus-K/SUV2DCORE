using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cells;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameWinOrDefeat;
    [SerializeField] private GameObject totalScore;
    [SerializeField] private GameObject deadCells;
    [SerializeField] private GameObject enemiesKilled;
    [SerializeField] private GameObject deadBloodCells;

    private void OnEnable()
    {
        ScenePoints.Status += UpdateGameState;
        ScenePoints.BloodStatusCheck += BloodStatusCheck;
        SceneSpawner.GameOverWon += GameOverState;
    }

    private void OnDisable()
    {
        ScenePoints.Status -= UpdateGameState;
        ScenePoints.BloodStatusCheck -= BloodStatusCheck;
        SceneSpawner.GameOverWon -= GameOverState;
    }


    private void UpdateGameState()
    {
        ICollection<enumCellType> types = ScenePoints.Instance.CellsList.Keys;
        var check = types.All(type => ScenePoints.Instance.CellsList[type].Count == 0);
        if (!check) return;
        GameOverState(true);
    }

    private void BloodStatusCheck()
    {
        if (ScenePoints.Instance.BloodCellsCount > 0) return;
        GameOverState(true);
    }

    private void GameOverState(bool isDefeated = false)
    {
        gameWinOrDefeat.GetComponentInChildren<TextMeshProUGUI>().text = isDefeated switch
        {
            true => "Game Defeated",
            false => "Game Won"
        };

        ScoreCalculator();
    }

    private void ScoreCalculator()
    {
        var scenePoints = ScenePoints.Instance;
        var cells = scenePoints.DeadCells.Keys;
        var deadCount = cells.Sum(type => scenePoints.DeadCells[type]);
        deadCells.GetComponentInChildren<TextMeshProUGUI>().text = deadCount.ToString();

        var virus = scenePoints.KilledVirus.Keys;
        var killedEnemies = virus.Sum(type => scenePoints.KilledVirus[type]);
        enemiesKilled.GetComponentInChildren<TextMeshProUGUI>().text = killedEnemies.ToString();

        var deadBlood = scenePoints.BloodCellsCount;
        deadBloodCells.GetComponentInChildren<TextMeshProUGUI>().text = deadBlood.ToString();

        var penalt = deadCount * 5;
        var score = scenePoints.Points + deadBlood - penalt;
        totalScore.GetComponentInChildren<TextMeshProUGUI>().text = score.ToString();

        gameOverScreen.SetActive(true);
    }
}
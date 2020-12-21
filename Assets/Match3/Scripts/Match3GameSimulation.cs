using System;
using System.Collections.Generic;
using Unity.Simulation.Games;
using UnityEngine;

// TODO: Game Simulation uses the following namespace: Unity.Simulation.Games
// Reference: https://docs.unity3d.com/Packages/com.unity.simulation.games@0.4/manual/ImplementationGuide.html

public class Match3GameSimulation : MonoBehaviour {

    [SerializeField] private List<LevelSO> levelList;
    [SerializeField] private Match3 match3;

    private LevelSO levelSO;

    private void Awake() {
        GameSimManager.Instance.FetchConfig(OnFetchConfig);

        match3.OnWin += Match3_OnWin;
        match3.OnOutOfMoves += Match3_OnOutOfMoves;
    }

    private void OnFetchConfig(GameSimConfigResponse response) {
        string levelName = response.GetString("level");
        foreach (var level in levelList)
        {
            if (level.name == levelName)
            {
                levelSO = level;
                match3.SetLevelSO(levelSO);
                break;
            }
        }
    }

    private void Match3_OnWin(object sender, System.EventArgs e) {
        Debug.Log("Match3_OnWin");

        GameSimManager.Instance.SetCounter("winMoveCount", match3.GetMoveCount());
        EndGameSimulation();
    }

    private void Match3_OnOutOfMoves(object sender, System.EventArgs e) {
        Debug.Log("Match3_OnOutOfMoves");

        GameSimManager.Instance.SetCounter("loseMoveCount", match3.GetMoveCount());
        EndGameSimulation();
    }

    // Gracefully end the simulation
    private void EndGameSimulation() {
    // It's convenient for this to function in the expected manner in editor..
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

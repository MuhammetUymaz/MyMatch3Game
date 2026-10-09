using System.Collections.Generic;
using UnityEngine;

//Used partial class to separate the class -clean code
public partial class BoosterManager : MonoBehaviour
{
    BoosterType currentBooster;

    List<Piece> shufflingList;

    public void Start()
    {
        currentBooster = BoosterType.None;

        shufflingList = new List<Piece>(400);

        SL.Register(this);
    }

    public void Run(Tile selectedTile)
    {
        if(currentBooster == BoosterType.Hammer)
        {
            DestroyPiece(selectedTile);
        }
        else if(currentBooster == BoosterType.RocketColumn)
        {
            DestroyColumn(selectedTile.y);
        }
        else if(currentBooster == BoosterType.RocketRow)
        {
            DestroyRow(selectedTile.x);
        }

        currentBooster = BoosterType.None;

        SL.Get<InfoManager>().ResetInfo();
    }

    public bool IsThereWaitingForBooster() => currentBooster != BoosterType.None;

    public UpdatingType updatingType { get => UpdatingType.JustOnGameplay; }
}
public class TriggerByNeighbour: PieceSystem
{
    public void Run(TriggerByNeighbourComponent component)
    {
        Piece piece = component.piece;

        piece.ReduceHealth();
    }
}

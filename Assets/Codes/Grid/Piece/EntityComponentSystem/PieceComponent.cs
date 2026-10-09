public abstract class PieceComponent
{
    public Piece piece { get; private set; }

    public PieceComponent(Piece piece)
    {
        this.piece = piece;
    }
}
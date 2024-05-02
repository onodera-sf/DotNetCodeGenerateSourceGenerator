public partial class Player
{
  public string Name { get; set; } = "";


  public float PositionX { get; set; }
  public float PositionY { get; set; }

  public int Level { get; set; } = 1;

  public override string ToString()
  {
    return $"{nameof(Player)} {{{nameof(Name)} = {Name}, {nameof(PositionX)} = {PositionX}, {nameof(PositionY)} = {PositionY}, {nameof(Level)} = {Level}}}";
  }
}
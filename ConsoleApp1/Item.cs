public partial class Item
{
  public string Name { get; set; } = "";

  public float PositionX { get; set; }
  public float PositionY { get; set; }

  public int Type { get; set; }

  public override string ToString()
  {
    return $"{nameof(Item)} {{{nameof(Name)} = {Name}, {nameof(PositionX)} = {PositionX}, {nameof(PositionY)} = {PositionY}, {nameof(Type)} = {Type}}}";
  }
}
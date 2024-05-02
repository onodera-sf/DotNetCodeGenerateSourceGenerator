Console.WriteLine("Hello, World!");

SampleClass.Hello();
Console.WriteLine();

Player player = new()
{
  Name = "Mike",
  PositionX = 10,
  PositionY = 5.5f,
  Level = 3,
};
Console.WriteLine(player);
player.Reset();
Console.WriteLine(player);
Console.WriteLine();

Item item = new()
{
  Name = "Banana",
  PositionX = 50,
  PositionY = 53.5f,
  Type = 12,
};
Console.WriteLine(item);
item.Reset();
Console.WriteLine(item);
Console.WriteLine();


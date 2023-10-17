
int number = Int32.Parse(Console.ReadLine());
Menu menu = new Menu();
List<Catalog> catalog = new List<Catalog>();
List<Nutrition> nutrition = new List<Nutrition>();
Converter converter = new Converter();
while (number > 0)
{
    string[] input = Input();
    menu.AddDish(input, converter);
    number--;
}
number = Int32.Parse(Console.ReadLine());
while (number > 0)
{
    string[] input = Input();
    catalog.Add(new Catalog(input, converter));
    number--;
}
number = Int32.Parse(Console.ReadLine());
while (number > 0)
{
    string[] input = Input();
    nutrition.Add(new Nutrition(input, converter));
    number--;
}
for (int i = 0; i < menu.dish.Count; i++)
{
    menu.dish[i].CalculateNutrition(nutrition);
}
List<Basket> basket = new List<Basket>();
int money = 0;
for (int i = 0; i < catalog.Count; i++)
{
    basket.Add(new Basket(menu, catalog[i]));
    money += basket[i].amountToBuy * catalog[i].price; 
}
Console.WriteLine();
Console.WriteLine(money);
for (int i = 0; i < basket.Count; i++)
{
    Console.WriteLine($"{basket[i].ingredientName} {basket[i].amountToBuy}");
}
for (int i = 0; i < menu.dish.Count; i++)
{
    Console.WriteLine($"{menu.dish[i].name} {menu.dish[i].nutritionOfDish[0]:#.###} " +
        $"{menu.dish[i].nutritionOfDish[1]:#.###} {menu.dish[i].nutritionOfDish[2]:#.###} {menu.dish[i].nutritionOfDish[3]:#.###}");
}



string[] Input()
{
    return Console.ReadLine().Split(' ').ToArray();
}

internal class Converter
{
    internal int Convert(int i, string s)
    {
        if (s == "l" || s == "kg") return i * 1000;
        else if (s == "tens") return i * 10;
        else return i;
    }
}

internal class Menu
{
    internal List<Dish> dish = new List<Dish> ();
    internal List<int> amount = new List<int> ();
    internal List<int> numberOfIngredients = new List<int> ();
    internal void AddDish(string[] input, Converter converter)
    {
        dish.Add(new Dish(input[0]));
        amount.Add(Int32.Parse(input[1]));
        numberOfIngredients.Add(Int32.Parse(input[2]));
        for (int i = 0; i < numberOfIngredients[^1]; i++)
        {
            dish[^1].AddIngredient(converter);
        }
    }
}
internal class Dish {
    internal string name{ get; set; }
    internal List<string> ingredientName = new List<string> ();
    internal List<int> amount = new List<int>();
    internal double[] nutritionOfDish = new double[] {0,0,0,0};
    internal Dish (string name)
    {
        this.name = name;
    }
    internal void AddIngredient(Converter converter)
    {
        string[] input = Console.ReadLine().Split(' ').ToArray();
        ingredientName.Add(input[0]);
        amount.Add(converter.Convert(Int32.Parse(input[1]), input[2]));
    }
    internal void CalculateNutrition(List<Nutrition> nutrition)
    {
        for (int i = 0; i < nutritionOfDish.Length; i++)
        {
            nutritionOfDish[i] = 0;
        }
        for (int i = 0; i < ingredientName.Count; i++)
        {
            for (int j = 0; j < nutrition.Count; j++)
            {
                if (ingredientName[i] == nutrition[j].ingredientName)
                {
                    for (int k = 0; k < nutritionOfDish.Length; k++)
                    {
                        nutritionOfDish[k] += nutrition[j].nutritions[k] * amount[i] / nutrition[j].amount;
                    }
                    break;
                }
            }
        }    
    }
}
internal class Catalog 
{
    internal string ingredientName { get; init; }
    internal int price { get; init; }
    internal int amountInPack {get; init; }
    internal Catalog(string[] input, Converter converter)
    {
        ingredientName = input[0];
        price = Int32.Parse(input[1]);
        amountInPack = converter.Convert(Int32.Parse(input[2]), input[3]);
    }
}
internal class Nutrition
{
    internal string ingredientName {get; init;}
    internal int amount { get; init; }
    internal double[] nutritions { get; init; }
    internal Nutrition(string[] input, Converter converter)
    {
        ingredientName = input[0];
        amount = converter.Convert(Int32.Parse(input[1]), input[2]);
        nutritions = new double[] { Double.Parse(input[3]), Double.Parse(input[4]), Double.Parse(input[5]), Double.Parse(input[6])};
    }
}
internal class Basket
{
    internal string ingredientName { get; init;}
    internal int amountNeeded { get; set; } = 0;
    internal int amountToBuy { get; set; } = 0;
    internal Basket(Menu menu, Catalog catalog)
    {
        ingredientName = catalog.ingredientName;
        for (int i = 0; i < menu.dish.Count; i++)
        {
            for (int j = 0; j < menu.dish[i].ingredientName.Count; j++)
            {
                if(ingredientName == menu.dish[i].ingredientName[j])
                {
                    amountNeeded += menu.dish[i].amount[j] * menu.amount[i];
                    break;
                }
            }
        }
        amountToBuy = (int)Math.Ceiling((double)amountNeeded / catalog.amountInPack);
    }
}
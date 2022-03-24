// See https://aka.ms/new-console-template for more information
var test = ",,,,,6,1,8,,,8,,,7,,3,9,,,3,,4,,,7,,,,,,5,,,8,,,9,,,,,,,,1,,,7,,,1,,,,,,9,,,3,,5,,,7,5,,6,,,3,,,2,6,9,,,,,";
var list = test.Split(',');
var count = 1;
foreach (var item in list) 
{   
    Console.Write((string.IsNullOrEmpty(item) ? "_" : item) + "|");
    if (count == 9)
    {
        Console.Write("\n");
        count = 0;
    }

        ++count;
}
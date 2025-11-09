using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public class Cafe
{
    List<string> item = new List<string>();
    List<float> price = new List<float>();
    List<int> quantity = new List<int>();
    string cafe_name = "Muncho's Munchy Cafe";
    float tax_rate = 0.095f;
    Boolean discountUsed = false;
    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "cafe.txt");

    public void ShowBanner()
    {
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine(cafe_name + " - Tax Rate: " + tax_rate);
        Console.WriteLine("-------------------------------------------");
    }

    public int mainMenu()
    {
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("{1} Add Items");
        Console.WriteLine("{2} View Cart");
        Console.WriteLine("{3} Remove Items");
        Console.WriteLine("{4} Checkout");
        Console.WriteLine("{5} Save Cart");
        Console.WriteLine("{6} Load Cart");
        Console.WriteLine("{7} Quit");
        try
        {
            int menuChoice = int.Parse(Console.ReadLine());
            Console.WriteLine("-------------------------------------------");
            if (menuChoice > 7 || menuChoice < 1)
            {
                throw new Exception("Sorry, please try again.");
            }
            return menuChoice;
        }
        catch (Exception)
        {
            Console.WriteLine("Sorry, please try again.");
            return mainMenu();
        }
    }

    public void addItems()
    {
        while (true)
        {
            Console.Write("Name of item:");
            string itemInput = Console.ReadLine();
            if (itemInput.ToUpper() == "BACK")
            {
                break;
            }
            if (itemInput == "")
            {
                Console.WriteLine("Sorry, you must enter an item.");
                continue;
            }
            float priceV = 0;
            while (true)
            {
                Console.Write("Price of item:");
                try
                {
                    priceV = float.Parse(Console.ReadLine());
                    if (priceV <= 0)
                    {
                        throw new Exception("Sorry, the price cannot be a negative.");
                    }
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Sorry, the price cannot be a negative.");
                }
            }
            int quantityV = 0;
            while (true)
            {
                Console.Write("Quantity of item:");
                try
                {
                    quantityV = int.Parse(Console.ReadLine());
                    if (quantityV <= 0)
                    {
                        throw new Exception("Sorry, the quantity cannot be a negative.");
                    }
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Sorry, the quantity cannot be a negative.");
                }
            }
            item.Add(itemInput);
            price.Add(priceV);
            quantity.Add(quantityV);
        }
    }

    public void viewCart()
    {
        if (item.Count != price.Count || price.Count != quantity.Count || quantity.Count != item.Count)
        {
            Console.WriteLine("Sorry, you must have one value per item.");
            return;
        }
        if (item.Count == 0)
        {
            Console.WriteLine("Sorry, you must have on value per item.");
        }
        else
        {
            for (int i = 0; i < item.Count; i++)
            {
                Console.WriteLine($"{item[i]}  ${price[i]}  x{quantity[i]}  =  ${price[i] * quantity[i]:F2}");
            }
        }
        float aveLine = averageLineTotal();
        Console.WriteLine($"Subtotal of cart: ${computeSubtotal():F2}");
        Console.WriteLine($"Average line total of cart: ${aveLine:F2}");
        var (itemName, itemPrice) = mostExpensive();
        if (itemName != null)
        {
            Console.WriteLine($"Most expensive item in cart: {itemName} at ${itemPrice:F2} per unit.");
        }
        Console.Write("Enter anything to go back.");
        string leavecart = Console.ReadLine();
    }

    public float averageLineTotal()
    {
        if (item.Count == 0)
        {
            return 0;
        }
        float cartTotal = 0;
        for (int i = 0; i < price.Count; i++)
        {
            cartTotal += price[i] * quantity[i];
        }
        return cartTotal / item.Count;
    }

    public (string, float) mostExpensive()
    {
        if (item.Count == 0)
        {
            return (null, 0);
        }
        float highestPrice = price.Max();
        for (int i = 0; i < item.Count; ++i)
        {
            if (price[i] == highestPrice)
            {
                return (item[i], price[i]);
            }
        }
        return (null, 0);
    }

    public float computeSubtotal()
    {
        float subtotal = 0;
        for (int i = 0; i < price.Count; ++i)
        {
            subtotal += price[i] * quantity[i];
        }
        return subtotal;
    }

    public void removeItem()
    {
        int n = 1;
        for (int i = 0; i < item.Count; i++)
        {
            Console.WriteLine($"{n}. {item[i]}  ${price[i]}  x{quantity[i]}  =  ${price[i] * quantity[i]:F2}");
            n++;
        }
        Console.Write("Type the number of the item you wish to delete.");
        string removeChoice = Console.ReadLine();
        if (removeChoice == "")
        {
            return;
        }
        try
        {
            int index = int.Parse(removeChoice) - 1;
            if (0 <= index && index < item.Count())
            {
                item.RemoveAt(index);
                price.RemoveAt(index);
                quantity.RemoveAt(index);
            }
            else
            {
                Console.WriteLine("Sorry, please enter a valid number.");
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Sorry, please enter a valid number.");
        }
    }

    public void checkout()
    {
        float subtotal = computeSubtotal();
        Console.WriteLine("CHECKOUT");
        Console.WriteLine($"Cart subtotal: ${subtotal:F2}");
        float discountedSubtotal = applyDiscount(subtotal);
        float tax = computeTax(discountedSubtotal);
        float discountAmount = subtotal - discountedSubtotal;
        float total = discountedSubtotal + tax;
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("---RECIEPT---");
        Console.WriteLine($"Subtotal: ${subtotal:F2}");
        Console.WriteLine($"Discount: -${discountAmount:F2}");
        Console.WriteLine($"Tax: ${tax:F2}");
        Console.WriteLine($"TOTAL: ${total:F2}");
        Console.WriteLine("Thank you for shopping at Muncho's Munchy Cafe!");
    }

    public float applyDiscount(float subtotal)
    {
        Console.Write("Enter discount code: (Or type anything to skip.)");
        string discount = Console.ReadLine();
        if (discountUsed)
        {
            Console.WriteLine("Code already used!");
            return subtotal;
        }
        if (discount == "STUDENT10")
        {
            float discountedSubtotal = subtotal * 0.90f;
            discountUsed = true;
            return discountedSubtotal;
        }
        else
        {
            return subtotal;
        }
    }

    public float computeTax(float discountedSubtotal)
    {
        return discountedSubtotal * tax_rate;
    }

    public void saveCart()
    {
        string toSave = "";
        for (int i = 0; i < item.Count; i++)
        {
            if (i == 0)
            {
                toSave += $"{item[i]}  ${price[i]}  x{quantity[i]}  =  ${price[i] * quantity[i]:F2}";
            }
            else
            {
                toSave += $"\n{item[i]}  ${price[i]}  x{quantity[i]}  =  ${price[i] * quantity[i]:F2}";
            }
        }
        File.WriteAllText(path, toSave);
        Console.WriteLine("Cart saved!");
    }

    public void loadCart()
    {
        try
        {
            item.Clear();
            price.Clear();
            quantity.Clear();
            string toLoad = File.ReadAllText(path);
            string[] loadLines = toLoad.Split('\n');
            foreach (string loadLine in loadLines)
            {
                string[] seperateParts = loadLine.Split(new string[] { "  $", "  x", "  =" }, StringSplitOptions.RemoveEmptyEntries);
                string itemName = seperateParts[0].Trim();
                float priceName = float.Parse(seperateParts[1].Trim());
                int quantityName = int.Parse(seperateParts[2].Trim());

                item.Add(itemName);
                price.Add(priceName);
                quantity.Add(quantityName);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.WriteLine("Cart loaded!");
    }

    public static void Main(string[] args)
    {
        Cafe cafeRun = new Cafe();
        cafeRun.ShowBanner();
        int menuChoice = 0;
        while (menuChoice != 7)
        {
            menuChoice = cafeRun.mainMenu();
            if (menuChoice == 1)
            {
                cafeRun.addItems();
            }
            if (menuChoice == 2)
            {
                cafeRun.viewCart();
            }
            if (menuChoice == 3)
            {
                cafeRun.removeItem();
            }
            if (menuChoice == 4)
            {
                cafeRun.checkout();
            }
            if (menuChoice == 5)
            {
                cafeRun.saveCart();
            }
            if (menuChoice == 6)
            {
                cafeRun.loadCart();
            }
            if (menuChoice == 7)
            {
                Console.WriteLine("Thank you for using the program! Goodbye!");
            }
        }
    }
}
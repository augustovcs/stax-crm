// See https://aka.ms/new-console-template for more information

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading;

namespace STAXCRM;


public class UserMain
{
    public int Id { get; set; }
    public string Name { get; set; }
    public char Sex { get; set; }
    public int Age { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}\nName: {Name}\nSex: {Sex}\nAge: {Age}\nHeight: {Height}\nWeight: {Weight}\n";
    }
}

public class MenuBool
{
    public bool MainMenu { get; set; }
    public bool ReturnMenuCreateUser { get; set; }
    public bool CreateUserMenu { get; set; }
    public bool ListAllUsersMenu { get; set; }
    public bool SearchUserMenu { get; set; }
    public bool UpdateUserMenu { get; set; }
    public bool DeleteUserMenu { get; set; }
    
    public MenuBool ()
    {
        MainMenu = true;
        ReturnMenuCreateUser = false;
    }

    public void ActivateCreateUserMenu()
    {
        MainMenu = false;
        ListAllUsersMenu = false;
        CreateUserMenu = true;
    }

    public void ActivateListAllUsersMenu()
    {
        MainMenu = false;
        ListAllUsersMenu = true;
    }
    
    public void ActivateSearchUserMenu()
    {
        MainMenu = false;
        SearchUserMenu = true;
    }

    public void ActivateUpdateUserMenu()
    {
        MainMenu = false;
        UpdateUserMenu = true;
        ListAllUsersMenu = false;
    }
    
    public void ActivateDeleteUserMenu()
    {
        MainMenu = false;
        DeleteUserMenu = true;
    }
    
    public void ActivateReturnMenuCreateUser()
    {
        MainMenu = false;
        ReturnMenuCreateUser = true;
    }

    public void ActivateMainMenu()
    {
        MainMenu = true;
        ReturnMenuCreateUser = false;
    }
       
}



public class Program

{
    //MODULAR FUNCTIONS VALIDATING INPUTS
    public static int ReadValidInt(string message, int min = int.MinValue, int max = int.MaxValue)
    {
        int value;
        Console.Write(message);
        while (!int.TryParse(Console.ReadLine(), out value) || value < min || value > max)
        {
            Console.Write("Invalid input. Please, enter a valid number: ");
        }
        return value;
    }

    public static double ReadValidDouble(string message, double min = double.MinValue, double max = double.MaxValue)
    {
        double value;
        Console.Write(message);
        while (true)
        {
            string enterdouble = Console.ReadLine().Replace('.', ',');
            if (double.TryParse(enterdouble, NumberStyles.Any, CultureInfo.GetCultureInfo("pt-BR"), out value)
                && value >= min && value <= max)
            {
                return value;
            }
            Console.Write("Invalid input. Please, enter a valid number: ");
        }
    }

    public static char ReadValidCharGender(string message)
    {
        char value;
        Console.Write(message);
        while (!char.TryParse(Console.ReadLine(), out value) || value != 'M' && value != 'F')
        {
            Console.Write("Invalid input. Please, enter a valid gender (M/F): ");
        }
        return value;
    }

    public static string ReadValidString(string message)
    {
        string value;
        Console.Write(message);
        while (string.IsNullOrWhiteSpace(value = Console.ReadLine())) 
        {
           Console.Write("Invalid input. Please, enter a valid string: " + message);
        }

        return value;
    }
    
    
    //Main Menu
    public static void Main(string[] args)
    {
        MenuBool state = new MenuBool();


        //Verify if has a database file, if not, create a new one
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "usersdatabase.json");

        List<UserMain> usersDB = new List<UserMain>();
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            usersDB = JsonSerializer.Deserialize<List<UserMain>>(jsonString);
        }
        else
        {
            usersDB = new List<UserMain>();
        }
            
        
        
        
        while (true)
        {
            if (state.MainMenu)
            {
                Console.WriteLine("----- CRM STAX beta 0.1 ----");
                Console.WriteLine("Please, choose a option: \n");
                Console.WriteLine("1 - Create a new user");
                Console.WriteLine("2 - List all users");
                Console.WriteLine("3 - Search a user");
                Console.WriteLine("4 - Update a user");
                Console.WriteLine("5 - Delete a user");
                Console.WriteLine("6 - Exit");
                Console.WriteLine("-------------------------");
                Console.Write("Your option: ");
                int optionChoose = int.Parse(Console.ReadLine());

                if (optionChoose == 1)
                {
                    state.ActivateCreateUserMenu();
                    string name = ReadValidString("Your full name: ");
                    int age = ReadValidInt("Your age: ", 1, 90);
                    char sex = ReadValidCharGender("Are you Male or Female? (M/F): ");
                    double height = ReadValidDouble("What is your height? ", 1.0, 3.0);
                    double weight = ReadValidDouble("What is your weight?(In Kilograms) ", 1.0, 300.0);
                    int newId = usersDB.Any() ? usersDB.Max(x => x.Id) + 1 : 1;

                    UserMain user = new UserMain
                    {
                        Id = newId,
                        Name = name,
                        Age = age,
                        Sex = sex,
                        Height = height,
                        Weight = weight
                    };

                    Console.WriteLine("Creating a new user...");
                    usersDB.Add(user);
                    Console.WriteLine("User created successfully!");

                    string jsonString = JsonSerializer.Serialize(usersDB, new JsonSerializerOptions
                        { WriteIndented = true });
                    File.WriteAllText(filePath, jsonString);
                    state.ActivateReturnMenuCreateUser();
                    
                }
                else if (optionChoose == 2)
                {
                        state.ActivateListAllUsersMenu();
                   
                }
                else if (optionChoose == 3)
                {
                    SearchAnUser(usersDB);
                    state.ActivateSearchUserMenu();
                    
                }
                
                else if (optionChoose == 4)
                {
                    UpdateAnUser(usersDB);
                    state.ActivateMainMenu();
                }
                
                else if (optionChoose == 5)
                {
                    DeleteAnUser(usersDB);
                    state.ActivateMainMenu();
                }

                else if (optionChoose == 6)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option! Try again. \n");
                }
            }
            
            // RETURN MENU OPTION AFTER CREATE USER
            else if(state.ReturnMenuCreateUser)
            {
                Console.WriteLine("\n Thanks, now what would you like to do?");
                Console.WriteLine("1 - Access Main Menu");
                Console.WriteLine("2 - Exit");
                Console.Write("Your option: ");
                
                int optionChoose = int.Parse(Console.ReadLine());
                
                if (optionChoose == 1)
                {
                    state.ActivateMainMenu();
                }
                else if (optionChoose == 2)
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                
            }
            
            //LIST ALL USERS MENU
            else if (state.ListAllUsersMenu)
            {
                ListAllUsers(usersDB);
                Console.WriteLine("\n What you want to do?" );
                Console.WriteLine("1 - Back to Main Menu ");
                Console.WriteLine("2 - Create a new user ");
                Console.WriteLine("3 - Select a user ");
                Console.WriteLine("4 - Update a user ");
                Console.WriteLine("5 - Delete a user ");
                Console.WriteLine("6 - Exit Program ");
                Console.Write("Your option: ");
                
                int optionChoose = int.Parse(Console.ReadLine());
                
                if (optionChoose == 1)
                {
                    state.ActivateMainMenu();
                }
                
                else if (optionChoose == 2)
                {
                    CreateUser(usersDB, filePath, state);
                }
                
                else if (optionChoose == 3)
                {
                    SearchAnUser(usersDB);
                }
                
                else if (optionChoose == 4)
                {
                    UpdateAnUser(usersDB);
                }
                
                else if (optionChoose == 5)
                {
                    DeleteAnUser(usersDB);
                }
                
                else if (optionChoose == 6)
                {
                    Console.WriteLine("Goodbye baby!");
                    break;
                }
            }
        }
        
    }
    
    //SAVE USERS FUNCTION
    public static void SaveUsers(List<UserMain> usersDB, string filePath)
    {
        string jsonString = JsonSerializer.Serialize(usersDB, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("usersdatabase.json", jsonString);
    }
    
    //DELETE USERS FUNCTION
    public static void DeleteUsers(List<UserMain> usersDB, string filePath, int id)
    {
        int removed = usersDB.RemoveAll(u => u.Id == id);
        if (removed > 0)
        {
            SaveUsers(usersDB, filePath);
            Console.WriteLine($"Removed {removed} users");
        }
        else
        {
            Console.WriteLine("No users find with that ID");
        }
        
        SaveUsers(usersDB, filePath);
    }
    
    
    
    //CREATE USER FUNCTION
    public static void CreateUser(List<UserMain> usersDB, string filePath, MenuBool state)
    {
        //Create a new user
        Console.WriteLine("\n ----- CREATE A NEW USER ----- \n");
        string name = ReadValidString("Your full name: ");
        int age = ReadValidInt("Your age: ", 1, 90);
        char sex = ReadValidCharGender("Are you Male or Female? (M/F): ");
        double height = ReadValidDouble("What is your height? ", 1.0, 3.0);
        double weight = ReadValidDouble("What is your weight?(In Kilograms) ", 1.0, 300.0);
   

        int newId = usersDB.Count + 1;

        UserMain user = new UserMain()
        {
            Id = newId,
            Name = name,
            Age = age,
            Sex = sex,
            Height = height,
            Weight = weight
        };
        
        Console.WriteLine("Creating a new user...");
        usersDB.Add(user);
        Console.WriteLine("User created successfully!");

        string jsonString = JsonSerializer.Serialize(usersDB, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, jsonString);
        state.ActivateReturnMenuCreateUser();

    }
    
    //LIST ALL USERS FUNCTION
    public static void ListAllUsers(List<UserMain> usersDB)
    {
        Console.WriteLine("\n ----- LIST OF USERS ----- \n");
        foreach (var user in usersDB)
        {
            Console.WriteLine(user);
        }
    }
    
    //SEARCH AN USER FUNCTION
    public static void SearchAnUser(List<UserMain> usersDB)
    {
        Console.WriteLine("\n ----- SEARCH AN USER ----- \n");
        Console.Write("Type the ID of the user you want to search: ");
        int id = int.Parse(Console.ReadLine());
        var user = usersDB.FirstOrDefault(x => x.Id == id);
        if (user != null)
        {
            Console.WriteLine(user);
        }
        else
        {
            Console.WriteLine("User not found!");
        }
    }



    //UPDATE AN USER FUNCTION
    public static void UpdateAnUser(List<UserMain> usersDB)
    {
        Console.WriteLine("\n ----- UPDATE AN USER ----- \n");
        Console.Write("Type the ID of the user you want to update: ");
        int id = int.Parse(Console.ReadLine());
        var user = usersDB.FirstOrDefault(x => x.Id == id);
        
        if (user != null)
        {
            ShowUpdateUserMenu(user, usersDB);
        }
        else
        {
            Console.WriteLine("User not found!");
        }
    }

    //UPDATE USER MENU FUNCTION
    public static void ShowUpdateUserMenu(UserMain user, List<UserMain> usersDB)
    {
        bool runningUpdateUserMenu = true;
        while (runningUpdateUserMenu)
        {
            Console.WriteLine(user);
            Console.WriteLine("What you want to change? ");
            Console.WriteLine("----------------------------");
            Console.WriteLine("1 - Name");
            Console.WriteLine("2 - Age");
            Console.WriteLine("3 - Sex");
            Console.WriteLine("4 - Height");
            Console.WriteLine("5 - Weight");
            Console.WriteLine("6 - Exit Menu");
            Console.WriteLine("----------------------------");
            Console.Write("Your option: ");
            
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int optionChoose))
            {
                Console.WriteLine("Invalid option! Try again. \n");
                continue;
            }
            
            switch (optionChoose)
            {
                case 1:
                    Console.Write("Type the new name: ");
                    user.Name = Console.ReadLine();
                    Console.WriteLine(user);
                    break;
                case 2:
                    Console.Write("Type the new age: ");
                    user.Age = int.Parse(Console.ReadLine());
                    break;
                case 3:
                    Console.Write("Type the new Sex: ");
                    user.Sex = char.Parse(Console.ReadLine());
                    break;
                case 4:
                    Console.Write("Type the new Height: ");
                    user.Height = double.Parse(Console.ReadLine());
                    break;
                case 5:
                    Console.Write("Type the new Weight: ");
                    user.Weight = double.Parse(Console.ReadLine());
                    break;
                case 6:
                    Console.WriteLine("Saving and exiting Update Menu...");
                    SaveUsers(usersDB, filePath: "usersdatabase.json");
                    runningUpdateUserMenu = false;
                    return;
                default:
                    Console.WriteLine("Invalid option! Try again. \n");
                    break;
            }
            
        }

    }

    public static void DeleteAnUser(List<UserMain> usersDB)
    {
        Console.WriteLine("\n ----- DELETE AN USER ----- \n");
        ListAllUsers(usersDB);
        Console.Write("Type the ID of the user you want to delete: ");
        int id = int.Parse(Console.ReadLine());
        var user = usersDB.FirstOrDefault(x => x.Id == id);
        
        if (user != null)
        {
           DeleteUsers(usersDB, filePath: "usersdatabase.json", id: id);;
        }
        else
        {
            Console.WriteLine("User not found!");
        }
    }
    
    
}
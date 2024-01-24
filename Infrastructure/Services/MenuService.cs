using Infrastructure.Dtos;

namespace Infrastructure.Services
{

    public class MenuService
    {
        private readonly UserService _userService;

        public MenuService(UserService userService)
        {
            _userService = userService;
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                DisplayMenuTitle("MENU OPTIONS");
                Console.WriteLine($"{"1.",-4} Add New User");
                Console.WriteLine($"{"2.",-4} Wiew Users");
                Console.WriteLine($"{"3.",-4} Wiew User Details");
                Console.WriteLine($"{"4.",-4} Delete User");
                Console.WriteLine($"{"0.",-4} Exit Application");
                Console.WriteLine();
                Console.Write("Enter Menu Option: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ShowAddUserOption();
                        break;
                    //case "2":
                    //    ShowViewUserListOption();
                    //    break;
                    //case "3":
                    //    ShowUserDetailOption();
                    //    break;
                    //case "4":
                    //    ShowUpdateUserOption();
                    //    break;
                    //case "5":
                    //    ShowDeleteUserOption();
                    //    break;
                    //case "0":
                    //    ShowExitApplicationOption();
                        //break;
                    default:
                        Console.WriteLine("\nInvalid option selected. Press any key to continue.");
                        Console.ReadKey();
                        break;

                }

            }

        }
        private void ShowAddUserOption()
        {
            UserRegistrationDto userData = new UserRegistrationDto();


            DisplayMenuTitle("Add New Contact");

            Console.Write("First Name: ");
            userData.FirstName = Console.ReadLine()!;

            Console.Write("Last Name: ");
            userData.LastName = Console.ReadLine()!;

            Console.Write("Street Name: ");
            userData.StreetName = Console.ReadLine()!;

            Console.Write("Postal Code: ");
            userData.PostalCode = Console.ReadLine()!;

            Console.Write("City: ");
            userData.City = Console.ReadLine()!;

            Console.Write("Role Name: ");
            userData.RoleName = Console.ReadLine()!;

            Console.Write("E-mail: ");
            userData.Email = Console.ReadLine()!;

            Console.Write("Password: ");
            userData.Password = Console.ReadLine()!;

            _userService.CreateUser(userData);

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("User added successfully. Press any key to continue.");

            Console.ReadKey();

        }

        //private void ShowUpdateUserOption()
        //{
        //}

        //    private void ShowDeleteUserOption()
        //{
        //    DisplayMenuTitle("Delete Contact");

        //    Console.Write("Enter the email of the contact to delete: ");
        //    var emailToDelete = Console.ReadLine();

        //    if (_personService.DeletePerson(emailToDelete!))
        //    {
        //        Console.WriteLine();
        //        Console.WriteLine("Contact deleted successfully. Press any key to continue.");
        //    }
        //    else
        //    {
        //        Console.WriteLine();
        //        Console.WriteLine("Contact not found. Press any key to continue.");
        //    }

        //    Console.ReadKey();

        //}

        //private void ShowViewUserListOption()
        //{
        //    var persons = _personService.GetPersonsFromList();



        //    DisplayMenuTitle("Contact List");

        //    foreach (var item in persons)
        //    {
        //        Console.WriteLine($"{item.FirstName} {item.LastName} {item.Email}");
        //    }

        //    Console.WriteLine();
        //    Console.WriteLine("Press any key to go back to MENU OPTIONS.");

        //    Console.ReadKey();
        //}

        //private void ShowUserDetailOption()
        //{
        //    DisplayMenuTitle("View Contact Details");

        //    Console.Write("Enter the email of the contact to view details: ");
        //    var emailToView = Console.ReadLine();

        //    var person = _personService.GetPersonByEmail(emailToView!);

        //    if (person != null)
        //    {
        //        Console.WriteLine($"Name: {person.FirstName} {person.LastName}");
        //        Console.WriteLine($"Street: {person.StreetName}");
        //        Console.WriteLine($"Postal Code: {person.PostalCode}");
        //        Console.WriteLine($"City: {person.City}");
        //        Console.WriteLine($"Email: {person.Email}");
        //        Console.WriteLine($"Phone: {person.Phone}");

        //        Console.WriteLine();
        //        Console.WriteLine("Press any key to go back to MENU OPTIONS.");
        //    }
        //    else
        //    {
        //        Console.WriteLine();
        //        Console.WriteLine("Contact not found. Press any key to continue.");
        //    }

        //    Console.ReadKey();
        //}



        private void ShowExitApplicationOption()
        {
            Console.Clear();
            Console.WriteLine("Are you sure you want to close this application? (y/n): ");
            var option = Console.ReadLine() ?? "";

            if (option.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                Environment.Exit(0);
        }

        private void DisplayMenuTitle(string title)
        {
            Console.Clear();
            Console.WriteLine($"## {title} ##");
            Console.WriteLine();
        }


    }
}

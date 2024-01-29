using Infrastructure.Dtos;
using System.Transactions;

namespace Infrastructure.Services
{

    public class MenuService(UserService userService)
    {
        private readonly UserService _userService = userService;

        public void ShowMainMenu()
        {
            while (true)
            {
                DisplayMenuTitle("MENU OPTIONS");
                Console.WriteLine($"{"1.",-4} Add New User");
                Console.WriteLine($"{"2.",-4} Wiew Users");
                Console.WriteLine($"{"3.",-4} Wiew User Details");
                Console.WriteLine($"{"4.",-4} Update User Details");
                Console.WriteLine($"{"5.",-4} Delete User");
                Console.WriteLine($"{"0.",-4} Exit Application");
                Console.WriteLine();
                Console.Write("Enter Menu Option: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ShowAddUserOption();
                        break;
                    case "2":
                        ShowViewUserListOption();
                        break;
                    case "3":
                        ShowUserDetailOption();
                        break;
                    //case "4":
                    //    ShowUpdateUserOption();
                    //    break;
                    case "5":
                        ShowDeleteUserOption();
                        break;
                    case "0":
                        ShowExitApplicationOption();
                        break;
                    default:
                        Console.WriteLine("\nInvalid option selected. Press any key to continue.");
                        Console.ReadKey();
                        break;

                }

            }

        }
        private async void ShowAddUserOption()
        {
            UserRegistrationDto userData = new UserRegistrationDto();


            DisplayMenuTitle("Add New User");

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

            await _userService.CreateUser(userData);

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("User added successfully. Press any key to continue.");

            Console.ReadKey();

        }


        //private void ShowUpdateUserOption()
        //{
        //    Console.Clear();
        //    Console.WriteLine("Enter User Id: ");
        //    var id = int.Parse(Console.ReadLine()!);

        //    var user = _userService.GetUserById(id);
        //    if (user != null)
        //    {
        //        Console.WriteLine($"{user.FirstName} {user.LastName} {user.Email}");
        //    }
        //}


        private async Task ShowDeleteUserOption()
        {
            DisplayMenuTitle("Delete User");

            Console.Write("Enter the email of the user you want to delete: ");
            var emailToDelete = Console.ReadLine();

            

            if (await _userService.DeleteUserByEmail(emailToDelete!))
            {
                Console.WriteLine();
                Console.WriteLine("User deleted successfully. Press any key to continue.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("User not found. Press any key to continue.");
            }

            Console.ReadKey();

        }



        private async Task ShowViewUserListOption()
        {
            var users = await _userService.GetAllUsers();

            DisplayMenuTitle("User List");

            foreach (var item in users)
            {
                Console.WriteLine($"{item.FirstName} {item.LastName}, {item.RoleName}");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to go back to MENU OPTIONS.");

            Console.ReadKey();
        }



        private async void ShowUserDetailOption()
        {
            DisplayMenuTitle("View User Details");

            Console.Write("Enter the email of the user to view details: ");
            var emailToView = Console.ReadLine();

            var user = await _userService.GetUserByEmailAsync(emailToView!);

            if (user != null)
            {
                Console.WriteLine($"Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Street: {user.StreetName}");
                Console.WriteLine($"Postal Code: {user.PostalCode}");
                Console.WriteLine($"City: {user.City}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Role: {user.RoleName}");

                Console.WriteLine();
                Console.WriteLine("Press any key to go back to MENU OPTIONS.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("User not found. Press any key to continue.");
            }

            Console.ReadKey();
        }



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

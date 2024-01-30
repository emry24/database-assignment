using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Transactions;

namespace Infrastructure.Services
{

    public class MenuService(UserService userService)
    {
        private readonly UserService _userService = userService;

        public async Task ShowMainMenu()
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
                        await ShowViewUserListOption();
                        break;
                    case "3":
                        ShowUserDetailOption();
                        break;
                    case "4":
                        await ShowUpdateUserOption();
                        break;
                    case "5":
                        await ShowDeleteUserOption();
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


        private async Task ShowUpdateUserOption()
        {
            Console.Clear();
            Console.WriteLine("Enter User Email: ");
            var email = Console.ReadLine();

            var existingUser = await _userService.GetUserByEmailAsync(email!);
            if (existingUser != null)
            {
                Console.WriteLine($"Current Address Details for {existingUser.FirstName} {existingUser.LastName}:");
                Console.WriteLine($"Street: {existingUser.StreetName}");
                Console.WriteLine($"Postal Code: {existingUser.PostalCode}");
                Console.WriteLine($"City: {existingUser.City}");

                Console.WriteLine("\nEnter Updated Address Details:");

                var updatedAddress = new UserRegistrationDto
                {
                    Email = email!, 
                    StreetName = GetUserInput("Street: "),
                    PostalCode = GetUserInput("Postal Code: "),
                    City = GetUserInput("City: ")
                };

                var success = await _userService.UpdateUserAddress(updatedAddress);
                if (success)
                {
                    Console.WriteLine("\nAddress updated successfully!");
                }
                else
                {
                    Console.WriteLine("\nFailed to update address.");
                }
            }
            else
            {
                Console.WriteLine("\nUser not found.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }







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

        private string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()!;
        }

    }
}

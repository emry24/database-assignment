using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
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
                Console.WriteLine($"{"4.",-4} Update User Address");
                Console.WriteLine($"{"5.",-4} Delete User");
                Console.WriteLine($"{"0.",-4} Exit Application");
                Console.WriteLine();
                Console.Write("Enter Menu Option: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ShowAddUserOption().ConfigureAwait(false).GetAwaiter().GetResult();

                        break;
                    case "2":
                        ShowViewUserListOption().ConfigureAwait(false).GetAwaiter().GetResult();
                        break;
                    case "3":
                        ShowUserDetailOption().ConfigureAwait(false).GetAwaiter().GetResult();
                        break;
                    case "4":
                        ShowUpdateUserOption().ConfigureAwait(false).GetAwaiter().GetResult();
                        break;
                    case "5":
                        ShowDeleteUserOption().ConfigureAwait(false).GetAwaiter().GetResult();
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

        private async Task<bool> ShowAddUserOption()
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
            Console.WriteLine("\nUser added successfully. Press any key to continue.");
            Console.ReadKey();

            return true;

        }


        private async Task ShowViewUserListOption()
        {
            var users = await _userService.GetAllUsers();

            DisplayMenuTitle("User List");

            foreach (var item in users)
            {
                Console.WriteLine($"{item.FirstName} {item.LastName}, {item.RoleName}");
            }

            Console.WriteLine("\nPress any key to go back to MENU OPTIONS.");

            Console.ReadKey();
        }


        private async Task ShowUserDetailOption()
        {
            DisplayMenuTitle("View User Details");

            Console.Write("Enter the email of the user to view details: ");
            var emailToView = Console.ReadLine();

            var user = await _userService.GetUserByEmailAsync(emailToView!);

            if (user != null)
            {
                Console.WriteLine($"Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Street Name: {user.StreetName}");
                Console.WriteLine($"Postal Code: {user.PostalCode}");
                Console.WriteLine($"City: {user.City}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Role: {user.RoleName}");

                Console.WriteLine("\nPress any key to go back to MENU OPTIONS.");
            }
            else
            {
                Console.WriteLine("\nUser not found. Press any key to continue.");
            }

            Console.ReadKey();
        }


        private async Task ShowUpdateUserOption()
        {

                Console.Clear();
                DisplayMenuTitle("Update User");

                Console.Write("Enter User Email: ");
                var emailToView = Console.ReadLine();

                var user = await _userService.GetUserByEmailAsync(emailToView!);

                if (user != null)
                {
                    Console.WriteLine($"Name: {user.FirstName} {user.LastName}");
                    Console.WriteLine($"Street Name: {user.StreetName}");
                    Console.WriteLine($"Postal Code: {user.PostalCode}");
                    Console.WriteLine($"City: {user.City}");

                    Console.WriteLine("\nEnter Updated Address Details: ");

                    Console.Write("Street Name: ");
                    var streetName = Console.ReadLine();

                    Console.Write("Postal Code: ");
                    var postalCode = Console.ReadLine();

                    Console.Write("City: ");
                    var city = Console.ReadLine();

                    var updatedAddress = new UserRegistrationDto
                    {
                        Email = emailToView!,
                        StreetName = streetName,
                        PostalCode = postalCode,
                        City = city
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

                Console.WriteLine("\nPress any key to continue.");
                Console.ReadKey();

        }







        private async Task ShowDeleteUserOption()
        {
            DisplayMenuTitle("Delete User");

            Console.Write("Enter the email of the user you want to delete: ");
            var emailToDelete = Console.ReadLine();

            

            if (await _userService.DeleteUserByEmail(emailToDelete!))
            {
                Console.WriteLine("\nUser deleted successfully. Press any key to continue.");
            }
            else
            {
                Console.WriteLine("\nUser not found. Press any key to continue.");
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

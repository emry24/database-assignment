using Infrastructure.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;

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
                Console.WriteLine($"{"4.",-4} Update User");
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
                        ShowUpdateUserOption(); //.ConfigureAwait(false).GetAwaiter().GetResult();
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

            await _userService.CreateUserAsync(userData);

            Console.Clear();
            Console.WriteLine("\nUser added successfully. Press any key to continue.");
            Console.ReadKey();

            return true;
        }

        private async Task ShowViewUserListOption()
        {
            var users = await _userService.GetAllUsersAsync();

            DisplayMenuTitle("User List");

            foreach (var item in users)
            {
                Console.WriteLine($"{item.FirstName} {item.LastName}, {item.RoleName}");
            }

            Console.WriteLine("\nPress any key to continue.");
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
                Console.Clear();
                Console.WriteLine($"Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Street Name: {user.StreetName}");
                Console.WriteLine($"Postal Code: {user.PostalCode}");
                Console.WriteLine($"City: {user.City}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Role: {user.RoleName}");

                Console.WriteLine("\nPress any key to continue.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\nUser not found.");
            }
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        private void ShowUpdateUserOption()
        {
            while (true)
            {
                DisplayMenuTitle("Update User");
                Console.WriteLine($"{"1.",-4} Update Contact Information");
                Console.WriteLine($"{"2.",-4} Update User Role");
                Console.WriteLine($"{"3.",-4} Update User Authentication");
                Console.WriteLine($"{"0.",-4} Go back to main menu");
                Console.Write("\nEnter Menu Option: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ShowUpdateContactOption().ConfigureAwait(false).GetAwaiter().GetResult();

                        break;
                    case "2":
                        ShowUpdateRoleOption().ConfigureAwait(false).GetAwaiter().GetResult();
                        break;
                    case "3":
                        ShowUpdateUserAuthOption().ConfigureAwait(false).GetAwaiter().GetResult();
                        break;
                    case "0":
                        ShowMainMenu();
                        break;
                    default:
                        Console.WriteLine("\nInvalid option selected. Press any key to continue.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task ShowUpdateContactOption()
        {

            Console.Clear();
            DisplayMenuTitle("Update Contact Information");

            Console.Write("Enter User Email: ");
            var emailToView = Console.ReadLine();

            var user = await _userService.GetUserByEmailAsync(emailToView!);

            if (user != null)
            {
                Console.Clear();
                Console.WriteLine($"Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Street Name: {user.StreetName}");
                Console.WriteLine($"Postal Code: {user.PostalCode}");
                Console.WriteLine($"City: {user.City}");

                Console.WriteLine("\nEnter Updated Address Details: ");

                Console.Write("First Name: ");
                var firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                var lastName = Console.ReadLine();

                Console.Write("Street Name: ");
                var streetName = Console.ReadLine();

                Console.Write("Postal Code: ");
                var postalCode = Console.ReadLine();

                Console.Write("City: ");
                var city = Console.ReadLine();

                var updatedAddress = new UserDto
                {
                    Email = emailToView!,
                    FirstName = firstName!,
                    LastName = lastName!,
                    StreetName = streetName,
                    PostalCode = postalCode,
                    City = city
                };

                var success = await _userService.UpdateUserAddressAsync(updatedAddress);
                if (success)
                {
                Console.Clear();
                Console.WriteLine("\nContact information updated successfully!");
                }
                else
                {
                Console.Clear();
                Console.WriteLine("\nFailed to update contact information.");
                }
            }
            else
            {
            Console.Clear();
            Console.WriteLine("\nUser not found.");
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        private async Task ShowUpdateRoleOption()
        {
            Console.Clear();
            DisplayMenuTitle("Update User Role");

            Console.Write("Enter User Email: ");
            var emailToView = Console.ReadLine();

            var user = await _userService.GetUserByEmailAsync(emailToView!);

            if (user != null)
            {
                Console.Clear();
                Console.WriteLine($"User Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Role Name: {user.RoleName}");

                Console.WriteLine("\nEnter New User Role: ");

                Console.Write("Role Name: ");
                var roleName = Console.ReadLine();

                var updatedRole = new UserDto
                {
                    Email = emailToView!,
                    RoleName = roleName!
                };

                var success = await _userService.UpdateUserRoleAsync(emailToView!, roleName!);
                if (success)
                {
                    Console.Clear();
                    Console.WriteLine("\nUser role updated successfully!");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\nFailed to update user role.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\nUser not found.");
            }

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }

        private async Task ShowUpdateUserAuthOption()
        {
            Console.Clear();
            DisplayMenuTitle("Update User Authentication");

            Console.Write("\nEnter User Email: ");
            var emailToUpdate = Console.ReadLine();

            var user = await _userService.GetUserByEmailAsync(emailToUpdate!);

            if (user != null)
            {
                Console.Clear();
                Console.WriteLine($"User Name: {user.FirstName} {user.LastName}");

                Console.WriteLine("\nEnter New User Authentication Details: ");

                Console.Write("New Email: ");
                var newEmail = Console.ReadLine();

                Console.Write("New Password: ");
                var newPassword = Console.ReadLine();

                var updatedUserDto = new UserRegistrationDto
                {
                    Email = newEmail!,
                    Password = newPassword!
                };

                var success = await _userService.UpdateUserAuthAsync(emailToUpdate!, updatedUserDto);
                if (success)
                {
                    Console.Clear();
                    Console.WriteLine("\nUser authentication details updated successfully!");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\nFailed to update user authentication details.");
                }

                Console.WriteLine("\nPress any key to continue.");
                Console.ReadKey();
            }
        }


        private async Task ShowDeleteUserOption()
        {
            DisplayMenuTitle("Delete User");

            Console.Write("Enter the email of the user you want to delete: ");
            var emailToDelete = Console.ReadLine();

            if (await _userService.DeleteUserByEmailAsync(emailToDelete!))
            {
                Console.WriteLine("\nUser deleted successfully. Press any key to continue.");
            }
            else
            {
                Console.WriteLine("\nUser not found.");
            }

            Console.WriteLine("\nPress any key to continue.");
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

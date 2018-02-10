using System;
using hubMarket_app;
using hubMarket_domain;

namespace hubMarket_console
{
    class Program
    {
        static void Main(string[] args)
        {
            var userApp = new UserApp();

            userApp.Add(new User { Login = "Teste1", Password = "123" });
            userApp.Add(new User { Login = "Teste2", Password = "123" });
            userApp.Add(new User { Login = "Teste3", Password = "123" });

            var users = userApp.GetAll();

            foreach (var item in users)
            {
                Console.WriteLine($"Login: {item.Id}-{item.Login}");
            }

            Console.ReadLine();

        }
    }
}

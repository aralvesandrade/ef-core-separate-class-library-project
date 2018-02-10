using System;
using System.Collections.Generic;
using System.Linq;
using hubMarket_data;
using hubMarket_domain;

namespace hubMarket_app
{
    public class UserApp
    {
        private readonly Context _context;

        public UserApp()
        {
            _context = new Context();
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }
    }
}
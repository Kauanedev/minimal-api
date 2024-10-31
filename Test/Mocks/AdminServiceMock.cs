using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Domain.Services;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;
using minimal_api.Domain.Dto;

namespace Test.Mocks
{
    public class AdminServiceMock : IAdminService
    {
        private static List<Admin> admins = new List<Admin>(
                new Admin
                {
                    Id = "fc82e878-92b5-43ea-bbb3-d09a1297c1b0",
                    Email = "admin1@example.com",
                    Password = "string",
                    Perfil = 1
                },
                new Admin
                {
                    Id = "fc82e878-92b5-43ea-bbb3-d09a1297c1b01",
                    Email = "Editor@example.com",
                    Password = "string",
                    Perfil = 2
                }
        );

        public Admin Create(Admin admin)
        {
            admin.Id = admins.Count().ToString();
            admins.Add(admin);
            return admin;
        }

        public List<Admin> GetAll(int? page)
        {
            return admins;
        }

        public Admin? GetById(string id)
        {
            return admins.FirstOrDefault(a => a.Id == id);
        }

        public Admin? Login(LoginDto loginDto)
        {
            return admins.FirstOrDefault(a => a.Email == loginDto.Email && a.Password == loginDto.Password);
        }

    }
}

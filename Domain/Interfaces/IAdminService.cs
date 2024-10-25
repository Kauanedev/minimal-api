using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalApi.Dto;
using MinimalApi.Entities;

namespace minimal_api.Domain.Interfaces
{
    public interface IAdminService
    {
        Admin? Login(LoginDto loginDto);
    }
}
using System;
using judo_univ_rennes.Contracts;
using judo_univ_rennes.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace judo_univ_rennes.Hubs
{
	public class NotifHub : Hub
    {

        private readonly UserManager<ApiUser> _userManager;
        private readonly JudoDbContext _db;
        
        string connectionString = "";

        public NotifHub(JudoDbContext db, IConfiguration configuration, UserManager<ApiUser> userManager)
        {

            _userManager = userManager;
            _db = db;
            connectionString = configuration.GetConnectionString("Account");

        }
    }
}


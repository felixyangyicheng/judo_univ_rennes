﻿using System;
using AutoMapper;
using Blazored.LocalStorage;
using judo_univ_rennes.Contracts;
using judo_univ_rennes.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace judo_univ_rennes.Services
{
	public class NewsService:INewsRepo
	{
        private readonly ILogger<NewsService> logger;
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        private readonly ITokenRepo tokenRepo;
        private readonly IMemoryCache memoryCache;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public NewsService(
            ILogger<NewsService> logger,
            UserManager<ApiUser> userManager,
            IMapper mapper,
            IConfiguration configuration,
            IMemoryCache memoryCache,
            IEmailSender emailSender,
            ITokenRepo tokenRepo,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider
        )
        {
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
            this.emailSender = emailSender;
            this.tokenRepo = tokenRepo;
            this.memoryCache = memoryCache;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task CallUpdate()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Create(News entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(News entity)
        {
            throw new NotImplementedException();
        }

        public async Task<News> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> isExists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(News entity)
        {
            throw new NotImplementedException();
        }
    }
}


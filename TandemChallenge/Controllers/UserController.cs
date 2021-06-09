﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TandemChallenge.Api.Models;
using TandemChallenge.Domain;

namespace TandemChallenge.Controllers.Api
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ILogger<UserController> logger;

        public UserController(IMediator mediator, IMapper mapper, ILogger<UserController> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new user within the system.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Add(AddUserViewModel viewModel)
        {
            var command = mapper.Map<CreateUserCommand>(viewModel);
            var user = await mediator.Send(command);


            return Ok(mapper.Map<UserViewModel>(user));
        }

        [NullFilter] // Returns a 404 response when the the result is null.
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserViewModel))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await mediator.Send(new FindUserByEmailQuery(email));

            return Ok(mapper.Map<UserViewModel>(user));
        }
    }
}

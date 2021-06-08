using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TandemChallenge.Api.Models;

namespace TandemChallenge.Controllers.Api
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, IMapper mapper, ILogger<UserController> logger)
        {
            this.mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user within the system.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public IActionResult Add(AddUserViewModel viewModel)
        {
            return Ok();
        }

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserViewModel))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet]
        public IEnumerable<UserViewModel> Get()
        {
            throw new NotImplementedException();
        }
    }
}

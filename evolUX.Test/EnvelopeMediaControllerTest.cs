using evolUX.Areas.EvolDP.Controllers;
using evolUX.Context;
using evolUX.Interfaces;
using evolUX.Repository;
using evolUX.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Xunit;

namespace evolUX.Test
{
    public class EnvelopeMediaControllerTest
    {
        EnvelopeMediaController _controller;
        IWrapperRepository _repository;
        DapperContext _context;
        ILoggerManager _logger;
        IConfiguration _configuration;

        public EnvelopeMediaControllerTest()
        {
            _logger = new LoggerManager();
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"appsettings.json", false, false)
                .AddEnvironmentVariables()
                .Build();
            _context = new DapperContext(_configuration);
            _repository = new WrapperRepository(_context);
            _controller = new EnvelopeMediaController(_repository, _logger);
            
        }

        [Fact]
        public async void GetEnvelopeMediaTest()
        {
            //Arrange
            //Act

            var result = await _controller.GetEnvelopeMedia();
            //Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var list = result.Result as OkObjectResult;

            Assert.IsType<List<dynamic>>(list.Value);

            var envList = list.Value as List<dynamic>;

            Assert.Equal(5, envList.Count);
        }
    }
}
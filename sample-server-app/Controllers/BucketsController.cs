using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Forge;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BucketsController : ControllerBase
    {
        private readonly ILogger<BucketsController> _logger;
        private readonly DataManagementClient _dataManagementClient;

        public BucketsController(ILogger<BucketsController> logger)
        {
            _logger = logger;
            var ForgeClientID = Environment.GetEnvironmentVariable("FORGE_CLIENT_ID");
            var ForgeClientSecret = Environment.GetEnvironmentVariable("FORGE_CLIENT_SECRET");
            _dataManagementClient = new DataManagementClient(ForgeClientID, ForgeClientSecret);
        }

        [HttpGet]
        public IEnumerable<DataManagementClient.Bucket> Get()
        {
            return _dataManagementClient.EnumerateBuckets().ToEnumerable<DataManagementClient.Bucket>();
        }
    }
}
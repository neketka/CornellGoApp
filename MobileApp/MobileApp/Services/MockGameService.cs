using CommunicationModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Services
{
    public class MockGameService : GameService
    {
        public MockGameService() : base(new MockGameClient()) { }
    }
}

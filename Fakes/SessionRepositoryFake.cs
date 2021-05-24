using MitForbrug.Interfaces.WebAPI;
using MitForbrug.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitForbrug_Tests.Mocks
{
    public class SessionRepositoryFake : ISessionRepository
    {
        public IAbstractApiCalls FlexComAPI { get; set; }

        public int Timeout
        {
            get { return 0; }
            set { }
        }

        public IAbstractApiCalls GetFlexComAPI()
        {
            return FlexComAPI;
        }

        public void ClearSession()
        {
        }

        public IAbstractApiCalls GetAbstractApiCalls()
        {
            return null;
        }

        

        public object GetFromKey(string key)
        {
            if (key == "customerName")
                return "TestKunde";
            return null;
        }

        public void SetKey(string key, object value)
        {
        }
    }
}

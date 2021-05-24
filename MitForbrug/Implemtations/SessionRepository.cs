using MitForbrug.Interfaces;
using MitForbrug.Interfaces.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MitForbrug.Implemtations
{
    public class SessionRepository : ISessionRepository
    {
        private System.Web.HttpSessionStateBase _session;
        public SessionRepository(System.Web.HttpSessionStateBase session)
        {
            _session = session;
        }

        public IAbstractApiCalls GetFlexComAPI()
        {
            return (IAbstractApiCalls)GetFromKey("PersonAndMetersAPI");
        }

        public object GetFromKey(string key)
        {
            return _session[key];
        }

        public void SetKey(string key, object value)
        {
            _session[key] = value;
        }

        public int Timeout
        {
            get
            {
                return _session.Timeout;
            }
            set
            {
                _session.Timeout = value;
            }
        }

        public void ClearSession()
        {
            _session.Clear();
        }

        public IAbstractApiCalls GetAbstractApiCalls()
        {
            return (AbstractApiCalls)System.Web.HttpContext.Current.Session["LeaseAndLoginAPI"];
        }
    }
}

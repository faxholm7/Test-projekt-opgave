using MitForbrug.Interfaces.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitForbrug.Interfaces
{
    public interface ISessionRepository
    {
        void ClearSession();

        IAbstractApiCalls GetFlexComAPI();

        IAbstractApiCalls GetAbstractApiCalls();

        object GetFromKey(string key);

        void SetKey(string key, object value);

        int Timeout { get; set; }
    }
}

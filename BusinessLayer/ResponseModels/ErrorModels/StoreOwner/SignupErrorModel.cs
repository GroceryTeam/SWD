using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.ErrorModels.StoreOwner
{
    public class SignupErrorModel
    {
        public enum SignupError
        {
            UsernameExists,
            EmailExists,
            PhoneNumberExists
        }
        public SignupError Error { get; set; }
    }
}

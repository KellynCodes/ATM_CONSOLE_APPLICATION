using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using Domain.Entities;
using CSPROJECT.UI;
using CSPROJECT.ATMAPP.App;

namespace Domain.Interfaces
{
    public interface IUserLogin
    {
        void CheckUserCardNumAndPassword();

    }
}
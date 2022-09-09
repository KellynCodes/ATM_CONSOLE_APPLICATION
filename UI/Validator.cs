using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using CSPROJECT.ATMAPP.App;

namespace CSPROJECT.UI
{
    public static class Validator
    {
        public static T Convert<T>(string prompt)
        {
            bool valid = false;
            string UserInput;

            while (!valid)
            {
                UserInput = Utility.GetUserInput(prompt);

                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null)
                    {
                        return (T)converter.ConvertFromString(UserInput);
                    }
                    else
                    {
                        return default;
                    }
                }
                catch
                {

                    Utility.PrintMessage("Invalid input. Try again.", false);
                }
            }
            return default;
        }
    }
}
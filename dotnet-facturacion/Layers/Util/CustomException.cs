using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
class CustomException : Exception
{
    public CustomException()
    {

    }
    public CustomException(string pParametro)
        : base(pParametro)
    {

    }
}


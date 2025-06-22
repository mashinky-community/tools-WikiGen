using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MashinkyWikiGen
{
    public interface IRule
    {
        Token Input1T { get;}
        int Input1C { get;}
        Token Input2T { get;}
        int Input2C { get;}
        Token Input3T { get;}
        int Input3C { get;}
        Token Output1T { get;}
        int Output1C { get;}
        Token Output2T { get;}
        int Output2C { get;}
        Token Output3T { get;}
        int Output3C { get;}


    }
}

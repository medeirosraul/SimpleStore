using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Monetaries
{
    public interface IMonetaryService
    {
        public string GetValueString(decimal? value);
    }

    public class MonetaryService : IMonetaryService
    {
        public string GetValueString(decimal? value)
        {
            if (!value.HasValue)
                return "R$0,00";

            return $"R${value.Value.ToString("F2")}";
        }
    }
}

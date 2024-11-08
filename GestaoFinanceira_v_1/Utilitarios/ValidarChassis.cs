using System.ComponentModel.DataAnnotations;

namespace GestaoFinanceira_v_1.Utilitarios
{
    public class ValidarChassis : ValidationAttribute
    {
        public override bool IsValid(object chassis)
        {
            if (chassis == null || string.IsNullOrEmpty(chassis.ToString()))
                return true;

            string a = chassis.ToString();

            if (a.Length != 17)
                return false;

            bool valido = true;
            return valido;
        }
       
    }
}


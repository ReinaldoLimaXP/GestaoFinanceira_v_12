using System.ComponentModel.DataAnnotations;

namespace GestaoFinanceira_v_1.Utilitarios
{
    public class ValidarRenavan : ValidationAttribute
    {
        public override bool IsValid(object renavan)
        {
            if (renavan == null || string.IsNullOrEmpty(renavan.ToString()))
                return true;

            string a = renavan.ToString();

            if (a.Length != 11)
                return false;

            bool valido = true;
            return valido;
        }
       
    }
}


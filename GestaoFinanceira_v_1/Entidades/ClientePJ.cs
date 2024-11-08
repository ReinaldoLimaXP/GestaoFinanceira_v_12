using GestaoFinanceira_v_1.Utilitarios;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("clientesPJ")]
    public class ClientePJ : Cliente
    {
        [Required(ErrorMessage = "CNPJ é obrigatório")]
        [ValidarCNPJ(ErrorMessage = "CNPJ inválido")]
        [MaxLength(120)]
        public string? CNPJ_PJ { get; set; }

        [Required]
        //[JsonProperty(PropertyName = "razao_social")]
        [MaxLength(255)]
        public string? RazaoSocial_PJ { get;set;}

        [MaxLength(255)]
        public string? NomeFantasia_PJ { get;set;}

        [MaxLength(120)]
        public string? InscricaoEstadual_PJ { get; set;}

        [Required]
        [MaxLength(120)]
        public string? AtividadeEconomica_PJ { get; set;}

        [MaxLength(120)]
        public string? Situacao_PJ { get; set; }

        [MaxLength(120)]
        public string? Responsavel_PJ { get; set;}
    }
}

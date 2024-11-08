using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.WebRequestMethods;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("Enderecos")]
    public class Endereco
    {
        [Key]
       public int Id_endereco { get; set; }

        [Required]
        [MaxLength(30)]
        public string Cep { get; set; }

        [Required]
        [MaxLength(150)]
        public string Logradouro { get; set; }

        [MaxLength(50)]
        public string? Complemento { get; set; }

        [Required]
        [MaxLength(15)]
        public string Numero { get; set; }

        [Required]
        [MaxLength(150)]
        public string Bairro { get; set; }

        //Nome da cidade
        [NotMapped]
        public string? Localidade { get; set; }

		//UF da cidade
		[NotMapped]
		public string? Uf { get; set; }

        [NotMapped]
        public string? Municipio { get; set; }

        [Required]
        public int? Fk_id_cidades { get; set; }
        public virtual Cidade? Cidade { get; set; }

        public static async Task<Endereco> BuscarLocalidade(string cep)
        {
            try
            {
                HttpClient http = new HttpClient();
                cep = cep.Replace("-", "");
                Endereco informacoesCep = new Endereco();
                string url = "https://viacep.com.br/ws/" + cep + "/json";

                HttpResponseMessage resposta = await http.GetAsync(url);
                string json = await resposta.Content.ReadAsStringAsync();
                informacoesCep = JsonConvert.DeserializeObject<Endereco>(json);


                return informacoesCep;
            }catch(Exception ex)
            {
                throw new Exception();
            }
           
        }

    }
}
